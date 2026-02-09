using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Attaque apprenable par les luxurosaures lors de leur montée de niveau
    /// </summary>
    [CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
    public class AttackSO : ScriptableObject
    {
        /// <summary>
        /// L'attribut de l'attaque
        /// </summary>
        [field: SerializeField]
        public ElementalAttribute Attribute { get; private set; }

        /// <summary>
        /// Le sprite de l'attribut de l'attaque
        /// </summary>
        [field: SerializeField]
        public Sprite AttributeSprite { get; private set; }

        /// <summary>
        /// Les dégâts de l'attaque
        /// </summary>
        [field: SerializeField]
        public int Damage { get; private set; }

        /// <summary>
        /// Le coût de l'attaque
        /// </summary>
        [field: SerializeField]
        public int Cost { get; private set; }

    }
}