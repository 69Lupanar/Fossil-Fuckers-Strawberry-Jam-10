using System;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Mineables;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Contrôleur du joueur
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand une case est minée
        /// </summary>
        public Action<LootSO, int> OnTileMined { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// true si le joueur est en train de miner
        /// </summary>
        public bool IsMining { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Les stats du joueur
        /// </summary>
        [SerializeField] private PlayerStatsManager _playerStats;

        /// <summary>
        /// Le sprite du joueur
        /// </summary>
        [SerializeField] private SpriteRenderer _renderer;

        /// <summary>
        /// Le collider du joueur
        /// </summary>
        [SerializeField] private Collider2D _col;

        /// <summary>
        /// Le Rigidbody
        /// </summary>
        [SerializeField] private Rigidbody2D _rb;

        /// <summary>
        /// Utilisé pour la détection du sol
        /// </summary>
        [SerializeField] private Transform _groundCheck;

        /// <summary>
        /// L'input
        /// </summary>
        [SerializeField] private PlayerInput _input;

        /// <summary>
        /// Le rayon de détection du sol
        /// </summary>
        [SerializeField] private float _groundCheckRadius = .25f;

        /// <summary>
        /// Le layer des objets représentant le sol
        /// </summary>
        [SerializeField] private LayerMask _groundLayerMask;

        /// <summary>
        /// Le layer des objets minables
        /// </summary>
        [SerializeField] private LayerMask _miningLayerMask;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// true si le contrôleur est désactivé
        /// </summary>
        private bool _disabled;

        /// <summary>
        /// Le nombre de sauts restants
        /// </summary>
        private int _curNbJumpsLeft;

        /// <summary>
        /// Le tween d'animation de minage
        /// </summary>
        private TweenerCore<Vector3, Vector3, VectorOptions> _miningTween;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _input.EnablePlayerInput(true);
            IsMining = false;
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (_playerStats.IsDead || IsMining)
            {
                return;
            }

            if (_input.HorizontalAxis > 0f)
            {
                _renderer.flipX = false;
            }
            if (_input.HorizontalAxis < 0f)
            {
                _renderer.flipX = true;
            }
        }

        /// <summary>
        /// Màj en sync avec la physique
        /// </summary>
        private void FixedUpdate()
        {
            if (_playerStats.IsDead)
            {
                return;
            }

            if (IsMining)
            {
                return;
            }

            // Si on doit miner, on saute le déplacement/saut

            if (_input.MiningIsPressed && _input.MiningDirection != Vector2.zero)
            {
                Vector2 raycastDir = Vector2.zero;

                // On est obligé de faire ça pour éviter les raycasts diagonaux

                if (Mathf.Abs(_input.MiningDirection.x) > Mathf.Abs(_input.MiningDirection.y))
                {
                    raycastDir = Vector2.right * Mathf.Sign(_input.MiningDirection.x);
                }
                else
                {
                    raycastDir = Vector2.up * Mathf.Sign(_input.MiningDirection.y);
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDir, 2f, _miningLayerMask);

                if (hit.collider != null)
                {
                    MineableTile tile = hit.collider.GetComponent<MineableTile>();

                    if (!tile.Tile.Indesctructible)
                    {
                        IsMining = true;
                        _col.isTrigger = true;

                        _miningTween = transform.DOMove(hit.collider.transform.position, _playerStats.MiningSpeed).OnComplete(() =>
                        {
                            IsMining = false;
                            _col.isTrigger = false;
                            _rb.linearVelocityY = 0f;

                            // On récupère l'objet miné et on l'ajoute à l'inventaire.

                            LootSO loot = tile.OnMined();
                            OnTileMined?.Invoke(loot, _playerStats.MiningQualityPercentage);
                        });
                    }
                }
                return;
            }

            _rb.linearVelocityX = _input.HorizontalAxis * _playerStats.MoveSpeed;
            bool grounded = Physics2D.CircleCast(_groundCheck.position, _groundCheckRadius, Vector3.down, 0f, _groundLayerMask);

            if (grounded && Mathf.Approximately(_rb.linearVelocityY, 0f))
            {
                _curNbJumpsLeft = _playerStats.NbMaxJumps;
            }

            if (_input.Jumped)
            {
                if (_curNbJumpsLeft > 0)
                {
                    --_curNbJumpsLeft;
                    _rb.linearVelocityY = 0f;
                    //_rb.linearVelocityY = Mathf.Max(_rb.linearVelocityY, 0f); // Donne trop de puissance au saut si le joueur appuie rapidement 2x de suite
                    _rb.AddForceY(_playerStats.JumpForce, ForceMode2D.Impulse);
                }

                _input.Jumped = false;
            }
        }

        #endregion

        #region Méthodes pubiques

        /// <summary>
        /// Appelé quand la santé du joueur tombe à 0
        /// </summary>
        public void Disable()
        {
            _input.EnablePlayerInput(false);
            IsMining = false;
            _col.isTrigger = false;
            _miningTween?.Kill(false);
            _rb.linearVelocity = Vector2.zero;
        }

        /// <summary>
        /// Appelé quand les stats du joueur sont restaurées
        /// </summary>
        public void Enable()
        {
            _input.EnablePlayerInput(true);
            _renderer.flipX = false;
        }

        #endregion
    }
}