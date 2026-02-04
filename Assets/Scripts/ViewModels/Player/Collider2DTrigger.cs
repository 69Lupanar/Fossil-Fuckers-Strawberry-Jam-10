using System;
using UnityEngine;

/// <summary>
/// Trigger se déclenchant quand un objet entre dedans
/// </summary>
public class Collider2DTrigger : MonoBehaviour
{
    #region Evénements

    /// <summary>
    /// Appelée quand un objet entre le trigger
    /// </summary>
    public Action<GameObject> OnTriggerEnter { get; set; }

    #endregion

    #region Méthodes Unity

    /// <summary>
    /// Appelée quand le joueur entre le trigger
    /// </summary>
    /// <param name="collision">Le Collider du joueur</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision.gameObject);
    }

    #endregion
}
