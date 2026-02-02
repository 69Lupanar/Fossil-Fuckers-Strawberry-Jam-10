using Assets.Scripts.ViewModels.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Player
{
    /// <summary>
    /// Affiche les données relatives au joueur
    /// </summary>
    public class PlayerStatsView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le joueur
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// Le manager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _manager;

        /// <summary>
        /// Le label indiquant la profondeur du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _depthLabel;

        /// <summary>
        /// Icône de la jauge de santé
        /// </summary>
        [SerializeField]
        private Image _healthIcon;

        /// <summary>
        /// Icône de la jauge d'énergie
        /// </summary>
        [SerializeField]
        private Image _energyIcon;

        /// <summary>
        /// Icône de la jauge de chaleur
        /// </summary>
        [SerializeField]
        private Image _heatIcon;

        /// <summary>
        /// Aiguille de la jauge de santé
        /// </summary>
        [SerializeField]
        private Transform _healthPointer;

        /// <summary>
        /// Aiguille de la jauge d'énergie
        /// </summary>
        [SerializeField]
        private Transform _energyPointer;

        /// <summary>
        /// Aiguille de la jauge de chaleur
        /// </summary>
        [SerializeField]
        private Transform _heatPointer;

        /// <summary>
        /// Couleur normale des icônes
        /// </summary>
        [SerializeField]
        private Color _normalColor;

        /// <summary>
        /// Couleur d'alerte des icônes
        /// </summary>
        [SerializeField]
        private Color _alertColor;

        /// <summary>
        /// L'intervalle de rotation des jauges de santé et d'énergie
        /// </summary>
        [SerializeField]
        private Vector2 _minMaxSideGaugeRot;

        /// <summary>
        /// L'intervalle de rotation de la jauge de chaleur
        /// </summary>
        [SerializeField]
        private Vector2 _minMaxHeatGaugeRot;

        #endregion
    }
}