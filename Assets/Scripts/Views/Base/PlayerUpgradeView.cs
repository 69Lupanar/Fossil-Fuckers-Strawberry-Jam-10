using System;
using Assets.Scripts.Models.Player;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.ViewModels.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'interface d'amélioration du joueur
    /// </summary>
    public class PlayerUpgradeView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsManager;

        /// <summary>
        /// Le manager
        /// </summary>
        [SerializeField]
        private PlayerUpgradeManager _manager;

        /// <summary>
        /// La prefab des options
        /// </summary>
        [SerializeField]
        private GameObject _upgradeOptionPrefab;

        /// <summary>
        /// Le parent des options
        /// </summary>
        [SerializeField]
        private Transform _optionsParent;

        /// <summary>
        /// Affiche l'exp du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _playerExpLabel;

        /// <summary>
        /// Affiche le nom de l'amélioration
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _upgradeNameLabel;

        /// <summary>
        /// Affiche la description de l'amélioration
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _upgradeDescLabel;

        /// <summary>
        /// Label affiché quand aucune option n'est sélectionnée
        /// </summary>
        [SerializeField]
        private GameObject _selectLabel;

        /// <summary>
        /// Label affiché quand le joueur a tout débloqué
        /// </summary>
        [SerializeField]
        private GameObject _noOptionLeftLabel;

        /// <summary>
        /// Le bouton Acheter
        /// </summary>
        [SerializeField]
        private Button _buyBtn;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// true si la vue a déjà été initialisée
        /// pour ne pas recréer les boutons
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// L'option sélectionnée
        /// </summary>
        private UpgradeOptionInstance _curSelectedOption;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée quand le menu est ouvert
        /// </summary>
        public void OnUpgradeMenuOpen()
        {
            if (!_initialized)
            {
                _initialized = true;
                CreateOptionBtns();
            }

            _playerExpLabel.SetText(_playerStatsManager.CurEXPPoints.ToString());
            //UpdateOptionAvailability();
            ResetDescriptionPanel();
        }

        /// <summary>
        /// Achète l'amélioration
        /// </summary>
        public void BuyUpgrade()
        {
            // Améliore la stat correspondante

            _playerStatsManager.LoseEXP(_curSelectedOption.Upgrade.Cost);
            _playerExpLabel.SetText(_playerStatsManager.CurEXPPoints.ToString());

            _manager.UpgradeStat(_curSelectedOption.Upgrade);

            // Màj le bouton pour l'amélioration suivante
            // ou le détruit s'il n'y en a plus

            if (_curSelectedOption.Upgrade.NextUpgrade != null)
            {
                _curSelectedOption.SetCurUpgrade(_curSelectedOption.Upgrade.NextUpgrade);
            }
            else
            {
                DestroyImmediate(_curSelectedOption.gameObject);
            }

            // Màj les panneaux

            //UpdateOptionAvailability();
            ResetDescriptionPanel();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Crée les options des boutons
        /// </summary>
        private void CreateOptionBtns()
        {
            for (int i = 0; i < _manager.FirstStageUpgrades.Length; ++i)
            {
                UpgradeOptionInstance optionInstance = Instantiate(_upgradeOptionPrefab, _optionsParent).GetComponent<UpgradeOptionInstance>();
                optionInstance.SetCurUpgrade(_manager.FirstStageUpgrades[i]);
                optionInstance.OnClick += OnOptionInstanceClick;
            }
        }

        /// <summary>
        /// Appelée quand on clique sur une option
        /// </summary>
        /// <param name="instance">Le bouton</param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnOptionInstanceClick(UpgradeOptionInstance instance)
        {
            _curSelectedOption = instance;
            UpdateDescriptionPanel(instance.Upgrade);
        }

        ///// <summary>
        ///// Rend les boutons interactable si le joueur a assez d'exp 
        ///// pour acheter leur amélioration
        ///// </summary>
        //private void UpdateOptionAvailability()
        //{
        //    foreach (Transform child in _optionsParent)
        //    {
        //        UpgradeOptionInstance instance = child.GetComponent<UpgradeOptionInstance>();
        //        child.GetComponent<Button>().interactable = _playerStatsManager.CurEXPPoints >= instance.Upgrade.Cost;
        //    }
        //}

        /// <summary>
        /// Vide le panneau de description
        /// </summary>
        private void ResetDescriptionPanel()
        {
            _upgradeNameLabel.enabled = false;
            _upgradeDescLabel.enabled = false;
            _selectLabel.SetActive(_optionsParent.childCount > 0);
            _noOptionLeftLabel.SetActive(_optionsParent.childCount == 0);
            _buyBtn.gameObject.SetActive(false);
        }

        /// <summary>
        /// Màj le panneau de description
        /// </summary>
        /// <param name="upgrade">L'amélioration</param>
        private void UpdateDescriptionPanel(PlayerUpgradeSO upgrade)
        {
            _upgradeNameLabel.enabled = true;
            _upgradeDescLabel.enabled = true;

            _upgradeNameLabel.SetText(upgrade.name);
            _upgradeDescLabel.SetText(upgrade.Description);

            _selectLabel.SetActive(false);
            _noOptionLeftLabel.SetActive(false);
            _buyBtn.gameObject.SetActive(true);
            _buyBtn.interactable = _playerStatsManager.CurEXPPoints >= _curSelectedOption.Upgrade.Cost;
        }

        #endregion
    }
}