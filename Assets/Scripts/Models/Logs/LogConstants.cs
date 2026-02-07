namespace Assets.Scripts.Models.Logs
{
    /// <summary>
    /// Les constantes relatives aux logs
    /// </summary>
    public static class LogConstants
    {
        // Items

        public const string FOUND_ITEM_MSG = "Found a {0}! +{1} EXP!";
        public const string DISCARDED_ITEM_MSG = "Inventory full, {0} discarded.";
        public const string FOUND_GEM_MSG = "Found a {0}! +{1} EXP!";
        public const string INVENTORY_FULL_MSG = "Inventory full.";
        public const string ITEM_DISCARDED_MSG = "{0} discarded.";

        // Lustosaurs

        public const string LUSTOSAUR_CREATED_TEAM_MSG = "{0} added to the active roster!";
        public const string LUSTOSAUR_CREATED_STANDBY_MSG = "Team full; your {0} was sent on standby.";
        public const string LUSTOSAUR_CREATED_DISCARDED_MSG = "Team and reserve full; your {0} was set free.";
        public const string LUSTOSAUR_DISCARDED_MSG = "{0} was set free.";
        public const string LAST_LUSTOSAUR_REMOVED_FROM_TEAM_ERROR_MSG = "You cannot remove your last Lustosaur in your team!";

        // Fusion

        public const string FUSION_NOT_ENOUGH_MATERIALS_MSG = "You must place all materials before you can start the cloning process.";
        public const string FUSION_INCOMPATIBLE_MATERIALS_MSG = "The input ingredients have no associated Lustosaur.\n\nTry again with a different recipe!";

        // Upgrades

        public const string MAX_MOVE_SPEED_UPGRADED_MSG = "Max move speed increased!";
        public const string MAX_JUMP_FORCE_UPGRADED_MSG = "Max jump height increased!";
        public const string MAX_NB_JUMPS_UPGRADED_MSG = "Max nb jumps increased!";
        public const string MAX_MINING_SPEED_UPGRADED_MSG = "Max mining speed increased!";
        public const string MAX_MINING_QUALITY_UPGRADED_MSG = "Max mining quality increased!";
        public const string MAX_HEALTH_UPGRADED_MSG = "Max health increased!";
        public const string MAX_ENERGY_UPGRADED_MSG = "Max energy increased!";
        public const string MAX_HEAT_UPGRADED_MSG = "Max heat tolerance increased!";
        public const string MAX_INVENTORY_SIZE_UPGRADED_MSG = "Max inventory size increased!";
        public const string UPGRADE_ACQUIRED_MSG = "Upgrade '{0}' acquired!";

        // Alerts

        public const string HEALTH_LOW_MSG = "Low health!";
        public const string ENERGY_LOW_MSG = "Low energy!";
        public const string OVERHEAT_MSG = "Overheating!";
        public static readonly string[] HEAT_THRESHOLDS_MSG = new string[3]
        {
            "The heat forces you to remove some of your clothes...",
            "You blush at the thought of being caught by a stranger like this...",
            "The heat is unbearable..."
        };
    }
}