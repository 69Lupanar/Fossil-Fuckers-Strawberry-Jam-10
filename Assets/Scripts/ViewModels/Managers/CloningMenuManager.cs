using System.Collections.Generic;
using System.Linq;
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
            CreatedLustosaur = null;

            // Arrête l'opération s'il manque des ingrédients

            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                if (ItemsInFusionSlots[i] == null)
                {
                    errorMsg = LogConstants.FUSION_NOT_ENOUGH_MATERIALS_MSG;
                    lustosaur = null;
                    return false;
                }
            }

            // On récupère les recettes correspondant aux objets renseignés par le joueur

            List<FusionIngredients> validRecipes = new(_fusionRecipes.FusionRecipes.Count);

            foreach (var pair in _fusionRecipes.FusionRecipes)
            {
                FusionIngredients recipe = pair.Key;

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

            // On trie les recettes pour avoir celles non-interchangeables en premier,
            // vu qu'elles référencent le plus souvent un luxurosaure hybride.

            validRecipes.OrderBy(recipe => !recipe.Interchangeable);

            // On prend la 1è recette et on crée son luxurosaure associé

            FusionResult[] possibleResults = _fusionRecipes.FusionRecipes[validRecipes[0]];
            int index = 0;

            if (possibleResults.Length > 1)
            {
                // S'il y a plusieurs Luxurosaures possibles, on le sélectionne
                // en fonction de leur chance d'apparition.
                // Si maxAlea n'atteint pas 100%, ce n'est pas grave, on prendra l'index 0 par défaut.

                float maxAlea = 0;
                NativeArray<Vector2> chanceIntervals = new(possibleResults.Length, Allocator.Temp);

                for (int i = 0; i < possibleResults.Length; ++i)
                {
                    chanceIntervals[i] = new Vector2(maxAlea, maxAlea + possibleResults[i].ChancePercentage);
                    maxAlea += possibleResults[i].ChancePercentage;
                }

                float rand = Random.Range(0f, 100f);

                for (int i = 0; i < chanceIntervals.Length; ++i)
                {
                    if (chanceIntervals[i].x < rand && rand < chanceIntervals[i].y)
                    {
                        index = i;
                        break;
                    }
                }
            }

            // On calcule la qualité moyenne du luxurosaure à créer

            int avgQuality = 0;

            for (int i = 0; i < ItemsInFusionSlots.Length; ++i)
            {
                avgQuality += ItemsInFusionSlots[i].Quality;
            }

            avgQuality /= ItemsInFusionSlots.Length;

            // On crée le luxurosaure

            CreatedLustosaur = LustosaurSO.CreateFrom(possibleResults[index].Lustosaur, avgQuality);

            lustosaur = CreatedLustosaur;
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Indique si les ingrédients renseignés correspondent à la recette
        /// </summary>
        /// <param name="recipe">La recette à suivre</param>
        /// <param name="itemsInFusionSlots">Les ingrédients renseignés</param>
        /// <returns>true si les ingrédients renseignés correspondent à la recette</returns>
        private bool IngredientsMatch(FusionIngredients recipe, LootSO[] itemsInFusionSlots)
        {
            if (recipe.Interchangeable)
            {
                // S'ils sont interchangeables,
                // on regarde d'abord si le nom de chaque objet est bien présent dans la recette

                for (int i = 0; i < itemsInFusionSlots.Length; ++i)
                {
                    if (!recipe.Ingredients.Any(ingredient => ingredient.name == itemsInFusionSlots[i].name))
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
                        if (recipe.Ingredients[j].name == itemsInFusionSlots[i].name &&
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
            }
            else
            {
                // Si les ingrédients ne sont pas interchangeables,
                // on regarde simplement s'ils partagent le même nom
                // dans le bon ordre

                for (int i = 0; i < itemsInFusionSlots.Length; ++i)
                {
                    if (recipe.Ingredients[i].name != itemsInFusionSlots[i].name)
                    {
                        return false;
                    }
                }
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

        #endregion
    }
}