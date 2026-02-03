using UnityEngine;

namespace Assets.Scripts.Models.Logs
{
    /// <summary>
    /// Données d'un message affiché à l'écran
    /// </summary>
    public class Log
    {
        #region Propriétés

        /// <summary>
        /// L'icône affiché dans le message
        /// </summary>
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Le message
        /// </summary>
        public string Message { get; private set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="sprite">L'icône affiché dans le message</param>
        /// <param name="message">Le message</param>
        public Log(Sprite sprite, string message)
        {
            Sprite = sprite;
            Message = message;
        }

        #endregion
    }
}