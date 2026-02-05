using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Objet appelant des callbacks en fonction des actions de la souris
    /// </summary>
    public class PointerCallbackReceiver : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
    {
        #region Evénements

        /// <summary>
        /// Appelée quand le curseur entre sur l'objet
        /// </summary>
        public Action<int, Vector3> OnPointerEnter { get; set; }

        /// <summary>
        /// Appelée quand le curseur clique sur l'objet
        /// </summary>
        public Action<PointerCallbackReceiver> OnPointerDown { get; set; }

        /// <summary>
        /// Appelée quand le curseur relâche l'objet
        /// </summary>
        public Action OnPointerUp { get; set; }

        /// <summary>
        /// Appelée quand le curseur déplace l'objet
        /// </summary>
        public Action<Vector2> OnPointerMove { get; set; }

        /// <summary>
        /// Appelée quand le curseur quitte l'objet
        /// </summary>
        public Action OnPointerExit { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// La position de l'objet dans la hiérachie de transforms
        /// </summary>       
        public int IndexInHierarchy { get; set; }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand le curseur entre sur l'objet
        /// </summary>
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnter?.Invoke(IndexInHierarchy, transform.position);
        }

        /// <summary>
        /// Appelée quand le curseur clique sur l'objet
        /// </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnPointerDown?.Invoke(this);
        }

        /// <summary>
        /// Appelée quand le curseur relâche l'objet
        /// </summary>
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnPointerUp?.Invoke();
        }

        /// <summary>
        /// Appelée quand le curseur déplace l'objet
        /// </summary>
        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            OnPointerMove?.Invoke(eventData.delta);
        }

        /// <summary>
        /// Appelée quand le curseur quitte l'objet
        /// </summary>
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            OnPointerExit?.Invoke();
        }

        #endregion
    }
}