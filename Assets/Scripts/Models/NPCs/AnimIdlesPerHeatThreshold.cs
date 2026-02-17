using System;
using UnityEngine;

namespace Assets.Scripts.Models.NPCs
{
    /// <summary>
    /// Sprites possibles d'un PNJ en fonction
    /// de son palier de chaleur
    /// </summary>
    [Serializable]
    public struct AnimIdlesPerHeatThreshold
    {
        /// <summary>
        /// Sprites possibles d'un PNJ en fonction
        /// de son palier de chaleur
        /// </summary>
        [field: SerializeField]
        public AnimationClip[] Value { get; private set; }
    }
}