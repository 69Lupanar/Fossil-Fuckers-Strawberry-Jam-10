using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Affiche l'inventaire à l'écran
/// </summary>
public class InventoryView : MonoBehaviour
{
    #region Variables Unity

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
        _manager.OnLootAdded += OnLootAdded;
        _manager.OnInventorySizeIncreased += OnInventorySizeIncreased;
    }

    private void Start()
    {
        _manager.InitializeInventory();
        _previousBtn.interactable = false;
        _nextBtn.interactable = false;
        _sizeLabel.SetText($"({_curTab + 1}/{_nbTabs})");
    }

    #endregion

    #region Méthodes privées

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
    /// Appelée quand
    /// </summary>
    private void OnLootAdded(LootSO _)
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
                {
                    _slotsImgs[count].sprite = _manager.Inventory[i].Sprite;
                }
            }

            ++count;
        }
    }

    #endregion
}
