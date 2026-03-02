using System;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.NPCs
{
    /// <summary>
    /// Logique d'un PNJ de type combattant
    /// </summary>
    public class NPCFighter : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand le joueur interagit avec ce PNJ
        /// </summary>
        public Action<NPCFighter> OnInteracted { get; set; }

        /// <summary>
        /// Appelée quand ce PNJ doit retourner à son ObjectPool
        /// </summary>
        public Action<NPCFighter> OnReturnToPoolRequested { get; set; }

        /// <summary>
        /// L'équipe de luxurosaures du PNJ, 
        /// générés aléatoirement en fonction de sa profondeur
        /// </summary>
        public LustosaurSO[] Team { get; private set; }

        /// <summary>
        /// Lignes de dialogue d'un PNJ, 
        /// choisies aléatoirement en fonction de son palier de chaleur
        /// </summary>
        public string[] DialogueLines { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le SpriteRenderer
        /// </summary>
        [SerializeField]
        private SpriteRenderer _renderer;

        /// <summary>
        /// Détecte la présence du joueur
        /// </summary>
        [SerializeField]
        private Collider2DTrigger _playerDetector;

        /// <summary>
        /// L'animator du PNJ
        /// </summary>
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// UI d'interaction
        /// </summary>
        [SerializeField]
        private GameObject _interactUI;

        #endregion

        #region Variables d'instance

        /// <summary>
        ///  La Transform du joueur
        /// </summary>
        private Transform _playerT;

        /// <summary>
        ///  L'input du joueur
        /// </summary>
        private PlayerInput _playerInput;

        /// <summary>
        ///  La Transform de ce pnj
        /// </summary>
        private Transform _t;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _t = transform;
            _playerT = FindAnyObjectByType<PlayerController>().transform;
            _playerInput = _playerT.GetComponent<PlayerInput>();
            _playerDetector.OnTriggerEnter += OnPlayerTriggerEnter;
            _playerDetector.OnTriggerExit += OnPlayerTriggerExit;
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _interactUI.SetActive(false);
            _renderer.flipX = UnityEngine.Random.Range(0, 2) == 0;
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (_interactUI.activeSelf && _playerInput.Interacted)
            {
                _renderer.flipX = _playerT.position.x > _t.position.x;
                OnInteracted?.Invoke(this);
            }
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne les données du PNJ
        /// </summary>
        /// <param name="team">L'équipe de luxurosaures du PNJ, générés aléatoirement en fonction de sa profondeur</param>
        /// <param name="dialogueLines">Lignes de dialogue d'un PNJ, choisies aléatoirement en fonction de son palier de chaleur</param>
        /// <param name="idle">L'animation du PNJ</param>
        public void SetData(LustosaurSO[] team, string[] dialogueLines, AnimationClip idle)
        {
            Team = team;
            DialogueLines = dialogueLines;

            if (idle != null)
            {
                _animator.Play(idle.name);
            }
        }

        /// <summary>
        /// Renvoie ce PNJ à son ObjectPool
        /// </summary>
        public void ReturnToPool()
        {
            OnReturnToPoolRequested?.Invoke(this);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand un objet entre le trigger
        /// </summary>
        /// <param name="obj">L'objet</param>
        private void OnPlayerTriggerEnter(GameObject obj)
        {
            if (obj.CompareTag("Player"))
            {
                _interactUI.SetActive(true);
            }
        }

        /// <summary>
        /// Appelée quand un objet quitte le trigger
        /// </summary>
        /// <param name="obj">L'objet</param>
        private void OnPlayerTriggerExit(GameObject obj)
        {
            if (obj.CompareTag("Player"))
            {
                _interactUI.SetActive(false);
            }
        }

        #endregion
    }
}