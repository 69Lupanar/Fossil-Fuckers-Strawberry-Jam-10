using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Données d'un luxurosaure
    /// </summary>
    [CreateAssetMenu(fileName = "Lustosaur", menuName = "Scriptable Objects/Lustosaur")]
    public class LustosaurSO : ScriptableObject
    {
        #region Propriétés

        /// <summary>
        /// Le montant d'exp actuel du luxurosaure
        /// </summary>
        public int CurEXP { get; set; } = 0;

        /// <summary>
        /// Le niveau actuel du luxurosaure
        /// </summary>
        public int CurLevel { get; set; } = 1;

        /// <summary>
        /// L'exp à gagner pour atteindre le niveau suivant
        /// </summary>
        public int ExpUntilNextLevel => Mathf.RoundToInt(EXPProgressCurve.Evaluate((float)(CurLevel) / (float)DinoConstants.MAX_LEVEL - 1) * DinoConstants.TOTAL_EXP);

        #endregion

        #region Variables Unity

        /// <summary>
        /// L'attribut du luxurosaure
        /// </summary>
        [field: SerializeField]
        public ElementalAttribute Attribute { get; private set; }

        /// <summary>
        /// Le sprite du luxurosaure
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Les stats de combat du luxurosaure
        /// </summary>
        [field: SerializeField]
        public FightingStats FightingStats { get; private set; }

        /// <summary>
        /// Les stats de soutien du luxurosaure
        /// </summary>
        [field: SerializeField]
        public FightingStats SupportStats { get; private set; }

        /// <summary>
        /// Courbe de progression de gaind d'EXP au fil des niveaux
        /// </summary>
        [field: SerializeField]
        public AnimationCurve EXPProgressCurve { get; private set; }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Gagne de l'expérience pour monter au niveau suivant
        /// </summary>
        /// <param name="amount">Le montant</param>
        public void GainEXP(int amount)
        {
            if (CurLevel == DinoConstants.MAX_LEVEL)
            {
                return;
            }

            CurEXP += amount;

            while (CurEXP > ExpUntilNextLevel)
            {
                GainLevel();
            }
        }

        /// <summary>
        /// Gagne un niveau et réajuste l'EXP restant
        /// </summary>
        public void GainLevel()
        {
            if (CurLevel < DinoConstants.MAX_LEVEL)
            {
                CurEXP -= ExpUntilNextLevel;
                ++CurLevel;
            }

            if (CurLevel == DinoConstants.MAX_LEVEL)
            {
                CurEXP = 0;
            }
        }

        #endregion
    }
}