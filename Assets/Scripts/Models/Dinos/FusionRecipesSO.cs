using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.Models.Dinos
{
    /// <summary>
    /// Recettes de fusion d'ADN de luxurosaures
    /// </summary>
    [CreateAssetMenu(fileName = "Fusion Recipes List", menuName = "Scriptable Objects/Fusion Recipes List")]
    public class FusionRecipesSO : ScriptableObject
    {
        /// <summary>
        /// Liste de luxurosaures créables à partir de listes d'ingrédients.
        /// Plusieurs listes d'ingrédients peuvent donner le même résultat.
        /// </summary>
        [field: SerializedDictionary("Ingredients", "Lustosaur")]
        [field: SerializeField]
        public SerializedDictionary<FusionIngredients, FusionResult[]> FusionRecipes { get; private set; }
    }
}