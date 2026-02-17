using Assets.Scripts.Models.EventArgs;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère le déplacement des 3 objets "Bounds",
    /// qui constituent les limites de la scène
    /// pour empêcher le joueur de tomber dans le vide
    /// </summary>
    public class BoundsManager : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le joueur
        /// </summary>
        [SerializeField] private Transform _player;

        /// <summary>
        /// Un des colliders bloquant le joueur
        /// </summary>
        [SerializeField] private Transform _boundsLeft;

        /// <summary>
        /// Un des colliders bloquant le joueur
        /// </summary>
        [SerializeField] private Transform _boundsRight;

        /// <summary>
        /// Un des colliders bloquant le joueur
        /// </summary>
        [SerializeField] private Transform _boundsBottom;

        /// <summary>
        /// Le sprite du mur de gauche
        /// </summary>
        [SerializeField] private SpriteRenderer _boundsSpriteLeft;

        /// <summary>
        /// Le sprite du mur de droite
        /// </summary>
        [SerializeField] private SpriteRenderer _boundsSpriteRight;

        /// <summary>
        /// Le sprite du fond de la mine
        /// </summary>
        [SerializeField] private SpriteRenderer _boundsSpriteBottom;

        /// <summary>
        /// Le spawner des cases
        /// </summary>
        [SerializeField] private MineableSpawner _mineableSpawner;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Les dimensions de la grille
        /// </summary>
        Vector2 _gridSize;

        /// <summary>
        /// L'espacement entre chaque instance
        /// </summary>
        float _spawnSpacing;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _mineableSpawner.OnGenerationCompleted += OnGenerationCompleted;
        }

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            _boundsLeft.position = new Vector3(-1.5f, _player.position.y, 0f);
            _boundsRight.position = new Vector3(_gridSize.x * _spawnSpacing - .5f, _player.position.y, 0f);
            _boundsBottom.position = new Vector3(_player.position.x, -_gridSize.y * _spawnSpacing + 0.5f, 0f);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée une fois la génération terminée
        /// </summary>
        /// <param name="sender">Le MineableSpawner</param>
        /// <param name="e">Les infos sur l'événement</param>
        private void OnGenerationCompleted(object sender, GenerationEventArgs e)
        {
            _gridSize = e.GridSize;
            _spawnSpacing = e.SpawnSpacing;

            Vector3 pos = _boundsSpriteLeft.transform.position;
            pos.y = -_gridSize.y * _spawnSpacing + 1f;
            _boundsSpriteLeft.transform.position = pos;
            _boundsSpriteLeft.size = new Vector2(_boundsSpriteLeft.size.x, e.GridSize.y * e.SpawnSpacing);

            pos = _boundsSpriteRight.transform.parent.transform.position;
            pos.x = _gridSize.x * _spawnSpacing - 1f;
            _boundsSpriteRight.transform.parent.transform.position = pos;

            pos = _boundsSpriteRight.transform.position;
            pos.y = -_gridSize.y * _spawnSpacing + 1f;
            _boundsSpriteRight.transform.position = pos;
            _boundsSpriteRight.size = new Vector2(_boundsSpriteLeft.size.x, e.GridSize.y * e.SpawnSpacing);

            pos = _boundsSpriteBottom.transform.parent.transform.position;
            pos.y = -_gridSize.y * _spawnSpacing + 1f;
            _boundsSpriteBottom.transform.parent.transform.position = pos;

            pos = _boundsSpriteBottom.transform.position;
            pos.x = _gridSize.x * _spawnSpacing / 2f - 1f;
            _boundsSpriteBottom.transform.position = pos;
            _boundsSpriteBottom.size = new Vector2(e.GridSize.x * e.SpawnSpacing, _boundsSpriteBottom.size.y);
        }

        #endregion
    }
}