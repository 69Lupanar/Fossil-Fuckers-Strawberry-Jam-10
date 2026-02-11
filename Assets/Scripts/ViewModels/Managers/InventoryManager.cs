using System;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.Models.Player;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère l'inventaire du joueur
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand la taille de l'inventaire augmente
        /// </summary>
        public Action<int> OnInventorySizeIncreased { get; set; }

        /// <summary>
        /// Appelée quand un objet est ajouté à l'inventaire
        /// </summary>
        public Action<LootSO> OnLootAdded { get; set; }

        /// <summary>
        /// Appelée quand une gemme est trouvée
        /// </summary>
        public Action<LootSO> OnGemFound { get; set; }

        /// <summary>
        /// Appelée quand l'inventaire est plein et ne peut plus ajouter d'objet
        /// </summary>
        public Action OnInventoryFull { get; set; }

        /// <summary>
        /// Appelée quand on échoue à ajouter un objet à l'inventaire car plus de place
        /// </summary>
        public Action<LootSO> OnLootDiscarded { get; set; }

        /// <summary>
        /// Appelée quand l'inventaire est vidé
        /// </summary>
        public Action OnClear { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// La liste des objets transportés par le joueur
        /// </summary>
        public LootSO[] Inventory { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsManager;

        /// <summary>
        /// Le PlayerUpgradeManager   
        /// </summary>
        [SerializeField]
        private PlayerUpgradeManager _playerUpgradeManager;

        /// <summary>
        /// Le joueur
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le nombre d'emplacements libres
        /// </summary>
        private int _nbFreeSlots = 0;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _playerController.OnTileMined += OnTileMined;
            _playerUpgradeManager.OnStatUpgraded += OnStatUpgraded;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Initialise l'inventaire
        /// </summary>
        /// <returns></returns>
        public void InitializeInventory()
        {
            Inventory = new LootSO[_playerStatsManager.MaxInventorySize];
            _nbFreeSlots = _playerStatsManager.MaxInventorySize;
        }

        /// <summary>
        /// Ajoute l'objet renseigné à l'inventaire
        /// </summary>
        /// <param name="loot">L'objet à ajouter</param>
        public void AddLoot(LootSO loot)
        {
            //S'il n'y a plus de place, on abandonne l'objet

            if (_nbFreeSlots == 0)
            {
                OnLootDiscarded?.Invoke(loot);
                return;
            }

            // Sinon, on l'ajoute dans le premier emplacement libre

            for (int i = 0; i < Inventory.Length; ++i)
            {
                if (Inventory[i] == null)
                {
                    --_nbFreeSlots;
                    Inventory[i] = loot;
                    OnLootAdded?.Invoke(loot);

                    if (_nbFreeSlots == 0)
                    {
                        OnInventoryFull?.Invoke();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Retire l'objet à l'emplacement rensigné s'il est occupé
        /// </summary>
        /// <param name="index">La position dee l'emplacement dans la liste</param>
        public void RemoveAt(int index)
        {
            if (Inventory[index] != null)
            {
                Inventory[index] = null;
                ++_nbFreeSlots;
            }
        }

        /// <summary>
        /// Màj la taille de l'inventaire pour correspondre à la capacité du joueur
        /// </summary>
        public void UpdateInventorySize()
        {
            LootSO[] newInventory = new LootSO[_playerStatsManager.MaxInventorySize];
            Array.Copy(Inventory, newInventory, Inventory.Length);
            Inventory = newInventory;
            _nbFreeSlots = _playerStatsManager.MaxInventorySize;
            OnInventorySizeIncreased?.Invoke(_playerStatsManager.MaxInventorySize);
        }

        /// <summary>
        /// Vide l'inventaire
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Inventory.Length; ++i)
            {
                if (Inventory[i] != null)
                {
                    Inventory[i] = null;
                }
            }

            OnClear?.Invoke();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand une case est minée
        /// </summary>
        /// <param name="loot">L'objet miné</param>
        /// <param name="miningQuality">La qualité de minage</param>
        private void OnTileMined(LootSO loot, int miningQuality)
        {
            if (loot == null)
            {
                return;
            }

            _playerStatsManager.GainEXP(loot.EXP);

            switch (loot)
            {
                case FossilLootSO fossil:
                    FossilLootSO clone = fossil.Clone();
                    clone.Quality = UnityEngine.Random.Range(miningQuality / 2, miningQuality);
                    AddLoot(clone);
                    break;
                case GemLootSO gem:
                    OnGemFound?.Invoke(gem);
                    break;
            }
        }

        /// <summary>
        /// Appelée quand le joueur reçoit une amélioration
        /// </summary>
        /// <param name="upgrade">L'amélioration</param>
        private void OnStatUpgraded(PlayerUpgradeSO upgrade)
        {
            if (_nbFreeSlots < _playerStatsManager.MaxInventorySize)
            {
                UpdateInventorySize();
            }
        }

        #endregion
    }
}