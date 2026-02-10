using Assets.Scripts.ViewModels.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Combat
{
    /// <summary>
    /// Gère l'affichage des combats
    /// </summary>
    public class CombatView : MonoBehaviour
    {
        #region Variables Unity

        [Header("General")]
        [Space(10)]

        /// <summary>
        /// Le GameManagerView
        /// </summary>
        [SerializeField]
        private GameManager _gameManager;

        /// <summary>
        /// Le CombatManager
        /// </summary>
        [SerializeField]
        private CombatManager _manager;

        /// <summary>
        /// Le canvas de l'environnement combat
        /// </summary>
        [SerializeField]
        private Canvas _combatCanvas;

        /// <summary>
        /// Le canvas de l'intro
        /// </summary>
        [SerializeField]
        private Canvas _introCanvas;

        /// <summary>
        /// Le canvas du terrain
        /// </summary>
        [SerializeField]
        private Canvas _terrainCanvas;

        /// <summary>
        /// Le canvas du menu des actions
        /// </summary>
        [SerializeField]
        private Canvas _actionMenuCanvas;

        /// <summary>
        /// Le canvas du menu des attaques
        /// </summary>
        [SerializeField]
        private Canvas _attackListCanvas;

        /// <summary>
        /// Le canvas de comparaison des PVs des équipes
        /// </summary>
        [SerializeField]
        private Canvas _healthComparisonCanvas;

        /// <summary>
        /// Le canvas des instructions
        /// </summary>
        [SerializeField]
        private Canvas _instructionsCanvas;

        /// <summary>
        /// Le canvas de victoire
        /// </summary>
        [SerializeField]
        private Canvas _victoryCanvas;

        /// <summary>
        /// Le canvas de défaite
        /// </summary>
        [SerializeField]
        private Canvas _defeatCanvas;

        /// <summary>
        /// Fondu en noir
        /// </summary>
        [SerializeField]
        private Image _blackFadeImg;

        /// <summary>
        /// Vitesse du fondu en noir
        /// </summary>
        [SerializeField]
        private float _fadeSpeed = .5f;

        [Space(10)]
        [Header("Intro")]
        [Space(10)]

        /// <summary>
        /// L'image de haut de l'intro
        /// </summary>
        [SerializeField]
        private RectTransform _slideUp;

        /// <summary>
        /// L'image du bas de l'intro
        /// </summary>
        [SerializeField]
        private RectTransform _slideDown;

        /// <summary>
        /// Fondu en blanc
        /// </summary>
        [SerializeField]
        private Image _whiteFadeImg;

        /// <summary>
        /// Logo de combat
        /// </summary>
        [SerializeField]
        private CanvasGroup _logo;

        [Space(10)]
        [Header("Terrain")]
        [Space(10)]

        /// <summary>
        /// Terrain du joueur
        /// </summary>
        [SerializeField]
        private RectTransform _playerTerrain;

        /// <summary>
        /// Terrain ennemi
        /// </summary>
        [SerializeField]
        private RectTransform _enemyTerrain;

        /// <summary>
        /// Panel des messages
        /// </summary>
        [SerializeField]
        private RectTransform _messagePanel;

        /// <summary>
        /// Compteurs de PPs du joueur
        /// </summary>
        [SerializeField]
        private RectTransform _playerFPCounter;

        /// <summary>
        /// Compteurs de PPs de l'ennemi
        /// </summary>
        [SerializeField]
        private RectTransform _enemyFPCounter;

        /// <summary>
        /// Icône à afficher avec le message
        /// </summary>
        [SerializeField]
        private Image _messageIcon;

        /// <summary>
        /// Label du message à afficher
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _messageLabel;

        /// <summary>
        /// Label du compteur de PPs du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _playerFPLabel;

        /// <summary>
        /// Label du compteur de PPs de l'ennemi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _enemyFPLabel;

        /// <summary>
        /// Zones de combat du joueur
        /// </summary>
        [SerializeField]
        private RectTransform[] _playerZones;

        /// <summary>
        /// Zones de combat de l'ennemi
        /// </summary>
        [SerializeField]
        private RectTransform[] _enemyZones;

        /// <summary>
        /// Icônes des luxurosaures du joueur
        /// </summary>
        [SerializeField]
        private Image[] _playerLustosaursImgs;

        /// <summary>
        /// Icônes des luxurosaures de l'ennemi
        /// </summary>
        [SerializeField]
        private Image[] _enemyLustosaursImgs;

        [Space(10)]
        [Header("Attacks")]
        [Space(10)]

        /// <summary>
        /// Préfab d'une attaque dans la liste déroulante
        /// </summary>
        [SerializeField]
        private GameObject _attackSlotPrefab;

        /// <summary>
        /// Conteneur des attaques dans la liste déroulante
        /// </summary>
        [SerializeField]
        private Transform _attackSlotsParent;

        /// <summary>
        /// Conteneur des attaques inactives dans la liste déroulante
        /// </summary>
        [SerializeField]
        private Transform _inactiveAttackSlotsParent;

        [Space(10)]
        [Header("Health Comparison")]
        [Space(10)]

        /// <summary>
        /// Comparateur du joueur
        /// </summary>
        [SerializeField]
        private RectTransform _playerComparer;

        /// <summary>
        /// Comparateur de l'ennemi
        /// </summary>
        [SerializeField]
        private RectTransform _enemyComparer;

        /// <summary>
        /// Label du total du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _playerTotalHPLabel;

        /// <summary>
        /// Label du total de l'ennemi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _enemyTotalHPLabel;

        /// <summary>
        /// Contenu affiché si le joueur parvient à s'échapper
        /// </summary>
        [SerializeField]
        private CanvasGroup _escapedContent;

        /// <summary>
        /// Contenu affiché si le joueur échoue à s'échapper
        /// </summary>
        [SerializeField]
        private CanvasGroup _escapeFailedContent;

        /// <summary>
        /// Contenu affiché si le joueur a l'initiative
        /// </summary>
        [SerializeField]
        private CanvasGroup _playerInitiativeContent;

        /// <summary>
        /// Contenu affiché si l'ennemi a l'initiative
        /// </summary>
        [SerializeField]
        private CanvasGroup _enemyInitiativeContent;

        [Space(10)]
        [Header("Instructions")]
        [Space(10)]

        /// <summary>
        /// Flèche indiquant le luxurosaure ciblé
        /// </summary>
        [SerializeField]
        private RectTransform _arrowTarget;

        /// <summary>
        /// Décalage de la flèche par rapport luxurosaure ciblé
        /// </summary>
        [SerializeField]
        private Vector3 _arrowTargetOffset;

        /// <summary>
        /// Panel des instructions
        /// </summary>
        [SerializeField]
        private GameObject _instructionsPanel;

        /// <summary>
        /// Les labels de chaque instruction
        /// </summary>
        [SerializeField]
        private GameObject[] _instructionsLabels;

        [Space(10)]
        [Header("Victory & Defeat")]
        [Space(10)]

        /// <summary>
        /// Label de l'EXP gagnée après le combat
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _victoryExpGainedLabel;

        /// <summary>
        /// Label de l'EXP gagnée après le combat
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _defeatExpGainedLabel;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _manager.OnCombatStarted += OnCombatStarted;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        private void OnCombatStarted()
        {

        }

        /// <summary>
        /// Quitte l'écran de combat
        /// </summary>
        private void QuitCombatScreen()
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                _combatCanvas.enabled = false;
                _gameManager.OnQuitCombatScreen();
                _blackFadeImg.DOFade(0f, _fadeSpeed);
            });
        }

        #endregion
    }
}