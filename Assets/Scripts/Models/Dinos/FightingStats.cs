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
        #region Propriétés

        /// <summary>
        /// Stats à zéro
        /// </summary>
        public static FightingStats Zero => new(0, 0, 0, 0, 0, 0);

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
        /// Le taux de précision du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Accuracy { get; private set; }

        /// <summary>
        /// Le taux d'esquive du luxurosaure
        /// </summary>
        [field: SerializeField]
        public int Evasion { get; private set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="health">La santé du luxurosaure</param>
        /// <param name="attack">L'attaque du luxurosaure</param>
        /// <param name="defense">La défense du luxurosaure</param>
        /// <param name="criticalHitRate">Le taux de coups critiques du luxurosaure</param>
        /// <param name="accuracy"> Le taux de précision du luxurosaure</param>
        /// <param name="evasion">Le taux d'esquive du luxurosaure</param>
        public FightingStats(int health, int attack, int defense, int criticalHitRate, int accuracy, int evasion)
        {
            Health = health;
            Attack = attack;
            Defense = defense;
            CriticalHitRate = criticalHitRate;
            Accuracy = accuracy;
            Evasion = evasion;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Opérateur +
        /// </summary>
        public static FightingStats operator +(FightingStats a, FightingStats b)
        {
            return new FightingStats
                (
                a.Health + b.Health,
                a.Attack + b.Attack,
                a.Defense + b.Defense,
                a.CriticalHitRate + b.CriticalHitRate,
                a.Accuracy + b.Accuracy,
                a.Evasion + b.Evasion
                );
        }

        /// <summary>
        /// Opérateur -
        /// </summary>
        public static FightingStats operator -(FightingStats a, FightingStats b)
        {
            return new FightingStats
                (
                a.Health - b.Health,
                a.Attack - b.Attack,
                a.Defense - b.Defense,
                a.CriticalHitRate - b.CriticalHitRate,
                a.Accuracy - b.Accuracy,
                a.Evasion - b.Evasion
                );
        }

        #endregion
    }
}