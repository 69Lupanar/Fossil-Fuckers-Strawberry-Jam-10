using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Combat;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Extensions;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les combats
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        public Action OnCombatStarted { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// L'équipe du joueur
        /// </summary>
        public LustosaurSO[] PlayerTeam { get; private set; }

        /// <summary>
        /// L'équipe de l'adversaire
        /// </summary>
        public LustosaurSO[] EnemyTeam { get; private set; }

        /// <summary>
        /// Le total de PVs des luxurosaures du joueur
        /// </summary>
        public int PlayerTotalHP { get; private set; }

        /// <summary>
        /// Le total de PVs des luxurosaures du joueur
        /// </summary>
        public int EnemyTotalHP { get; private set; }

        /// <summary>
        /// Le total des stats de soutien apportées au luxurosaure du joueur
        /// </summary>
        public FightingStats PlayerTotalAppliedSupportStats { get; private set; } = FightingStats.Zero;

        /// <summary>
        /// Le total des stats de soutien apportées au luxurosaure de l'ennemi
        /// </summary>
        public FightingStats EnemyTotalAppliedSupportStats { get; private set; } = FightingStats.Zero;

        /// <summary>
        /// true si le joueur a l'initiative (ses luxurosaures ont moins de PV que ceux de l'ennemi)
        /// </summary>
        public bool PlayerHasInitiative => PlayerTotalHP < EnemyTotalHP;

        /// <summary>
        /// Le dernier palier de chaleur qu'à atteint le joueur.
        /// Indique si ses luxurosaures doivent être excités ou non.
        /// </summary>
        public int PlayerHeatLevel => _playerStatsManager.LastHeatLevel;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu des vêtements
        /// </summary>
        public FightingStats HornyBonusStats => _hornyBonusStats;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu tous ses vêtements
        /// </summary>
        public FightingStats VeryHornyBonusStats => _veryHornyBonusStats;

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsManager;

        /// <summary>
        /// Le gain de PP au début de chaque tour
        /// </summary>
        [SerializeField]
        private int _FPIncreaseOnTurnStart = 50;

        /// <summary>
        /// Le gain de PP à la perte d'un allié
        /// </summary>
        [SerializeField]
        private int _FPIncreaseOnAllyDeath = 100;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu des vêtements
        /// </summary>
        [SerializeField]
        private FightingStats _hornyBonusStats;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu tous ses vêtements
        /// </summary>
        [SerializeField]
        private FightingStats _veryHornyBonusStats;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Lance un combat avec l'équipe adverse renseignée
        /// </summary>
        /// <param name="enemyTeam">L'équipe ennemie</param>
        public void StartCombat(LustosaurSO[] enemyTeam)
        {
            // On copie les luxurosaures au lieu de directement les référencer.
            // Ca nous permet de modifier leurs stats sans avoir à toucher aux originaux.

            PlayerTeam = new LustosaurSO[CombatConstants.NB_PARTICIPATING_LUSTOSAURS];
            EnemyTeam = new LustosaurSO[CombatConstants.NB_PARTICIPATING_LUSTOSAURS];

            for (int i = 0; i < Mathf.Min(CombatConstants.NB_PARTICIPATING_LUSTOSAURS, _teamMenuManager.PlayerTeam.Count); ++i)
            {
                PlayerTeam[i] = _teamMenuManager.PlayerTeam[i].Clone();
            }

            // Pour l'ennemi, on les mélange avant de les cloner.
            // Ca nous permet d'avoir une chance d'utiliser les luxurosaures
            // en réserve

            List<LustosaurSO> shuffled = new();

            for (int i = 0; i < enemyTeam.Length; ++i)
            {
                if (enemyTeam[i] != null)
                {
                    shuffled.Add(enemyTeam[i]);
                }
            }

            shuffled.Shuffle();

            for (int i = 0; i < Mathf.Min(CombatConstants.NB_PARTICIPATING_LUSTOSAURS, shuffled.Count); ++i)
            {
                EnemyTeam[i] = shuffled[i].Clone();
            }

            OnCombatStarted?.Invoke();
        }

        /// <summary>
        /// Calcule l'initiative des combattants
        /// </summary>
        public void CalculateInitiative()
        {
            PlayerTotalHP = EnemyTotalHP = 0;

            // On ne prend que les 3 premiers dinos

            for (int i = 0; i < CombatConstants.NB_PARTICIPATING_LUSTOSAURS; ++i)
            {
                if (PlayerTeam[i] != null)
                {
                    PlayerTotalHP += PlayerTeam[i].CurFightingStats.Health;
                }

                if (EnemyTeam[i] != null)
                {
                    EnemyTotalHP += EnemyTeam[i].CurFightingStats.Health;
                }
            }
        }

        /// <summary>
        /// Modifie les stats de soutien
        /// </summary>
        /// <param name="bonusStats">Les stats bonus</param>
        public void ApplySupportStatChange(FightingStats bonusStats)
        {
            PlayerTotalAppliedSupportStats += bonusStats;
        }

        /// <summary>
        /// Commence un nouveau tour
        /// </summary>
        public void StartNewTurn()
        {

        }

        #endregion
    }
}