using Assets.Scripts.Models.Dinos;
using Assets.Scripts.Models.EventArgs;
using Assets.Scripts.Models.Loot;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.ViewModels.NPCs;
using Assets.Scripts.Views.Base;
using Assets.Scripts.Views.Combat;
using Assets.Scripts.Views.Inventory;
using Assets.Scripts.Views.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Views.Tooltip
{
    /// <summary>
    /// Gère l'affichage des tooltips
    /// </summary>
    public class TooltipView : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le CombatView
        /// </summary>
        [SerializeField]
        private CombatView _combatView;

        /// <summary>
        /// L'InventoryView
        /// </summary>
        [SerializeField]
        private InventoryView _inventoryView;

        /// <summary>
        /// Le CloningMenuView
        /// </summary>
        [SerializeField]
        private CloningMenuView _cloningMenuView;

        /// <summary>
        /// Le MineableSpawner
        /// </summary>
        [SerializeField]
        private MineableSpawner _mineableSpawner;

        /// <summary>
        /// Le TeamMenuView
        /// </summary>
        [SerializeField]
        private TeamMenuView _teamMenuView;

        /// <summary>
        /// Tooltip texte uniquement
        /// </summary>
        [SerializeField]
        private TooltipText _tooltipText;

        /// <summary>
        /// Tooltip affichant les données d'un LootSO
        /// </summary>
        [SerializeField]
        private TooltipLoot _tooltipLoot;

        /// <summary>
        /// Tooltip affichant les données d'un LustosaurSO
        /// </summary>
        [SerializeField]
        private TooltipLustosaur _tooltipLustosaur;

        /// <summary>
        /// Tooltip affichant les données d'un NPCFighter
        /// </summary>
        [SerializeField]
        private TooltipNPCFighter _tooltipNPCFighter;

        /// <summary>
        /// Messages des tooltips liés aux StatSlotInstances
        /// </summary>
        [SerializeField, TextArea(2, 3)]
        private string[] _statsTooltipMsgs;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _combatView.OnStatDisplayPointerEnter += OnStatDisplayPointerEnter;
            _combatView.OnStatDisplayPointerExit += HideTooltips;

            _inventoryView.OnLootSlotPointerEnter += OnLootSlotPointerEnter;
            _inventoryView.OnLootSlotPointerExit += HideTooltips;
            _cloningMenuView.OnLootSlotPointerEnter += OnLootSlotPointerEnter;
            _cloningMenuView.OnLootSlotPointerExit += HideTooltips;

            _teamMenuView.OnLustosaurSlotPointerEnter += OnLustosaurSlotPointerEnter;
            _teamMenuView.OnLustosaurSlotPointerExit += HideTooltips;

            _mineableSpawner.OnBeforeGenerationStart += OnBeforeGenerationStart;
            _mineableSpawner.OnGenerationCompleted += OnGenerationCompleted;
        }

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            HideTooltips();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée quand le curseur entre sur un des StatSlotInstances
        /// </summary>
        /// <param name="index">Index du message à afficher</param>
        private void OnStatDisplayPointerEnter(int index)
        {
            _tooltipText.gameObject.SetActive(true);
            _tooltipText.SetText(_statsTooltipMsgs[index]);
        }

        /// <summary>
        /// Appelée quand le curseur entre sur un des InventorySlotInstances
        /// </summary>
        /// <param name="loot">L'objet à afficher</param>
        private void OnLootSlotPointerEnter(LootSO loot)
        {
            _tooltipLoot.gameObject.SetActive(true);
            _tooltipLoot.SetData(loot);
        }

        /// <summary>
        /// Appelée quand le curseur entre sur un des LustosaurSlotInstances
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure à afficher</param>
        private void OnLustosaurSlotPointerEnter(LustosaurSO lustosaur)
        {
            _tooltipLustosaur.gameObject.SetActive(true);
            _tooltipLustosaur.SetData(lustosaur);
        }

        /// <summary>
        /// Appelée avant le nettoyage et le début de la génération
        /// </summary>
        private void OnBeforeGenerationStart()
        {
            foreach (NPCFighter npc in _mineableSpawner.ActiveNPCFighters)
            {
                npc.GetComponent<MouseHandler>().OnMouseEnterEvent.RemoveListener(OnNPCFighterMouseEnter(npc));
                npc.GetComponent<MouseHandler>().OnMouseExitEvent.RemoveListener(HideTooltips);
            }
        }

        /// <summary>
        /// Appelée une fois la génération terminée
        /// </summary>
        private void OnGenerationCompleted(object sender, GenerationEventArgs _)
        {
            foreach (NPCFighter npc in _mineableSpawner.ActiveNPCFighters)
            {
                npc.GetComponent<MouseHandler>().OnMouseEnterEvent.AddListener(OnNPCFighterMouseEnter(npc));
                npc.GetComponent<MouseHandler>().OnMouseExitEvent.AddListener(HideTooltips);
            }
        }

        /// <summary>
        /// Appelée quand le curseur entre sur un NPCFighter
        /// </summary>
        /// <param name="npc">Le combattant à afficher</param>
        private UnityAction OnNPCFighterMouseEnter(NPCFighter npc)
        {
            return new UnityAction(() =>
            {
                _tooltipNPCFighter.gameObject.SetActive(true);
                _tooltipNPCFighter.SetData(npc);
            });
        }

        /// <summary>
        /// Masque les tooltips
        /// </summary>
        private void HideTooltips()
        {
            _tooltipText.gameObject.SetActive(false);
            _tooltipLoot.gameObject.SetActive(false);
            _tooltipLustosaur.gameObject.SetActive(false);
            _tooltipNPCFighter.gameObject.SetActive(false);
        }

        #endregion
    }
}