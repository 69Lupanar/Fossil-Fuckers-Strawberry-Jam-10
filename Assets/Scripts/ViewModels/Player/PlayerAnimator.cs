using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Anime le perso en fonction des actions du joueur
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {

        #region Variables Unity

        [Header("General")]
        [Space(10)]

        /// <summary>
        /// Les stats du joueur
        /// </summary>
        [SerializeField] private PlayerStatsManager _playerStats;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField] private PlayerController _playerController;

        /// <summary>
        /// Le sprite du joueur
        /// </summary>
        [SerializeField] private SpriteRenderer _renderer;

        /// <summary>
        /// L'Animator du joueur
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        /// L'input
        /// </summary>
        [SerializeField] private PlayerInput _input;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _playerController.OnTileMined += (_, _) => { UpdateSpriteFlip(); };
            _playerController.OnEnabled += OnEnabled;
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            UpdateAnimationParameters();

            if (_playerStats.IsDead || _playerController.IsMining)
            {
                return;
            }

            UpdateSpriteFlip();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand le contrôleur est activé
        /// </summary>
        private void OnEnabled()
        {
            _renderer.flipX = true;
        }

        /// <summary>
        /// Vérifie la direction dans laquelle doit se tourner le sprite
        /// </summary>
        private void UpdateSpriteFlip()
        {
            if (_input.HorizontalAxis > 0f)
            {
                _renderer.flipX = true;
            }
            if (_input.HorizontalAxis < 0f)
            {
                _renderer.flipX = false;
            }
        }

        /// <summary>
        /// Màj les paramètres des transitions de l'Animator
        /// </summary>
        private void UpdateAnimationParameters()
        {
            _animator.SetBool("IsMoving", _playerController.IsMining || !Mathf.Approximately(_input.HorizontalAxis, 0f));
            _animator.SetInteger("Heat", _playerStats.LastHeatLevel);
        }

        #endregion
    }
}