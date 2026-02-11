using System;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.Models.Mineables;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Mineables
{
    /// <summary>
    /// Représente une instance d'objet minable dans la scène
    /// </summary>
    public class MineableTile : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée pour renvoyer cette case dans son ObjectPool
        /// </summary>
        public Action<MineableTile> ReturnToPool { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// Liste des sprites possibles pour la case (choisis aléatoirement)
        /// </summary>
        [field: SerializeField]
        private SpriteRenderer _renderer;

        /// <summary>
        /// L'objet minable représenté par cette instance
        /// </summary>
        public MineableTileSO Tile { get; set; }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne les données à la case
        /// </summary>
        /// <param name="data">L'objet minable représenté par cette instance</param>
        public void SetData(MineableTileSO data)
        {
            Tile = data;
            _renderer.sprite = data.Sprites[UnityEngine.Random.Range(0, data.Sprites.Length)];
        }

        /// <summary>
        /// Obtient un loot au hasard de la case
        /// </summary>
        /// <returns>L'objet miné</returns>
        public LootSO GetLoot()
        {
            return (Tile.PossibleLoots == null || Tile.PossibleLoots.Length == 0) ? null : Tile.PossibleLoots[UnityEngine.Random.Range(0, Tile.PossibleLoots.Length)];
        }

        /// <summary>
        /// Renvoie cette case à son ObjectPool
        /// </summary>
        public void ReturnTileToPool()
        {
            ReturnToPool?.Invoke(this);
        }

        #endregion
    }
}