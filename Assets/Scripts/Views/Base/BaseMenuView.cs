using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'interface du mnu de la base
    /// </summary>
    public class BaseMenuView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le BaseMenuManager
        /// </summary>
        [SerializeField]
        private BaseMenuManager _manager;

        /// <summary>
        /// Le PlayerUpgradeView
        /// </summary>
        [SerializeField]
        private PlayerUpgradeView _upgradeView;

        /// <summary>
        /// Le CloningMenuView
        /// </summary>
        [SerializeField]
        private CloningMenuView _cloningView;

        /// <summary>
        /// Le TeamMenuView
        /// </summary>
        [SerializeField]
        private TeamMenuView _teamView;

        /// <summary>
        /// Le GameManager
        /// </summary>
        [SerializeField]
        private GameManager _gameManager;

        /// <summary>
        /// Le canvas racine du menu de la base
        /// </summary>
        [SerializeField]
        private Canvas _baseMenuCanvas;

        /// <summary>
        /// Le canvas du menu d'amélioration
        /// </summary>
        [SerializeField]
        private Canvas _upgradeMenuCanvas;

        /// <summary>
        /// Le canvas du menu de clonage
        /// </summary>
        [SerializeField]
        private Canvas _cloningMenuCanvas;

        /// <summary>
        /// Le canvas du menu de l'équipe
        /// </summary>
        [SerializeField]
        private Canvas _teamMenuCanvas;

        /// <summary>
        /// Le canvas du menu de la gallerie
        /// </summary>
        [SerializeField]
        private Canvas _sleepMenuCanvas;

        /// <summary>
        /// Le conteneur des btns du menu de la base
        /// </summary>
        [SerializeField]
        private GameObject _baseMenuBtnsParent;

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

        #endregion

        #region Variables d'instance

        /// <summary>
        /// L'ID de l'objet de l'inventaire survolé par le curseur
        /// </summary>
        private int _curHoveredItemID = -1;

        /// <summary>
        /// La position de l'objet de l'inventaire survolé par le curseur
        /// </summary>
        private Vector3 _curHoveredItemStartPos;

        /// <summary>
        /// Les objets dans chaque emplacement de fusion
        /// </summary>
        private LootSO[] _itemsInFusionSlots;

        /// <summary>
        /// La position de la souris à la frame précédente
        /// </summary>
        private Vector3 _previousMousePos;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _baseMenuCanvas.enabled = false;
            _upgradeMenuCanvas.enabled = false;
            _cloningMenuCanvas.enabled = false;
            _teamMenuCanvas.enabled = false;
            _sleepMenuCanvas.enabled = false;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ouvre le menu de la base
        /// </summary>
        public void OpenBaseMenu()
        {
            _baseMenuCanvas.enabled = true;
            _baseMenuBtnsParent.SetActive(true);
        }

        /// <summary>
        /// Ouvre le menu de la base
        /// </summary>
        public void CloseBaseMenu()
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                _baseMenuCanvas.enabled = false;
                _gameManager.RespawnPlayer();
                _gameManager.EnableController();
                _blackFadeImg.DOFade(0f, _fadeSpeed);
            });
        }

        /// <summary>
        /// Ouvre le menu d'amélioration
        /// </summary>
        public void OpenUpgradeMenu()
        {
            _upgradeView.OnUpgradeMenuOpen();
            _baseMenuBtnsParent.SetActive(false);
            _upgradeMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenCloningMenu()
        {
            _cloningView.OnCloningMenuOpen();
            _baseMenuBtnsParent.SetActive(false);
            _cloningMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenTeamMenu()
        {
            _teamView.OnTeamMenuOpen();
            _baseMenuBtnsParent.SetActive(false);
            _teamMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenSleepMenu()
        {
            _baseMenuBtnsParent.SetActive(false);
            _sleepMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void BackToBaseMenu()
        {
            _baseMenuBtnsParent.SetActive(true);
            _upgradeMenuCanvas.enabled = false;
            _cloningMenuCanvas.enabled = false;
            _teamMenuCanvas.enabled = false;
            _sleepMenuCanvas.enabled = false;

            _cloningView.CleanupOnWindowClosed();
        }

        #endregion
    }
}