using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Représente les stats du joueur
    /// </summary>
    public class PlayerStatsManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// Les stats du joueur
        /// </summary>
        public PlayerStatsSO Stats { get; set; }

        /// <summary>
        /// La vitesse de déplacement du joueur
        /// </summary>
        public float MoveSpeed => Stats.MoveSpeed;

        /// <summary>
        /// La vitesse de saut du joueur
        /// </summary>
        public float JumpForce => Stats.JumpForce;

        /// <summary>
        /// Le nombre de sauts max du joueur
        /// </summary>
        public int NbMaxJumps => Stats.NbMaxJumps;

        /// <summary>
        /// La vitesse de minage
        /// </summary>
        public float MiningSpeed => Stats.MiningSpeed;

        /// <summary>
        /// Le pourcentage de qualité de minage
        /// </summary>
        public float MiningQualityPercentage => Stats.MiningQualityPercentage;

        /// <summary>
        /// La santé max du joueur
        /// </summary>
        public float MaxHealth => Stats.MaxHealth;

        /// <summary>
        /// L'énergie max du joueur
        /// </summary>
        public float MaxEnergy => Stats.MaxEnergy;

        /// <summary>
        /// Les paliers de chaleur pouvant être supportés par le joueur.
        /// A chaque fois qu'il atteint un palier, il perd une couche de vêtements.
        /// Affecte également les stats de ses dinos en combat.
        /// </summary>
        public float[] MaxHeatThresholds => Stats.MaxHeatThresholds;

        /// <summary>
        /// Le nombre max d'objets que le joueur peut transporter
        /// avant de devoir retourner à la base
        /// </summary>
        public int MaxInventorySize => Stats.MaxInventorySize;

        /// <summary>
        /// Santé actuelle du joueur
        /// </summary>
        public float CurHealth { get; private set; }

        /// <summary>
        /// Energie actuelle du joueur
        /// </summary>
        public float CurEnergy { get; private set; }

        /// <summary>
        /// Chaleur actuelle du joueur
        /// </summary>
        public float CurHeat { get; private set; }

        /// <summary>
        /// Le montant d'EXP du joueur.
        /// Peut être utilisé pour l'achat ou la construction d'objets ou d'améliorations.
        /// </summary>
        public int CurDepth { get; private set; }

        /// <summary>
        /// La distance où se trouve le joueur par rapport à la surface
        /// </summary>
        public int CurEXPPoints { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// La transform du joueur
        /// </summary>
        [SerializeField]
        private Transform _player;

        /// <summary>
        /// Energie consomméee en marchant
        /// </summary>
        [SerializeField]
        private float _movingEnergyConsumption = .001f;

        /// <summary>
        /// Energie consomméee par bloc miné
        /// </summary>
        [SerializeField]
        private float _miningEnergyConsumption = .01f;

        /// <summary>
        /// Niveau de chaleur minimum par niveau de profondeur du joueur
        /// </summary>
        [SerializeField]
        private float _heatIncreasePerDepth = .05f;

        /// <summary>
        /// Niveau de chaleur généré par le minage d'un bloc
        /// </summary>
        [SerializeField]
        private float _miningHeatIncrease = .05f;

        /// <summary>
        /// Utilisé pour le calcul de la profondeur
        /// </summary>
        [SerializeField]
        private float _spawnSpacing = 2f;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Chaleur actuelle du joueur générée par le minage
        /// </summary>
        private float _curMiningHeatIncrease;

        /// <summary>
        /// Le dernier palier de chaleur qu'à atteint le joueur.
        /// Une fois un palier atteint, il perd la couche de vêtements correspondant.
        /// Le palier ne peut être ramené à 0 qu'en terminant la journée à la base.
        /// </summary>
        private int _lastHeatThreshold;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            CurDepth = -Mathf.Min(0, Mathf.RoundToInt(_player.position.y / _spawnSpacing) - 1);
            CurHeat = CurDepth * _heatIncreasePerDepth + _curMiningHeatIncrease;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Restaure les stats du joueur au max
        /// </summary>
        public void RestoreStats()
        {
            CurHealth = MaxHealth;
            CurEnergy = MaxEnergy;
            CurHeat = 0f;
            _lastHeatThreshold = 0;
            CurDepth = 0;
        }

        /// <summary>
        /// Gagne un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant gagné</param>
        public void GainEXP(int amount)
        {
            CurEXPPoints += amount;
        }

        /// <summary>
        /// Perd un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant perdu</param>
        public void LoseEXP(int amount)
        {
            CurEXPPoints -= amount;
        }

        #endregion
    }
}