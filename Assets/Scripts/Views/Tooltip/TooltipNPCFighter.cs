using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Tooltip contenant les infos d'un NPCFighter
    /// </summary>
    public class TooltipNPCFighter : TooltipBase
    {
        #region Variables Unity

        /// <summary>
        /// Les emplacements contenant les icônes et niveaux 
        /// des luxurosaures de l'équipe
        /// </summary>
        [SerializeField]
        private GameObject[] _enemyTeamSlots;

        /// <summary>
        /// Les icônes des luxurosaures de l'équipe
        /// </summary>
        [SerializeField]
        private Image[] _icons;

        /// <summary>
        /// Les labels des niveaux des luxurosaures de l'équipe
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI[] _levelLabels;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Initialise les champs
        /// </summary>
        /// <param name="fighter">Le combattant à afficher</param>
        public void SetData(NPCFighter fighter)
        {
            for (int i = 0; i < fighter.Team.Length; ++i)
            {
                LustosaurSO lustosaur = fighter.Team[i];
                _enemyTeamSlots[i].SetActive(lustosaur != null);

                if (lustosaur != null)
                {
                    _icons[i].sprite = lustosaur.NormalSprite;
                    _levelLabels[i].SetText(lustosaur.CurLevel.ToString());
                }
            }

            UpdateReferences();
        }

        #endregion
    }
}