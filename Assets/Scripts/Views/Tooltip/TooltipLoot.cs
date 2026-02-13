using Assets.Scripts.Models.Loot;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Tooltip contenant les infos d'un LootSO
    /// </summary>
    public class TooltipLoot : TooltipBase
    {
        #region Variables Unity

        /// <summary>
        /// Le label du nom de l'objet
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lootNameLabel;

        /// <summary>
        /// Le label de la description de l'objet
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lootDescLabel;

        /// <summary>
        /// Le conteneur "qualité"
        /// </summary>
        [SerializeField]
        private GameObject _qualityContent;

        /// <summary>
        /// Le label de la qualité de l'objet
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lootQualityLabel;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Initialise les champs
        /// </summary>
        /// <param name="loot">L'objet à afficher</param>
        public void SetData(LootSO loot)
        {
            _lootDescLabel.gameObject.SetActive(!string.IsNullOrEmpty(loot.Description));
            _qualityContent.SetActive(loot.Quality > 0);
            _lootNameLabel.SetText(loot.name);
            _lootDescLabel.SetText(loot.Description);
            _lootQualityLabel.SetText($"{loot.Quality}%");

            UpdateReferences();
        }

        #endregion
    }
}