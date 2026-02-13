using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Views.UI
{
    /// <summary>
    /// Gère les interactions avec la souris
    /// </summary>
    public class MouseHandler : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Appelée quand le curseur entre sur l'objet
        /// </summary>
        public UnityEvent OnMouseEnterEvent;

        /// <summary>
        /// Appelée quand le curseur quitte l'objet
        /// </summary>
        public UnityEvent OnMouseExitEvent;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Appelée quand le curseur entre sur l'objet
        /// </summary>
        private void OnMouseEnter()
        {
            OnMouseEnterEvent?.Invoke();
        }

        /// <summary>
        /// Appelée quand le curseur quitte l'objet
        /// </summary>
        private void OnMouseExit()
        {
            OnMouseExitEvent?.Invoke();
        }

        #endregion
    }
}