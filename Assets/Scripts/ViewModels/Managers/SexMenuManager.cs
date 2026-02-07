using Assets.Scripts.Models;
using Assets.Scripts.Models.Adult;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère la scène adulte
    /// </summary>
    public class SexMenuManager : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// La scène adulte en cours de lecture
        /// </summary>
        public AdultSceneSO AdultScene { get; set; }

        /// <summary>
        /// La raison pour laqulle une scène de sexe doit se jouer
        /// </summary>
        public ReasonForSex ReasonForSex { get; set; }

        /// <summary>
        /// L'index du paragraphe actuel
        /// </summary>
        public int CurIndex { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// La couleur de l'arrière-plan par type d'environnement
        /// </summary>
        [SerializedDictionary("Sex Environment", "Color")]
        [SerializeField]
        public SerializedDictionary<SexEnvironment, Color> ColorPerEnvironment;

        /// <summary>
        /// La scène adulte à jouer dans la gallerie
        /// </summary>
        [SerializedDictionary("Associated Lustosaur name", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesInGallery;

        /// <summary>
        /// La scène adulte à jouer après une défaite en combat
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesOnDefeat;

        /// <summary>
        /// La scène adulte à jouer après que la santé ou l'énergie du joueur soit vidée
        /// </summary>
        [SerializedDictionary("Associated Lustosaur", "Adult Scene")]
        [SerializeField]
        public SerializedDictionary<string, AdultSceneSO> ScenesOnStatDepleted;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne la scène adulte à lire
        /// </summary>
        /// <param name="adultScene">La scène adulte à lire</param>
        public void SetAdultScene(AdultSceneSO adultScene)
        {
            AdultScene = adultScene;
            CurIndex = -1;
        }

        /// <summary>
        /// Passe au paragraphe suivant de la scène en cours.
        /// S'il n'y en a plus, on arrête la scène.
        /// </summary>
        public string Next()
        {
            if (CurIndex == AdultScene.Paragraphs.Length - 1)
            {
                return string.Empty;
            }

            ++CurIndex;

            return AdultScene.Paragraphs[CurIndex];
        }

        #endregion
    }
}