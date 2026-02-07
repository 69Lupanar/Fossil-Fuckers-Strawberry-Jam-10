using Assets.Scripts.Models;
using Assets.Scripts.ViewModels.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Base
{
    /// <summary>
    /// Gère l'affichage de l'équipe du joueur
    /// </summary>
    public class SleepMenuView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le SleepMenuManager
        /// </summary>
        [SerializeField]
        private SleepMenuManager _manager;

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// Le BaseMenuView
        /// </summary>
        [SerializeField]
        private BaseMenuView _baseMenuView;

        /// <summary>
        /// Le conteneur des btns du menu de la base
        /// </summary>
        [SerializeField]
        private RectTransform _inventoryGrid;

        /// <summary>
        /// Le bouton du luxurosaure sélectionné
        /// </summary>
        [SerializeField]
        private PointerCallbackReceiver _selectedLustosaurHandler;

        /// <summary>
        /// Le bouton du luxurosaure sélectionné
        /// </summary>
        [SerializeField]
        private Image _selectedLustosaurImg;

        /// <summary>
        /// Le ? du luxurosaure sélectionné
        /// </summary>
        [SerializeField]
        private Image _questionMarkImg;

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

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// Init
        /// </summary>
        private void Start()
        {
            InitializePointerHandlers();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée quand le menu de gestion d'équipe est ouvert
        /// </summary>
        public void OnSleepMenuOpen()
        {
            DisplayLustosaurs();
        }

        /// <summary>
        /// Nettoyage à la fermeture de la fenêtre
        /// </summary>
        public void CleanupOnWindowClosed()
        {
            UnselectLustosaur();
            _manager.SelectedLustosaur = null;
        }

        /// <summary>
        /// Si un luxurosaure est sélectionné, lance sa scène adulte.
        /// </summary>
        public void StartSleep()
        {
            _baseMenuView.OpenSexMenu(ReasonForSex.Gallery, SexEnvironment.Base, _manager.SelectedLustosaur);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Initialise les objets à glisser/déposer
        /// </summary>
        private void InitializePointerHandlers()
        {
            _selectedLustosaurHandler.OnPointerDown += OnSelectedItemPointerDown;

            for (int i = 0; i < _inventoryGrid.childCount; ++i)
            {
                PointerCallbackReceiver draggableItem = _inventoryGrid.GetChild(i).GetChild(0).GetComponent<PointerCallbackReceiver>();
                draggableItem.IndexInHierarchy = i;
                draggableItem.OnPointerDown += OnItemPointerDown;
            }

            for (int i = 0; i < _teamSlotsHandlers.Length; ++i)
            {
                PointerCallbackReceiver draggableItem = _teamSlotsHandlers[i];
                draggableItem.IndexInHierarchy = _inventoryGrid.childCount + i;
                draggableItem.OnPointerDown += OnItemPointerDown;
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

                slot.gameObject.SetActive(i < _teamMenuManager.StandbyReserve.Count);

                if (i < _teamMenuManager.StandbyReserve.Count)
                {
                    slot.GetComponent<Image>().sprite = _teamMenuManager.StandbyReserve[i].Sprite;
                }
            }

            for (int i = 0; i < _teamSlotsDropZones.Length; ++i)
            {
                _teamSlotsDropZones[i].GetChild(0).gameObject.SetActive(i < _teamMenuManager.PlayerTeam.Count);

                if (i < _teamMenuManager.PlayerTeam.Count)
                {
                    _teamSlotsDropZones[i].GetChild(0).GetComponent<Image>().sprite = _teamMenuManager.PlayerTeam[i].Sprite;
                }
            }
        }

        /// <summary>
        /// Appelée quand le curseur clque sur l'objet
        /// </summary>
        /// <param name="handler">L'objet cliqué</param>
        private void OnItemPointerDown(PointerCallbackReceiver handler)
        {
            if (_curHoveredItemID != handler.IndexInHierarchy)
            {
                _curHoveredItemID = handler.IndexInHierarchy;
                SetSelectedLustosaur();
            }
            else
            {
                UnselectLustosaur();
            }
        }

        /// <summary>
        /// Appelée par le btn du luxurosaure sélectionné
        /// </summary>
        /// <param name="handler">L'objet cliqué</param>
        private void OnSelectedItemPointerDown(PointerCallbackReceiver handler)
        {
            UnselectLustosaur();
        }

        /// <summary>
        /// Sélectionne le luxurosaure actif
        /// </summary>
        private void SetSelectedLustosaur()
        {
            if (_curHoveredItemID >= _inventoryGrid.childCount)
            {
                _manager.SelectedLustosaur = _teamMenuManager.PlayerTeam[_curHoveredItemID - _inventoryGrid.childCount];
            }
            else
            {
                _manager.SelectedLustosaur = _teamMenuManager.StandbyReserve[_curHoveredItemID];
            }

            _questionMarkImg.enabled = false;
            _selectedLustosaurImg.gameObject.SetActive(true);
            _selectedLustosaurImg.sprite = _manager.SelectedLustosaur.Sprite;
        }

        /// <summary>
        /// Désélectionne le luxurosaure actif
        /// </summary>
        private void UnselectLustosaur()
        {
            _curHoveredItemID = -1;
            _questionMarkImg.enabled = true;
            _selectedLustosaurImg.gameObject.SetActive(false);
        }

        #endregion
    }
}