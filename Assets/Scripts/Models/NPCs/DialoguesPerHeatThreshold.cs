using System;
using UnityEngine;

namespace Assets.Scripts.Models.NPCs
{
    /// <summary>
    /// Lignes de dialogue d'un PNJ en fonction
    /// du palier de chaleur atteint par le joueur
    /// </summary>
    [Serializable]
    public struct DialoguesPerHeatThreshold
    {
        /// <summary>
        /// Tag pour retrouver l'entrée plus facilement dans l'inspecteur
        /// </summary>
        [field: SerializeField]
        public string Tag { get; private set; }

        /// <summary>
        /// Lignes de dialogue d'un PNJ en fonction
        /// du palier de chaleur atteint par le joueur
        /// </summary>
        [field: SerializeField]
        public DialogueLinesPerHeatThreshold[] Lines { get; private set; }
    }
}