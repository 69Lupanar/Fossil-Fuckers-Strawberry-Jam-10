using System;
using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Résultats possibles suite à une fusion d'ingrédients
    /// </summary>
    [Serializable]
    public struct FusionResult
    {
        /// <summary>
        /// Le luxurosaure issu de la fusion des ingrédients
        /// </summary>
        [field: SerializeField]
        public LustosaurSO Lustosaur { get; set; }

        /// <summary>
        /// Le %age de chance d'obtenir ce luxurosaure si d'autres sont possibles
        /// via la même recette.
        /// S'il est le seul de la liste, le %age = 100%,
        /// </summary>
        [field: SerializeField]
        public float ChancePercentage { get; set; }

    }
}