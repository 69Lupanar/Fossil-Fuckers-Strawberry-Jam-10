using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Contrôleur du joueur
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables d'instance

        /// <summary>
        /// Le sprite du joueur
        /// </summary>
        [SerializeField] private SpriteRenderer _renderer;

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
        /// Le layer des objets représentant le sol
        /// </summary>
        [SerializeField] private LayerMask _groundLayerMask;

        /// <summary>
        /// Le nombre de sauts restants
        /// </summary>
        private int _curNbJumpsLeft;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _input.EnablePlayerInput(true);
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