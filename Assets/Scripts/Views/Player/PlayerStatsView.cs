using Assets.Scripts.Models.EventArgs;
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
        /// Le sprite du joueur par défaut
        /// </summary>
        [SerializeField]
        private Sprite _playerDefaultSprite;

        /// <summary>
        /// Les sprites du joueur pour chaque palier de chaleur
        /// </summary>
        [SerializeField]
        private Sprite[] _playerSpritePerHeatThreshold;

        /// <summary>
        /// Le SpriteRenderer du joueur
        /// </summary>
        [SerializeField]
        private SpriteRenderer _playerSpriteRenderer;

        /// <summary>
        /// Le label indiquant la profondeur du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _depthLabel;

        /// <summary>
        /// Le label indiquant l'exp du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _expLabel;

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
            _manager.OnCurEXPChanged += OnCurEXPChanged;
            _manager.OnNewHeatThresholdReached += OnNewHeatThresholdReached;
            _manager.OnRestored += OnRestored;
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Start()
        {
            InitMeters();
        }

        private void Update()
        {
            UpdateDepthMeter();
            UpdateHeatMeter();
            UpdateHealthMeter();
            UpdateEnergyMeter();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand le joueur gagne ou perd de l'exp
        /// </summary>
        /// <param name="sender">Le joueur</param>
        /// <param name="e">Les infos sur l'événement</param>
        private void OnCurEXPChanged(object sender, EXPChangedEventArgs e)
        {
            _expLabel.SetText(e.CurEXP.ToString());
        }

        /// <summary>
        /// Appelée quand le joueur atteint un nouveau palier de chaleur
        /// </summary>
        /// <param name="newThreshold">Le nouveau palier</param>
        private void OnNewHeatThresholdReached(int newThreshold)
        {
            UpdatePlayerSprite(newThreshold);
        }

        /// <summary>
        /// Appelée quand les stats du joueur sont restaurées
        /// </summary>
        private void OnRestored()
        {
            _playerSpriteRenderer.sprite = _playerDefaultSprite;
        }

        /// <summary>
        /// Initialise les jauges
        /// </summary>
        private void InitMeters()
        {
            _expLabel.SetText(_manager.CurEXPPoints.ToString());
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
            Vector3 euler = _heatPointer.eulerAngles;
            euler.z = Mathf.Lerp(_minMaxHeatGaugeRot.x, _minMaxHeatGaugeRot.y, _manager.CurHeat / _manager.Stats.MaxHeatThresholds[^1]);
            _heatPointer.DORotate(euler, .5f);

            _heatIcon.color = _manager.CurHeat > _manager.Stats.MaxHeatThresholds[^2] ?
                                Color.Lerp(_normalColor, _alertColor, Mathf.PingPong(Time.time * _iconFlickeringSpeed, 1f)) :
                                _normalColor;
        }

        /// <summary>
        /// Màj la jauge de santé
        /// </summary>
        private void UpdateHealthMeter()
        {
            Vector3 euler = _healthPointer.localEulerAngles;
            euler.z = Mathf.Lerp(_minMaxSideGaugeRot.y, _minMaxSideGaugeRot.x, _manager.CurHealth / _manager.Stats.MaxHealth);
            _healthPointer.DOLocalRotate(euler, .5f);

            _healthIcon.color = _manager.CriticalHealthReached ?
                                Color.Lerp(_normalColor, _alertColor, Mathf.PingPong(Time.time * _iconFlickeringSpeed, 1f)) :
                                _normalColor;
        }

        /// <summary>
        /// Màj la jauge d'énergie
        /// </summary>
        private void UpdateEnergyMeter()
        {
            Vector3 euler = _energyPointer.localEulerAngles;
            euler.z = Mathf.Lerp(_minMaxSideGaugeRot.y, _minMaxSideGaugeRot.x, _manager.CurEnergy / _manager.Stats.MaxEnergy);
            _energyPointer.DOLocalRotate(euler, .5f);

            _energyIcon.color = _manager.CriticalEnergyReached ?
                                Color.Lerp(_normalColor, _alertColor, Mathf.PingPong(Time.time * _iconFlickeringSpeed, 1f)) :
                                _normalColor;
        }

        /// <summary>
        /// Màj le sprite du joueur
        /// </summary>
        /// <param name="newThreshold">Le nouveau palier</param>
        private void UpdatePlayerSprite(int newThreshold)
        {
            if (newThreshold < _playerSpritePerHeatThreshold.Length)
            {
                _playerSpriteRenderer.sprite = _playerSpritePerHeatThreshold[newThreshold];
            }
        }

        #endregion
    }
}