using Assets.Scripts.Models;
using Assets.Scripts.ViewModels.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'affichage de la scène adulte
    /// </summary>
    public class SexMenuView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le SexMenuManager
        /// </summary>
        [SerializeField]
        private SexMenuManager _manager;

        /// <summary>
        /// Le BaseMenuView
        /// </summary>
        [SerializeField]
        private BaseMenuView _baseMenuView;

        /// <summary>
        /// L'arrière-plan
        /// </summary>
        [SerializeField]
        private RawImage _bgImg;

        /// <summary>
        /// Le sprite de la scène adulte
        /// </summary>
        [SerializeField]
        private Image _sexImg;

        /// <summary>
        /// Le label contenant le paragraphe à lire
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _paragraphLabel;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée quand le menu adulte est ouvert
        /// </summary>
        /// <param name="reasonForSex">La raison pour laqulle une scène de sexe doit se jouer</param>
        /// <param name="sexEnvironment">L'environnement où se déroule la scène adulte</param>
        /// <param name="selectedLustosaur">Le luxurosaure correspondant à la scène adulte</param>
        public void OnSexMenuOpen(ReasonForSex reasonForSex, SexEnvironment sexEnvironment, Models.Dinos.LustosaurSO selectedLustosaur)
        {
            string lustosaurName = selectedLustosaur == null ? "null" : selectedLustosaur.name;
            _manager.ReasonForSex = reasonForSex;
            _manager.SetSpermLootFromLustosaur(lustosaurName, selectedLustosaur.Quality);
            _bgImg.color = _manager.ColorPerEnvironment[sexEnvironment];

            switch (reasonForSex)
            {
                case ReasonForSex.Gallery:
                    _manager.SetAdultScene(_manager.ScenesInGallery[lustosaurName]);
                    break;
                case ReasonForSex.Defeat:
                    _manager.SetAdultScene(_manager.ScenesOnDefeat[lustosaurName]);
                    break;
                case ReasonForSex.Victory:
                    _manager.SetAdultScene(_manager.ScenesOnVictory[lustosaurName]);
                    break;
                case ReasonForSex.StatDepleted:
                    _manager.SetAdultScene(_manager.ScenesOnStatDepleted[lustosaurName]);
                    break;
            }

            _sexImg.enabled = _manager.AdultScene.Sprite != null;
            _sexImg.sprite = _manager.AdultScene.Sprite;

            Next();
        }

        /// <summary>
        /// Passe au paragraphe suivant de la scène en cours.
        /// S'il n'y en a plus, on arrête la scène.
        /// </summary>
        public void Next()
        {
            string nextParagraph = _manager.Next();

            if (nextParagraph == string.Empty)
            {
                _baseMenuView.EndSexScene(_manager.ReasonForSex);
                return;
            }

            _paragraphLabel.SetText(nextParagraph);
        }

        #endregion
    }
}