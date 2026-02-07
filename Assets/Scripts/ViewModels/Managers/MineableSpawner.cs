using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.EventArgs;
using Assets.Scripts.Models.Mineables;
using Assets.Scripts.ViewModels.Mineables;
using AYellowpaper.SerializedCollections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère procéduralement un nouveau terrain d'objets minables
    /// </summary>
    public class MineableSpawner : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée une fois la génération terminée
        /// </summary>
        public System.EventHandler<GenerationEventArgs> OnGenerationCompleted { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// true pour générer un nouveau niveau au lancement de la scène
        /// </summary>
        [SerializeField] private bool _generateOnStart;

        /// <summary>
        /// Paramètres de génération
        /// </summary>
        [SerializeField] private GenerationSettingsSO _settings;

        /// <summary>
        /// La prefab de l'objet minable
        /// </summary>
        [SerializeField] private Transform _mineableItemPrefab;

        /// <summary>
        /// Le conteneur des cases
        /// </summary>
        [SerializeField] private Transform _tilesParent;

        /// <summary>
        /// Espacement entre les objets à instancier
        /// </summary>
        [SerializeField] private float _spawnSpacing = 1.5f;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Les dimensions de la grille
        /// </summary>
        private Vector2Int _gridSize;

        /// <summary>
        /// La grille contenant les IDs de tous les objets minables à instancier
        /// </summary>
        private string[] _idsGrid;

        /// <summary>
        /// Liste des ObjectPools pour chaque ID d'objet minable
        /// </summary>
        [SerializedDictionary("IDs", "Mineables Prefabs")]
        private readonly Dictionary<string, ObjectPool<MineableTile>> _poolsPerID = new();

        /// <summary>
        /// La liste des objets minables actifs
        /// </summary>
        private readonly List<MineableTile> _activeTiles = new();

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            if (_generateOnStart)
            {
                Generate();
            }
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Génère un nouveau niveau
        /// </summary>
        [ContextMenu("Generate")]
        public void Generate()
        {
            Clear();
            CreateGrid();
            PopulateGrid(_settings);
            InstantiateGrid();

            OnGenerationCompleted?.Invoke(this, new GenerationEventArgs(_gridSize, _spawnSpacing));
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            while (_activeTiles.Count > 0)
            {
                MineableTile tile = _activeTiles[0];
                _poolsPerID[tile.Tile.name].Release(tile);
            }
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Génère une nouvelle grille
        /// </summary>
        private void CreateGrid()
        {
            _gridSize = new(Random.Range(_settings.MinMaxGridLength.x, _settings.MinMaxGridLength.y),
                           Random.Range(_settings.MinMaxGridDepth.x, _settings.MinMaxGridDepth.y));

            _idsGrid = new string[_gridSize.x * _gridSize.y];
        }

        /// <summary>
        /// Replit la grille avec les IDs des cases à instancier
        /// </summary>
        /// <param name="settings">Paramètres de génération</param>
        private void PopulateGrid(GenerationSettingsSO settings)
        {
            #region Calcule les probabilités de chaque case d'être instanciée

            float maxAlea = 0;
            string[] names = settings.MineablesPrefabs.Keys.Select(item => item.name).ToArray();
            Vector2Int[] depths = settings.MineablesPrefabs.Values.Select(setting => setting.MinMaxDepth).ToArray();
            NativeArray<Vector2> chanceIntervals = new(names.Length, Allocator.Temp);

            int count = 0;

            foreach (var pair in settings.MineablesPrefabs)
            {
                chanceIntervals[count] = new Vector2(maxAlea, maxAlea + pair.Value.Rarity);
                maxAlea += pair.Value.Rarity;
                ++count;
            }

            #endregion

            #region Génère les cases de base pouvant être minées

            count = 0;

            for (int y = 0; y < _gridSize.y; ++y)
            {
                for (int x = 0; x < _gridSize.x; ++x)
                {
                    // Pour chaque case, on regarde si la probabilité tombe dans son intervalle.
                    // Si c'est le cas, on l'ajoute à la liste.
                    // Si on tombe en dehors de la plus haute intervalle, on place un case vide par défaut.

                    float rand = Random.Range(0f, 100f);
                    string id = string.Empty;

                    for (int i = 0; i < names.Length; ++i)
                    {
                        if (chanceIntervals[i].x < rand && rand <= chanceIntervals[i].y &&
                            depths[i].x <= y && y <= depths[i].y)
                        {
                            id = names[i];
                        }
                    }

                    _idsGrid[count + x] = id;

                    // On ajoute cet ID à la liste des ObjectPools pour instancier ses copies

                    if (id != string.Empty && !_poolsPerID.ContainsKey(id))
                    {
                        _poolsPerID.Add(id, new ObjectPool<MineableTile>(CreateTile, GetTile, ReleaseTile, DestroyTile, true, 10000, 10000));
                    }
                }

                count += _gridSize.x;
            }

            #endregion

            #region Génère les structures spéciales (base, caves, corridors, lacs, PNJs, etc.)

            #region Sol de la base

            for (int i = 0; i < settings.BaseGroundLength; ++i)
            {
                _idsGrid[i] = settings.BaseGroundTile.name;
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// Instancie les éléments minables
        /// </summary>
        private void InstantiateGrid()
        {
            for (int i = 0; i < _idsGrid.Length; ++i)
            {
                string key = _idsGrid[i];
                Vector3 pos = new Vector3(i % _gridSize.x, -i / _gridSize.x) * _spawnSpacing;

                if (_poolsPerID.ContainsKey(key))
                {
                    _poolsPerID[key].Get(out MineableTile mineable);
                    mineable.SetData(_settings.MineablesPrefabs.First(item => item.Key.name == key).Key);
                    mineable.transform.position = pos;
                    mineable.name = key;
                }
            }
        }

        private MineableTile CreateTile()
        {
            return Instantiate(_mineableItemPrefab, _tilesParent).GetComponent<MineableTile>();
        }

        private void GetTile(MineableTile tile)
        {
            tile.gameObject.SetActive(true);
            _activeTiles.Add(tile);
        }

        private void ReleaseTile(MineableTile tile)
        {
            tile.gameObject.SetActive(false);
            _activeTiles.Remove(tile);
        }

        private void DestroyTile(MineableTile tile)
        {
            if (_activeTiles.Contains(tile))
            {
                _activeTiles.Remove(tile);
            }

            Destroy(tile.gameObject);
        }

        #endregion
    }
}