using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.Models.Loot
{
    /// <summary>
    /// Données de l'objet "Sperme"
    /// </summary>
    [CreateAssetMenu(fileName = "Lustosaur Sperm Extract", menuName = "Scriptable Objects/Loot/Lustosaur Sperm Extract")]
    public class SpermLootSO : LootSO
    {
        #region Variables Unity

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
        public SpermLootSO Clone()
        {
            SpermLootSO clone = ScriptableObject.CreateInstance<SpermLootSO>();
            clone.name = name;
            clone.Sprite = Sprite;
            clone.EXP = EXP;
            clone.Lustosaur = Lustosaur;
            return clone;
        }

        #endregion
    }
}