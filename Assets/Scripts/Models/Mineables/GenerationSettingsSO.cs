using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.Models.Mineables
{
    /// <summary>
    /// Paramètres de génération d'un niveau
    /// </summary>
    [CreateAssetMenu(fileName = "Generation Settings", menuName = "Scriptable Objects/Generation Settings")]
    public class GenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// Intervalle de taille possible pour une grille
        /// </summary>
        [field: SerializeField]
        public Vector2Int MinMaxGridLength { get; private set; }

        /// <summary>
        /// Intervalle de taille possible pour une grille
        /// </summary>
        [field: SerializeField]
        public Vector2Int MinMaxGridDepth { get; private set; }

        /// <summary>
        /// Liste d'objets minables à instancier.
        /// Si le total des probabilités est en dessous de 100%, le reste représentera des cases vides
        /// </summary>
        [field: SerializedDictionary("IDs", "Mineables Prefabs")]
        [field: SerializeField]
        public SerializedDictionary<MineableItemSO, MineableItemSpawnSettings> MineablesPrefabs { get; private set; }
    }
}