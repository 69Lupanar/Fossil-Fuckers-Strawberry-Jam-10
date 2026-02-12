using Assets.Scripts.Models.EventArgs;
using Assets.Scripts.ViewModels.NPCs;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les événements et le relancement de la partie
    /// en cas de défaite
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le BaseMenuManager
        /// </summary>
        [SerializeField]
        private BaseMenuManager _baseMenuManager;

        /// <summary>
        /// Le MineableSpawner
        /// </summary>
        [SerializeField]
        private MineableSpawner _mineableSpawner;

        /// <summary>
        /// L'InventoryManager
        /// </summary>
        [SerializeField]
        private InventoryManager _inventoryManager;

        /// <summary>
        /// Le DialogueManager
        /// </summary>
        [SerializeField]
        private DialogueManager _dialogueManager;

        /// <summary>
        /// Le CombatManager
        /// </summary>
        [SerializeField]
        private CombatManager _combatManager;

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _statsManager;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _controller;

        /// <summary>
        /// Le point de spawn
        /// </summary>
        [SerializeField]
        private Transform _spawnPoint;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le PNJ combattant en cours de dialogue,
        /// gardé temporairement le temps de son dialogue
        /// </summary>
        private NPCFighter _tempNPCFighter;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _mineableSpawner.OnBeforeGenerationStart += OnBeforeGenerationStart;
            _mineableSpawner.OnGenerationCompleted += OnGenerationCompleted;
        }

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            RespawnPlayer();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void DisableController()
        {
            _controller.Disable();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void EnableController()
        {
            _controller.Enable();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void ResetPlayer()
        {
            EnableController();
            RespawnPlayer();
            _statsManager.LoseEXP(_statsManager.CurEXPPoints);
            _statsManager.RestoreStats();
            _inventoryManager.Clear();
        }

        /// <summary>
        /// Ramène le joueur au point de départ
        /// et vide son inventaire
        /// </summary>
        public void RespawnPlayer()
        {
            _controller.transform.position = _spawnPoint.position;
        }

        /// <summary>
        /// Transfère tous les objets de l'inventaire du joueur
        /// vers celui de la base
        /// </summary>
        public void TransferInventoryToBase()
        {

        }

        /// <summary>
        /// Ramène le joueur au point de départ,
        /// vide son inventaire
        /// et régénère la mine
        /// </summary>
        /// <param name="loseAllEXP">true si l'exp du joueur doit retomber à 0</param>
        public void RestartLevel(bool loseAllEXP)
        {
            _mineableSpawner.Generate();
            EnableController();
            RespawnPlayer();

            if (loseAllEXP)
            {
                _statsManager.LoseEXP(_statsManager.CurEXPPoints);
            }

            _statsManager.RestoreStats();
            _inventoryManager.Clear();
        }

        /// <summary>
        /// Quitte l'écran de combat
        /// </summary>
        public void OnQuitCombatScreen()
        {
            _tempNPCFighter.ReturnToPool();
            _tempNPCFighter = null;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelée avant le nettoyage et le début de la génération
        /// </summary>
        private void OnBeforeGenerationStart()
        {
            foreach (NPCFighter npc in _mineableSpawner.ActiveNPCFighters)
            {
                npc.OnInteracted -= OnNPCFighterInteracted;
            }
        }

        /// <summary>
        /// Appelée une fois la génération terminée
        /// </summary>
        private void OnGenerationCompleted(object sender, GenerationEventArgs _)
        {
            foreach (NPCFighter npc in _mineableSpawner.ActiveNPCFighters)
            {
                npc.OnInteracted += OnNPCFighterInteracted;
            }
        }

        /// <summary>
        /// Appelée quand le joueur interagit avec un NPC
        /// </summary>
        /// <param name="npcTeam"></param>
        /// <param name="npcDialogue"></param>
        private void OnNPCFighterInteracted(NPCFighter npcFighter)
        {
            DisableController();
            _tempNPCFighter = npcFighter;
            _dialogueManager.StartDialogue(npcFighter.DialogueLines);
            _dialogueManager.OnDialogueEnded += OnDialogueEnded;
        }

        /// <summary>
        /// Appelée une fois un dialogue terminé
        /// </summary>
        private void OnDialogueEnded()
        {
            //EnableController();
            _combatManager.StartCombat(_tempNPCFighter.Team);
            _dialogueManager.OnDialogueEnded -= OnDialogueEnded;
        }

        #endregion
    }
}