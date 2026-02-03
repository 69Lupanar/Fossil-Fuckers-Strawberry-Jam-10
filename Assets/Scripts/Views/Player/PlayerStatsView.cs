using Assets.Scripts.ViewModels.Player;
using DG.Tweening;
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

        /// <summary>
        /// Vitesse de clignotement des icônes
        /// </summary>
        [SerializeField]
        private float _iconFlickeringSpeed = 1f;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            InitMeters();
        }

        private void Update()
        {
            UpdateDepthMeter();
            UpdateHeatMeter();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Initialise les jauges
        /// </summary>
        private void InitMeters()
        {
            _depthLabel.SetText("0");
            _heatPointer.eulerAngles = new Vector3(0f, 0f, _minMaxHeatGaugeRot.x);
            _healthPointer.localEulerAngles = new Vector3(0f, 0f, _minMaxSideGaugeRot.x);
            _energyPointer.localEulerAngles = new Vector3(0f, 0f, _minMaxSideGaugeRot.x);
        }

        /// <summary>
        /// Màj la jauge de profondeur
        /// </summary>
        private void UpdateDepthMeter()
        {
            _depthLabel.SetText(_manager.CurDepth.ToString());
        }

        /// <summary>
        /// Màj la jauge de chaleur
        /// </summary>
        private void UpdateHeatMeter()
        {
            Vector3 euler = _healthPointer.eulerAngles;
            euler.z = Mathf.Lerp(_minMaxHeatGaugeRot.x, _minMaxHeatGaugeRot.y, _manager.CurHeat / _manager.Stats.MaxHeatThresholds[^1]);
            _heatPointer.DORotate(euler, .5f);

            _heatIcon.color = _manager.CurHeat > _manager.Stats.MaxHeatThresholds[^2] ?
                                Color.Lerp(_normalColor, _alertColor, Mathf.PingPong(Time.time * _iconFlickeringSpeed, 1f)) :
                                _normalColor;
        }

        #endregion
    }
}