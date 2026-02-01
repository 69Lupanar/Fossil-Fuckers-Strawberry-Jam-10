using Assets.Scripts.ViewModels.Mineables;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Contrôleur du joueur
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables Unity

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
        /// La vitesse de déplacement du joueur
        /// </summary>
        [SerializeField] private float _moveSpeed = 5f;

        /// <summary>
        /// La vitesse de saut du joueur
        /// </summary>
        [SerializeField] private float _jumpForce = 5f;

        /// <summary>
        /// Le nombre de sauts max du joueur
        /// </summary>
        [SerializeField] private int _nbMaxJumps = 2;

        /// <summary>
        /// Le rayon de détection du sol
        /// </summary>
        [SerializeField] private float _groundCheckRadius = .25f;

        /// <summary>
        /// La vitesse de minage
        /// </summary>
        [SerializeField] private float _miningSpeed = 1f;

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
        /// Le nombre de sauts restants
        /// </summary>
        private int _curNbJumpsLeft;

        /// <summary>
        /// true si le joueur est en train de miner
        /// </summary>
        private bool _isMining;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _input.EnablePlayerInput(true);
            _isMining = false;
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
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
            if (_isMining)
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
                    _isMining = true;
                    _col.isTrigger = true;

                    transform.DOMove(hit.collider.transform.position, _miningSpeed).OnComplete(() =>
                    {
                        MineableTile tile = hit.collider.GetComponent<MineableTile>();
                        tile.OnMined();
                        _isMining = false;
                        _col.isTrigger = false;
                        _rb.linearVelocityY = 0f;
                    });
                }

                return;
            }

            _rb.linearVelocityX = _input.HorizontalAxis * _moveSpeed;
            bool grounded = Physics2D.CircleCast(_groundCheck.position, _groundCheckRadius, Vector3.down, 0f, _groundLayerMask);

            if (grounded && Mathf.Approximately(_rb.linearVelocityY, 0f))
            {
                _curNbJumpsLeft = _nbMaxJumps;
            }

            if (_input.Jumped)
            {
                if (_curNbJumpsLeft > 0)
                {
                    --_curNbJumpsLeft;
                    _rb.linearVelocityY = Mathf.Max(_rb.linearVelocityY, 0f);
                    _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
                }

                _input.Jumped = false;
            }
        }

        #endregion
    }
}