using UnityEngine;

namespace Assets.Scripts.Models.Mineables
{
    /// <summary>
    /// Classe de base pour implémenter les objets minables
    /// </summary>
    [CreateAssetMenu(fileName = "Mineable Item", menuName = "Scriptable Objects/Mineable Item")]
    public class MineableItemSO : ScriptableObject
    {
        /// <summary>
        /// Liste des sprites possibles pour la case (choisis aléatoirement)
        /// </summary>
        [field: SerializeField]
        public Sprite[] Sprites { get; private set; }

        /// <summary>
        /// true si l'objet ne peut pas être détruit au minage
        /// </summary>
        [field: SerializeField]
        public bool Indesctructible { get; private set; }
    }
}