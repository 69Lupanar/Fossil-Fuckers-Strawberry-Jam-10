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

        #endregion

        #region Variables Unity

        /// <summary>
        /// Les stats du joueur
        /// </summary>
        public PlayerStatsSO Stats { get; set; }

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Santé actuelle du joueur
        /// </summary>
        private float _curHealth;

        /// <summary>
        /// Energie actuelle du joueur
        /// </summary>
        private float _curEnergy;

        /// <summary>
        /// Chaleur actuelle du joueur
        /// </summary>
        private float _curHeat;

        /// <summary>
        /// Le dernier palier de chaleur qu'à atteint le joueur.
        /// Une fois un palier atteint, il perd la couche de vêtements correspondant.
        /// Le palier ne peut être ramené à 0 qu'en terminant la journée à la base.
        /// </summary>
        private int _lastHeatThreshold;

        /// <summary>
        /// Le montant d'EXP du joueur.
        /// Peut être utilisé pour l'achat ou la construction d'objets ou d'améliorations.
        /// </summary>
        private int _curEXPPoints;

        /// <summary>
        /// La distance où se trouve le joueur par rapport à la surface
        /// </summary>
        private int _curDepth;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Restaure les stats du joueur au max
        /// </summary>
        public void RestoreStats()
        {
            _curHealth = MaxHealth;
            _curEnergy = MaxEnergy;
            _curHeat = 0f;
            _lastHeatThreshold = 0;
            _curDepth = 0;
        }

        /// <summary>
        /// Gagne un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant gagné</param>
        public void GainEXP(int amount)
        {
            _curEXPPoints += amount;
        }

        /// <summary>
        /// Perd un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant perdu</param>
        public void LoseEXP(int amount)
        {
            _curEXPPoints -= amount;
        }

        #endregion
    }
}