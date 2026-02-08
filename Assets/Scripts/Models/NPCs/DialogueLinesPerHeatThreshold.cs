using System;
using UnityEngine;

namespace Assets.Scripts.Models.NPCs
{
    /// <summary>
    /// Lignes de dialogue d'un PNJ en fonction
    /// du palier de chaleur atteint par le joueur
    /// </summary>
    [Serializable]
    public struct DialogueLinesPerHeatThreshold
    {
        /// <summary>
        /// Lignes de dialogue d'un PNJ en fonction
        /// du palier de chaleur atteint par le joueur
        /// </summary>
        [field: SerializeField, TextArea(3, 20)]
        public string[] Lines { get; private set; }
    }
}