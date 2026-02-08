using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.NPCs;
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
        [field: Header("Grid")]
        [field: Space(10)]

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

        [field: Space(10)]
        [field: Header("Mining")]
        [field: Space(10)]

        /// <summary>
        /// Le type de case servant de sol à la base
        /// </summary>
        [field: SerializeField]
        public MineableTileSO BaseGroundTile { get; private set; }

        /// <summary>
        /// La longueur du sol de la base
        /// </summary>
        [field: SerializeField]
        public int BaseGroundLength { get; private set; }

        /// <summary>
        /// Liste d'objets minables à instancier.
        /// Si le total des probabilités est en dessous de 100%, le reste représentera des cases vides
        /// </summary>
        [field: SerializedDictionary("IDs", "Mineables Prefabs")]
        [field: SerializeField]
        public SerializedDictionary<MineableTileSO, MineableItemSpawnSettings> MineablesPrefabs { get; private set; }


        [field: Space(10)]
        [field: Header("NPCs")]
        [field: Space(10)]

        /// <summary>
        /// Le nombre max de luxurosaures par PNJs combattant
        /// </summary>
        [field: SerializeField]
        public int NbMaxLustosaursPerNPC { get; private set; }

        /// <summary>
        /// Le nombre max de PNJs combattants
        /// pouvant être instanciés par carte
        /// </summary>
        [field: SerializeField]
        public Vector2Int MinMaxNbNPCFighters { get; private set; }

        /// <summary>
        /// Lignes de dialogue d'un PNJ en fonction
        /// de son palier de chaleur
        /// </summary>
        [field: SerializeField]
        public DialoguesPerHeatThreshold[] DialoguesPerHeatThreshold { get; private set; }

        /// <summary>
        /// Luxurosaures utilisables par les PNJs combattants,
        /// sélectionnés au hasard lors de la génération
        /// </summary>
        [field: SerializeField]
        public LustosaurSO[] LustosaursUsableByNPCFighters { get; private set; }
    }
}