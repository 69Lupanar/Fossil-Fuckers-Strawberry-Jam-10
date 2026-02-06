using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère l'équipe du luxurosaures du joueur
    /// </summary>
    public class TeamMenuManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un Luxurosaure est créé
        /// </summary>
        public Action<LustosaurSO> OnLustosaurCreated { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// La réserve de luxurosaures
        /// </summary>
        public List<LustosaurSO> StandbyReserve { get; private set; }

        /// <summary>
        /// L'équipe du joueur
        /// </summary>
        public LustosaurSO[] Team { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// La capacité max de la réserve de luxurosaures
        /// </summary>
        [SerializeField]
        private int _teamStandbyCapacity = 20;

        /// <summary>
        /// La capacité max de l'équipe du joueur
        /// </summary>
        [SerializeField]
        private int _playerTeamCapacity = 5;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            StandbyReserve = new List<LustosaurSO>(_teamStandbyCapacity);
            Team = new LustosaurSO[_playerTeamCapacity];
        }

        #endregion
        #region Méthodes publiques

        /// <summary>
        /// Ajoute un nouveau luxurosaure à l'équipe.
        /// Si l'équipe est pleine, il sera envoyé à la réserve.
        /// Si la réserve est pleine, il sera défaussé.
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure à ajouter</param>
        public void AddLustosaur(LustosaurSO lustosaur)
        {
            OnLustosaurCreated?.Invoke(lustosaur);
        }

        #endregion
    }
}