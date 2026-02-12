using Assets.Scripts.Models;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.ViewModels.Player;
using Assets.Scripts.Views.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Game
{
    /// <summary>
    /// Gère la vue ppale du jeu
    /// </summary>
    public class GameManagerView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le BaseMenuView
        /// </summary>
        [SerializeField]
        private BaseMenuView _baseMenuView;

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// Le GameManager
        /// </summary>
        [SerializeField]
        private GameManager _manager;

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _statsManager;

        /// <summary>
        /// Le Collider2DTrigger du joueur
        /// </summary>
        [SerializeField]
        private Collider2DTrigger _playerTrigger;

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

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _playerTrigger.OnTriggerEnter += OnBaseEntered;
            _statsManager.OnDeath += OnDeath;
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _blackFadeImg.gameObject.SetActive(true);
            _blackFadeImg.DOFade(0f, _fadeSpeed);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand le joueur entre dans la base
        /// </summary>
        /// <param name="obj">Objet détecté</param>
        private void OnBaseEntered(GameObject obj)
        {
            if (obj.CompareTag("Base"))
            {
                _manager.DisableController();

                _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
                {
                    _baseMenuView.OpenBaseMenu();
                    _blackFadeImg.DOFade(0f, _fadeSpeed);
                });
            }
        }

        /// <summary>
        /// Appelée quand le joueur perd
        /// </summary>
        private void OnDeath()
        {
            _manager.DisableController();

            // On a une chance sur deux de lancer une scène de sexe avec l'un de nos luxurosaures
            // si le perso s'évanouit

            float rand = Random.Range(0f, 100f);

            if (rand > 50f)
            {
                LustosaurSO randomLustosaur = _teamMenuManager.GetRandomActiveLustosaur();
                _baseMenuView.OpenSexMenu(ReasonForSex.StatDepleted, SexEnvironment.Mine, randomLustosaur);
            }
            else
            {
                _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
                {
                    _manager.ResetPlayer();
                    _blackFadeImg.DOFade(0f, _fadeSpeed);
                });
            }
        }

        #endregion
    }
}