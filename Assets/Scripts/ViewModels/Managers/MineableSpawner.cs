using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.EventArgs;
using Assets.Scripts.Models.Mineables;
using Assets.Scripts.Models.NPCs;
using Assets.Scripts.ViewModels.Mineables;
using Assets.Scripts.ViewModels.NPCs;
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
        /// Appelée avant le nettoyage et le début de la génération
        /// </summary>
        public System.Action OnBeforeGenerationStart { get; set; }

        /// <summary>
        /// Appelée une fois la génération terminée
        /// </summary>
        public System.EventHandler<GenerationEventArgs> OnGenerationCompleted { get; set; }

        #endregion

        #region Propriétés

        public List<NPCFighter> ActiveNPCFighters { get => _activeNPCFighters; }

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
        [SerializeField] private GameObject _mineableItemPrefab;

        /// <summary>
        /// La prefab du PNJ combattant
        /// </summary>
        [SerializeField] private GameObject _npcFighterPrefab;

        /// <summary>
        /// Le conteneur des cases
        /// </summary>
        [SerializeField] private Transform _tilesParent;

        /// <summary>
        /// Le conteneur des PNJs combattants
        /// </summary>
        [SerializeField] private Transform _npcFightersParent;

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

        /// <summary>
        /// La liste des PNJs combattants actifs
        /// </summary>
        private readonly List<NPCFighter> _activeNPCFighters = new();

        /// <summary>
        /// L'ObjectPool des PNJs combattants
        /// </summary>
        private ObjectPool<NPCFighter> _npcFighterPool;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            _npcFighterPool = new ObjectPool<NPCFighter>(CreateNPCFighter, GetNPCFighter, ReleaseNPCFighter, DestroyNPCFighter, true, _settings.MinMaxNbNPCFighters.y, 100);

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
            OnBeforeGenerationStart?.Invoke();

            Clear();
            CreateGrid();
            PopulateGrid(_settings);
            InstantiateGrid();
            InstantiateNPCs(_settings);

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

            while (ActiveNPCFighters.Count > 0)
            {
                _npcFighterPool.Release(ActiveNPCFighters[0]);
            }
        }

        /// <summary>
        /// Désactive un PNJ combattant
        /// </summary>
        /// <param name="npc">Le PNJ combattant</param>
        public void ReleaseFighter(NPCFighter npc)
        {
            _npcFighterPool.Release(npc);
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

            #region Génère les structures spéciales (base, caves, corridors, lacs, etc.)

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

        /// <summary>
        /// Instancie les PNJs
        /// </summary>
        /// <param name="settings">Paramètres de génération</param>
        private void InstantiateNPCs(GenerationSettingsSO settings)
        {
            int nbFreeTiles = _idsGrid.Where(id => id == string.Empty).Count();
            int nbNPCFighters = Mathf.Min(nbFreeTiles, Random.Range(settings.MinMaxNbNPCFighters.x, settings.MinMaxNbNPCFighters.y));
            NativeList<Vector3> possiblePositions = new(nbNPCFighters, Allocator.Temp);

            // Récupère les emplacements libres

            for (int i = 0; i < _idsGrid.Length; ++i)
            {
                if (_idsGrid[i] == string.Empty)
                {
                    possiblePositions.Add(new Vector3(i % _gridSize.x, -i / _gridSize.x) * _spawnSpacing);
                }
            }

            // Crée des PNJs à des emplacements libres choisis au hasard

            for (int i = 0; i < nbNPCFighters; ++i)
            {
                int index = Random.Range(0, possiblePositions.Length);
                Vector3 randomPos = possiblePositions[index];
                possiblePositions.RemoveAt(index);

                _npcFighterPool.Get(out NPCFighter npcFighter);
                npcFighter.transform.position = randomPos;

                int depth = -(int)(randomPos.y / _spawnSpacing);
                LustosaurSO[] team = CreateTeam(settings.LustosaursUsableByNPCFighters, settings.NbMaxLustosaursPerNPC, depth, _gridSize.y);
                string[] lines = SelectDialogueLines(settings.DialoguesPerHeatThreshold, depth, _gridSize.y);

                npcFighter.SetData(team, lines);
            }
        }

        #region Object Pool NPCFighter

        /// <summary>
        /// Crée une équipe au hasard parmi les luxurosaures disponibles
        /// </summary>
        /// <param name="lustosaursUsableByNPCFighters">Les luxurosaures disponibles</param>
        /// <param name="nbMaxLustosaursPerNPC">Le nombre max de luxurosaures par PNJs combattant</param>
        /// <param name="depth">Influe sur la puissance des luxurosaures</param>
        /// <param name="maxPossibleDepth">La profondeur max possible</param>
        /// <returns>Une équipe générée aléatoirement</returns>
        private LustosaurSO[] CreateTeam(LustosaurSO[] lustosaursUsableByNPCFighters, int nbMaxLustosaursPerNPC, int depth, int maxPossibleDepth)
        {
            int nbLustosaurs = Mathf.Clamp(Mathf.RoundToInt((float)depth / (float)maxPossibleDepth * nbMaxLustosaursPerNPC), 0, nbMaxLustosaursPerNPC);
            int lustosaurLevel = Mathf.RoundToInt((float)depth / (float)maxPossibleDepth * DinoConstants.MAX_LEVEL);
            int lustosaurQuality = Mathf.RoundToInt((float)depth / (float)maxPossibleDepth * DinoConstants.MAX_QUALITY);
            LustosaurSO[] newTeam = new LustosaurSO[nbMaxLustosaursPerNPC];

            for (int i = 0; i < nbLustosaurs; ++i)
            {
                LustosaurSO template = lustosaursUsableByNPCFighters[Random.Range(0, lustosaursUsableByNPCFighters.Length)];
                newTeam[i] = LustosaurSO.CreateFrom(template, Mathf.Clamp(lustosaurQuality, 1, DinoConstants.MAX_QUALITY), Mathf.Clamp(lustosaurLevel, 1, DinoConstants.MAX_LEVEL));
            }

            return newTeam;
        }

        /// <summary>
        /// Sélectionne un dialogue au hasard pour le PNJ
        /// </summary>
        /// <param name="dialogues">Les dialogues possibles</param>
        /// <param name="depth">Influe sur le dialogue à jouer</param>
        /// <param name="maxPossibleDepth">La profondeur max possible</param>
        /// <returns></returns>
        private static string[] SelectDialogueLines(DialoguesPerHeatThreshold[] dialogues, int depth, int maxPossibleDepth)
        {
            int dialogueIndex = Mathf.RoundToInt((float)(depth * dialogues.Length) / (float)maxPossibleDepth);
            DialoguesPerHeatThreshold dialogue = dialogues[Mathf.Clamp(dialogueIndex, 0, dialogues.Length - 1)];
            DialogueLinesPerHeatThreshold lines = dialogue.Lines[Random.Range(0, dialogue.Lines.Length)];
            return lines.Lines;
        }

        private NPCFighter CreateNPCFighter()
        {
            return Instantiate(_npcFighterPrefab, _npcFightersParent).GetComponent<NPCFighter>();
        }

        private void GetNPCFighter(NPCFighter npc)
        {
            npc.gameObject.SetActive(true);
            ActiveNPCFighters.Add(npc);
        }

        private void ReleaseNPCFighter(NPCFighter npc)
        {
            npc.gameObject.SetActive(false);
            ActiveNPCFighters.Remove(npc);
        }

        private void DestroyNPCFighter(NPCFighter npc)
        {
            if (ActiveNPCFighters.Contains(npc))
            {
                ActiveNPCFighters.Remove(npc);
            }

            Destroy(npc.gameObject);
        }

        #endregion

        #region Object Pool MineableTile

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

        #endregion
    }
}