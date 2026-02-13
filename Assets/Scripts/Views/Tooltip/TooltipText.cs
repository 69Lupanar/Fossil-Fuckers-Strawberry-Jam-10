using TMPro;
using UnityEngine;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Tooltip contenant du texte
    /// </summary>
    public class TooltipText : TooltipBase
    {
        #region Variables Unity

        /// <summary>
        /// Le label du tooltip
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _label;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne le texte du tooltip
        /// </summary>
        public void SetText(string text)
        {
            _label.SetText(text);
        }

        #endregion
    }
}