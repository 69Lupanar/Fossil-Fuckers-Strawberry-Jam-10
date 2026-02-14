using System;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.ViewModels.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Inventory
{
    /// <summary>
    /// Affiche l'inventaire à l'écran
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        #region Evénéments

        /// <summary>
        /// Appelée quand le curseur entre sur un des InventorySlotInstances
        /// </summary>
        public Action<LootSO> OnLootSlotPointerEnter { get; set; }

        /// <summary>
        /// Appelée quand le curseur quitte un des InventorySlotInstances
        /// </summary>
        public Action OnLootSlotPointerExit { get; set; }

        #endregion

        #region Variables Unity

        [Header("Général")]
        [Space(10)]

        /// <summary>
        /// Le 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// L'inventaire
        /// </summary>
        [SerializeField]
        private InventoryManager _manager;

        /// <summary>
        /// Le label affichant la capacité de l'inventaire
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _sizeLabel;

        /// <summary>
        /// Le bouton des onglets
        /// </summary>
        [SerializeField]
        private Button _previousBtn;

        /// <summary>
        /// Le bouton des onglets
        /// </summary>
        [SerializeField]
        private Button _nextBtn;

        /// <summary>
        /// Les emplacements de l'inventaire
        /// </summary>
        [SerializeField]
        private GameObject[] _slots;

        /// <summary>
        /// Les images de chaque emplacement de l'inventaire
        /// </summary>
        [SerializeField]
        private Image[] _slotsImgs;

        [Space(10)]
        [Header("Audio")]
        [Space(10)]

        /// <summary>
        /// L'AudioManager
        /// </summary>
        [SerializeField]
        private AudioManager _audioManager;

        /// <summary>
        /// Le son joué lrosqu'on déterre un objet
        /// </summary>
        [SerializeField]
        private AudioClip _onGemFoundSFX;

        /// <summary>
        /// Le son joué lrosqu'on déterre un objet
        /// </summary>
        [SerializeField]
        private AudioClip _onItemFoundSFX;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le nombre d'onglets disponibles, changeant en fonction de la taille de l'inventaire
        /// </summary>
        private int _nbTabs = 1;

        /// <summary>
        /// L'onglet actuel
        /// </summary>
        private int _curTab = 0;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _playerController.OnTileMined += OnTileMined;
            _manager.OnLootAdded += OnLootAdded;
            _manager.OnInventorySizeIncreased += OnInventorySizeIncreased;
            _manager.OnClear += OnClear;
        }

        private void Start()
        {
            _manager.InitializeInventory();
            _previousBtn.interactable = false;
            _nextBtn.interactable = false;
            _sizeLabel.SetText($"({_curTab + 1}/{_nbTabs})");
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée par le bouton previous
        /// </summary>
        public void OnPreviousBtn()
        {
            --_curTab;
            _nextBtn.interactable = true;
            _previousBtn.interactable = _curTab > 0;
            _sizeLabel.SetText($"({_curTab + 1}/{_nbTabs})");
            DisplayItemsInCurTab();
        }

        /// <summary>
        /// Appelée par le bouton next
        /// </summary>
        public void OnNextBtn()
        {
            ++_curTab;
            _previousBtn.interactable = true;
            _nextBtn.interactable = _curTab < _nbTabs - 1;
            _sizeLabel.SetText($"({_curTab + 1}/{_nbTabs})");
            DisplayItemsInCurTab();
        }

        /// <summary>
        /// Appelée quand le curseur entre sur un des InventorySlotInstances
        /// </summary>
        public void OnInventorySlotPointerEnter(int index)
        {
            LootSO loot = _manager.Inventory[index + _curTab * _slotsImgs.Length];

            if (loot != null)
            {
                OnLootSlotPointerEnter?.Invoke(loot);
            }
        }
        /// <summary>
        /// Appelée quand le curseur quitte un des InventorySlotInstances
        /// </summary>
        public void OnInventorySlotPointerExit()
        {
            OnLootSlotPointerExit?.Invoke();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand l'inventaire est vidé
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnClear()
        {
            for (int i = 0; i < _slotsImgs.Length; ++i)
            {
                _slotsImgs[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Appelée quand la taille de l'inventaire augmente
        /// </summary>
        /// <param name="newSize">La nouvelle taille</param>
        private void OnInventorySizeIncreased(int newSize)
        {
            _nbTabs = newSize / _slotsImgs.Length;
            _nextBtn.interactable = true;
            _sizeLabel.SetText($"({_curTab + 1}/{_nbTabs})");
        }

        /// <summary>
        /// Appelée quand une case est minée
        /// </summary>
        /// <param name="loot">L'obje déterré</param>
        private void OnTileMined(LootSO loot, int _)
        {
            switch (loot)
            {
                case GemLootSO:
                    _audioManager.Play(_onGemFoundSFX);
                    break;
                case FossilLootSO or SpermLootSO:
                    _audioManager.Play(_onItemFoundSFX);
                    break;
            }
        }

        /// <summary>
        /// Appelée quand
        /// </summary>
        private void OnLootAdded(LootSO _)
        {
            DisplayItemsInCurTab();
        }

        /// <summary>
        /// Affiche les éléments de l'onglet actif
        /// </summary>
        private void DisplayItemsInCurTab()
        {
            int nbVisibleSlots = _slotsImgs.Length;
            Vector2Int range = new(_curTab * nbVisibleSlots, _curTab * nbVisibleSlots + nbVisibleSlots);

            int count = 0;

            for (int i = range.x; i < range.y; ++i)
            {
                // On désactive les emplacements en trop,
                // dans le cas où la taille de l'inventaire n'est pas un multiple de nbVisibleSlots

                _slots[count].SetActive(i < _manager.Inventory.Length);

                if (_slots[count].activeSelf)
                {
                    // Si l'inventaire a un objet à cet emplacement, on l'affiche

                    _slotsImgs[count].gameObject.SetActive(_manager.Inventory[i] != null);

                    if (_manager.Inventory[i] != null)
                        _slotsImgs[count].sprite = _manager.Inventory[i].Sprite;
                }

                ++count;
            }
        }

        #endregion
    }
}