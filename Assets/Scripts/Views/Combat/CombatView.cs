using System;
using System.Collections;
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

        /// <summary>
        /// Vitesse du fondu en noir
        /// </summary>
        [SerializeField]
        private float _fadeSpeed = .5f;

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

        /// <summary>
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _messageFadeSpeed = .5f;

        /// <summary>
        /// Durée d'un message à l'écran
        /// </summary>
        [SerializeField]
        private float _messageDuration = 3f;

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
        /// Vitesse d'animation
        /// </summary>
        [SerializeField]
        private float _FPChangeSpeed = 1f;

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

        [Space(10)]
        [Header("Instructions")]
        [Space(10)]

        /// <summary>
        /// Flèche indiquant le luxurosaure ciblé
        /// </summary>
        [SerializeField]
        private RectTransform _arrowTarget;

        /// <summary>
        /// Décalage de la flèche par rapport luxurosaure ciblé
        /// </summary>
        [SerializeField]
        private Vector3 _arrowTargetOffset;

        /// <summary>
        /// Panel des instructions
        /// </summary>
        [SerializeField]
        private GameObject _instructionsPanel;

        /// <summary>
        /// Les labels de chaque instruction
        /// </summary>
        [SerializeField]
        private GameObject[] _instructionsLabels;

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

        #endregion

        #region Variables d'instance

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

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Appelée par le bouton Fight
        /// </summary>
        public void OnFightBtnClick()
        {

        }

        /// <summary>
        /// Appelée par le bouton Fight
        /// </summary>
        public void OnFormationBtnClick()
        {

        }

        /// <summary>
        /// Appelée par le bouton Escape
        /// </summary>
        public void OnEscapeBtnClick()
        {

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
        /// Appelée quand on clique sur un luxurosaure
        /// </summary>
        public void OnLustosaurBtnClick(int index)
        {

        }

        #endregion

        #region Méthodes privées

        #region Général

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        private void OnCombatStarted()
        {
            InitComponents();
            _manager.CalculateInitiative();
            StartCoroutine(PlayAnimationsCo());
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

        #endregion

        #region Combat

        /// <summary>
        /// Appelée par les boutons des attaques
        /// </summary>
        private void OnAttackBtnClick()
        {

        }

        #endregion

        #region Animations

        /// <summary>
        /// Joue les animations d'intro avant de rendre le contrôle au joueur
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayAnimationsCo()
        {
            _animator.enabled = true;

            // Joue l'intro

            _animator.Play(_introAnim.name);

            // Joue la comparaison des totaux de PV

            yield return WaitCo(new WaitForSeconds(_introAnim.length / 2f));
            _terrainCanvas.enabled = true;
            _healthComparisonCanvas.enabled = true;
            yield return WaitCo(new WaitForSeconds(_introAnim.length / 2f));
            _introCanvas.enabled = false;
            _animator.Play(_healthComparisonAnim.name);
            yield return WaitCo(new WaitForSeconds(.5f));
            yield return IncrementTotalHPLabelsCo(1f);

            // Affiche qui gagne l'initiative

            _playerInitiativeContent.gameObject.SetActive(_manager.PlayerHasInitiative);
            _enemyInitiativeContent.gameObject.SetActive(!_manager.PlayerHasInitiative);

            yield return WaitCo(new WaitForSeconds(1.5f));
            _healthComparisonCanvas.enabled = false;

            // Joue l'arrivée des luxurosaures

            _animator.Play(_lustosaurEntranceAnim.name);
            yield return WaitCo(new WaitForSeconds(_lustosaurEntranceAnim.length));

            // On reprend le contrôle

            _animator.enabled = false;

            // Assigne les stats bonus avant le début du combat
            // si le joueur a perdu des vêtements

            ShowHornyMessageAndStats();

            yield return new WaitForSeconds(1f);

            // On commence le 1er tour

            _manager.StartNewTurn();
        }

        /// <summary>
        /// Assigne les stats bonus avant le début du combat
        /// si le joueur a perdu des vêtements
        /// </summary>
        private void ShowHornyMessageAndStats()
        {
            string msg = string.Empty;
            Sprite sprite = null;
            FightingStats bonusStats = FightingStats.Zero;

            switch (_manager.PlayerHeatLevel)
            {
                case 0:
                    msg = CombatConstants.HORNY_MESSAGES[UnityEngine.Random.Range(0, CombatConstants.HORNY_MESSAGES.Length)];
                    sprite = _hornyIcon;
                    bonusStats = _manager.HornyBonusStats;
                    break;
                case >= 1:
                    msg = CombatConstants.VERY_HORNY_MESSAGES[UnityEngine.Random.Range(0, CombatConstants.VERY_HORNY_MESSAGES.Length)];
                    sprite = _veryHornyIcon;
                    bonusStats = _manager.VeryHornyBonusStats;
                    break;
            }

            if (msg != string.Empty)
            {
                ShowMessage(msg, sprite);
                _manager.ApplySupportStatChange(bonusStats);
                UpdateStatHandlers(_playerDisplayStats, _manager.PlayerTotalAppliedSupportStats, true);
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

            _messagePanel.DOFade(1f, _messageFadeSpeed).OnComplete(() =>
            {
                _messageLabel.StartCoroutine(WaitCo(new WaitForSeconds(_messageDuration), () =>
                {
                    _messagePanel.DOFade(0f, _messageFadeSpeed);
                }));
            });
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
        /// <param name="wfs">Délai</param>
        /// <param name="callback">Action suivante</param>
        /// <returns></returns>
        private IEnumerator WaitCo(WaitForSeconds wfs, Action callback = null)
        {
            yield return wfs;
            callback?.Invoke();
        }

        #endregion

        #endregion
    }
}