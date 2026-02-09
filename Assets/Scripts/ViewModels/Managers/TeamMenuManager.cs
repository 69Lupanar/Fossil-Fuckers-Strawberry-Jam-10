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
        /// Appelée dans la Start
        /// </summary>
        public Action OnStart { get; set; }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est ajouté à l'équipe
        /// </summary>
        public Action<LustosaurSO> OnCreatedLustosaurAddedToTeam { get; set; }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est ajouté à la reserve
        /// </summary>
        public Action<LustosaurSO> OnCreatedLustosaurAddedToReserve { get; set; }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est défaussé
        /// </summary>
        public Action<LustosaurSO> OnCreatedLustosaurDiscarded { get; set; }

        /// <summary>
        /// Appelée quand un Luxurosaure créé est défaussé
        /// </summary>
        public Action OnCancelTryRemoveLastLustosaurFromTeam { get; set; }

        /// <summary>
        /// Appelée quand un Luxurosaure est relâché de la réserve
        /// </summary>
        public Action<LustosaurSO> OnLustosaurDiscardedFromStandby { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// La réserve de luxurosaures
        /// </summary>
        public List<LustosaurSO> StandbyReserve { get; private set; }

        /// <summary>
        /// L'équipe du joueur
        /// </summary>
        public List<LustosaurSO> PlayerTeam { get; private set; }

        /// <summary>
        /// La capacité max de l'équipe du joueur
        /// </summary>
        public int PlayerTeamCapacity => _playerTeamCapacity;

        #endregion

        #region Variables Unity

        /// <summary>
        /// La capacité max de la réserve de luxurosaures
        /// </summary>
        [SerializeField]
        private int _standbyReserveCapacity = 20;

        /// <summary>
        /// La capacité max de l'équipe du joueur
        /// </summary>
        [SerializeField]
        private int _playerTeamCapacity = 5;

        /// <summary>
        /// L'équipe de départ du joueur (optionnel)
        /// </summary>
        [SerializeField]
        private LustosaurSO[] StartingTeam;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            StandbyReserve = new List<LustosaurSO>(_standbyReserveCapacity);
            PlayerTeam = new List<LustosaurSO>(_playerTeamCapacity);

            for (int i = 0; i < StartingTeam.Length; ++i)
            {
                PlayerTeam.Add(LustosaurSO.CreateFrom(StartingTeam[i], 100));
                StandbyReserve.Add(LustosaurSO.CreateFrom(StartingTeam[i], 100));
            }

            OnStart?.Invoke();
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
            if (PlayerTeam.Count < _playerTeamCapacity)
            {
                PlayerTeam.Add(lustosaur);
                OnCreatedLustosaurAddedToTeam?.Invoke(lustosaur);
            }
            else if (StandbyReserve.Count < _standbyReserveCapacity)
            {
                StandbyReserve.Add(lustosaur);
                OnCreatedLustosaurAddedToReserve?.Invoke(lustosaur);
            }
            else
            {
                OnCreatedLustosaurDiscarded?.Invoke(lustosaur);
            }
        }

        /// <summary>
        /// Annule la tentative de retirer le dernir luxurosaure de l'équipe
        /// </summary>
        public void CancelTryRemoveLastLustosaurFromTeam()
        {
            OnCancelTryRemoveLastLustosaurFromTeam?.Invoke();
        }

        /// <summary>
        /// Relâche un Luxurosaure de la réserve
        /// </summary>
        /// <param name="index">La position de l'objet dans la liste</param>
        public void DiscardLustosaur(int index)
        {
            LustosaurSO lustosaur = StandbyReserve[index];
            StandbyReserve.RemoveAt(index);
            OnLustosaurDiscardedFromStandby?.Invoke(lustosaur);
        }

        #endregion
    }
}