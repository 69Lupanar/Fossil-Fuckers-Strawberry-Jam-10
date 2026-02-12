using Assets.Scripts.Models;
using Assets.Scripts.Models.Dinos;
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
        /// Le SleepMenuView
        /// </summary>
        [SerializeField]
        private SleepMenuView _sleepView;

        /// <summary>
        /// Le SexMenuView
        /// </summary>
        [SerializeField]
        private SexMenuView _sexView;

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
        /// Le canvas de la scène adulte
        /// </summary>
        [SerializeField]
        private Canvas _sexMenuCanvas;

        /// <summary>
        /// Le canvas de la scène de combat
        /// </summary>
        [SerializeField]
        private Canvas _combatCanvas;

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
            _sexMenuCanvas.enabled = false;
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
        /// Ferma le menu de la base
        /// </summary>
        public void CloseBaseMenu()
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                CloseAll();
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
            _sleepView.OnSleepMenuOpen();
            _baseMenuBtnsParent.SetActive(false);
            _sleepMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        /// <param name="reasonForSex">La raison pour laqulle une scène de sexe doit se jouer</param>
        /// <param name="sexEnvironment">L'environnement où se déroule la scène adulte</param>
        /// <param name="selectedLustosaur">Le luxurosaure correspondant à la scène adulte</param>
        public void OpenSexMenu(ReasonForSex reasonForSex, SexEnvironment sexEnvironment, LustosaurSO selectedLustosaur)
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                _combatCanvas.enabled = false;
                _baseMenuCanvas.enabled = true;
                _sexMenuCanvas.enabled = true;
                _sexView.OnSexMenuOpen(reasonForSex, sexEnvironment, selectedLustosaur);
                _blackFadeImg.DOFade(0f, _fadeSpeed);
            });
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
            _sleepView.CleanupOnWindowClosed();
        }

        /// <summary>
        /// Ferme le menu complètement
        /// </summary>
        public void CloseAll()
        {
            _baseMenuCanvas.enabled = false;
            _upgradeMenuCanvas.enabled = false;
            _cloningMenuCanvas.enabled = false;
            _teamMenuCanvas.enabled = false;
            _sleepMenuCanvas.enabled = false;
            _sexMenuCanvas.enabled = false;

            _cloningView.CleanupOnWindowClosed();
            _sleepView.CleanupOnWindowClosed();
        }

        /// <summary>
        /// Termine la scène adulte en cours
        /// </summary>
        /// <param name="reasonForSex">La raison pour laqulle une scène de sexe doit se jouer</param>
        public void EndSexScene(ReasonForSex reasonForSex)
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                CloseAll();
                _gameManager.RestartLevel(reasonForSex != ReasonForSex.Gallery);

                _blackFadeImg.DOFade(0f, _fadeSpeed);
            });

        }

        #endregion
    }
}