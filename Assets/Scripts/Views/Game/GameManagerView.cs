using System;
using System.Collections;
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

        [Header("General")]
        [Space(10)]

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
        /// Le canvas du menu ppal
        /// </summary>
        [SerializeField]
        private Canvas _mainMenuCanvas;

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
        [Header("Audio")]
        [Space(10)]

        /// <summary>
        /// L'AudioManager
        /// </summary>
        [SerializeField]
        private AudioManager _audioManager;

        /// <summary>
        /// La musique de l'intro
        /// </summary>
        [SerializeField]
        private AudioClip _introBGM;

        /// <summary>
        /// Les musiques pouvant être jouées durant la partie
        /// </summary>
        [SerializeField]
        private AudioClip[] _mineBgms;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// La musique en cours pendant la phase d'exploration
        /// </summary>
        private AudioClip _curBGM;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _manager.OnCombatStarted += OnCombatStarted;
            _playerTrigger.OnTriggerEnter += OnBaseEntered;
            _statsManager.OnDeath += OnDeath;
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _manager.DisableController();

            _mainMenuCanvas.enabled = true;
            _blackFadeImg.gameObject.SetActive(true);
            _blackFadeImg.DOFade(0f, _fadeSpeed);
            _audioManager.Play(_introBGM, _fadeSpeed);
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée par le bouton Play
        /// </summary>
        public void OnPlayBtn()
        {
            _audioManager.Stop(_introBGM, _fadeSpeed);
            _blackFadeImg.DOFade(_fadeSpeed, _fadeSpeed).OnComplete(() =>
            {
                _manager.StartCoroutine(WaitCo(_fadeSpeed, () =>
                {
                    ReturnToGame();

                    _mainMenuCanvas.enabled = false;
                    _blackFadeImg.DOFade(0f, _fadeSpeed);
                }));
            });
        }

        /// <summary>
        /// Retourne à la scène ppale
        /// </summary>
        /// <param name="respawnPlayer">true si on doit replacer le joueur</param>
        public void ReturnToGame(bool respawnPlayer = false)
        {
            if (respawnPlayer)
            {
                _manager.RespawnPlayer();
            }

            _manager.EnableController();

            _blackFadeImg.DOFade(0f, _fadeSpeed);
            _curBGM = _mineBgms[UnityEngine.Random.Range(0, _mineBgms.Length)];
            _audioManager.Play(_curBGM, _fadeSpeed);
        }

        /// <summary>
        /// Ramène le joueur au point de départ,
        /// vide son inventaire
        /// et régénère la mine
        /// </summary>
        /// <param name="loseAllEXP">true si l'exp du joueur doit retomber à 0</param>
        public void RestartLevel(/*bool loseAllEXP*/)
        {
            _manager.RestartLevel(/*loseAllEXP*/);

            _blackFadeImg.DOFade(0f, _fadeSpeed);
            _curBGM = _mineBgms[UnityEngine.Random.Range(0, _mineBgms.Length)];
            _audioManager.Play(_curBGM, _fadeSpeed);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand un combat commence
        /// </summary>
        private void OnCombatStarted()
        {
            _audioManager.Stop(_curBGM);
        }

        /// <summary>
        /// Appelée quand le joueur entre dans la base
        /// </summary>
        /// <param name="obj">Objet détecté</param>
        private void OnBaseEntered(GameObject obj)
        {
            if (obj.CompareTag("Base"))
            {
                _manager.DisableController();
                _audioManager.Stop(_curBGM, _fadeSpeed);

                _blackFadeImg.DOFade(_fadeSpeed, _fadeSpeed).OnComplete(() =>
                {
                    _manager.StartCoroutine(WaitCo(_fadeSpeed, () =>
                    {
                        _baseMenuView.OpenBaseMenu();
                        _blackFadeImg.DOFade(0f, _fadeSpeed);
                    }));
                });
            }
        }

        /// <summary>
        /// Appelée quand le joueur perd
        /// </summary>
        private void OnDeath()
        {
            _manager.DisableController();
            _audioManager.Stop(_curBGM, _fadeSpeed);

            // On a une chance sur deux de lancer une scène de sexe avec l'un de nos luxurosaures
            // si le perso s'évanouit

            float rand = UnityEngine.Random.Range(0f, 100f);

            if (rand > 50f)
            {
                LustosaurSO randomLustosaur = _teamMenuManager.GetRandomActiveLustosaur();
                _baseMenuView.OpenSexMenu(ReasonForSex.StatDepleted, SexEnvironment.Mine, randomLustosaur);
            }
            else
            {
                _blackFadeImg.DOFade(_fadeSpeed, _fadeSpeed).OnComplete(() =>
                {
                    _manager.StartCoroutine(WaitCo(_fadeSpeed, () =>
                    {
                        _manager.ResetPlayer();

                        _curBGM = _mineBgms[UnityEngine.Random.Range(0, _mineBgms.Length)];
                        _audioManager.Play(_curBGM, _fadeSpeed);
                        _blackFadeImg.DOFade(0f, _fadeSpeed);
                    }));
                });
            }
        }

        /// <summary>
        /// Délai
        /// </summary>
        /// <param name="duration">délai</param>
        private IEnumerator WaitCo(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }

        #endregion
    }
}