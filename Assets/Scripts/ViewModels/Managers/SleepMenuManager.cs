using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère la sélection du luxurosaure du joueur
    /// </summary>
    public class SleepMenuManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// Le luxurosaure sélectionné par le joueur
        /// </summary>
        public LustosaurSO SelectedLustosaur { get; set; }

        #endregion
    }
}