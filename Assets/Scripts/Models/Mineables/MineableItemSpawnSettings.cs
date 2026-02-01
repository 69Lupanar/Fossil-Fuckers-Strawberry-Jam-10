using System;
using UnityEngine;

namespace Assets.Scripts.Models.Mineables
{
    /// <summary>
    /// Paramètres d'instantiation d'un objet minable
    /// </summary>
    [Serializable]
    public struct MineableItemSpawnSettings
    {
        /// <summary>
        /// Intervalle de profondeur où peut être instancié l'objet
        /// </summary>
        [field: SerializeField]
        public Vector2Int MinMaxDepth { get; private set; }

        /// <summary>
        /// La rareté de l'objet. Plus elle est basse, plus l'objet est rare.
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public float Rarity { get; private set; }

    }
}