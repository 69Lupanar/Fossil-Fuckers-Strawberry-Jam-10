using System;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère la lecture des dialogues
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée une fois un dialogue démarré
        /// </summary>
        public Action OnDialogueStarted { get; set; }

        /// <summary>
        /// Appelée une fois un dialogue terminé
        /// </summary>
        public Action OnDialogueEnded { get; set; }

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Les lignes de dialogues à afficher
        /// </summary>
        private string[] _dialogueLines;

        /// <summary>
        /// L'index de la ligne en cours
        /// </summary>
        private int _curIndex = -1;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Lance le dialogue
        /// </summary>
        /// <param name="npcDialogue">L dialogue à lire</param>
        public void StartDialogue(string[] npcDialogue)
        {
            _dialogueLines = npcDialogue;
            _curIndex = -1;
            OnDialogueStarted?.Invoke();
        }

        /// <summary>
        /// Passe à la réplique suivante du dialogue
        /// </summary>
        public string Next()
        {
            if (_curIndex == _dialogueLines.Length - 1)
            {
                OnDialogueEnded?.Invoke();
                return string.Empty;
            }

            return _dialogueLines[++_curIndex];
        }

        #endregion
    }
}