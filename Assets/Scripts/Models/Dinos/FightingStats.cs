using System;
using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Stats de combat d'un luxurosaure
    /// </summary>
    [Serializable]
    public struct FightingStats
    {
        /// <summary>
        /// La santé du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Health { get; private set; }

        /// <summary>
        /// L'attaque du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Attack { get; private set; }

        /// <summary>
        /// La défense du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Defense { get; private set; }

        /// <summary>
        /// Le taux de coups critiques du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int CriticalHitRate { get; private set; }

        /// <summary>
        /// Le taux d'esquive du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Evasion { get; private set; }
    }
}