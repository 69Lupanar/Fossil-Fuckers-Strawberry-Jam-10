using System.Collections.Generic;
using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les actions du menu de la base
    /// </summary>
    public class BaseMenuManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// La réserve de luxurosaures
        /// </summary>
        public List<LustosaurSO> StandbyReserve { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// La capacité max de la réserve de luxurosaures
        /// </summary>
        [SerializeField]
        private int _teamStandbyCapacity = 20;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            StandbyReserve = new List<LustosaurSO>(_teamStandbyCapacity);
        }

        #endregion
    }
}