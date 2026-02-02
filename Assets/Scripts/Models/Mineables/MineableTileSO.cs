using Assets.Scripts.Models.Loot;
using UnityEngine;

namespace Assets.Scripts.Models.Mineables
{
    /// <summary>
    /// Classe de base pour implémenter les objets minables
    /// </summary>
    [CreateAssetMenu(fileName = "Mineable Tile", menuName = "Scriptable Objects/Mineable Tile")]
    public class MineableTileSO : ScriptableObject
    {
        /// <summary>
        /// Liste des sprites possibles pour la case (choisi aléatoirement)
        /// </summary>
        [field: SerializeField]
        public Sprite[] Sprites { get; private set; }

        /// <summary>
        /// true si l'objet ne peut pas être détruit au minage
        /// </summary>
        [field: SerializeField]
        public bool Indesctructible { get; private set; }

        /// <summary>
        /// L'objet obtenu après minage de la case (choisi aléatoirement)
        /// </summary>
        [field: SerializeField]
        public LootSO[] PossibleLoots { get; private set; }
    }
}