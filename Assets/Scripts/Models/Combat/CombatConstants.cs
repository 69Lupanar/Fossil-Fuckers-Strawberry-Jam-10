namespace Assets.Scripts.Models.Combat
{
    /// <summary>
    /// Constantes utilisées en combat
    /// </summary>
    public static class CombatConstants
    {
        /// <summary>
        /// Nb max de luxurosaures participant à un combat donné
        /// </summary>
        public const int NB_PARTICIPATING_LUSTOSAURS = 3;

        /// <summary>
        /// Bonus de dégâts d'un coup critique
        /// </summary>
        public const float CRITICAL_HIT_DMG_BONUS = 1.5f;

        /// <summary>
        /// Réduction de dégâts si l'attaquant ou le défenseur sont dans la zone de soutien
        /// </summary>
        public const int SUPPORT_ZONE_DMG_REDUCTION = 2;

        /// <summary>
        /// EXP gagnée par ennemi vainu
        /// </summary>
        public const int EXP_GAINED_PER_DEFEATED_ENEMY = 250;

        /// <summary>
        /// EXP perdue par allié vainu
        /// </summary>
        public const int EXP_LOST_PER_DEFEATED_ALLY = 100;

        /// <summary>
        /// EXP gagnée par différence de niveau moyen entre le joueur et l'ennemi,
        /// seulement si l'ennemi est plus fort que le joueur
        /// </summary>
        public const int EXP_GAINED_PER_LEVEL_DIFFERENCE = 100;

        /// <summary>
        /// Si vaincu, l'EXP gagnée est divisée par ce nombre
        /// </summary>
        public const int EXP_REDUCTION_COEF_IF_DEFEATED = 10;

        /// <summary>
        /// Message d'attaque sur un ennemi
        /// </summary>
        public const string ALLY_ATTACK_MSG = "{0} attacks the opposing {1}!";

        /// <summary>
        /// Message d'attaque sur un allié
        /// </summary>
        public const string ENEMY_ATTACK_MSG = "The opposing {0} attacks your {1}!";

        /// <summary>
        /// Nb max de luxurosaures participant à un combat donné
        /// </summary>
        public const string LUSTOSAUR_DEFEATED_MSG = "{0} has been defeated!";

        /// <summary>
        /// Messages se jouant si le joueur a perdu une partie de ses vêtements
        /// </summary>
        public static readonly string[] HORNY_MESSAGES = new string[]
        {
            "Your lustosaurs are distracted by your lack of public decency...",
            "Your team leader's nostrils flare out at the scent of your sweat...",
            "One of your lustosaurs stomps its talon, staring at your exposed crotch..."
        };

        /// <summary>
        /// Messages se jouant si le joueur a perdu tous ses vêtements
        /// </summary>
        public static readonly string[] VERY_HORNY_MESSAGES = new string[]
        {
            "The scent of your dripping pussy drives your lustosaurs into a frenzy!",
            "Your lustosaurs' cocks are now clearly visible, throbbing out in the open.",
            "Your team leader growls, his cock pulsating at the sight of your naked body..."
        };
    }
}