using Assets.Scripts.Models.Logs;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Chargé d'améliorer les stats du joueur
    /// </summary>
    public class PlayerUpgradeManager : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le joueur
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _player;

        /// <summary>
        /// Le LogManager
        /// </summary>
        [SerializeField]
        private LogManager _logManager;

        /// <summary>
        /// L'icône d'amélioration
        /// </summary>
        [SerializeField]
        private Sprite _upgradeIcon;

        /// <summary>
        /// Les stats de départ du joueur
        /// </summary>
        [SerializeField]
        private PlayerStatsSO _defaultStats;

        /// <summary>
        /// La liste des stats par niveau
        /// </summary>
        [SerializeField]
        private PlayerStatsSO[] _upgradeStats;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le niveau du joueur dans chacune de ses statistiques.
        /// Au début du jeu, il commence au niveau 0
        /// </summary>
        private int[] _playerLevelsPerStat = new int[9]
        {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _player.Stats = _defaultStats.Clone();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Obtient si la stat renseignée a été améliorée au niveau max
        /// </summary>
        /// <param name="statIndex">L'id de la stat</param>
        /// <returns>true si elle est au niveau max</returns>
        public bool IsFullyUpgraded(int statIndex)
        {
            return _playerLevelsPerStat[statIndex] == _upgradeStats.Length;
        }

        /// <summary>
        /// Augmente la vitesse de marche
        /// </summary>
        public void UpgradeMoveSpeed()
        {
            _player.Stats.MoveSpeed = _upgradeStats[++_playerLevelsPerStat[0]].MoveSpeed;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_MOVE_SPEED_MSG);
        }

        /// <summary>
        /// Augmente la hauteur du saut
        /// </summary>
        public void UpgradeJumpForce()
        {
            _player.Stats.JumpForce = _upgradeStats[++_playerLevelsPerStat[1]].JumpForce;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_JUMP_FORCE_MSG);
        }

        /// <summary>
        /// Augmente le nb max de sauts
        /// </summary>
        public void UpgradeNbMaxJumps()
        {
            _player.Stats.NbMaxJumps = _upgradeStats[++_playerLevelsPerStat[2]].NbMaxJumps;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_NB_JUMPS_MSG);
        }

        /// <summary>
        /// Augmente la vitesse de minage
        /// </summary>
        public void UpgradeMiningSpeed()
        {
            _player.Stats.MiningSpeed = _upgradeStats[++_playerLevelsPerStat[3]].MiningSpeed;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_MINING_SPEED_MSG);
        }

        /// <summary>
        /// Augmente la qualité de minage
        /// </summary>
        public void UpgradeMiningQualityPercentage()
        {
            _player.Stats.MiningQualityPercentage = _upgradeStats[++_playerLevelsPerStat[4]].MiningQualityPercentage;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_MINING_QUALITY_MSG);
        }

        /// <summary>
        /// Augmente la santé max
        /// </summary>
        public void UpgradeMaxHealth()
        {
            _player.Stats.MaxHealth = _upgradeStats[++_playerLevelsPerStat[5]].MaxHealth;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_HEALTH_UPGRADED_MSG);
        }

        /// <summary>
        /// Augmente l'énergie max
        /// </summary>
        public void UpgradeMaxEnergy()
        {
            _player.Stats.MaxEnergy = _upgradeStats[++_playerLevelsPerStat[6]].MaxEnergy;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_ENERGY_UPGRADED_MSG);
        }

        /// <summary>
        /// Augmente la chaleur max
        /// </summary>
        public void UpgradeMaxHeatThresholds()
        {
            _player.Stats.MaxHeatThresholds = _upgradeStats[++_playerLevelsPerStat[7]].MaxHeatThresholds;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_HEAT_UPGRADED_MSG);
        }

        /// <summary>
        /// Augmente la taille max de l'inventaire du joueur
        /// </summary>
        public void UpgradeMaxInventorySize()
        {
            _player.Stats.MaxInventorySize = _upgradeStats[++_playerLevelsPerStat[8]].MaxInventorySize;
            _logManager.AddLog(_upgradeIcon, LogConstants.MAX_INVENTORY_SIZE_MSG);
        }

        #endregion
    }
}