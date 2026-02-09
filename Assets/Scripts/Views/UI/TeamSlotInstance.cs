using Assets.Scripts.Models.Dinos;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.UI
{
    /// <summary>
    /// Représente les emplacements d'équipe en haut de l'écran
    /// </summary>
    public class TeamSlotInstance : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// L'icône affichant le luxurosaure associé
        /// </summary>
        [SerializeField]
        private Image _lustosaurImg;

        /// <summary>
        /// La barre d'expérience
        /// </summary>
        [SerializeField]
        private Image _expBarFill;

        /// <summary>
        /// Le dégradé radial activé lors d'une montée de niveau
        /// </summary>
        [SerializeField]
        private Image _radialGradient;

        /// <summary>
        /// Vitesse d'anim
        /// </summary>
        [SerializeField]
        private Vector3 _radialGradientLevelupScale;

        /// <summary>
        /// Vitesse d'anim
        /// </summary>
        [SerializeField]
        private float _radialGradientAnimSpeed = .5f;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le luxurosaure associé
        /// </summary>
        private LustosaurSO _lustosaur;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne le luxurosaure associé
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure</param>
        public void SetLustosaur(LustosaurSO lustosaur)
        {
            _lustosaur = lustosaur;
            _lustosaurImg.enabled = lustosaur != null;

            if (lustosaur != null)
            {
                _lustosaurImg.sprite = lustosaur.NormalSprite;
                SetExpFillAmount((float)lustosaur.CurEXP / (float)lustosaur.ExpUntilNextLevel);
            }
            else
            {
                SetExpFillAmount(0f);
            }

        }

        /// <summary>
        /// Fait monter la jauge d'exp
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void SetExpFillAmount(float amount)
        {
            _expBarFill.fillAmount = amount;
        }

        /// <summary>
        /// Joue l'anim de montée de niveau
        /// </summary>
        public void PlayLevelUpAnimation()
        {
            _radialGradient.DOFade(1f, 0f);
            _radialGradient.transform.localScale = Vector3.one;
            _radialGradient.DOFade(0f, _radialGradientAnimSpeed);
            _radialGradient.transform.DOScale(_radialGradientLevelupScale, _radialGradientAnimSpeed);
        }

        #endregion
    }
}