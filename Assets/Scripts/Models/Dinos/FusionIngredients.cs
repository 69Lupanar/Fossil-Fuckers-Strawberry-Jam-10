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
        /// true si les ingrédients peuvent être placés dans n'importe quel ordre,
        /// false s'ils doivent être mis dans l'ordre stipulé dans la liste.
        /// Pour les listes utilisant les 3 mêmes ingrédients,
        /// il est préférable de mettre Interchangeable à false,
        /// car la varification est plus rapide.
        /// </summary>
        [field: SerializeField]
        public bool Interchangeable { get; set; }

        /// <summary>
        /// La liste des ingrédients
        /// </summary>
        [field: SerializeField]
        public LustosaurSO[] Ingredients { get; set; }
    }
}