using Assets.Scripts.Models.Logs;
using Assets.Scripts.ViewModels.Managers;
using UnityEngine;

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
        /// Le nombre max de messages pouvant apparaître à la fois
        /// </summary>
        [SerializeField]
        private int _nbMaxLogsOnScreen = 5;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Awake()
        {
            _manager.OnLogAdded += OnLogAdded;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelé quand un nouveau message est ajouté
        /// </summary>
        /// <param name="log">Le message à afficher</param>
        private void OnLogAdded(Log log)
        {

        }

        #endregion
    }
}