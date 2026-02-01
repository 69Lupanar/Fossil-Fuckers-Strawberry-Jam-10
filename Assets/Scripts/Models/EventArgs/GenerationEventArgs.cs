using UnityEngine;

namespace Assets.Scripts.Models.EventArgs
{
    /// <summary>
    /// Infos sur la génération du niveau
    /// </summary>
    public class GenerationEventArgs : System.EventArgs
    {
        #region Propriétés

        /// <summary>
        /// Les dimensions de la grille
        /// </summary>
        public Vector2Int GridSize { get; }

        /// <summary>
        /// L'espacement entre les cases
        /// </summary>
        public float SpawnSpacing { get; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="spawnSpacing">L'espacement entre les cases</param>
        public GenerationEventArgs(Vector2Int gridSize, float spawnSpacing)
        {
            GridSize = gridSize;
            SpawnSpacing = spawnSpacing;
        }

        #endregion
    }
}