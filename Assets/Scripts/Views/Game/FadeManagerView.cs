using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Game
{
    /// <summary>
    /// Gère le fondu en noir
    /// </summary>
    public class FadeManagerView : MonoBehaviour
    {
        #region Variables Unity

        [Header("General")]
        [Space(10)]

        /// <summary>
        /// Fondu en noir
        /// </summary>
        [SerializeField]
        private Image _blackFadeImg;

        /// <summary>
        /// Vitesse du fondu en noir
        /// </summary>
        [SerializeField]
        private float _fadeSpeed = 1f;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            FadeToScene();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Fondu vers le noir
        /// </summary>
        /// <param name="fadeSpeed">Si non null, remplace la vitesse d'anim par défaut</param>
        /// <param name="onComplete"></param>
        public void FadeToBlack(float? fadeSpeed = null, Action onComplete = null)
        {
            float speed = fadeSpeed ?? _fadeSpeed;
            _blackFadeImg.gameObject.SetActive(true);

            _blackFadeImg.DOFade(1f, speed).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// Fondu vers le noir
        /// </summary>
        /// <param name="fadeSpeed">Si non null, remplace la vitesse d'anim par défaut</param>
        /// <param name="onComplete"></param>
        public void FadeToScene(float? fadeSpeed = null, Action onComplete = null)
        {
            float speed = fadeSpeed ?? _fadeSpeed;
            _blackFadeImg.gameObject.SetActive(true);

            _blackFadeImg.DOFade(0f, speed).OnComplete(() =>
            {
                onComplete?.Invoke();
                _blackFadeImg.gameObject.SetActive(false);
            });
        }

        #endregion
    }
}