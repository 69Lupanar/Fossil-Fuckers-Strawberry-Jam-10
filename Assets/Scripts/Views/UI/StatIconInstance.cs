using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.UI
{
    /// <summary>
    /// Icône + label représentant une stat d'un luxurosaure
    /// </summary>
    public class StatIconInstance : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// La bordure
        /// </summary>
        [SerializeField]
        private Image _border;

        /// <summary>
        /// L'icône
        /// </summary>
        [SerializeField]
        private Image _icon;

        /// <summary>
        /// Le label
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _label;

        /// <summary>
        /// true pour garder l'icône affichée si la stat est à 0%
        /// </summary>
        [SerializeField]
        private bool _showIfStatZero;

        /// <summary>
        /// Couleur si la stat > 0%
        /// </summary>
        [SerializeField]
        private Color _positiveColor;

        /// <summary>
        /// Couleur si la stat < 0%
        /// </summary>
        [SerializeField]
        private Color _negativeColor;

        /// <summary>
        /// Couleur si la stat = 0%
        /// </summary>
        [SerializeField]
        private Color _neutralColor;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _valueIncrementSpeed = .5f;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Value de fin d'anim au cas où on arrête la coroutine prématurément
        /// </summary>
        private int _endValue = 0;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne la valeur de la stat
        /// </summary>
        /// <param name="value">La valeur</param>
        /// <param name="animate">true pour incrémenter la valeur au fil du temps, sinon l'assigne directement</param>
        public void SetValue(int value, bool animate)
        {
            bool show = _showIfStatZero || value != 0;

            if (animate)
            {
                StopAllCoroutines();
                _label.SetText($"{_endValue}%");
                StartCoroutine(IncrementCurValueCo(value, show));
            }
            else
            {
                _border.gameObject.SetActive(show);
                _label.gameObject.SetActive(show);

                _border.color = _icon.color = value > 0 ? _positiveColor :
                                              value < 0 ? _negativeColor :
                                              _neutralColor;

                _label.SetText($"{value}%");
            }
        }

        /// <summary>
        /// Incrémente la valeur au fil du temps
        /// </summary>
        /// <param name="value">La nouvelle valeur</param>
        /// <param name="showAtEnd">true si les instances doivent être visibles à la fin de l'animation</param>
        private IEnumerator IncrementCurValueCo(int value, bool showAtEnd)
        {
            _endValue = value;
            int curValue = int.Parse(_label.text[..^1]);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * _valueIncrementSpeed;
                int val = Mathf.RoundToInt(Mathf.Lerp(curValue, value, t));
                _label.SetText($"{val}%");

                _border.color = _icon.color = val > 0 ? _positiveColor :
                                              val < 0 ? _negativeColor :
                                              _neutralColor;

                yield return null;
            }

            _border.color = _icon.color = value > 0 ? _positiveColor :
                                          value < 0 ? _negativeColor :
                                          _neutralColor;

            _label.SetText($"{value}%");
            _border.gameObject.SetActive(showAtEnd);
            _label.gameObject.SetActive(showAtEnd);

        }

        #endregion
    }
}