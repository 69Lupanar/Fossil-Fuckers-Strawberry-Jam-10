using UnityEngine;

namespace Assets.Scripts.Models.Adult
{
    /// <summary>
    /// Représente une scène adulte racontée après une défaite
    /// ou via la gallerie
    /// </summary>
    [CreateAssetMenu(fileName = "Adult Story", menuName = "Scriptable Objects/Adult Story")]
    public class AdultSceneSO : ScriptableObject
    {
        /// <summary>
        /// Le sprite affiché pendant la scène
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Les paragraphes de chaque scène
        /// </summary>
        [field: SerializeField, TextArea(5, 20)]
        public string[] Paragraphs { get; private set; }
    }
}