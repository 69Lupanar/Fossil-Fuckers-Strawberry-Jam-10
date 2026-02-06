using UnityEngine;

namespace Assets.Scripts.Models.Player
{
    /// <summary>
    /// Amélioration de stat pour le joueur
    /// </summary>
    [CreateAssetMenu(fileName = "Player Upgrade", menuName = "Scriptable Objects/Player Upgrade")]
    public class PlayerUpgradeSO : ScriptableObject
    {
        /// <summary>
        /// L'icône de l'amélioration
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// La description de l'amélioration
        /// </summary>
        [field: SerializeField, TextArea(3, 5)]
        public string Description { get; private set; }

        /// <summary>
        /// Le coût de l'amélioration
        /// </summary>
        [field: SerializeField]
        public int Cost { get; private set; }

        /// <summary>
        /// L'ID de la stat associée à l'amélioration
        /// </summary>
        [field: SerializeField]
        public int UpgradeIndex { get; private set; }

        /// <summary>
        /// L'amélioration supérieure débloquée une fois cell-ci achetée.
        /// S'il n'y en a pas, celle-ci est la dernière.
        /// </summary>
        [field: SerializeField]
        public PlayerUpgradeSO NextUpgrade { get; private set; }
    }
}