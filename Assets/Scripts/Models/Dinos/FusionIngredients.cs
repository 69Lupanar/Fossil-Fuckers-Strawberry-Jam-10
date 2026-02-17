using System;
using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Ingrédients nécessaires au clonage d'un luxurosaure
    /// </summary>
    [Serializable]
    public struct FusionIngredients
    {
        /// <summary>
        /// La liste des ingrédients
        /// </summary>
        [field: SerializeField]
        public LustosaurSO[] Ingredients { get; set; }
    }
}