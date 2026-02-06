using System;
using Assets.Scripts.Models.Player;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Chargé d'améliorer les stats du joueur
    /// </summary>
    public class PlayerUpgradeManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand une stat est améliorée
        /// </summary>
        public Action<PlayerUpgradeSO> OnStatUpgraded { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// La liste des améliorations de 1er niveau
        /// </summary>
        public PlayerUpgradeSO[] FirstStageUpgrades => _firstStageUpgrades;

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le joueur
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsMaanger;

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

        /// <summary>
        /// La liste des améliorations de 1er niveau
        /// </summary>
        [SerializeField]
        private PlayerUpgradeSO[] _firstStageUpgrades;

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
            _playerStatsMaanger.Stats = _defaultStats.Clone();
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
        /// Màj la stat via l'amélioration reseignée
        /// </summary>
        /// <param name="upgrade">L'amélioration</param>
        public void UpgradeStat(PlayerUpgradeSO upgrade)
        {
            switch (upgrade.UpgradeIndex)
            {
                case 0:
                    UpgradeMoveSpeed();
                    break;
                case 1:
                    UpgradeJumpForce();
                    break;
                case 2:
                    UpgradeNbMaxJumps();
                    break;
                case 3:
                    UpgradeMiningSpeed();
                    break;
                case 4:
                    UpgradeMiningQualityPercentage();
                    break;
                case 5:
                    UpgradeMaxHealth();
                    break;
                case 6:
                    UpgradeMaxEnergy();
                    break;
                case 7:
                    UpgradeMaxHeatThresholds();
                    break;
                case 8:
                    UpgradeMaxInventorySize();
                    break;
            }

            _playerStatsMaanger.RestoreStats();
            OnStatUpgraded?.Invoke(upgrade);
        }

        /// <summary>
        /// Augmente la vitesse de marche
        /// </summary>
        public void UpgradeMoveSpeed()
        {
            _playerStatsMaanger.Stats.MoveSpeed = _upgradeStats[_playerLevelsPerStat[0]++].MoveSpeed;
        }

        /// <summary>
        /// Augmente la hauteur du saut
        /// </summary>
        public void UpgradeJumpForce()
        {
            _playerStatsMaanger.Stats.JumpForce = _upgradeStats[_playerLevelsPerStat[1]++].JumpForce;
        }

        /// <summary>
        /// Augmente le nb max de sauts
        /// </summary>
        public void UpgradeNbMaxJumps()
        {
            _playerStatsMaanger.Stats.NbMaxJumps = _upgradeStats[_playerLevelsPerStat[2]++].NbMaxJumps;
        }

        /// <summary>
        /// Augmente la vitesse de minage
        /// </summary>
        public void UpgradeMiningSpeed()
        {
            _playerStatsMaanger.Stats.MiningSpeed = _upgradeStats[_playerLevelsPerStat[3]++].MiningSpeed;
        }

        /// <summary>
        /// Augmente la qualité de minage
        /// </summary>
        public void UpgradeMiningQualityPercentage()
        {
            _playerStatsMaanger.Stats.MiningQualityPercentage = _upgradeStats[_playerLevelsPerStat[4]++].MiningQualityPercentage;
        }

        /// <summary>
        /// Augmente la santé max
        /// </summary>
        public void UpgradeMaxHealth()
        {
            _playerStatsMaanger.Stats.MaxHealth = _upgradeStats[_playerLevelsPerStat[5]++].MaxHealth;
        }

        /// <summary>
        /// Augmente l'énergie max
        /// </summary>
        public void UpgradeMaxEnergy()
        {
            _playerStatsMaanger.Stats.MaxEnergy = _upgradeStats[_playerLevelsPerStat[6]++].MaxEnergy;
        }

        /// <summary>
        /// Augmente la chaleur max
        /// </summary>
        public void UpgradeMaxHeatThresholds()
        {
            _playerStatsMaanger.Stats.MaxHeatThresholds = _upgradeStats[_playerLevelsPerStat[7]++].MaxHeatThresholds;
        }

        /// <summary>
        /// Augmente la taille max de l'inventaire du joueur
        /// </summary>
        public void UpgradeMaxInventorySize()
        {
            _playerStatsMaanger.Stats.MaxInventorySize = _upgradeStats[_playerLevelsPerStat[8]++].MaxInventorySize;
        }

        #endregion
    }
}