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
        public int CurEXP { get; set; }

        /// <summary>
        /// Le niveau actuel du luxurosaure
        /// </summary>
        public int CurLevel { get; set; }

        /// <summary>
        /// L'exp à gagner pour atteindre le niveau suivant
        /// </summary>
        public int ExpUntilNextLevel => Mathf.RoundToInt(EXPProgressCurve.Evaluate((float)(CurLevel + 1) / (float)DinoConstants.MAX_LEVEL) * DinoConstants.TOTAL_EXP);

        /// <summary>
        /// La qualité moyenne du luxurosaure.
        /// Impacte ses stats et la qualité de son sperme.
        /// </summary>
        public int Quality { get; private set; }

        /// <summary>
        /// Les stats de combat du luxurosaure en tenant compte de son niveau et qualité actuels
        /// </summary>
        public FightingStats CurFightingStats { get; private set; }

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
        /// Les stats de combat du luxurosaure à une qualité maximale
        /// </summary>
        [field: SerializeField]
        public FightingStats FightingStatsAtMaxQuality { get; private set; }

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
        /// Crée un nouveau luxurosaure à partir d'un modèle et ajuste ses stats en fonction de sa qualité.
        /// </summary>
        /// <param name="source">L'asset de référence dans le dossier du projet, 
        /// considéré comme ayant une qualité de 100% (donc avec ses stats au max pour le calcul)</param>
        /// <param name="quality">La qualité du luxurosaure à créer</param>
        /// <param name="level">Le niveau de base du luxurosaure</param>
        public static LustosaurSO CreateFrom(LustosaurSO source, int quality, int level = 1)
        {
            LustosaurSO clone = ScriptableObject.CreateInstance<LustosaurSO>();

            clone.name = source.name;
            clone.Attribute = source.Attribute;
            clone.Sprite = source.Sprite;
            clone.SupportStats = source.SupportStats;
            clone.EXPProgressCurve = source.EXPProgressCurve;
            clone.CurEXP = 0;
            clone.CurLevel = level;
            clone.Quality = quality;
            float t = (float)quality / 100f;

            // La qualité minimale que peut avoir un luxurosaure est 0%,
            // mais ses stats max ne peuvent pas descendre plus bas que 50% du montant max à 100% de qualité.

            FightingStats statsAt0Quality = new
                (
                source.FightingStatsAtMaxQuality.Health / 2,
                source.FightingStatsAtMaxQuality.Attack / 2,
                source.FightingStatsAtMaxQuality.Defense / 2,
                source.FightingStatsAtMaxQuality.CriticalHitRate / 2,
                source.FightingStatsAtMaxQuality.Evasion / 2
                );

            // Les stats max finales du luxurosaure créé vont donc se situer
            // à (Quality)% entre les stats min et max

            clone.FightingStatsAtMaxQuality = new FightingStats
                (
                    Mathf.RoundToInt(Mathf.Lerp(statsAt0Quality.Health, source.FightingStatsAtMaxQuality.Health, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAt0Quality.Attack, source.FightingStatsAtMaxQuality.Attack, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAt0Quality.Defense, source.FightingStatsAtMaxQuality.Defense, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAt0Quality.CriticalHitRate, source.FightingStatsAtMaxQuality.CriticalHitRate, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAt0Quality.Evasion, source.FightingStatsAtMaxQuality.Evasion, t))
                );

            // Au niveau 1, ses stats actuelles seront à 50% de ses stats max
            // et seront donc à 100% au niveau (DinoConstants.MAX_LEVEL)

            clone.CurFightingStats = new FightingStats
                (
                    clone.FightingStatsAtMaxQuality.Health / 2,
                    clone.FightingStatsAtMaxQuality.Attack / 2,
                    clone.FightingStatsAtMaxQuality.Defense / 2,
                    clone.FightingStatsAtMaxQuality.CriticalHitRate / 2,
                    clone.FightingStatsAtMaxQuality.Evasion / 2
                );

            if (level > 1)
            {
                // On recalcule ses stats si so niveau de base est supérieur à 1

                clone.RecalculateStats();
            }

            return clone;
        }

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
                RecalculateStats();
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

        /// <summary>
        /// Recalcule les stats du joueur en fonction de son niveau
        /// </summary>
        private void RecalculateStats()
        {
            FightingStats oldStats = CurFightingStats;

            FightingStats statsAtLevel1 = new
                (
                    FightingStatsAtMaxQuality.Health / 2,
                    FightingStatsAtMaxQuality.Attack / 2,
                    FightingStatsAtMaxQuality.Defense / 2,
                    FightingStatsAtMaxQuality.CriticalHitRate / 2,
                    FightingStatsAtMaxQuality.Evasion / 2
                );

            // On calcule les stats du joueur à son niveau actuel

            float t = (float)CurLevel / (float)DinoConstants.MAX_LEVEL;

            CurFightingStats = new FightingStats
                (
                    Mathf.RoundToInt(Mathf.Lerp(statsAtLevel1.Health, FightingStatsAtMaxQuality.Health, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAtLevel1.Attack, FightingStatsAtMaxQuality.Attack, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAtLevel1.Defense, FightingStatsAtMaxQuality.Defense, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAtLevel1.CriticalHitRate, FightingStatsAtMaxQuality.CriticalHitRate, t)),
                    Mathf.RoundToInt(Mathf.Lerp(statsAtLevel1.Evasion, FightingStatsAtMaxQuality.Evasion, t))
                );

            // On regarde combien de points on a gagné dans chaque stat.
            // Pas utile tout de suite, mais si on veut l'afficher plus tard, on l'aura au moins.

            FightingStats diff = CurFightingStats - oldStats;
        }

        #endregion
    }
}