using Assets.Scripts.Models.Mineables;

namespace Assets.Scripts.Models.EventArgs
{
    /// <summary>
    /// Info retournée lors du minage d'une case
    /// </summary>
    public class TileMinedEventArgs : System.EventArgs
    {
        #region Propriétés

        /// <summary>
        /// La case minable
        /// </summary>
        public MineableTileSO Tile { get; private set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="tile">La case minable</param>
        public TileMinedEventArgs(MineableTileSO tile)
        {
            Tile = tile;
        }

        #endregion
    }
}