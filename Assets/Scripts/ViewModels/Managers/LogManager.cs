using System;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.Logs;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.Models.Player;
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
        /// Le CloningMenuManager
        /// </summary>
        [SerializeField]
        private CloningMenuManager _cloningMenuManager;

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// Le PlayerUpgradeManager
        /// </summary>
        [SerializeField]
        private PlayerUpgradeManager _playerUpgradeMaanger;

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

            _cloningMenuManager.OnItemDiscarded += OnCloningItemDiscarded;

            _teamMenuManager.OnCreatedLustosaurAddedToTeam += OnCreatedLustosaurAddedToTeam;
            _teamMenuManager.OnCreatedLustosaurAddedToReserve += OnCreatedLustosaurAddedToReserve;
            _teamMenuManager.OnCreatedLustosaurDiscarded += OnCreatedLustosaurDiscarded;
            _teamMenuManager.OnCancelTryRemoveLastLustosaurFromTeam += OnCancelTryRemoveLastLustosaurFromTeam;
            _teamMenuManager.OnLustosaurDiscardedFromStandby += OnLustosaurDiscardedFromStandby;

            _playerUpgradeMaanger.OnStatUpgraded += OnStatUpgraded;
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
            AddLog(loot.Sprite, string.Format(LogConstants.INVENTORY_ITEM_DISCARDED_MSG, loot.name));
        }

        /// <summary>
        /// Appelée quand on retire un objet de l'inventaire de clonage
        /// </summary>
        /// <param name="loot">L'objet défaussé</param>
        private void OnCloningItemDiscarded(LootSO loot)
        {
            AddLog(loot.Sprite, string.Format(LogConstants.FUSION_ITEM_DISCARDED_MSG, loot.name));
        }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est ajouté à l'équipe
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé</param>
        private void OnCreatedLustosaurAddedToTeam(LustosaurSO lustosaur)
        {
            AddLog(lustosaur.NormalSprite, string.Format(LogConstants.LUSTOSAUR_CREATED_TEAM_MSG, lustosaur.name));
        }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est ajouté à la réserve
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé</param>
        private void OnCreatedLustosaurAddedToReserve(LustosaurSO lustosaur)
        {
            AddLog(lustosaur.NormalSprite, string.Format(LogConstants.LUSTOSAUR_CREATED_STANDBY_MSG, lustosaur.name));
        }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est défaussé
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé</param>
        private void OnCreatedLustosaurDiscarded(LustosaurSO lustosaur)
        {
            AddLog(lustosaur.NormalSprite, string.Format(LogConstants.LUSTOSAUR_CREATED_DISCARDED_MSG, lustosaur.name));
        }

        /// <summary>
        /// Appelée quand le joueur tente de retirer le dernier luxurosaure de son équipe
        /// </summary>
        private void OnCancelTryRemoveLastLustosaurFromTeam()
        {
            AddLog(_alertIcon, LogConstants.LAST_LUSTOSAUR_REMOVED_FROM_TEAM_ERROR_MSG);
        }

        /// <summary>
        /// Appelée quand un Luxurosaure est relâché de la réserve
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure défaussé</param>
        private void OnLustosaurDiscardedFromStandby(LustosaurSO lustosaur)
        {
            AddLog(lustosaur.NormalSprite, string.Format(LogConstants.LUSTOSAUR_DISCARDED_MSG, lustosaur.name));
        }

        /// <summary>
        /// Appelée quand une stat du joueur est améliorée
        /// </summary>
        /// <param name="upgrade">L'amélioration</param>
        private void OnStatUpgraded(PlayerUpgradeSO upgrade)
        {
            string msg = string.Empty;

            switch (upgrade.UpgradeIndex)
            {
                case 0:
                    msg = LogConstants.MAX_MOVE_SPEED_UPGRADED_MSG;
                    break;
                case 1:
                    msg = LogConstants.MAX_JUMP_FORCE_UPGRADED_MSG;
                    break;
                case 2:
                    msg = LogConstants.MAX_NB_JUMPS_UPGRADED_MSG;
                    break;
                case 3:
                    msg = LogConstants.MAX_MINING_SPEED_UPGRADED_MSG;
                    break;
                case 4:
                    msg = LogConstants.MAX_MINING_QUALITY_UPGRADED_MSG;
                    break;
                case 5:
                    msg = LogConstants.MAX_HEALTH_UPGRADED_MSG;
                    break;
                case 6:
                    msg = LogConstants.MAX_ENERGY_UPGRADED_MSG;
                    break;
                case 7:
                    msg = LogConstants.MAX_HEAT_UPGRADED_MSG;
                    break;
                case 8:
                    msg = LogConstants.MAX_INVENTORY_SIZE_UPGRADED_MSG;
                    break;
            }

            AddLog(upgrade.Sprite, string.Format(LogConstants.UPGRADE_ACQUIRED_MSG, upgrade.name));
            AddLog(upgrade.Sprite, msg);
        }

        #endregion
    }
}