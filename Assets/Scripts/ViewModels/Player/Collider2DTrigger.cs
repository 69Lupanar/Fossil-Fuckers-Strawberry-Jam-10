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

    /// <summary>
    /// Appelée quand un objet quitte le trigger
    /// </summary>
    public Action<GameObject> OnTriggerExit { get; set; }

    #endregion

    #region Méthodes Unity

    /// <summary>
    /// Appelée quand un objet entre le trigger
    /// </summary>
    /// <param name="collision">Le Collider de l'objet</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision.gameObject);
    }

    /// <summary>
    /// Appelée quand un objet entre le trigger
    /// </summary>
    /// <param name="collision">Le Collider de l'objet</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision.gameObject);
    }

    #endregion
}
