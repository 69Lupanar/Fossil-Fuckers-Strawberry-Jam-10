using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les événements et le relancement de la partie
    /// en cas de défaite
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// L'InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _statsManager;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _controller;

        /// <summary>
        /// Le point de spawn
        /// </summary>
        [SerializeField]
        private Transform _spawnPoint;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            RespawnPlayer();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void DisableController()
        {
            _controller.Disable();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void EnableController()
        {
            _controller.Enable();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void ResetPlayer()
        {
            EnableController();
            RespawnPlayer();
            _statsManager.RestoreStats();
            _inventoryManager.Clear();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void RespawnPlayer()
        {
            _controller.transform.position = _spawnPoint.position;
        }

        #endregion
    }
}