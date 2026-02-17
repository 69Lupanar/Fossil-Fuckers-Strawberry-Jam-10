using System;
using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Ingrédients nécessaires au clonage d'un luxurosaure
    /// </summary>
    [Serializable]
    public struct FusionAttributeIngredients
    {
        /// <summary>
        /// La liste des ingrédients
        /// </summary>
        [field: SerializeField]
        public ElementalAttribute[] Ingredients { get; set; }
    }
}