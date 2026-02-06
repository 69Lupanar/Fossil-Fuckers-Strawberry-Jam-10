using System;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.Logs;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Crée les logs suivant une action (objet ramassé, alerte, etc.)
    /// </summary>
    public class LogManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un log est ajouté
        /// </summary>
        public Action<Log> OnLogAdded { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsManager;

        /// <summary>
        /// L'InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// L'icôned d'alerte
        /// </summary>
        [SerializeField]
        private Sprite _alertIcon;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _playerStatsManager.OnCriticalHealthReached += OnCriticalHealthReached;
            _playerStatsManager.OnCriticalEnergyReached += OnCriticalEnergyReached;
            _playerStatsManager.OnCriticalHeatReached += OnCriticalHeatReached;
            _playerStatsManager.OnNewHeatThresholdReached += OnNewHeatThresholdReached;

            _inventoryManager.OnLootAdded += OnLootAdded;
            _inventoryManager.OnGemFound += OnGemFound;
            _inventoryManager.OnInventoryFull += OnInventoryFull;
            _inventoryManager.OnLootDiscarded += OnLootDiscarded;

            _teamMenuManager.OnLustosaurCreated += OnLustosaurCreated;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ajoute un nouveau message à la queue
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="message"></param>
        public void AddLog(Sprite sprite, string message)
        {
            OnLogAdded?.Invoke(new Log(sprite, message));
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand la santé du joueur atteint un niveau critique
        /// </summary>
        private void OnCriticalHealthReached()
        {
            AddLog(_alertIcon, LogConstants.HEALTH_LOW_MSG);
        }

        /// <summary>
        /// Appelée quand l'énergie du joueur atteint un niveau critique
        /// </summary>
        private void OnCriticalEnergyReached()
        {
            AddLog(_alertIcon, LogConstants.ENERGY_LOW_MSG);
        }

        /// <summary>
        /// Appelée quand la chaleur du joueur atteint un niveau critique
        /// </summary>
        private void OnCriticalHeatReached()
        {
            AddLog(_alertIcon, LogConstants.OVERHEAT_MSG);
        }

        /// <summary>
        /// Appelée quand le joueur atteint un nouveau palier de chaleur
        /// </summary>
        /// <param name="newThreshold">Le nouveau palier</param>
        private void OnNewHeatThresholdReached(int newThreshold)
        {
            AddLog(_alertIcon, LogConstants.HEAT_THRESHOLDS_MSG[newThreshold]);
        }

        /// <summary>
        /// Appelée quand un objet est ajouté à l'inventaire
        /// </summary>
        /// <param name="loot">L'objet ajouté</param>
        private void OnLootAdded(LootSO loot)
        {
            AddLog(loot.Sprite, string.Format(LogConstants.FOUND_ITEM_MSG, loot.name, loot.EXP));
        }

        /// <summary>
        /// Appelée quand une gemme est trouvée
        /// </summary>
        /// <param name="loot">L'objet ajouté</param>
        private void OnGemFound(LootSO loot)
        {
            AddLog(loot.Sprite, string.Format(LogConstants.FOUND_GEM_MSG, loot.name, loot.EXP));
        }

        /// <summary>
        /// Appelée quand l'inventaire est plein et ne peut plus ajouter d'objet
        /// </summary>
        private void OnInventoryFull()
        {
            AddLog(null, LogConstants.INVENTORY_FULL_MSG);
        }

        /// <summary>
        /// Appelée quand on échoue à ajouter un objet à l'inventaire car plus de place
        /// </summary>
        /// <param name="loot">L'objet défaussé</param>
        private void OnLootDiscarded(LootSO loot)
        {
            AddLog(loot.Sprite, string.Format(LogConstants.DISCARDED_ITEM_MSG, loot.name));
        }

        /// <summary>
        /// Appelée quand un Luxurosaure est créé
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé</param>
        private void OnLustosaurCreated(LustosaurSO lustosaur)
        {
            AddLog(lustosaur.Sprite, string.Format(LogConstants.LUSTOSAUR_CREATED_MSG, lustosaur.name));
        }

        #endregion
    }
}