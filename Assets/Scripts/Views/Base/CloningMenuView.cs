using System;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'interface du menu de clonage
    /// </summary>
    public class CloningMenuView : MonoBehaviour
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

        /// <summary>
        /// Le CloningMenuManager
        /// </summary>
        [SerializeField]
        private CloningMenuManager _manager;

        /// <summary>
        /// L'image déplaçable représentant l'objet à déplacer
        /// </summary>
        [SerializeField]
        private Transform _draggableImg;

        /// <summary>
        /// Vitesse d'animation des popups
        /// </summary>
        [SerializeField]
        private float _popupAnimSpeed = .5f;

        /// <summary>
        /// Le conteneur des btns du menu de la base
        /// </summary>
        [SerializeField]
        private RectTransform _inventoryGrid;

        /// <summary>
        /// Les zones de dépot des emplacements de fusion
        /// </summary>
        [SerializeField]
        private RectTransform[] _fusionSlotsDropZones;

        /// <summary>
        /// Les ? des emplacements de fusion
        /// </summary>
        [SerializeField]
        private GameObject[] _questionMarks;

        /// <summary>
        /// Les zones de clic des emplacements de fusion
        /// </summary>
        [SerializeField]
        private PointerCallbackReceiver[] _fusionSlotsHandlers;

        /// <summary>
        /// Le popup de succès après une fusion réussie
        /// </summary>
        [SerializeField]
        private CanvasGroup _successPopup;

        /// <summary>
        /// Le popup d'échec après une fusion échouée
        /// </summary>
        [SerializeField]
        private CanvasGroup _failurePopup;

        /// <summary>
        /// Le label du nom du luxurosaure fusionné
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _lustosaurNameLabel;

        /// <summary>
        /// Le label du message d'erreur de la fusion
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _fusionFailErrorLabel;

        /// <summary>
        /// L'icône du luxurosaure fusionné
        /// </summary>
        [SerializeField]
        private Image _lustosaurIcon;

        /// <summary>
        /// La couleur du label du nom
        /// en fonction de l'attribut du luxurosaire
        /// </summary>
        [field: SerializedDictionary("Attribute", "Color")]
        [field: SerializeField]
        public SerializedDictionary<ElementalAttribute, Color> _textColorPerAttribute { get; private set; }

        #endregion

        #region Variables d'instance

        /// <summary>
        /// L'ID de l'objet de l'inventaire survolé par le curseur
        /// </summary>
        private int _curHoveredItemID = -1;

        /// <summary>
        /// La position de l'objet de l'inventaire survolé par le curseur
        /// </summary>
        private Vector3 _curHoveredItemStartPos;

        /// <summary>
        /// La position de la souris à la frame précédente
        /// </summary>
        private Vector3 _previousMousePos;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _successPopup.alpha = 0f;
            _successPopup.blocksRaycasts = false;
            _successPopup.interactable = false;
            _failurePopup.alpha = 0f;
            _failurePopup.blocksRaycasts = false;
            _failurePopup.interactable = false;
            InitializePointerHandlers();
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !_draggableImg.gameObject.activeSelf && _curHoveredItemID != -1 && _curHoveredItemID < _inventoryGrid.childCount)
            {
                _manager.DiscardItem(_curHoveredItemID);
                _curHoveredItemID = -1;
                DisplayItems();
                return;
            }

            if (Input.GetMouseButtonDown(0) && _curHoveredItemID != -1)
            {
                _draggableImg.gameObject.SetActive(true);
                _draggableImg.transform.position = _curHoveredItemStartPos;

                if (_curHoveredItemID < _inventoryGrid.childCount)
                {
                    _inventoryGrid.GetChild(_curHoveredItemID).GetChild(0).GetComponent<Image>().enabled = false;
                    _draggableImg.GetComponent<Image>().sprite = _manager.Inventory[_curHoveredItemID].Sprite;
                }
                else
                {
                    int index = _curHoveredItemID - _inventoryGrid.childCount;
                    _fusionSlotsHandlers[index].GetComponent<Image>().enabled = false;
                    _draggableImg.GetComponent<Image>().sprite = _manager.ItemsInFusionSlots[index].Sprite;
                }
            }

            if (_draggableImg.gameObject.activeSelf)
            {
                Vector3 drag = Input.mousePosition - _previousMousePos;
                _draggableImg.position += drag;
            }

            if (Input.GetMouseButtonUp(0) && _curHoveredItemID != -1)
            {
                OnDraggableImgReleased();
                DisplayItems();

                if (_curHoveredItemID < _inventoryGrid.childCount)
                {
                    _inventoryGrid.GetChild(_curHoveredItemID).GetChild(0).GetComponent<Image>().enabled = true;
                }
                else
                {
                    _fusionSlotsHandlers[_curHoveredItemID - _inventoryGrid.childCount].GetComponent<Image>().enabled = true;
                }

                _draggableImg.gameObject.SetActive(false);
                _curHoveredItemID = -1;
            }

            _previousMousePos = Input.mousePosition;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée quand le menu de clonage est ouvert
        /// </summary>
        public void OnCloningMenuOpen()
        {
            _draggableImg.gameObject.SetActive(false);
            _manager.TransferInventoryToBase();
            DisplayItems();
        }

        /// <summary>
        /// Lance une tentative de fusion
        /// </summary>
        public void TryFusion()
        {
            if (_manager.TryFusion(out LustosaurSO lustosaur, out string errorMsg))
            {
                ShowSuccessPopup(lustosaur);

                // Nettoie la table de fusion

                _manager.CleanupFusionSlots();
                DisplayItems();
            }
            else
            {
                ShowFailurePopup(errorMsg);
            }
        }

        /// <summary>
        /// Ajoute le luxurosaure créé à l'équipe
        /// </summary>
        public void AddCreatedLustosaurToTeam()
        {
            _manager.AddCreatedLustosaurToTeam();
        }

        /// <summary>
        /// Affiche un popup de succès après une fusion réussie
        /// </summary>
        public void HideSuccessPopup()
        {
            _successPopup.blocksRaycasts = false;
            _successPopup.interactable = false;
            _successPopup.DOFade(0f, _popupAnimSpeed);
        }

        /// <summary>
        /// Affiche un popup d'échec après une fusion échouée
        /// </summary>
        public void HideFailurePopup()
        {
            _failurePopup.blocksRaycasts = false;
            _failurePopup.interactable = false;
            _failurePopup.DOFade(0f, _popupAnimSpeed);
        }

        /// <summary>
        /// Nettoyage à la fermeture de la fenêtre
        /// </summary>
        public void CleanupOnWindowClosed()
        {
            _successPopup.blocksRaycasts = false;
            _successPopup.alpha = 0f;
            _failurePopup.blocksRaycasts = false;
            _failurePopup.alpha = 0f;

            _manager.CleanupOnWindowClosed();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Initialise les objets à glisser/déposer
        /// </summary>
        private void InitializePointerHandlers()
        {
            for (int i = 0; i < _inventoryGrid.childCount; ++i)
            {
                PointerCallbackReceiver draggableItem = _inventoryGrid.GetChild(i).GetChild(0).GetComponent<PointerCallbackReceiver>();
                draggableItem.IndexInHierarchy = i;
                draggableItem.OnPointerEnter += OnDraggableItemPointerEnter;
                draggableItem.OnPointerExit += OnDraggableItemPointerExit;
            }

            for (int i = 0; i < _fusionSlotsHandlers.Length; ++i)
            {
                PointerCallbackReceiver draggableItem = _fusionSlotsHandlers[i];
                draggableItem.IndexInHierarchy = _inventoryGrid.childCount + i;
                draggableItem.OnPointerEnter += OnDraggableItemPointerEnter;
                draggableItem.OnPointerExit += OnDraggableItemPointerExit;
            }
        }

        /// <summary>
        /// Affiche les objets dans l'inventaire
        /// </summary>
        private void DisplayItems()
        {
            for (int i = 0; i < _inventoryGrid.childCount; ++i)
            {
                _inventoryGrid.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
            for (int i = 0; i < _manager.Inventory.Count; ++i)
            {
                _inventoryGrid.GetChild(i).GetChild(0).gameObject.SetActive(true);
                _inventoryGrid.GetChild(i).GetChild(0).GetComponent<Image>().sprite = _manager.Inventory[i].Sprite;
            }

            for (int i = 0; i < _fusionSlotsHandlers.Length; ++i)
            {
                _fusionSlotsHandlers[i].gameObject.SetActive(_manager.ItemsInFusionSlots[i] != null);
                _questionMarks[i].SetActive(_manager.ItemsInFusionSlots[i] == null);

                if (_manager.ItemsInFusionSlots[i] != null)
                {
                    _fusionSlotsHandlers[i].GetComponent<Image>().sprite = _manager.ItemsInFusionSlots[i].Sprite;
                }
            }
        }

        /// <summary>
        /// Appelée quand le curseur entre sur l'objet
        /// </summary>
        /// <param name="index">L'id de l'objet</param>
        /// <param name="pos">La position de l'objet</param>
        private void OnDraggableItemPointerEnter(int index, Vector3 pos)
        {
            if (!_draggableImg.gameObject.activeSelf)
            {
                _curHoveredItemID = index;
                _curHoveredItemStartPos = pos;
            }

            LootSO loot;

            if (index < _inventoryGrid.childCount)
            {
                loot = _manager.Inventory[index];
            }
            else
            {
                loot = _manager.ItemsInFusionSlots[index - _inventoryGrid.childCount];
            }

            if (loot != null)
            {
                OnLootSlotPointerEnter?.Invoke(loot);
            }
        }

        /// <summary>
        /// Appelée quand le curseur quitte l'objet
        /// </summary>
        private void OnDraggableItemPointerExit()
        {
            if (!_draggableImg.gameObject.activeSelf)
            {
                _curHoveredItemID = -1;
            }

            OnLootSlotPointerExit?.Invoke();
        }

        /// <summary>
        /// Appelée quand le curseur relâche l'objet
        /// </summary>
        private void OnDraggableImgReleased()
        {
            if (_curHoveredItemID < _inventoryGrid.childCount)
            {
                // Si on déplace un objet de l'inventaire,
                // on regarde s'il est déplacé sur un emplacement de fusion

                int targetFusionSlotIndex = -1;

                for (int i = 0; i < _fusionSlotsDropZones.Length; ++i)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(_fusionSlotsDropZones[i], Input.mousePosition))
                    {
                        targetFusionSlotIndex = i;
                        break;
                    }

                }

                if (targetFusionSlotIndex > -1)
                {
                    // Si l'objet est déplacé sur un emplacement de fusion, on regarde s'il a déjà un objet

                    LootSO draggedLoot = _manager.Inventory[_curHoveredItemID];

                    // Si ce n'est pas un matériel de fusion, on abandonne

                    if (_manager.ItemsInFusionSlots[targetFusionSlotIndex] != draggedLoot &&
                        draggedLoot is FossilLootSO or SpermLootSO)
                    {
                        // Si oui, on échange leur place.
                        // Sinon, on se contente d'ajouter l'objet déplacé.

                        if (_manager.ItemsInFusionSlots[targetFusionSlotIndex] != null)
                        {
                            _manager.Inventory.Add(_manager.ItemsInFusionSlots[targetFusionSlotIndex]);
                        }

                        _manager.ItemsInFusionSlots[targetFusionSlotIndex] = draggedLoot;
                        _manager.Inventory.Remove(draggedLoot);
                    }
                }
            }
            else
            {
                // Si on déplace un objet à fusionner,
                // on regarde s'il est déplacé dans l'inventaire ou vers un autre emplacement de fusion

                int curFusionSlotIndex = _curHoveredItemID - _inventoryGrid.childCount;

                if (RectTransformUtility.RectangleContainsScreenPoint(_inventoryGrid, Input.mousePosition))
                {
                    // Si c'est l'inventaire, on le retire de la fusion et on le renvoie à l'inventaire

                    LootSO draggedLoot = _manager.ItemsInFusionSlots[curFusionSlotIndex];
                    _manager.ItemsInFusionSlots[curFusionSlotIndex] = null;
                    _manager.Inventory.Add(draggedLoot);
                }
                else
                {
                    // Si c'est un autre emplacement de fusion, on récupère son ID

                    int targetFusionSlotIndex = -1;

                    for (int i = 0; i < _fusionSlotsDropZones.Length; ++i)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(_fusionSlotsDropZones[i], Input.mousePosition))
                        {
                            targetFusionSlotIndex = i;
                            break;
                        }

                    }

                    if (targetFusionSlotIndex > -1 && targetFusionSlotIndex != curFusionSlotIndex)
                    {
                        // Si ce n'est pas le même emplacement, on échange leur place

                        (_manager.ItemsInFusionSlots[targetFusionSlotIndex], _manager.ItemsInFusionSlots[curFusionSlotIndex]) = (_manager.ItemsInFusionSlots[curFusionSlotIndex], _manager.ItemsInFusionSlots[targetFusionSlotIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Affiche un popup de succès après une fusion réussie
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure créé</param>
        private void ShowSuccessPopup(LustosaurSO lustosaur)
        {
            _lustosaurIcon.sprite = lustosaur.NormalSprite;
            Color col = _textColorPerAttribute[lustosaur.Attribute];
            _lustosaurNameLabel.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(col)}>{lustosaur.name}</color>");

            _successPopup.blocksRaycasts = true;
            _successPopup.interactable = true;
            _successPopup.DOFade(1f, _popupAnimSpeed);
        }

        /// <summary>
        /// Affiche un popup d'échec après une fusion échouée
        /// </summary>
        /// <param name="errorMsg">Message d'erreur à afficher</param>
        private void ShowFailurePopup(string errorMsg)
        {
            _fusionFailErrorLabel.SetText(errorMsg);
            _failurePopup.blocksRaycasts = true;
            _failurePopup.interactable = true;
            _failurePopup.DOFade(1f, _popupAnimSpeed);
        }

        #endregion
    }
}