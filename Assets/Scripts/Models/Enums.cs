namespace Assets.Scripts.Models
{
    /// <summary>
    /// L'élément d'un luxurosaure.
    /// Eau bat le Feu,
    /// Feu bat le Vent,
    /// Vent bat la Terre,
    /// Terre bat l'Eau,
    /// Neutre n'a aucun bonus ou malus
    /// </summary>
    public enum ElementalAttribute
    {
        Neutral,
        Water,
        Fire,
        Wind,
        Earth
    }

    /// <summary>
    /// Le lieu dans lequel se déroule la scène adulte
    /// </summary>
    public enum SexEnvironment
    {
        Base,
        Mine,
        CombatDefeat,
        CombatVictory
    }

    /// <summary>
    /// La raison pour laqulle une scène de sexe doit se jouer
    /// </summary>
    public enum ReasonForSex
    {
        Gallery,
        Defeat,
        Victory,
        StatDepleted
    }

    /// <summary>
    /// En combat, indique quels luxurosaures peuvent être sélectionnés (aucun, joueur, adversaire, les deux)
    /// </summary>
    public enum CombatSelectionLockLevel
    {
        None,
        Player,
        Enemy,
        Both
    }

    /// <summary>
    /// Etat actuel d'un combat
    /// </summary>
    public enum BattleState
    {
        Ongoing,
        Victory,
        Defeat
    }

    /// <summary>
    /// Indique la résistance d'un luxurosaure à une attaque
    /// </summary>
    public enum LustosaurResistance
    {
        Neutral,
        Resistant,
        Vulnerable
    }
}