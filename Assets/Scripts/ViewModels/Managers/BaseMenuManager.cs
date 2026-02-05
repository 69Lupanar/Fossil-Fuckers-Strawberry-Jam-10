using System.Collections.Generic;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.Loot;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les actions du menu de la base
    /// </summary>
    public class BaseMenuManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// L'inventaire de la base
        /// </summary>
        public List<LootSO> Inventory { get; private set; }

        /// <summary>
        /// La réserve de luxurosaures
        /// </summary>
        public List<LustosaurSO> StandbyReserve { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// L'InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// La capacité max de l'inventaire
        /// </summary>
        [SerializeField]
        private int _inventoryCapacity = 64;

        /// <summary>
        /// La capacité max de la réserve de luxurosaures
        /// </summary>
        [SerializeField]
        private int _teamStandbyCapacity = 20;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            Inventory = new List<LootSO>(_inventoryCapacity);
            StandbyReserve = new List<LustosaurSO>(_teamStandbyCapacity);
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

        #endregion
    }
}