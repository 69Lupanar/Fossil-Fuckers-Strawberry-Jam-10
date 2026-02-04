using UnityEngine;

namespace Assets.Scripts.Models.Loot
{
    /// <summary>
    /// Données d'un objet récupéré depuis une case minée
    /// </summary>
    public abstract class LootSO : ScriptableObject
    {
        /// <summary>
        /// L'icône de l'objet
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; protected set; }

        /// <summary>
        /// L'EXP obtenue après minage de la case
        /// </summary>
        [field: SerializeField]
        public int EXP { get; protected set; }
    }
}