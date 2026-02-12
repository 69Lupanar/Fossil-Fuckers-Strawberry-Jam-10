using System;
using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Combat;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Managers;
using Assets.Scripts.Views.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.Combat
{
    /// <summary>
    /// Gère l'affichage des combats
    /// </summary>
    public class CombatView : MonoBehaviour
    {
        #region Variables Unity

        [Header("General")]
        [Space(10)]

        /// <summary>
        /// Le GameManagerView
        /// </summary>
        [SerializeField]
        private GameManager _gameManager;

        /// <summary>
        /// Le CombatManager
        /// </summary>
        [SerializeField]
        private CombatManager _manager;

        /// <summary>
        /// Le canvas de l'environnement combat
        /// </summary>
        [SerializeField]
        private Canvas _combatCanvas;

        /// <summary>
        /// Le canvas de l'intro
        /// </summary>
        [SerializeField]
        private Canvas _introCanvas;

        /// <summary>
        /// Le canvas du terrain
        /// </summary>
        [SerializeField]
        private Canvas _terrainCanvas;

        /// <summary>
        /// Le canvas du menu des actions
        /// </summary>
        [SerializeField]
        private Canvas _actionMenuCanvas;

        /// <summary>
        /// Le canvas du menu des attaques
        /// </summary>
        [SerializeField]
        private Canvas _attackListCanvas;

        /// <summary>
        /// Le canvas de comparaison des PVs des équipes
        /// </summary>
        [SerializeField]
        private Canvas _healthComparisonCanvas;

        /// <summary>
        /// Le canvas des instructions
        /// </summary>
        [SerializeField]
        private Canvas _instructionsCanvas;

        /// <summary>
        /// Le canvas de victoire
        /// </summary>
        [SerializeField]
        private Canvas _victoryCanvas;

        /// <summary>
        /// Le canvas de défaite
        /// </summary>
        [SerializeField]
        private Canvas _defeatCanvas;

        /// <summary>
        /// Fondu en noir
        /// </summary>
        [SerializeField]
        private Image _blackFadeImg;

        [Space(10)]
        [Header("Health Comparison")]
        [Space(10)]

        /// <summary>
        /// Label du total du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _playerTotalHPLabel;

        /// <summary>
        /// Label du total de l'ennemi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _enemyTotalHPLabel;

        /// <summary>
        /// Contenu affiché si le joueur parvient à s'échapper
        /// </summary>
        [SerializeField]
        private CanvasGroup _escapedContent;

        /// <summary>
        /// Contenu affiché si le joueur échoue à s'échapper
        /// </summary>
        [SerializeField]
        private CanvasGroup _escapeFailedContent;

        /// <summary>
        /// Contenu affiché si le joueur a l'initiative
        /// </summary>
        [SerializeField]
        private CanvasGroup _playerInitiativeContent;

        /// <summary>
        /// Contenu affiché si l'ennemi a l'initiative
        /// </summary>
        [SerializeField]
        private CanvasGroup _enemyInitiativeContent;

        [Space(10)]
        [Header("Messages")]
        [Space(10)]

        /// <summary>
        /// Panel des messages
        /// </summary>
        [SerializeField]
        private CanvasGroup _messagePanel;

        /// <summary>
        /// Icône à afficher avec le message
        /// </summary>
        [SerializeField]
        private Image _messageIcon;

        /// <summary>
        /// Label du message à afficher
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _messageLabel;

        /// <summary>
        /// Icône du message où le joueur perd partiellement ses vêtements
        /// </summary>
        [SerializeField]
        private Sprite _hornyIcon;

        /// <summary>
        /// Icône du message où le joueur perd tous ses vêtements
        /// </summary>
        [SerializeField]
        private Sprite _veryHornyIcon;

        /// <summary>
        /// Icône du message où un luxurosaure est battu
        /// </summary>
        [SerializeField]
        private Sprite _deathIcon;

        [Space(10)]
        [Header("Terrain")]
        [Space(10)]

        /// <summary>
        /// Label du compteur de PPs du joueur
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _playerFPLabel;

        /// <summary>
        /// Label du compteur de PPs de l'ennemi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _enemyFPLabel;

        /// <summary>
        /// Zones de combat du joueur
        /// </summary>
        [SerializeField]
        private RectTransform[] _playerZones;

        /// <summary>
        /// Zones de combat de l'ennemi
        /// </summary>
        [SerializeField]
        private RectTransform[] _enemyZones;

        /// <summary>
        /// Icônes des luxurosaures du joueur
        /// </summary>
        [SerializeField]
        private CombatLustosaurHandler[] _playerLustosaursHandlers;

        /// <summary>
        /// Icônes des luxurosaures de l'ennemi
        /// </summary>
        [SerializeField]
        private CombatLustosaurHandler[] _enemyLustosaursHandlers;

        /// <summary>
        /// Icônes des stats des luxurosaures du joueur
        /// </summary>
        [SerializeField]
        private StatIconInstance[] _playerDisplayStats;

        /// <summary>
        /// Icônes des stats des luxurosaures de l'ennemi
        /// </summary>
        [SerializeField]
        private StatIconInstance[] _enemyDisplayStats;

        [Space(10)]
        [Header("Actions menu")]
        [Space(10)]

        /// <summary>
        /// Bouton Fight
        /// </summary>
        [SerializeField]
        private Button _fightBtn;

        /// <summary>
        /// Bouton Formation
        /// </summary>
        [SerializeField]
        private Button _formationBtn;

        [Space(10)]
        [Header("Attacks")]
        [Space(10)]

        /// <summary>
        /// Préfab d'une attaque dans la liste déroulante
        /// </summary>
        [SerializeField]
        private GameObject _attackSlotPrefab;

        /// <summary>
        /// Conteneur des attaques dans la liste déroulante
        /// </summary>
        [SerializeField]
        private Transform _attackSlotsParent;

        /// <summary>
        /// Conteneur des attaques inactives dans la liste déroulante
        /// </summary>
        [SerializeField]
        private Transform _inactiveAttackSlotsParent;

        /// <summary>
        /// Icône affichant les dégâts
        /// </summary>
        [SerializeField]
        private CanvasGroup _normalHitIcon;

        /// <summary>
        /// Icône affichant les dégâts
        /// </summary>
        [SerializeField]
        private CanvasGroup _criticalHitIcon;

        /// <summary>
        /// Icône indiquant que l'attaque a échoué
        /// </summary>
        [SerializeField]
        private CanvasGroup _missHitIcon;

        /// <summary>
        /// Label affichant les dégâts
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _normalHitDmgLabel;

        /// <summary>
        /// Label affichant les dégâts
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _criticalHitDmgLabel;

        /// <summary>
        /// Label indiquant que l'attaque a échoué
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _missHitLabel;

        [Space(10)]
        [Header("Instructions")]
        [Space(10)]

        /// <summary>
        /// Flèche indiquant le luxurosaure ciblé
        /// </summary>
        [SerializeField]
        private RectTransform _arrowTarget;

        /// <summary>
        /// Le label affichant l'instruction
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _instructionLabel;

        /// <summary>
        /// Les instructions
        /// </summary>
        [SerializeField]
        private string[] _instructions;

        [Space(10)]
        [Header("Victory & Defeat")]
        [Space(10)]

        /// <summary>
        /// Label de l'EXP gagnée après le combat
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _victoryExpGainedLabel;

        /// <summary>
        /// Label de l'EXP gagnée après le combat
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _defeatExpGainedLabel;

        [Space(10)]
        [Header("Animations")]
        [Space(10)]

        /// <summary>
        /// Vitesse du fondu en noir
        /// </summary>
        [SerializeField]
        private float _blackFadeDuration = .5f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _messageFadeDuration = .5f;

        /// <summary>
        /// Durée d'un message à l'écran
        /// </summary>
        [SerializeField]
        private float _messageDuration = 5f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _FPChangeSpeed = 2f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _lustosaurSupportAnimDuration = .25f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _lustosaurAttackAnimDuration = .5f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _lustosaurAttackStunDuration = .5f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _lustosaurDeathDuration = 1f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _lustosaurSwapAnimDuration = 1f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _arrowTargetMoveDuration = .5f;

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _hitIconFadeDuration = .5f;

        /// <summary>
        /// Durée de l'icône des dégâts à l'écran
        /// </summary>
        [SerializeField]
        private float _hitIconDuration = 1f;

        /// <summary>
        /// Décalage du luxurosaure lorsqu'il attaque ou assigne ses stats de soutien
        /// </summary>
        [SerializeField]
        private Vector2 _lustosaurBounceOffsets;

        /// <summary>
        /// Décalage de la flèche de sélection
        /// </summary>
        [SerializeField]
        private Vector3 _arrowTargetOffset;

        /// <summary>
        /// Animator du canvas
        /// </summary>
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// Anim de l'intro
        /// </summary>
        [SerializeField]
        private AnimationClip _introAnim;

        /// <summary>
        /// Anim de l'apparition des comparateurs de PV
        /// </summary>
        [SerializeField]
        private AnimationClip _healthComparisonAnim;

        /// <summary>
        /// Anim de l'arrivée des luxurosaures sur le terrain
        /// </summary>
        [SerializeField]
        private AnimationClip _lustosaurEntranceAnim;

        /// <summary>
        /// Courbe permettant de faire revenir une valeur à son état initial
        /// après un tween
        /// </summary>
        [SerializeField]
        private AnimationCurve _bounceCurve;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Indique quels luxurosaures peuvent être sélectionnés (aucun, joueur, adversaire, les deux)
        /// </summary>
        private CombatSelectionLockLevel _selectionLockLevel = CombatSelectionLockLevel.None;

        /// <summary>
        /// true si le joueur est en train de changer la formation de ses luxurosaures
        /// </summary>
        private bool _isChangingFormation;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _manager.OnCombatStarted += OnCombatStarted;
            _combatCanvas.enabled = false;
        }

        ///// <summary>
        ///// màj à chaque frame
        ///// </summary>
        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(1) && _isChangingFormation)
        //    {
        //        // TAF: Au clic droit, annule la décision de formation
        //        // et retourne au menu d'action

        //        CancelChangeFormation();
        //    }
        //}

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée par le bouton Fight
        /// </summary>
        public void OnFightBtnClick()
        {
            _actionMenuCanvas.enabled = false;
            _attackListCanvas.enabled = true;
            _selectionLockLevel = CombatSelectionLockLevel.Player;
            _manager.SelectAllyLustosaur(0);
            ShowArrowTarget(_playerLustosaursHandlers, 0);
            ShowInstruction(0);
        }

        /// <summary>
        /// Appelée par le bouton Fight
        /// </summary>
        public void OnFormationBtnClick()
        {
            // Pas implémenté pour la jam ; pas assez de temps,
            // et le code est pas assez robuste pour ça

            //_isChangingFormation = true;
            //_actionMenuCanvas.enabled = false;
            //_selectionLockLevel = CombatSelectionLockLevel.Player;
            //_manager.SelectAllyLustosaur(0);
            //ShowArrowTarget(_playerLustosaursHandlers, 0);
            //ShowInstruction(1);
        }

        /// <summary>
        /// Appelée par le bouton End Turn
        /// </summary>
        public void OnEndTurnBtnClick()
        {
            if (_manager.PlayerHasInitiative)
            {
                _actionMenuCanvas.enabled = false;
                ProcessOpponentTurn();
            }
            else
            {
                StartCoroutine(StartNewTurnCo());
            }
        }

        /// <summary>
        /// Appelée par le bouton Submit
        /// </summary>
        public void OnSubmitBtnClick()
        {

        }

        /// <summary>
        /// Appelée par le bouton Yes
        /// </summary>
        public void OnYesBtnClick()
        {

        }

        /// <summary>
        /// Appelée par le bouton No
        /// </summary>
        public void OnNoBtnClick()
        {

        }

        /// <summary>
        /// Appelée par le bouton Next
        /// </summary>
        public void OnNextBtnClick()
        {

        }

        /// <summary>
        /// Appelée quand le curseur survole un luxurosaure
        /// </summary>
        public void OnLustosaurBtnHover(int index)
        {
            if (index > 0)
            {
                if (_selectionLockLevel == CombatSelectionLockLevel.Player || _selectionLockLevel == CombatSelectionLockLevel.Both)
                {
                    ShowArrowTarget(_playerLustosaursHandlers, index - 1);
                }
            }
            else
            {
                if (_selectionLockLevel == CombatSelectionLockLevel.Enemy || _selectionLockLevel == CombatSelectionLockLevel.Both)
                {
                    ShowArrowTarget(_enemyLustosaursHandlers, -index - 1);
                }
            }
        }

        /// <summary>
        /// Appelée quand on clique sur un luxurosaure
        /// </summary>
        public void OnLustosaurBtnClick(int index)
        {
            if (index > 0)
            {
                if (_selectionLockLevel == CombatSelectionLockLevel.Player || _selectionLockLevel == CombatSelectionLockLevel.Both)
                {
                    _manager.SelectAllyLustosaur(index - 1);
                    PopulateAttacksList();
                }
            }
            else
            {
                if (_selectionLockLevel == CombatSelectionLockLevel.Enemy || _selectionLockLevel == CombatSelectionLockLevel.Both)
                {
                    _manager.SelectEnemyLustosaur(-index - 1);

                    switch (_manager.BattleState)
                    {
                        case BattleState.Ongoing:
                            _attackListCanvas.enabled = false;
                            _instructionsCanvas.enabled = false;
                            HideEnemyResistances();

                            ConductAttack(_manager.SelectedAlly, _manager.SelectedEnemy, _manager.SelectedAttack, true);
                            break;

                        case BattleState.Victory:
                            // TAF : Lancer scène adulte
                            break;
                    }
                }
            }
        }

        #endregion

        #region Méthodes privées

        #region UI

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        private void OnCombatStarted()
        {
            InitComponents();
            _manager.CalculateInitiative();
            StartCoroutine(PlayIntroAnimationsCo());
        }

        /// <summary>
        /// init
        /// </summary>
        private void InitComponents()
        {
            _animator.enabled = false;
            _combatCanvas.enabled = true;
            _introCanvas.enabled = true;

            _terrainCanvas.enabled = false;
            _healthComparisonCanvas.enabled = false;
            _actionMenuCanvas.enabled = false;
            _attackListCanvas.enabled = false;
            _instructionsCanvas.enabled = false;
            _victoryCanvas.enabled = false;
            _defeatCanvas.enabled = false;
            _playerInitiativeContent.alpha = 0f;
            _enemyInitiativeContent.alpha = 0f;
            _escapedContent.alpha = 0f;
            _escapeFailedContent.alpha = 0f;
            _playerInitiativeContent.gameObject.SetActive(true);
            _enemyInitiativeContent.gameObject.SetActive(true);
            _escapedContent.gameObject.SetActive(false);
            _escapeFailedContent.gameObject.SetActive(false);

            _playerFPLabel.SetText("0");
            _enemyFPLabel.SetText("0");
            _playerTotalHPLabel.SetText("0");
            _enemyTotalHPLabel.SetText("0");
            _victoryExpGainedLabel.SetText("0");
            _defeatExpGainedLabel.SetText("0");

            UpdateStatHandlers(_playerDisplayStats, FightingStats.Zero, false);
            UpdateStatHandlers(_enemyDisplayStats, FightingStats.Zero, false);
            SetLustosaursHandlers();

            _selectionLockLevel = CombatSelectionLockLevel.None;
        }

        /// <summary>
        /// Assigne les valeurs des icônes de stats
        /// </summary>
        /// <param name="statIcons">Les icônes</param>
        /// <param name="stats">Les stats</param>
        /// <param name="animate">true pour incrémenter la valeur au fil du temps, sinon l'assigne directement</param>
        private void UpdateStatHandlers(StatIconInstance[] statIcons, FightingStats stats, bool animate)
        {
            statIcons[0].SetValue(stats.Health, animate);
            statIcons[1].SetValue(stats.Attack, animate);
            statIcons[2].SetValue(stats.Defense, animate);
            statIcons[3].SetValue(stats.CriticalHitRate, animate);
            statIcons[4].SetValue(stats.Accuracy, animate);
            statIcons[5].SetValue(stats.Evasion, animate);
        }

        /// <summary>
        /// Assigne les sprites des luxurosaurs
        /// </summary>
        private void SetLustosaursHandlers()
        {
            for (int i = 0; i < _manager.PlayerTeam.Length; ++i)
            {
                LustosaurSO lustosaur = _manager.PlayerTeam[i];
                CombatLustosaurHandler handler = _playerLustosaursHandlers[i];
                handler.gameObject.SetActive(lustosaur != null);

                if (lustosaur != null)
                {
                    handler.SetLustosaurSprite(_manager.PlayerHeatLevel == -1 ? lustosaur.NormalSprite : lustosaur.HornySprite);
                    handler.HideResistanceIcon();
                }
            }

            for (int i = 0; i < _manager.EnemyTeam.Length; ++i)
            {
                LustosaurSO lustosaur = _manager.EnemyTeam[i];
                CombatLustosaurHandler handler = _enemyLustosaursHandlers[i];
                handler.gameObject.SetActive(lustosaur != null);

                if (lustosaur != null)
                {
                    handler.SetLustosaurSprite(lustosaur.NormalSprite);
                    handler.HideResistanceIcon();
                }
            }
        }

        /// <summary>
        /// Remplit la liste des attaques
        /// </summary>
        private void PopulateAttacksList()
        {
            int activeChildren = _attackSlotsParent.childCount;

            // Retire les instances en trop

            for (int i = activeChildren - 1; i >= 0; --i)
            {
                if (i > _manager.SelectedAlly.LearnedAttacks.Count - 1)
                {
                    _attackSlotsParent.GetChild(i).SetParent(_inactiveAttackSlotsParent);
                }
            }

            // Recycle les instances restantes,
            // ou en crée de nouvelles s'il n'y en a pas assez

            for (int i = 0; i < _manager.SelectedAlly.LearnedAttacks.Count; ++i)
            {
                AttackSO attack = _manager.SelectedAlly.LearnedAttacks[i];
                AttackSlotInstance instance;

                if (i < _attackSlotsParent.childCount)
                {
                    instance = _attackSlotsParent.GetChild(i).GetComponent<AttackSlotInstance>();
                }
                else
                {
                    instance = Instantiate(_attackSlotPrefab, _attackSlotsParent).GetComponent<AttackSlotInstance>();
                }

                instance.SetData(attack, OnAttackBtnClick);
                instance.SetInteractable(attack.Cost <= _manager.PlayerFP);
            }
        }

        /// <summary>
        /// Assigne les stats bonus avant le début du combat
        /// si le joueur a perdu des vêtements
        /// </summary>
        private void ShowHornyMessage()
        {
            string msg = string.Empty;
            Sprite sprite = null;

            switch (_manager.PlayerHeatLevel)
            {
                case 0:
                    msg = CombatConstants.HORNY_MESSAGES[UnityEngine.Random.Range(0, CombatConstants.HORNY_MESSAGES.Length)];
                    sprite = _hornyIcon;
                    break;
                case >= 1:
                    msg = CombatConstants.VERY_HORNY_MESSAGES[UnityEngine.Random.Range(0, CombatConstants.VERY_HORNY_MESSAGES.Length)];
                    sprite = _veryHornyIcon;
                    break;
            }

            if (msg != string.Empty)
            {
                ShowMessage(msg, sprite);
                UpdateStatHandlers(_playerDisplayStats, _manager.HornySupportStats + _manager.PlayerSupportStats, true);
            }
        }

        /// <summary>
        /// Affiche un message à l'écran
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="sprite">Icône</param>
        private void ShowMessage(string msg, Sprite sprite)
        {
            // Pour interrompre le précédent message

            _messageLabel.StopAllCoroutines();
            _messagePanel.DOKill();

            _messagePanel.alpha = 0f;
            _messageLabel.SetText(msg);
            _messageIcon.sprite = sprite;

            _messagePanel.DOFade(1f, _messageFadeDuration).OnComplete(() =>
            {
                _messageLabel.StartCoroutine(WaitCo(_messageDuration, () =>
                {
                    _messagePanel.DOFade(0f, _messageFadeDuration);
                }));
            });
        }

        /// <summary>
        /// Affiche la flèche de sélection au dessus du luxurosaure correspondant
        /// </summary>
        /// <param name="handlers">La liste des handlers</param>
        /// <param name="index">La position du luxurosaure dans la liste</param>
        private void ShowArrowTarget(CombatLustosaurHandler[] handlers, int index)
        {
            _arrowTarget.DOLocalMove(handlers[index].transform.position + _arrowTargetOffset, _arrowTargetMoveDuration);
        }

        /// <summary>
        /// Affiche l'instruction en haut de l'écran
        /// </summary>
        /// <param name="index">L'id de l'instruction</param>
        private void ShowInstruction(int index)
        {
            _instructionsCanvas.enabled = true;
            _instructionLabel.SetText(_instructions[index]);
        }

        /// <summary>
        /// Affiche les résistances et vulnérabilités de chaque défenser
        /// en fonction de l'attaque sélectionnée
        /// </summary>
        /// <param name="resistances">Les niveaux de résistance de chaque ennemi</param>
        private void ShowEnemyResistances(LustosaurResistance[] resistances)
        {
            for (int i = 0; i < _manager.EnemyTeam.Length; ++i)
            {
                LustosaurSO lustosaur = _manager.EnemyTeam[i];
                CombatLustosaurHandler handler = _enemyLustosaursHandlers[i];

                if (lustosaur != null)
                {
                    switch (resistances[i])
                    {
                        case LustosaurResistance.Neutral:
                            handler.SetNeutralIcon();
                            break;
                        case LustosaurResistance.Resistant:
                            handler.SetResistantIcon();
                            break;
                        case LustosaurResistance.Vulnerable:
                            handler.SetVulnerableIcon();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Masque les résistances
        /// </summary>
        private void HideEnemyResistances()
        {
            for (int i = 0; i < _manager.EnemyTeam.Length; ++i)
            {
                LustosaurSO lustosaur = _manager.EnemyTeam[i];
                CombatLustosaurHandler handler = _enemyLustosaursHandlers[i];

                if (lustosaur != null)
                {
                    handler.HideResistanceIcon();
                }
            }
        }

        /// <summary>
        /// Affiche l'écran de victoire
        /// </summary>
        private void ShowVictoryScreen()
        {

        }

        /// <summary>
        /// Affiche l'écran de défaite
        /// </summary>
        private void ShowDefeatScreen()
        {

        }

        #endregion

        #region Combat

        /// <summary>
        /// Commence un nouveau tour
        /// </summary>
        private IEnumerator StartNewTurnCo()
        {
            _manager.StartNewTurn();

            StartCoroutine(IncrementFPLabelCo(_playerFPLabel, _manager.PlayerFP, _FPChangeSpeed));
            StartCoroutine(IncrementFPLabelCo(_enemyFPLabel, _manager.EnemyFP, _FPChangeSpeed));
            UpdateStatHandlers(_playerDisplayStats, _manager.HornySupportStats + _manager.PlayerSupportStats, true);
            UpdateStatHandlers(_enemyDisplayStats, _manager.EnemySupportStats, true);

            if (_manager.PlayerHasInitiative)
            {
                yield return PlaySupportAnimationCo(_playerLustosaursHandlers);
                yield return PlaySupportAnimationCo(_enemyLustosaursHandlers);
                ShowActionMenu();
            }
            else
            {
                yield return PlaySupportAnimationCo(_enemyLustosaursHandlers);
                yield return PlaySupportAnimationCo(_playerLustosaursHandlers);
                ProcessOpponentTurn();
            }
        }

        /// <summary>
        /// Affiche le menu des actions du joueur
        /// </summary>
        private void ShowActionMenu()
        {
            _actionMenuCanvas.enabled = true;
            _fightBtn.interactable = _manager.ActiveFighterHasEnoughFPToAttack(true);
            _formationBtn.interactable = !_manager.PlayerHasChangedFormationThisTurn;
        }

        /// <summary>
        /// Appelée par les boutons des attaques
        /// </summary>
        /// <param name="index">La position de l'index dans la liste</param>
        private void OnAttackBtnClick(int index)
        {
            _selectionLockLevel = CombatSelectionLockLevel.Enemy;
            _manager.SelectAttack(_manager.SelectedAlly, index);
            LustosaurResistance[] enemyResistances = _manager.GetEnemyResistances(_manager.SelectedAttack.Attribute);

            ShowEnemyResistances(enemyResistances);
            ShowArrowTarget(_enemyLustosaursHandlers, 0);
            ShowInstruction(2);
        }

        /// <summary>
        /// Conduit une attaque
        /// </summary>
        /// <param name="attacker">L'attaquant</param>
        /// <param name="defender">Le défenseur</param>
        /// <param name="attack">L'attaque</param>
        /// <param name="isPlayerTurn">true si c'est le tour du joueur</param>
        private void ConductAttack(LustosaurSO attacker, LustosaurSO defender, AttackSO attack, bool isPlayerTurn)
        {
            _manager.ConductAttack(attacker, defender, attack, isPlayerTurn, out int dmg, out bool criticalHit, out bool miss);

            // Affiche le message d'attaque

            string msg = isPlayerTurn ? CombatConstants.ALLY_ATTACK_MSG : CombatConstants.ENEMY_ATTACK_MSG;
            ShowMessage(string.Format(msg, attacker.name, defender.name), null);

            // Anime l'attaque

            StartCoroutine(PlayAttackAnimationCo(attacker, defender, dmg, criticalHit, miss, isPlayerTurn));
        }

        /// <summary>
        /// Exécute un tour pour l'adversaire
        /// </summary>
        private void ProcessOpponentTurn()
        {

        }

        /// <summary>
        /// Annule le changement de formation et revient au menu des actions
        /// </summary>
        private void CancelChangeFormation()
        {
            _isChangingFormation = false;
            _instructionsCanvas.enabled = false;
            _actionMenuCanvas.enabled = true;
        }

        #endregion

        #region Animations

        /// <summary>
        /// Joue les animations d'intro avant de rendre le contrôle au joueur
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayIntroAnimationsCo()
        {
            _animator.enabled = true;

            // Joue l'intro

            _animator.Play(_introAnim.name);

            // Joue la comparaison des totaux de PV

            yield return new WaitForSeconds(_introAnim.length / 2f);
            _terrainCanvas.enabled = true;
            _healthComparisonCanvas.enabled = true;
            yield return new WaitForSeconds(_introAnim.length / 2f);
            _introCanvas.enabled = false;
            _animator.Play(_healthComparisonAnim.name);
            yield return new WaitForSeconds(.5f);
            yield return IncrementTotalHPLabelsCo(1f);

            // Affiche qui gagne l'initiative

            _playerInitiativeContent.gameObject.SetActive(_manager.PlayerHasInitiative);
            _enemyInitiativeContent.gameObject.SetActive(!_manager.PlayerHasInitiative);

            yield return new WaitForSeconds(1.5f);
            _healthComparisonCanvas.enabled = false;

            // Joue l'arrivée des luxurosaures

            _animator.Play(_lustosaurEntranceAnim.name);
            yield return new WaitForSeconds(_lustosaurEntranceAnim.length);

            // On reprend le contrôle

            _animator.enabled = false;

            // Assigne les stats bonus avant le début du combat
            // si le joueur a perdu des vêtements

            ShowHornyMessage();

            yield return new WaitForSeconds(1f);

            // On commence le 1er tour

            yield return StartNewTurnCo();
        }

        /// <summary>
        /// Jour l'animation de support des luxurosaures
        /// </summary>
        /// <param name="handlers">Les luxurosaures à animer</param>
        private IEnumerator PlaySupportAnimationCo(CombatLustosaurHandler[] handlers)
        {
            float delay = 0f;

            // Joue une animation de bond pour chaque luxurosaure encore en vie

            if (handlers[1] != null)
            {
                delay += _lustosaurSupportAnimDuration;
                Transform t1 = handlers[1].transform;
                t1.DOLocalMoveY(t1.localPosition.y + _lustosaurBounceOffsets.y, _lustosaurSupportAnimDuration).SetEase(_bounceCurve).OnComplete(() =>
                {
                    if (handlers[2] != null)
                    {
                        delay += _lustosaurSupportAnimDuration;
                        Transform t2 = handlers[2].transform;
                        t2.DOLocalMoveY(t2.localPosition.y + _lustosaurBounceOffsets.y, _lustosaurSupportAnimDuration).SetEase(_bounceCurve);
                    }
                });
            }

            if (handlers[2] != null)
            {
                delay += _lustosaurSupportAnimDuration;
                Transform t2 = handlers[2].transform;
                t2.DOLocalMoveY(t2.localPosition.y + _lustosaurBounceOffsets.y, _lustosaurSupportAnimDuration).SetEase(_bounceCurve).OnComplete(() =>
                {
                    if (handlers[1] != null)
                    {
                        delay += _lustosaurSupportAnimDuration;
                        Transform t1 = handlers[1].transform;
                        t1.DOLocalMoveY(t1.localPosition.y + _lustosaurBounceOffsets.y, _lustosaurSupportAnimDuration).SetEase(_bounceCurve);
                    }
                });
            }

            // Pour la coroutine, on attend _lustosaurSupportAnimSpeed * le nb d'anims jouées

            yield return new WaitForSeconds(delay);
        }

        /// <summary>
        /// Affiche les animations liées à l'attaque
        /// </summary>
        /// <param name="attacker">L'attaquant</param>
        /// <param name="defender">Le défenseur</param>
        /// <param name="dmg">Les dégâts infligés</param>
        /// <param name="criticalHit">true s'il s'agit d'un coup critique</param>
        /// <param name="miss">true si l'attaque a échoué</param>
        /// <param name="isPlayerTurn">true si c'est le tour du joueur</param>
        private IEnumerator PlayAttackAnimationCo(LustosaurSO attacker, LustosaurSO defender, int dmg, bool criticalHit, bool miss, bool isPlayerTurn)
        {
            // Modifie le compteur de FP correspondant

            if (isPlayerTurn)
            {
                StartCoroutine(IncrementFPLabelCo(_playerFPLabel, _manager.PlayerFP, _FPChangeSpeed));
            }
            else
            {
                StartCoroutine(IncrementFPLabelCo(_enemyFPLabel, _manager.EnemyFP, _FPChangeSpeed));
            }

            // Récupère les bons components

            CombatLustosaurHandler attackerHandler = isPlayerTurn ? _playerLustosaursHandlers[_manager.SelectedAllyIndex] : _enemyLustosaursHandlers[_manager.SelectedEnemyIndex];
            CombatLustosaurHandler defenderHandler = isPlayerTurn ? _enemyLustosaursHandlers[_manager.SelectedEnemyIndex] : _playerLustosaursHandlers[_manager.SelectedAllyIndex];

            Transform attackerT = attackerHandler.transform;
            Transform defenderT = defenderHandler.transform;

            // Déplace l'attaquant et le défenseur

            attackerT.DOLocalMoveX(attackerT.localPosition.x + _lustosaurBounceOffsets.x, _lustosaurAttackAnimDuration).SetEase(_bounceCurve);
            yield return new WaitForSeconds(_lustosaurAttackAnimDuration / 2f);
            defenderT.DOLocalMoveX(defenderT.localPosition.x - _lustosaurBounceOffsets.x, _lustosaurAttackAnimDuration).OnComplete(() =>
            {
                StartCoroutine(WaitCo(_lustosaurAttackStunDuration, () =>
                {
                    defenderT.DOLocalMoveX(defenderT.localPosition.x + _lustosaurBounceOffsets.x, _lustosaurAttackAnimDuration);
                }));
            });

            // Affiche le montant de dégâts

            if (miss)
            {
                _missHitIcon.DOFade(1f, _hitIconFadeDuration).OnComplete(() =>
                {
                    _missHitLabel.StartCoroutine(WaitCo(_hitIconDuration, () =>
                    {
                        _missHitIcon.DOFade(0f, _hitIconFadeDuration);
                    }));
                });
            }
            else if (criticalHit)
            {
                _criticalHitDmgLabel.SetText(dmg.ToString());

                _criticalHitIcon.DOFade(1f, _hitIconFadeDuration).OnComplete(() =>
                {
                    _criticalHitDmgLabel.StartCoroutine(WaitCo(_hitIconDuration, () =>
                    {
                        _criticalHitIcon.DOFade(0f, _hitIconFadeDuration);
                    }));
                });
            }
            else
            {
                _normalHitDmgLabel.SetText(dmg.ToString());

                _normalHitIcon.DOFade(1f, _hitIconFadeDuration).OnComplete(() =>
                {
                    _normalHitDmgLabel.StartCoroutine(WaitCo(_hitIconDuration, () =>
                    {
                        _normalHitIcon.DOFade(0f, _hitIconFadeDuration);
                    }));
                });
            }

            // Modifie la barre de vie

            defenderHandler.SetHealthValue((float)defender.CurHealth / (float)defender.CurFightingStats.Health);

            // Après les animations, si le défenseur n'a plus de vie, on le fait disparaître

            yield return new WaitForSeconds(_lustosaurAttackAnimDuration + _lustosaurAttackStunDuration + _lustosaurAttackAnimDuration / 2f);

            if (defender.CurHealth == 0)
            {
                ShowMessage(string.Format(CombatConstants.LUSTOSAUR_DEFEATED_MSG, defender.name), _deathIcon);
                defenderHandler.SetAlpha(0f, true);
                yield return new WaitForSeconds(_lustosaurDeathDuration);
                defenderHandler.gameObject.SetActive(false);
            }

            // Redonne le contrôle à l'attaquant

            if (isPlayerTurn)
            {
                if (_manager.BattleState == BattleState.Victory)
                {
                    ShowVictoryScreen();
                }
                else
                {
                    ShowActionMenu();
                }
            }
            else
            {
                if (_manager.BattleState == BattleState.Defeat)
                {
                    ShowDefeatScreen();
                }
                else
                {
                    ProcessOpponentTurn();
                }
            }
        }

        /// <summary>
        /// Incrémente le label des PPs d'un combattant au fil du temps
        /// </summary>
        /// <param name="fpLabel">Le label à incrémenter</param>
        /// <param name="speed">vitesse d'animation</param>
        private IEnumerator IncrementFPLabelCo(TextMeshProUGUI fpLabel, int newValue, float speed)
        {
            int oldValue = int.Parse(fpLabel.text);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * speed;

                fpLabel.SetText(Mathf.RoundToInt(Mathf.Lerp(oldValue, newValue, t)).ToString());

                yield return null;
            }

            fpLabel.SetText(newValue.ToString());
        }

        /// <summary>
        /// Incrémente les labels des totaux au fil du temps
        /// </summary>
        /// <param name="speed">vitesse d'animation</param>
        private IEnumerator IncrementTotalHPLabelsCo(float speed)
        {
            _playerTotalHPLabel.SetText("0");
            _enemyTotalHPLabel.SetText("0");

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * speed;

                _playerTotalHPLabel.SetText(Mathf.RoundToInt(Mathf.Lerp(0, _manager.PlayerTotalHP, t)).ToString());
                _enemyTotalHPLabel.SetText(Mathf.RoundToInt(Mathf.Lerp(0, _manager.EnemyTotalHP, t)).ToString());

                yield return null;
            }

            _playerTotalHPLabel.SetText(_manager.PlayerTotalHP.ToString());
            _enemyTotalHPLabel.SetText(_manager.EnemyTotalHP.ToString());
        }

        /// <summary>
        /// Délai
        /// </summary>
        /// <param name="duration">Délai</param>
        /// <param name="callback">Action suivante</param>
        /// <returns></returns>
        private IEnumerator WaitCo(float duration, Action callback = null)
        {
            yield return new WaitForSeconds(duration);
            callback?.Invoke();
        }

        #endregion

        #endregion
    }
}