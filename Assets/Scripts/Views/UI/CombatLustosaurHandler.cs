using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.UI
{
    /// <summary>
    /// Gère l'affichage des luxurosaures des combattants
    /// </summary>
    public class CombatLustosaurHandler : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le canvasGroup
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// L'image affichant le luxurosaure
        /// </summary>
        [SerializeField]
        private Image _lustosaurImg;

        /// <summary>
        /// L'image affichant la résistance
        /// </summary>
        [SerializeField]
        private Image _resistanceIcon;

        /// <summary>
        /// La barre de vie
        /// </summary>
        [SerializeField]
        private Image _healthBarFill;

        /// <summary>
        /// Sprite indiquant que le luxurosaure est vulnérable à l'attaque
        /// </summary>
        [SerializeField]
        private Sprite _vulnerableIcon;

        /// <summary>
        /// Sprite indiquant que le luxurosaure est neutre à l'attaque
        /// </summary>
        [SerializeField]
        private Sprite _neutralIcon;

        /// <summary>
        /// Sprite indiquant que le luxurosaure est résistant à l'attaque
        /// </summary>
        [SerializeField]
        private Sprite _resistantIcon;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _canvasGroupFadeDuration = 1f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _healthBarFillDuration = 1f;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne le sprite du luxurosaure
        /// </summary>
        /// <param name="lustosaurSprite">Le sprite du luxurosaure</param>
        public void SetLustosaurSprite(Sprite lustosaurSprite)
        {
            _lustosaurImg.sprite = lustosaurSprite;
        }

        /// <summary>
        /// Assigne la valeur de la barre de vie
        /// </summary>
        /// <param name="amount">Le montant de vie</param>
        public void SetHealthBarFillAmount(float amount)
        {
            _healthBarFill.DOFillAmount(amount, _healthBarFillDuration);
        }

        /// <summary>
        /// Assigne l'opacité
        /// </summary>
        /// <param name="amount">L'opacité</param>
        /// <param name="animate">true pour faire un tween sinon l'assigne directement</param>
        public void SetAlpha(float amount, bool animate)
        {
            if (animate)
            {
                _canvasGroup.DOFade(amount, _canvasGroupFadeDuration);
            }
            else
            {
                _canvasGroup.alpha = amount;
            }
        }

        /// <summary>
        /// Masque l'icône des résistances
        /// </summary>
        public void HideResistanceIcon()
        {
            _resistanceIcon.gameObject.SetActive(false);
        }

        /// <summary>
        /// Indique que le luxurosaure est résistant à l'attaque
        /// </summary>
        public void SetResistantIcon()
        {
            _resistanceIcon.gameObject.SetActive(_resistantIcon != null);
            _resistanceIcon.sprite = _resistantIcon;
        }

        /// <summary>
        /// Indique que le luxurosaure est neutre à l'attaque
        /// </summary>
        public void SetNeutralIcon()
        {
            _resistanceIcon.gameObject.SetActive(_neutralIcon != null);
            _resistanceIcon.sprite = _neutralIcon;
        }

        /// <summary>
        /// Indique que le luxurosaure est vulnérable à l'attaque
        /// </summary>
        public void SetVulnerableIcon()
        {
            _resistanceIcon.gameObject.SetActive(_vulnerableIcon != null);
            _resistanceIcon.sprite = _vulnerableIcon;
        }

        #endregion
    }
}