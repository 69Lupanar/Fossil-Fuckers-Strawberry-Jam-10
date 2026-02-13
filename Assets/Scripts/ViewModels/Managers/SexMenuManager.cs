using Assets.Scripts.Models;
using Assets.Scripts.Models.Adult;
using Assets.Scripts.Models.Loot;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère la scène adulte
    /// </summary>
    public class SexMenuManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// La scène adulte en cours de lecture
        /// </summary>
        public AdultSceneSO AdultScene { get; set; }

        /// <summary>
        /// La raison pour laqulle une scène de sexe doit se jouer
        /// </summary>
        public ReasonForSex ReasonForSex { get; set; }

        /// <summary>
        /// L'index du paragraphe actuel
        /// </summary>
        public int CurIndex { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// L' InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// La couleur de l'arrière-plan par type d'environnement
        /// </summary>
        [SerializedDictionary("Sex Environment", "Color")]
        [SerializeField]
        public SerializedDictionary<SexEnvironment, Color> ColorPerEnvironment;

        /// <summary>
        /// La scène adulte à jouer dans la gallerie
        /// </summary>
        [SerializedDictionary("Associated Lustosaur name", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesInGallery;

        /// <summary>
        /// La scène adulte à jouer après une défaite en combat
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesOnDefeat;

        /// <summary>
        /// La scène adulte à jouer après que la santé ou l'énergie du joueur soit vidée
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesOnStatDepleted;

        /// <summary>
        /// La scène adulte à jouer après une victoire en combat
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesOnVictory;

        /// <summary>
        /// Le SpermLoot à ajouter à l'inventaire du joueur après une scène adulte
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Sperm LootS O")]
        [SerializeField]
        public SerializedDictionary<string, SpermLootSO> SpermLootPerLustosaur;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le SpermLoot à ajouter à l'inventaire du joueur après une scène adulte
        /// </summary>
        private SpermLootSO _spermLootAfterSexScene;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne la scène adulte à lire
        /// </summary>
        /// <param name="adultScene">La scène adulte à lire</param>
        public void SetAdultScene(AdultSceneSO adultScene)
        {
            AdultScene = adultScene;
            CurIndex = -1;
        }

        /// <summary>
        /// Passe au paragraphe suivant de la scène en cours.
        /// S'il n'y en a plus, on arrête la scène.
        /// </summary>
        public string Next()
        {
            if (CurIndex == AdultScene.Paragraphs.Length - 1)
            {
                // Ajoute le SpermLootSO associé au luxurosaure de la scène

                if (_spermLootAfterSexScene != null)
                {
                    _inventoryManager.AddLoot(_spermLootAfterSexScene);
                    _spermLootAfterSexScene = null;
                }

                return string.Empty;
            }

            ++CurIndex;

            return AdultScene.Paragraphs[CurIndex];
        }

        /// <summary>
        /// Obtient le SpermLoot à ajouter à l'inventaire du joueur après une scène adulte
        /// </summary>
        /// <param name="lustosaurName">Le nom du luxurosaure</param>
        /// <param name="quality">La qualité du sperme à générer</param>
        public void SetSpermLootFromLustosaur(string lustosaurName, int quality)
        {
            _spermLootAfterSexScene = null;

            if (lustosaurName == "null")
            {
                return;
            }

            bool nameExists = false;

            switch (ReasonForSex)
            {
                case ReasonForSex.Victory:
                    nameExists = ScenesOnVictory.ContainsKey(lustosaurName);
                    break;
                case ReasonForSex.Defeat:
                    nameExists = ScenesOnDefeat.ContainsKey(lustosaurName);
                    break;
                case ReasonForSex.Gallery:
                    nameExists = ScenesInGallery.ContainsKey(lustosaurName);
                    break;
                case ReasonForSex.StatDepleted:
                    nameExists = ScenesOnStatDepleted.ContainsKey(lustosaurName);
                    break;
            }

            if (nameExists)
            {
                _spermLootAfterSexScene = SpermLootPerLustosaur[lustosaurName];

                if (_spermLootAfterSexScene != null)
                {
                    _spermLootAfterSexScene = _spermLootAfterSexScene.Clone();
                    _spermLootAfterSexScene.Quality = quality;
                }
            }
        }

        #endregion
    }
}