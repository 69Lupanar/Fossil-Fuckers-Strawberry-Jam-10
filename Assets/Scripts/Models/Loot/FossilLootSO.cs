using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.Models.Loot
{
    /// <summary>
    /// Données de l'objet "Fossile"
    /// </summary>
    [CreateAssetMenu(fileName = "Fossil", menuName = "Scriptable Objects/Loot/Fossil")]
    public class FossilLootSO : LootSO
    {
        #region Propriétés

        /// <summary>
        /// La qualité de l'objet miné
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Le luxurosaure associé à ce fossile
        /// </summary>
        [field: SerializeField]
        public LustosaurSO Lustosaur { get; private set; }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Clone l'objet
        /// </summary>
        /// <returns>Une copie de l'objet</returns>
        public FossilLootSO Clone()
        {
            FossilLootSO clone = ScriptableObject.CreateInstance<FossilLootSO>();
            clone.name = name;
            clone.Sprite = Sprite;
            clone.EXP = EXP;
            clone.Lustosaur = Lustosaur;
            return clone;
        }

        #endregion
    }
}