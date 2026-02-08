using Assets.Scripts.ViewModels.Managers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Views.Dialogue
{
    /// <summary>
    /// Gère l'affichage des dialogues
    /// </summary>
    public class DialogueView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le DialogueManager
        /// </summary>
        [SerializeField]
        private DialogueManager _manager;

        /// <summary>
        /// Le canvas du dialogue
        /// </summary>
        [SerializeField]
        private Canvas _dialogueCanvas;

        /// <summary>
        /// Le texte contenant la réplique actuelle
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _paragraphLabel;

        /// <summary>
        /// Les canvas à masquer pendant l'affichage du dialogue
        /// </summary>
        [SerializeField]
        private Canvas[] _canvasesToHide;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _manager.OnDialogueStarted += OnDialogueStarted;
            _manager.OnDialogueEnded += OnDialogueEnded;
        }

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _dialogueCanvas.enabled = false;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Passe à la réplique suivante du dialogue
        /// </summary>
        public void Next()
        {
            string nextLine = _manager.Next();

            if (nextLine != string.Empty)
            {
                _paragraphLabel.SetText(nextLine);
            }
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée une fois un dialogue démarré
        /// </summary>
        private void OnDialogueStarted()
        {
            _dialogueCanvas.enabled = true;

            foreach (Canvas canvas in _canvasesToHide)
            {
                canvas.enabled = false;
            }

            Next();
        }

        /// <summary>
        /// Appelée une fois un dialogue terminé
        /// </summary>
        private void OnDialogueEnded()
        {
            _dialogueCanvas.enabled = false;

            foreach (Canvas canvas in _canvasesToHide)
            {
                canvas.enabled = true;
            }
        }

        #endregion
    }
}