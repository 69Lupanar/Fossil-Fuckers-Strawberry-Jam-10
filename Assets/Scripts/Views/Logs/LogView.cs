using System.Collections.Generic;
using Assets.Scripts.Models.Logs;
using Assets.Scripts.ViewModels.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Logs
{
    /// <summary>
    /// Affiche les logs à l'écran
    /// </summary>
    public class LogView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le manager
        /// </summary>
        [SerializeField]
        private LogManager _manager;

        /// <summary>
        /// La prefab des logs
        /// </summary>
        [SerializeField]
        private GameObject _logPrefab;

        /// <summary>
        /// Le parent des logs visibles
        /// </summary>
        [SerializeField]
        private Transform _activeLogsParent;

        /// <summary>
        /// Le parent des logs inactifs
        /// </summary>
        [SerializeField]
        private Transform _inactiveLogsParent;

        /// <summary>
        /// Le nombre max de messages pouvant apparaître à la fois
        /// </summary>
        [SerializeField]
        private int _nbMaxLogsOnScreen = 5;

        /// <summary>
        /// L'espacement entre les logs
        /// </summary>
        [SerializeField]
        private int _spawnSpacing = 75;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _animSpeed = .5f;

        /// <summary>
        /// Durée de vie d'un log à l'écran
        /// </summary>
        [SerializeField]
        private float _logDisplayDuration = 5f;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Nombre d'instances visibles
        /// </summary>
        private int _nbActive;

        /// <summary>
        /// Les durées de vie par instance
        /// </summary>
        private List<float> _lifespanPerInstance;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Awake()
        {
            _manager.OnLogAdded += OnLogAdded;
        }

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _nbActive = 0;
            _lifespanPerInstance = new List<float>(_nbMaxLogsOnScreen);
        }

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            for (int i = _lifespanPerInstance.Count - 1; i >= 0; --i)
            {
                _lifespanPerInstance[i] += Time.deltaTime;

                if (_lifespanPerInstance[i] >= _logDisplayDuration)
                {
                    --_nbActive;
                    _lifespanPerInstance.RemoveAt(i);
                    Transform log = _activeLogsParent.GetChild(i);
                    DoHideAnimation(log);
                }
            }
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelé quand un nouveau message est ajouté
        /// </summary>
        /// <param name="log">Le message à afficher</param>
        private void OnLogAdded(Log log)
        {
            // On ajoute une nouvelle instance

            Transform newLog;

            if (_inactiveLogsParent.childCount > 0)
            {
                newLog = _inactiveLogsParent.GetChild(0);
                newLog.SetParent(_activeLogsParent);
            }
            else
            {
                newLog = Instantiate(_logPrefab, _activeLogsParent).transform;
            }

            newLog.SetAsFirstSibling();
            _lifespanPerInstance.Insert(0, 0f);

            // On assigne son contenu au nouveau log

            SetData(log, newLog);

            // On lance son animation d'apparition

            DoShowAnimation(newLog);

            // Si on dépasse _nbMaxLogsOnScreen,
            // on lance l'animation de disparition des logs les plus anciens

            ++_nbActive;
            bool overflow = _nbActive > _nbMaxLogsOnScreen;

            if (overflow)
            {
                for (int i = _nbActive - 1; i >= _nbMaxLogsOnScreen; --i)
                {
                    --_nbActive;
                    Transform lastLog = _activeLogsParent.GetChild(i);
                    lastLog.SetParent(_inactiveLogsParent);
                    DoHideAnimation(lastLog);
                    _lifespanPerInstance.RemoveAt(_lifespanPerInstance.Count - 1);
                }
            }

            // Pour tous les autres, on les élève

            for (int i = 1; i < _activeLogsParent.childCount; ++i)
            {
                Transform siblingLog = _activeLogsParent.GetChild(i);
                DoRiseAnimation(siblingLog);
            }
        }

        /// <summary>
        /// Assigne les données à l'instance
        /// </summary>
        /// <param name="log">Message à afficher</param>
        /// <param name="instance">L'objet à afficher</param>
        private static void SetData(Log log, Transform instance)
        {
            Image icon = instance.GetChild(0).GetComponent<Image>();
            icon.enabled = log.Sprite != null;
            icon.sprite = log.Sprite;
            instance.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(log.Message);
        }

        /// <summary>
        /// Animation affichant l'objet
        /// </summary>
        /// <param name="log">L'objet à afficher</param>
        private void DoShowAnimation(Transform log)
        {
            log.DOKill();
            log.localPosition = new Vector3(-50f, -_spawnSpacing, 0f);
            log.DOLocalMove(Vector3.zero, _animSpeed);
            log.GetComponent<CanvasGroup>().DOFade(1f, _animSpeed);
        }

        /// <summary>
        /// Animation élevant l'objet
        /// </summary>
        /// <param name="log">L'objet à masquer</param>
        private void DoRiseAnimation(Transform log)
        {
            log.DOKill();
            log.DOLocalMove(_spawnSpacing * (log.GetSiblingIndex()) * Vector3.up, _animSpeed);
        }

        /// <summary>
        /// Animation masquant l'objet
        /// </summary>
        /// <param name="log">L'objet à masquer</param>
        private void DoHideAnimation(Transform log)
        {
            log.DOKill();
            log.DOLocalMove(log.localPosition + new Vector3(-50f, _spawnSpacing, 0f), _animSpeed);
            log.GetComponent<CanvasGroup>().DOFade(0f, _animSpeed);
        }

        #endregion
    }
}