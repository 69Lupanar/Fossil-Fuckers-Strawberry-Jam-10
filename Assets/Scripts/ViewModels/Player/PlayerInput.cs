using UnityEngine;

namespace Assets.Scripts.ViewModels.Player
{
    /// <summary>
    /// Lit les entrées clavier du joueur
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// Obtient la direction du mouvement du joueur
        /// </summary>
        public float HorizontalAxis { get; private set; }

        /// <summary>
        /// true si le joueur a appuyé sur le bouton de saut
        /// </summary>
        public bool Jumped { get; set; }

        /// <summary>
        /// true si le joueur a appuyé sur le bouton d'interaction
        /// </summary>
        public bool Interacted { get; private set; }

        /// <summary>
        /// true si le joueur a appuyé sur le clic gauche
        /// </summary>
        public bool Clicked { get; private set; }

        /// <summary>
        /// true si le joueur maintient le bouton de minage
        /// </summary>
        public bool MiningIsPressed { get; private set; }

        #endregion

        #region variables d'instance

        private PlayerInputActions _inputActions;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        void Awake()
        {
            _inputActions = new PlayerInputActions();
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            HorizontalAxis = _inputActions.Player.Move.ReadValue<float>();
            Interacted = _inputActions.Player.Interact.triggered;
            Clicked = _inputActions.Player.Click.triggered;
            MiningIsPressed = _inputActions.Player.Mine.IsPressed();

            if (_inputActions.Player.Jump.WasPressedThisFrame())
            {
                Jumped = true;
            }
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Active les contrôles du joueur
        /// </summary>
        public void EnablePlayerInput(bool enable)
        {
            if (enable)
            {
                _inputActions.Player.Enable();
            }
            else
            {
                _inputActions.Player.Disable();
            }
        }

        /// <summary>
        /// Active les contrôles de l'UI
        /// </summary>
        public void EnableUIInput(bool enable)
        {
            if (enable)
            {
                _inputActions.UI.Enable();
            }
            else
            {
                _inputActions.UI.Disable();
            }
        }

        #endregion
    }
}