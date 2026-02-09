using System;
using Assets.Scripts.Models.Dinos;
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

        #region Variables Unity

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        #endregion

        #region Propriétés

        /// <summary>
        /// L'équipe du joueur
        /// </summary>
        private LustosaurSO[] PlayerTeam;

        /// <summary>
        /// L'équipe de l'adversaire
        /// </summary>
        private LustosaurSO[] EnemyTeam;

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

            PlayerTeam = new LustosaurSO[_teamMenuManager.PlayerTeamCapacity];
            EnemyTeam = new LustosaurSO[_teamMenuManager.PlayerTeamCapacity];

            for (int i = 0; i < _teamMenuManager.PlayerTeam.Count; ++i)
            {
                PlayerTeam[i] = _teamMenuManager.PlayerTeam[i].Clone();
            }
            for (int i = 0; i < enemyTeam.Length; ++i)
            {
                EnemyTeam[i] = enemyTeam[i].Clone();
            }

            OnCombatStarted?.Invoke();
        }

        #endregion
    }
}