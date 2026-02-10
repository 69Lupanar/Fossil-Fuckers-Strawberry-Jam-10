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

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne la valeur de la stat
        /// </summary>
        /// <param name="value">La valeur</param>
        public void SetValue(int value)
        {
            bool show = _showIfStatZero || value != 0;

            _border.gameObject.SetActive(show);
            _label.gameObject.SetActive(show);

            _border.color = _icon.color = value > 0 ? _positiveColor :
                                          value < 0 ? _negativeColor :
                                          _neutralColor;

            _label.SetText($"{value}%");
        }

        #endregion
    }
}