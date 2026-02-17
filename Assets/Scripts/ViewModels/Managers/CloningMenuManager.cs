using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.Logs;
using Assets.Scripts.Models.Loot;
using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les actions du menu de clonage
    /// </summary>
    public class CloningMenuManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un objet est retiré de l'inventaire
        /// </summary>
        public Action<LootSO> OnItemDiscarded { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// L'inventaire de la base
        /// </summary>
        public List<LootSO> Inventory { get; private set; }

        /// <summary>
        /// Les objets dans chaque emplacement de fusion
        /// </summary>
        public LootSO[] ItemsInFusionSlots { get; private set; }

        /// <summary>
        /// Le luxurosaure créé par fusion
        /// </summary>
        public LustosaurSO CreatedLustosaur { get; private set; }


        #endregion

        #region Variables Unity

        /// <summary>
        /// L'InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamManager;

        /// <summary>
        /// Les recettes de fusion possibles
        /// </summary>
        [SerializeField]
        private FusionRecipesSO _fusionRecipes;

        /// <summary>
        /// La capacité max de l'inventaire
        /// </summary>
        [SerializeField]
        private int _inventoryCapacity = 64;

        /// <summary>
        /// Le nb d'emplacements de fusion
        /// </summary>
        [SerializeField]
        private int _nbFusionSlots = 3;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            Inventory = new List<LootSO>(_inventoryCapacity);
            ItemsInFusionSlots = new LootSO[_nbFusionSlots];
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Transfère tous les objets de l'inventaire du joueur
        /// vers celui de la base
        /// </summary>
        public void TransferInventoryToBase()
        {
            for (int i = 0; i < _inventoryManager.Inventory.Length; ++i)
            {
                LootSO item = _inventoryManager.Inventory[i];
                if (item != null && Inventory.Count < Inventory.Capacity)
                {
                    Inventory.Add(item);
                }
            }

            _inventoryManager.Clear();
        }

        /// <summary>
        /// Lance une tentative de fusion à partir des ingrédients renseignés
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé si fusion réussie</param>
        /// <param name="errorMsg">Le message d'erreur à afficher</param>
        /// <returns>true si la fusion a réussi</returns>
        public bool TryFusion(out LustosaurSO lustosaur, out string errorMsg)
        {
            lustosaur = null;
            CreatedLustosaur = null;


            // Arrête l'opération s'il manque des ingrédients

            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                if (ItemsInFusionSlots[i] == null)
                {
                    errorMsg = LogConstants.FUSION_NOT_ENOUGH_MATERIALS_MSG;
                    return false;
                }
            }

            if (TryHybridFusion(out lustosaur, out errorMsg) ||
               TryNormalFusion(out lustosaur, out errorMsg))
            {
                CreatedLustosaur = lustosaur;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Lance une tentative de fusion à partir des ingrédients renseignés
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé si fusion réussie</param>
        /// <param name="errorMsg">Le message d'erreur à afficher</param>
        /// <returns>true si la fusion a réussi</returns>
        private bool TryHybridFusion(out LustosaurSO lustosaur, out string errorMsg)
        {
            // On récupère les recettes correspondant aux objets renseignés par le joueur

            List<FusionAttributeIngredients> validRecipes = new(_fusionRecipes.HybridFusionRecipes.Count);

            foreach (var pair in _fusionRecipes.HybridFusionRecipes)
            {
                FusionAttributeIngredients recipe = pair.Key;

                if (IngredientsMatch(recipe, ItemsInFusionSlots))
                {
                    validRecipes.Add(recipe);
                }
            }

            // Arrête l'opération si aucune recette n'est valide

            if (validRecipes.Count == 0)
            {
                errorMsg = LogConstants.FUSION_INCOMPATIBLE_MATERIALS_MSG;
                lustosaur = null;
                return false;
            }

            // On prend la 1è recette et on crée son luxurosaure hybride associé

            FusionResult[] possibleResults = _fusionRecipes.HybridFusionRecipes[validRecipes[0]];
            int index = -1;

            // On le sélectionne en fonction de leur chance d'apparition.

            float maxAlea = 0;
            NativeArray<Vector2> chanceIntervals = new(possibleResults.Length, Allocator.Temp);

            for (int i = 0; i < possibleResults.Length; ++i)
            {
                chanceIntervals[i] = new Vector2(maxAlea, maxAlea + possibleResults[i].ChancePercentage);
                maxAlea += possibleResults[i].ChancePercentage;
            }

            float rand = UnityEngine.Random.Range(0f, 100f);

            for (int i = 0; i < chanceIntervals.Length; ++i)
            {
                if (chanceIntervals[i].x < rand && rand < chanceIntervals[i].y)
                {
                    index = i;
                    break;
                }
            }

            // Arrête l'opération si on n'a pas eu assez de chance pour obtenir un hybride.
            // On passera dans la méthode de fusion normale.

            if (index == -1)
            {
                lustosaur = null;
                errorMsg = string.Empty;
                return false;
            }

            // On calcule la qualité moyenne du luxurosaure à créer

            int avgQuality = 0;

            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                avgQuality += ItemsInFusionSlots[i].Quality;
            }

            avgQuality /= ItemsInFusionSlots.Length;

            // On crée le luxurosaure

            lustosaur = LustosaurSO.CreateFrom(possibleResults[index].Lustosaur, avgQuality);
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Lance une tentative de fusion à partir des ingrédients renseignés
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé si fusion réussie</param>
        /// <param name="errorMsg">Le message d'erreur à afficher</param>
        /// <returns>true si la fusion a réussi</returns>
        private bool TryNormalFusion(out LustosaurSO lustosaur, out string errorMsg)
        {
            // Au lieu d'arrêter l'opération si aucune recette n'est valide,
            // on prend un des ingrédients au hasard et on clone son luxurosaure associé

            int rand = UnityEngine.Random.Range(0, ItemsInFusionSlots.Length);

            // On crée le luxurosaure

            lustosaur = ItemsInFusionSlots[rand] switch
            {
                FossilLootSO fossil => LustosaurSO.CreateFrom(fossil.Lustosaur, fossil.Quality),
                SpermLootSO sperm => LustosaurSO.CreateFrom(sperm.Lustosaur, sperm.Quality),
                _ => null
            };

            errorMsg = lustosaur != null ? string.Empty : LogConstants.FUSION_INCOMPATIBLE_MATERIALS_MSG;
            return lustosaur != null;
        }

        /// <summary>
        /// Indique si les ingrédients renseignés correspondent à la recette
        /// </summary>
        /// <param name="recipe">La recette à suivre</param>
        /// <param name="itemsInFusionSlots">Les ingrédients renseignés</param>
        /// <returns>true si les ingrédients renseignés correspondent à la recette</returns>
        private bool IngredientsMatch(FusionAttributeIngredients recipe, LootSO[] itemsInFusionSlots)
        {
            // S'ils sont interchangeables,
            // on regarde d'abord si le nom de chaque objet est bien présent dans la recette

            for (int i = 0; i < itemsInFusionSlots.Length; ++i)
            {
                ElementalAttribute ingredientAttribute = ElementalAttribute.Neutral;

                switch (itemsInFusionSlots[i])
                {
                    case FossilLootSO fossil:
                        ingredientAttribute = fossil.Lustosaur.Attribute;
                        break;

                    case SpermLootSO sperm:
                        ingredientAttribute = sperm.Lustosaur.Attribute;
                        break;
                }

                if (!recipe.Ingredients.Any(ingredient => ingredient == ingredientAttribute))
                {
                    return false;
                }
            }

            // Ensuite, on procède par élimination, traversant la recette pour chaque ingrédient
            // et notant son index s'il est présent.
            // S'ils sont tous présents, la recette est valide.

            NativeList<int> observedItems = new(recipe.Ingredients.Length, Allocator.Temp);

            for (int i = 0; i < itemsInFusionSlots.Length; ++i)
            {
                for (int j = 0; j < recipe.Ingredients.Length; ++j)
                {
                    ElementalAttribute ingredientAttribute = ElementalAttribute.Neutral;

                    switch (itemsInFusionSlots[i])
                    {
                        case FossilLootSO fossil:
                            ingredientAttribute = fossil.Lustosaur.Attribute;
                            break;

                        case SpermLootSO sperm:
                            ingredientAttribute = sperm.Lustosaur.Attribute;
                            break;
                    }

                    if (recipe.Ingredients[j] == ingredientAttribute &&
                        !observedItems.Contains(j))
                    {
                        observedItems.Add(j);
                        break;
                    }
                }
            }

            if (observedItems.Length < recipe.Ingredients.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ajoute le luxurosaure créé à l'équipe
        /// </summary>
        public void AddCreatedLustosaurToTeam()
        {
            _teamManager.AddLustosaur(CreatedLustosaur);
        }

        /// <summary>
        /// Nettoyage à la fermeture de la fenêtre
        /// </summary>
        public void CleanupOnWindowClosed()
        {
            // Avant de fermer la fenêtre, on renvoie tous les objets
            // des emplacements de fusion à l'inventaire

            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                if (ItemsInFusionSlots[i] != null)
                {
                    Inventory.Add(ItemsInFusionSlots[i]);
                }
            }

            CleanupFusionSlots();
        }

        /// <summary>
        /// Nettoyage de la table de fusion
        /// </summary>
        public void CleanupFusionSlots()
        {
            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                ItemsInFusionSlots[i] = null;
            }
        }

        /// <summary>
        /// Retire l'objet de l'inventaire
        /// </summary>
        /// <param name="index">La position de l'objet dans la liste</param>
        public void DiscardItem(int index)
        {
            LootSO item = Inventory[index];
            Inventory.RemoveAt(index);
            OnItemDiscarded?.Invoke(item);
        }

        #endregion
    }
}