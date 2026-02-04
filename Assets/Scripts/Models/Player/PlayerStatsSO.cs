using System;
using UnityEngine;

/// <summary>
/// Les statistiques du joueur lors de la phase de minage
/// </summary>
[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Objects/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    #region Propriétés

    /// <summary>
    /// La vitesse de déplacement du joueur
    /// </summary>
    [field: SerializeField]
    public float MoveSpeed { get; set; } = 5f;

    /// <summary>
    /// La vitesse de saut du joueur
    /// </summary>
    [field: SerializeField]
    public float JumpForce { get; set; } = 6f;

    /// <summary>
    /// Le nombre de sauts max du joueur
    /// </summary>
    [field: SerializeField]
    public int NbMaxJumps { get; set; } = 2;

    /// <summary>
    /// La vitesse de minage
    /// </summary>
    [field: SerializeField]
    public float MiningSpeed { get; set; } = .75f;

    /// <summary>
    /// Le pourcentage de qualité de minage
    /// </summary>
    [field: SerializeField]
    public int MiningQualityPercentage { get; set; } = 50;

    /// <summary>
    /// La santé max du joueur
    /// </summary>
    [field: SerializeField]
    public float MaxHealth { get; set; } = 1f;

    /// <summary>
    /// L'énergie max du joueur
    /// </summary>
    [field: SerializeField]
    public float MaxEnergy { get; set; } = 1f;

    /// <summary>
    /// Les paliers de chaleur pouvant être supportés par le joueur.
    /// A chaque fois qu'il atteint un palier, il perd une couche de vêtements.
    /// Affecte également les stats de ses dinos en combat.
    /// </summary>
    [field: SerializeField]
    public float[] MaxHeatThresholds { get; set; } = new float[3] { .5f, .75f, 1f };

    /// <summary>
    /// Le nombre max d'objets que le joueur peut transporter
    /// avant de devoir retourner à la base
    /// </summary>
    [field: SerializeField]
    public int MaxInventorySize { get; set; } = 8;

    #endregion

    #region Méthodes publiques

    /// <summary>
    /// Clone l'objet
    /// </summary>
    /// <returns>Une copie de l'objet</returns>
    public PlayerStatsSO Clone()
    {
        PlayerStatsSO clone = (PlayerStatsSO)MemberwiseClone();
        Array.Copy(MaxHeatThresholds, clone.MaxHeatThresholds, MaxHeatThresholds.Length);
        return clone;
    }

    #endregion

}
