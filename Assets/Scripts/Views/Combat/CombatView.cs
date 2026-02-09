using Assets.Scripts.ViewModels.Managers;
using UnityEngine;

namespace Assets.Scripts.Views.Combat
{
    /// <summary>
    /// Gère l'affichage des combats
    /// </summary>
    public class CombatView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le CombatManager
        /// </summary>
        [SerializeField]
        private CombatManager _manager;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _manager.OnCombatStarted += OnCombatStarted;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        private void OnCombatStarted()
        {

        }

        #endregion
    }
}