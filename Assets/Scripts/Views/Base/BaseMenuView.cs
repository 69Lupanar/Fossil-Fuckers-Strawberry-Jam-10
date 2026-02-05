using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'interface du mnu de la base
    /// </summary>
    public class BaseMenuView : MonoBehaviour
    {
        #region Variables Unity

        [Header("Components")]
        [Space(10)]

        /// <summary>
        /// Le BaseMenuManager
        /// </summary>
        [SerializeField]
        private BaseMenuManager _manager;

        /// <summary>
        /// Le GameManager
        /// </summary>
        [SerializeField]
        private GameManager _gameManager;

        /// <summary>
        /// Le canvas racine du menu de la base
        /// </summary>
        [SerializeField]
        private Canvas _baseMenuCanvas;

        /// <summary>
        /// Le canvas du menu de clonage
        /// </summary>
        [SerializeField]
        private Canvas _cloningMenuCanvas;

        /// <summary>
        /// Le canvas du menu de l'équipe
        /// </summary>
        [SerializeField]
        private Canvas _teamMenuCanvas;

        /// <summary>
        /// Le canvas du menu de la gallerie
        /// </summary>
        [SerializeField]
        private Canvas _sleepMenuCanvas;

        /// <summary>
        /// Le conteneur des btns du menu de la base
        /// </summary>
        [SerializeField]
        private GameObject _baseMenuBtnsParent;

        /// <summary>
        /// Fondu en noir
        /// </summary>
        [SerializeField]
        private Image _blackFadeImg;

        /// <summary>
        /// Vitesse du fondu en noir
        /// </summary>
        [SerializeField]

        private float _fadeSpeed = .5f;

        [Space(10)]
        [Header("Components")]
        [Space(10)]

        /// <summary>
        /// L'image déplaçable représentant l'objet à déplacer
        /// </summary>
        [SerializeField]
        private Transform _draggableImg;

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
        /// Les objets dans chaque emplacement de fusion
        /// </summary>
        private LootSO[] _itemsInFusionSlots;

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
            _baseMenuCanvas.enabled = false;
            _cloningMenuCanvas.enabled = false;
            _teamMenuCanvas.enabled = false;
            _sleepMenuCanvas.enabled = false;

            InitializePointerHandlers();
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
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
                    _draggableImg.GetComponent<Image>().sprite = _itemsInFusionSlots[index].Sprite;
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
                _draggableImg.gameObject.SetActive(false);

                if (_curHoveredItemID < _inventoryGrid.childCount)
                {
                    _inventoryGrid.GetChild(_curHoveredItemID).GetChild(0).GetComponent<Image>().enabled = true;
                }
                else
                {
                    _fusionSlotsHandlers[_curHoveredItemID - _inventoryGrid.childCount].GetComponent<Image>().enabled = true;
                }

                _curHoveredItemID = -1;
            }

            _previousMousePos = Input.mousePosition;
        }

        #endregion

        #region Méthodes publiques

        #region Menus

        /// <summary>
        /// Ouvre le menu de la base
        /// </summary>
        public void OpenBaseMenu()
        {
            _manager.TransferInventoryToBase();
            DisplayItems();
            _baseMenuCanvas.enabled = true;
            _baseMenuBtnsParent.SetActive(true);
            _draggableImg.gameObject.SetActive(false);
        }

        /// <summary>
        /// Ouvre le menu de la base
        /// </summary>
        public void CloseBaseMenu()
        {
            _blackFadeImg.DOFade(1f, _fadeSpeed).OnComplete(() =>
            {
                _baseMenuCanvas.enabled = false;
                _gameManager.RespawnPlayer();
                _gameManager.EnableController();
                _blackFadeImg.DOFade(0f, _fadeSpeed);
            });
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenCloningMenu()
        {
            _baseMenuBtnsParent.SetActive(false);
            _cloningMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenTeamMenu()
        {
            _baseMenuBtnsParent.SetActive(false);
            _teamMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void OpenSleepMenu()
        {
            _baseMenuBtnsParent.SetActive(false);
            _sleepMenuCanvas.enabled = true;
        }

        /// <summary>
        /// Ouvre le menu de clonage
        /// </summary>
        public void BackToBaseMenu()
        {
            _baseMenuBtnsParent.SetActive(true);
            _cloningMenuCanvas.enabled = false;
            _teamMenuCanvas.enabled = false;
            _sleepMenuCanvas.enabled = false;

            Cleanup();
        }

        private void Cleanup()
        {
            // TAF:  Penser à réinitialiser les champs remplis comme les slots

            // Menu de clonage

            for (int i = 0; i < _itemsInFusionSlots.Length; ++i)
            {
                _itemsInFusionSlots[i] = null;
            }
        }

        #endregion

        #region Cloning

        /// <summary>
        /// Initialise les objets à glisser/déposer
        /// </summary>
        private void InitializePointerHandlers()
        {
            _itemsInFusionSlots = new LootSO[_fusionSlotsDropZones.Length];

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
                _fusionSlotsHandlers[i].gameObject.SetActive(_itemsInFusionSlots[i] != null);
                _questionMarks[i].SetActive(_itemsInFusionSlots[i] == null);

                if (_itemsInFusionSlots[i] != null)
                {
                    _fusionSlotsHandlers[i].GetComponent<Image>().sprite = _itemsInFusionSlots[i].Sprite;
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

                    if (_itemsInFusionSlots[targetFusionSlotIndex] != draggedLoot)
                    {
                        // Si oui, on échange leur place.
                        // Sinon, on se contente d'ajouter l'objet déplacé.

                        if (_itemsInFusionSlots[targetFusionSlotIndex] != null)
                        {
                            _manager.Inventory.Add(_itemsInFusionSlots[targetFusionSlotIndex]);
                        }

                        _itemsInFusionSlots[targetFusionSlotIndex] = draggedLoot;
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

                    LootSO draggedLoot = _itemsInFusionSlots[curFusionSlotIndex];
                    _itemsInFusionSlots[curFusionSlotIndex] = null;
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

                        (_itemsInFusionSlots[targetFusionSlotIndex], _itemsInFusionSlots[curFusionSlotIndex]) = (_itemsInFusionSlots[curFusionSlotIndex], _itemsInFusionSlots[targetFusionSlotIndex]);
                    }
                }
            }

            DisplayItems();
        }

        #endregion

        #endregion
    }
}