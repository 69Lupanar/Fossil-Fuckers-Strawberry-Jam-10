using System;
using Assets.Scripts.Models.EventArgs;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Représente les stats du joueur
    /// </summary>
    public class PlayerStatsManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand le joueur gagne ou perd de l'expérience
        /// </summary>
        public EventHandler<EXPChangedEventArgs> OnCurEXPChanged { get; set; }

        /// <summary>
        /// Appelée quand la santé du joueur atteint un niveau critique
        /// </summary>
        public Action OnCriticalHealthReached { get; set; }

        /// <summary>
        /// Appelée quand l'énergie du joueur atteint un niveau critique
        /// </summary>
        public Action OnCriticalEnergyReached { get; set; }

        /// <summary>
        /// Appelée quand la chaleur du joueur atteint un niveau critique
        /// </summary>
        public Action OnCriticalHeatReached { get; set; }

        /// <summary>
        /// Appelée quand le joueur atteint un nouveau palier de chaleur
        /// </summary>
        public Action<int> OnNewHeatThresholdReached { get; set; }

        /// <summary>
        /// Appelée quand les stats du joueur sont restaurées
        /// </summary>
        public Action OnRestored { get; set; }

        /// <summary>
        /// Appelée quand le joueur n'a plus de vie
        /// </summary>
        public Action OnDeath { get; set; }

        #endregion

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
        public int MiningQualityPercentage => Stats.MiningQualityPercentage;

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

        /// <summary>
        /// true si la santé du joueur a atteint un niveau critique
        /// </summary>
        public bool CriticalHealthReached { get; private set; }

        /// <summary>
        /// true si l'énergie du joueur a atteint un niveau critique
        /// </summary>
        public bool CriticalEnergyReached { get; private set; }

        /// <summary>
        /// true si la chaleur du joueur a atteint un niveau critique
        /// </summary>
        public bool CriticalHeatReached { get; private set; }

        /// <summary>
        /// true si le joueur n'a plus de vie
        /// </summary>
        public bool IsDead { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// La transform du joueur
        /// </summary>
        [SerializeField]
        private Transform _player;

        /// <summary>
        /// Le contrôleur du joueur
        /// </summary>
        [SerializeField]
        private PlayerController _controller;

        /// <summary>
        /// Dégâts infligés par surchauffe
        /// </summary>
        [SerializeField]
        private Vector2 _minMaxOverheatDmg = new(.05f, .1f);

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

        /// <summary>
        /// Le %age de santé/énergie restant à partir duquel les clignotants s'activent
        /// </summary>
        [SerializeField]
        private float _thresholdBeforeCriticalLevel = 20f;

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
        private int _lastHeatLevel;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            RestoreStats();
        }

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (IsDead)
            {
                return;
            }

            // Augmente la chaleur si le joueur creuse

            if (_controller.IsMining)
            {
                _curMiningHeatIncrease += Time.deltaTime * _miningHeatIncrease;
                LoseEnergy(Time.deltaTime * _miningEnergyConsumption);
            }
            else
            {
                _curMiningHeatIncrease -= Time.deltaTime * _miningHeatIncrease;
            }

            CurDepth = -Mathf.Min(0, Mathf.RoundToInt(_player.position.y / _spawnSpacing) - 1);
            _curMiningHeatIncrease = Mathf.Clamp(_curMiningHeatIncrease, 0f, MaxHeatThresholds[^1] - CurDepth * _heatIncreasePerDepth);
            CurHeat = CurDepth * _heatIncreasePerDepth + _curMiningHeatIncrease;

            // Indique si les jauges ont atteint un niveau critique

            bool previousCriticalHealthReached = CriticalHealthReached;
            bool previousCriticalEnergyReached = CriticalEnergyReached;
            bool previousCriticalHeatReached = CriticalHeatReached;

            CriticalHealthReached = CurHealth < MaxHealth * _thresholdBeforeCriticalLevel / 100f;
            CriticalEnergyReached = CurEnergy < MaxHealth * _thresholdBeforeCriticalLevel / 100f;
            CriticalHeatReached = CurHeat > Stats.MaxHeatThresholds[^2];

            if (CriticalHealthReached && !previousCriticalHealthReached)
            {
                OnCriticalHealthReached?.Invoke();
            }

            if (CriticalEnergyReached && !previousCriticalEnergyReached)
            {
                OnCriticalEnergyReached?.Invoke();
            }

            // Assigne le dernier palier de chaleur atteint
            // pour retirer les vêtements du joueur

            for (int i = 0; i < MaxHeatThresholds.Length - 1; ++i)
            {
                if (_lastHeatLevel < i && CurHeat > MaxHeatThresholds[i])
                {
                    ++_lastHeatLevel;
                    OnNewHeatThresholdReached?.Invoke(_lastHeatLevel);
                }
            }

            if (CriticalHeatReached)
            {
                // Réduit la santé si la chaleur est trop forte.
                // Cette réduction est plus forte lorsqu'on atteint le dernier palier

                float heatDmg = Mathf.Lerp(_minMaxOverheatDmg.x, _minMaxOverheatDmg.y, Mathf.InverseLerp(Stats.MaxHeatThresholds[^2], Stats.MaxHeatThresholds[^1], CurHeat));
                LoseHealth(Time.deltaTime * heatDmg);

                if (!previousCriticalHeatReached)
                {
                    OnCriticalHeatReached?.Invoke();
                }
            }
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
            _lastHeatLevel = -1;
            _curMiningHeatIncrease = 0f;
            CurDepth = 0;
            CriticalHealthReached = false;
            CriticalEnergyReached = false;
            CriticalHeatReached = false;
            IsDead = false;

            OnRestored?.Invoke();
        }

        /// <summary>
        /// Gagne un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant gagné</param>
        public void GainEXP(int amount)
        {
            CurEXPPoints += amount;
            OnCurEXPChanged?.Invoke(this, new EXPChangedEventArgs(CurEXPPoints, amount));
        }

        /// <summary>
        /// Perd un montant de points d'EXP
        /// </summary>
        /// <param name="amount">Le montant perdu</param>
        public void LoseEXP(int amount)
        {
            CurEXPPoints -= amount;
            OnCurEXPChanged?.Invoke(this, new EXPChangedEventArgs(CurEXPPoints, amount));
        }

        /// <summary>
        /// Restaure la santé du joueur
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void GainHealth(float amount)
        {
            CurHealth = Mathf.Min(MaxHealth, CurHealth + amount);
        }

        /// <summary>
        /// Inflige des dégâts au joueur
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void LoseHealth(float amount)
        {
            if (IsDead)
            {
                return;
            }

            CurHealth -= amount;

            if (CurHealth <= 0f)
            {
                CurHealth = 0f;
                IsDead = true;
                OnDeath?.Invoke();
            }
        }

        /// <summary>
        /// Restaure l'énergie du joueur
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void GainEnergy(float amount)
        {
            CurEnergy = Mathf.Min(MaxEnergy, CurEnergy + amount);
        }

        /// <summary>
        /// Fait perdre de l'énergie au joueur
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void LoseEnergy(float amount)
        {
            if (IsDead)
            {
                return;
            }

            CurEnergy -= amount;

            if (CurEnergy <= 0f)
            {
                CurEnergy = 0f;
                IsDead = true;
                OnDeath?.Invoke();
            }
        }

        #endregion
    }
}