using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.Views.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'affichage de l'équipe du joueur
    /// </summary>
    public class TeamMenuView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _manager;

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
        /// Les emplacements d'équipe en haut de l'écran
        /// </summary>
        [SerializeField]
        private TeamSlotInstance[] _teamSlotsInstances;

        /// <summary>
        /// Les zones de dépot des emplacements de l'équipe
        /// </summary>
        [SerializeField]
        private RectTransform[] _teamSlotsDropZones;

        /// <summary>
        /// Les zones de clic des emplacements de l'équipe
        /// </summary>
        [SerializeField]
        private PointerCallbackReceiver[] _teamSlotsHandlers;

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
        private void Awake()
        {
            _manager.OnStart += OnStart;
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            InitializePointerHandlers();
        }

        /// <summary>
        /// Màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !_draggableImg.gameObject.activeSelf && _curHoveredItemID != -1 && _curHoveredItemID < _inventoryGrid.childCount)
            {
                _manager.DiscardLustosaur(_curHoveredItemID);
                _curHoveredItemID = -1;
                DisplayLustosaurs();
                return;
            }

            if (Input.GetMouseButtonDown(0) && _curHoveredItemID != -1)
            {
                _draggableImg.gameObject.SetActive(true);
                _draggableImg.transform.position = _curHoveredItemStartPos;

                if (_curHoveredItemID < _inventoryGrid.childCount)
                {
                    _inventoryGrid.GetChild(_curHoveredItemID).GetChild(0).GetComponent<Image>().enabled = false;
                    _draggableImg.GetComponent<Image>().sprite = _manager.StandbyReserve[_curHoveredItemID].Sprite;
                }
                else
                {
                    int index = _curHoveredItemID - _inventoryGrid.childCount;
                    _teamSlotsHandlers[index].GetComponent<Image>().enabled = false;
                    _draggableImg.GetComponent<Image>().sprite = _manager.PlayerTeam[index].Sprite;
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
                DisplayLustosaurs();

                if (_curHoveredItemID < _inventoryGrid.childCount)
                {
                    _inventoryGrid.GetChild(_curHoveredItemID).GetChild(0).GetComponent<Image>().enabled = true;
                }
                else
                {
                    _teamSlotsHandlers[_curHoveredItemID - _inventoryGrid.childCount].GetComponent<Image>().enabled = true;
                }

                _draggableImg.gameObject.SetActive(false);
                _curHoveredItemID = -1;
            }

            _previousMousePos = Input.mousePosition;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée quand le menu de gestion d'équipe est ouvert
        /// </summary>
        public void OnTeamMenuOpen()
        {
            _draggableImg.gameObject.SetActive(false);
            DisplayLustosaurs();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée dans la Start du manager
        /// </summary>
        private void OnStart()
        {
            DisplayLustosaurs();
        }

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

            for (int i = 0; i < _teamSlotsHandlers.Length; ++i)
            {
                PointerCallbackReceiver draggableItem = _teamSlotsHandlers[i];
                draggableItem.IndexInHierarchy = _inventoryGrid.childCount + i;
                draggableItem.OnPointerEnter += OnDraggableItemPointerEnter;
                draggableItem.OnPointerExit += OnDraggableItemPointerExit;
            }
        }

        /// <summary>
        /// Affiche les luxurosaures de l'équipe
        /// </summary>
        private void DisplayLustosaurs()
        {
            for (int i = 0; i < _inventoryGrid.childCount; ++i)
            {
                _inventoryGrid.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }

            for (int i = 0; i < _teamSlotsDropZones.Length; ++i)
            {
                _teamSlotsDropZones[i].GetChild(0).gameObject.SetActive(false);
            }

            for (int i = 0; i < _inventoryGrid.childCount; ++i)
            {
                Transform slot = _inventoryGrid.GetChild(i).GetChild(0);

                slot.gameObject.SetActive(i < _manager.StandbyReserve.Count);

                if (i < _manager.StandbyReserve.Count)
                {
                    slot.GetComponent<Image>().sprite = _manager.StandbyReserve[i].Sprite;
                }
            }

            for (int i = 0; i < _teamSlotsDropZones.Length; ++i)
            {
                _teamSlotsDropZones[i].GetChild(0).gameObject.SetActive(i < _manager.PlayerTeam.Count);

                if (i < _manager.PlayerTeam.Count)
                {
                    _teamSlotsDropZones[i].GetChild(0).GetComponent<Image>().sprite = _manager.PlayerTeam[i].Sprite;
                    _teamSlotsInstances[i].SetLustosaur(_manager.PlayerTeam[i]);
                }
                else
                {
                    _teamSlotsInstances[i].SetLustosaur(null);
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
                // on regarde s'il est déplacé sur un emplacement de l'équipe

                int targetTeamSlotIndex = -1;

                for (int i = 0; i < _teamSlotsDropZones.Length; ++i)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(_teamSlotsDropZones[i], Input.mousePosition))
                    {
                        targetTeamSlotIndex = i;
                        break;
                    }
                }

                if (targetTeamSlotIndex > -1)
                {
                    // Si l'objet est déplacé sur un emplacement d'équipe, on regarde s'il a déjà un luxurosaure

                    LustosaurSO draggedLustosaur = _manager.StandbyReserve[_curHoveredItemID];

                    if (targetTeamSlotIndex < _manager.PlayerTeam.Count)
                    {
                        // Si oui, on échange leur place.

                        _manager.StandbyReserve.Add(_manager.PlayerTeam[targetTeamSlotIndex]);
                        _manager.PlayerTeam[targetTeamSlotIndex] = draggedLustosaur;
                    }
                    else
                    {
                        // Si l'emplacement est vide,
                        // on se contente d'ajouter le luxurosaure déplacé.

                        _manager.PlayerTeam.Add(draggedLustosaur);
                    }

                    _manager.StandbyReserve.Remove(draggedLustosaur);
                }
            }
            else
            {
                // Si on déplace un luxurosaure de l'équipe,
                // on regarde s'il est déplacé dans la réserve ou vers un autre emplacement

                int curTeamSlotIndex = _curHoveredItemID - _inventoryGrid.childCount;

                if (RectTransformUtility.RectangleContainsScreenPoint(_inventoryGrid, Input.mousePosition))
                {
                    if (_manager.PlayerTeam.Count > 1)
                    {
                        // Si c'est l'inventaire, et que ce n'est pas le seul luxurosaure de l'équipe, on le retire de l'équipe et on le renvoie à l'inventaire

                        LustosaurSO draggedLustosaur = _manager.PlayerTeam[curTeamSlotIndex];
                        _manager.PlayerTeam.RemoveAt(curTeamSlotIndex);
                        _manager.StandbyReserve.Add(draggedLustosaur);
                    }
                    else
                    {
                        // Affiche un message au joueur l'informant qu'il ne peut pas avoir une équipe vide

                        _manager.CancelTryRemoveLastLustosaurFromTeam();
                    }
                }
                else
                {
                    // Si c'est un autre emplacement d'équipe, on récupère son ID

                    int targetTeamSlotIndex = -1;

                    for (int i = 0; i < _teamSlotsDropZones.Length; ++i)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(_teamSlotsDropZones[i], Input.mousePosition))
                        {
                            targetTeamSlotIndex = i;
                            break;
                        }
                    }

                    if (targetTeamSlotIndex > -1 && targetTeamSlotIndex != curTeamSlotIndex)
                    {
                        // Si ce n'est pas le même emplacement, on regarde s'il est déjà occupé

                        if (targetTeamSlotIndex < _manager.PlayerTeam.Count)
                        {
                            // S'il l'est occupé, on échange leur place

                            (_manager.PlayerTeam[targetTeamSlotIndex], _manager.PlayerTeam[curTeamSlotIndex]) = (_manager.PlayerTeam[curTeamSlotIndex], _manager.PlayerTeam[targetTeamSlotIndex]);
                        }
                        else
                        {
                            // Sinon, on l'ajoute en fin de liste
                            _manager.PlayerTeam.Add(_manager.PlayerTeam[curTeamSlotIndex]);
                            _manager.PlayerTeam.RemoveAt(curTeamSlotIndex);
                        }
                    }
                }
            }
        }

        #endregion
    }
}