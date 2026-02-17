using UnityEngine;

namespace Assets.Scripts.ViewModels.Parallax
{
    /// <summary>
    /// Gère le parallax de l'arrière-plan
    /// </summary>
    public class BackgroundParallax : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// La Transform de la cible à suivre
        /// </summary>
        [SerializeField]
        private Transform _targetT;

        /// <summary>
        /// Les différentes sections de l'arrière-plan
        /// </summary>
        [SerializeField]
        private Transform[] _children;

        /// <summary>
        /// Les différentes vitesses de défilement par enfant
        /// </summary>
        [SerializeField]
        private float[] _childrenScrollSpeedsX;

        /// <summary>
        /// Les différentes vitesses de défilement par enfant
        /// </summary>
        [SerializeField]
        private float[] _childrenScrollSpeedsY;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// La Transform du l'arrière-plan
        /// </summary>
        private Transform _t;

        /// <summary>
        /// La position de départ du l'arrière-plan
        /// </summary>
        private Vector3 _startPos;

        /// <summary>
        /// Les Materials des enfants
        /// </summary>
        private Material[] _childMaterials;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _t = transform;
            _startPos = _t.position;
            _childMaterials = new Material[_children.Length];

            for (int i = 0; i < _children.Length; ++i)
            {
                _childMaterials[i] = _children[i].GetComponent<Renderer>().material;
            }
        }

        private void LateUpdate()
        {
            _t.position = new Vector3(_targetT.position.x, _t.position.y, _t.position.z);

            for (int i = 0; i < _children.Length; ++i)
            {
                Move(_targetT.position, _childMaterials[i], _childrenScrollSpeedsX[i], _childrenScrollSpeedsY[i]);
            }
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Déplace l'enfant de l'arrière-plan
        /// </summary>
        /// <param name="delta">La différence de position</param>
        /// <param name="childMat">L'enfant</param>
        /// <param name="parallaxFactorX">Facteur de décalage de l'image</param>
        /// <param name="parallaxFactorY">Facteur de décalage de l'image</param>
        private void Move(Vector3 delta, Material childMat, float parallaxFactorX, float parallaxFactorY)
        {
            childMat.SetTextureOffset("_MainTex", new Vector2(delta.x * parallaxFactorX, delta.y * parallaxFactorY));
        }

        #endregion
    }
}