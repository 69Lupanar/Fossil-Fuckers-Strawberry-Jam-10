using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Base de la logique pour tous types de tooltips
    /// </summary>
    [ExecuteInEditMode]
    public abstract class TooltipBase : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// La Transform du tooltip
        /// </summary>
        [SerializeField]
        protected RectTransform _rt;

        /// <summary>
        /// La Transform du canvas parent
        /// </summary>
        [SerializeField]
        protected RectTransform _canvasT;

        /// <summary>
        /// La Transform du Content
        /// </summary>
        [SerializeField]
        protected RectTransform _contentT;

        /// <summary>
        /// Restreint la largeur du tooltip à une valeur max
        /// </summary>
        [SerializeField]
        protected LayoutElement _layoutElement;

        /// <summary>
        /// Adapte la taille du conteneur à son contenu
        /// </summary>
        [SerializeField]
        protected ContentSizeFitter _contentSizeFitter;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Permet d'attendre une frame pour forcer la màj du rect
        /// </summary>
        private RectTransform _largestChildContent;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            UpdateReferences();
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            //Si notre texte dépasse la limite de caractères,
            //on active le LayoutElement pour limiter la largeur du tooltip

            _layoutElement.enabled = _largestChildContent.rect.width > _layoutElement.preferredWidth;
            _contentSizeFitter.enabled = !_layoutElement.enabled;

            SetPosition();
        }

        #endregion

        #region Méthodes protégées

        /// <summary>
        /// Màj les références.
        /// A appeler quand on change le contenu de Content dans l'Inspector
        /// </summary>
        [ContextMenu("Update References")]
        protected void UpdateReferences()
        {
            _largestChildContent = null;
            _largestChildContent = _contentT.GetComponentsInChildren<RectTransform>().Where(rt => rt != _contentT).OrderByDescending(rt => rt.rect.width).FirstOrDefault();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Place le Tooltip à la position du curseur
        /// en veillant à le garder dans les limites de l'écran
        /// </summary>
        private void SetPosition()
        {
            if (!Application.isPlaying)
                return;

            Vector2 anchoredPos = Input.mousePosition / _canvasT.localScale.x;

            //On le descend car son pivot inverse sa position en Y

            anchoredPos.y -= _canvasT.rect.height;

            //Confine le tooltip à l'écran

            anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0f, _canvasT.rect.width - _rt.rect.width);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, _rt.rect.height - _canvasT.rect.height, 0f);

            _rt.anchoredPosition = anchoredPos;
        }

        #endregion
    }
}