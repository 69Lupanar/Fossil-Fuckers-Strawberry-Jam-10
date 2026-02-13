using Assets.Scripts.Models;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Views.UI;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Tooltip contenant les infos d'un LustosaurSO
    /// </summary>
    public class TooltipLustosaur : TooltipBase
    {
        #region Variables Unity

        /// <summary>
        /// Le label du nom du luxurosaure
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lustosaurNameLabel;

        /// <summary>
        /// Le label du niveau du luxurosaure
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lustosaurLevelLabel;

        /// <summary>
        /// Le label de l'exp actuel du luxurosaure
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lustosaurCurEXPLabel;

        /// <summary>
        /// Le label de l'exp du niveau suivant du luxurosaure
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lustosaurNextLevelEXPLabel;

        /// <summary>
        /// Les couleurs du label du nom en fonction de l'attribut du luxurosaure
        /// </summary>
        [SerializeField, SerializedDictionary("Attribute", "Color")]
        private SerializedDictionary<ElementalAttribute, Color> _nameLabelColors;

        /// <summary>
        /// Les icônes des stats de combat du luxurosaure
        /// </summary>
        [SerializeField]
        private StatIconInstance[] _fightingStatsIcons;

        /// <summary>
        /// Les icônes des stats de combat du luxurosaure
        /// </summary>
        [SerializeField]
        private StatIconInstance[] _supportStatsIcons;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Initialise les champs
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure à afficher</param>
        public void SetData(LustosaurSO lustosaur)
        {
            _lustosaurNameLabel.SetText(lustosaur.name);
            _lustosaurNameLabel.color = _nameLabelColors[lustosaur.Attribute];
            _lustosaurLevelLabel.SetText(lustosaur.CurLevel.ToString());
            _lustosaurCurEXPLabel.SetText(lustosaur.CurEXP.ToString());
            _lustosaurNextLevelEXPLabel.SetText(lustosaur.ExpUntilNextLevel.ToString());

            FightingStats fightingStats = lustosaur.CurFightingStats;
            FightingStats supportStats = lustosaur.SupportStats;

            _fightingStatsIcons[0].SetValue(fightingStats.Health, false);
            _fightingStatsIcons[1].SetValue(fightingStats.Attack, false);
            _fightingStatsIcons[2].SetValue(fightingStats.Defense, false);
            _fightingStatsIcons[3].SetValue(fightingStats.CriticalHitRate, false);
            _fightingStatsIcons[4].SetValue(fightingStats.Accuracy, false);
            _fightingStatsIcons[5].SetValue(fightingStats.Evasion, false);

            _supportStatsIcons[0].SetValue(supportStats.Health, false);
            _supportStatsIcons[1].SetValue(supportStats.Attack, false);
            _supportStatsIcons[2].SetValue(supportStats.Defense, false);
            _supportStatsIcons[3].SetValue(supportStats.CriticalHitRate, false);
            _supportStatsIcons[4].SetValue(supportStats.Accuracy, false);
            _supportStatsIcons[5].SetValue(supportStats.Evasion, false);
        }

        #endregion
    }
}