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
        Mine
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
}