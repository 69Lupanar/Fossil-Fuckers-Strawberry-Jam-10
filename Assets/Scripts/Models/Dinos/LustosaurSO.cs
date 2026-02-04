using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Données d'un luxurosaure
    /// </summary>
    [CreateAssetMenu(fileName = "Lustosaur", menuName = "Scriptable Objects/Lustosaur")]
    public class LustosaurSO : ScriptableObject
    {
        /// <summary>
        /// L'attribut du luxurosaure
        /// </summary>
        [field: SerializeField]
        public ElementalAttribute Attribute { get; private set; }

        /// <summary>
        /// Le sprite du luxurosaure
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Les stats de combat du luxurosaure
        /// </summary>
        [field: SerializeField]
        public FightingStats FightingStats { get; private set; }

        /// <summary>
        /// Les stats de soutien du luxurosaure
        /// </summary>
        [field: SerializeField]
        public FightingStats SupportStats { get; private set; }

    }
}