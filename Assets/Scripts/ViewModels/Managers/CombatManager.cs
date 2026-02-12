using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Combat;
using Assets.Scripts.Models.Dinos;
using Assets.Scripts.ViewModels.Extensions;
using Assets.Scripts.ViewModels.Player;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère les combats
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un combat est commencé
        /// </summary>
        public Action OnCombatStarted { get; set; }

        #endregion

        #region Propriétés

        /// <summary>
        /// Etat du combat en cours
        /// </summary>
        public BattleState BattleState { get; private set; }

        /// <summary>
        /// L'équipe du joueur
        /// </summary>
        public LustosaurSO[] PlayerTeam { get; private set; }

        /// <summary>
        /// L'équipe de l'adversaire
        /// </summary>
        public LustosaurSO[] EnemyTeam { get; private set; }

        /// <summary>
        /// Le luxurosaure du joueur sélectionné
        /// </summary>
        public LustosaurSO SelectedAlly { get; private set; }

        /// <summary>
        /// Le luxurosaure de l'adversaire sélectionné
        /// </summary>
        public LustosaurSO SelectedEnemy { get; private set; }

        /// <summary>
        /// L'index du luxurosaure du joueur sélectionné
        /// </summary>
        public int SelectedAllyIndex { get; private set; }

        /// <summary>
        /// L'index du luxurosaure de l'adversaire sélectionné
        /// </summary>
        public int SelectedEnemyIndex { get; private set; }

        /// <summary>
        /// L'attaque sélectionnée à jouer
        /// </summary>
        public AttackSO SelectedAttack { get; private set; }

        /// <summary>
        /// Le total de PVs des luxurosaures du joueur
        /// </summary>
        public int PlayerTotalHP { get; private set; }

        /// <summary>
        /// Le total de PVs des luxurosaures du joueur
        /// </summary>
        public int EnemyTotalHP { get; private set; }

        /// <summary>
        /// Le total de PPs du joueur
        /// </summary>
        public int PlayerFP { get; private set; }

        /// <summary>
        /// Le total de PPs de l'ennemi
        /// </summary>
        public int EnemyFP { get; private set; }

        /// <summary>
        /// Le total d'EXP gagné à la fin de la partie
        /// </summary>
        public int EXPGained { get; private set; }

        /// <summary>
        /// Le total des stats de soutien apportées au luxurosaure du joueur
        /// </summary>
        public FightingStats PlayerSupportStats { get; private set; } = FightingStats.Zero;

        /// <summary>
        /// Le total des stats de soutien apportées en fonction du niveau de chaleur du joueur
        /// </summary>
        public FightingStats HornySupportStats { get; private set; } = FightingStats.Zero;

        /// <summary>
        /// Le total des stats de soutien apportées au luxurosaure de l'ennemi
        /// </summary>
        public FightingStats EnemySupportStats { get; private set; } = FightingStats.Zero;

        /// <summary>
        /// true si le joueur a l'initiative (ses luxurosaures ont moins de PV que ceux de l'ennemi)
        /// </summary>
        public bool PlayerHasInitiative => PlayerTotalHP < EnemyTotalHP;

        /// <summary>
        /// Le dernier palier de chaleur qu'à atteint le joueur.
        /// Indique si ses luxurosaures doivent être excités ou non.
        /// </summary>
        public int PlayerHeatLevel => _playerStatsManager.LastHeatLevel;

        /// <summary>
        /// true si le joueur a changé de formation ce tour
        /// </summary>
        public bool PlayerHasChangedFormationThisTurn { get; private set; }

        /// <summary>
        /// true si l'ennemi a changé de formation ce tour
        /// </summary>
        public bool EnemyHasChangedFormationThisTurn { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le TeamMenuManager
        /// </summary>
        [SerializeField]
        private TeamMenuManager _teamMenuManager;

        /// <summary>
        /// Le PlayerStatsManager
        /// </summary>
        [SerializeField]
        private PlayerStatsManager _playerStatsManager;

        /// <summary>
        /// Le gain de PP au début de chaque tour
        /// </summary>
        [SerializeField]
        private int _FPIncreaseOnTurnStart = 50;

        /// <summary>
        /// Le gain de PP à la perte d'un allié
        /// </summary>
        [SerializeField]
        private int _FPIncreaseOnAllyDeath = 100;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu des vêtements
        /// </summary>
        [SerializeField]
        private FightingStats _hornyBonusStats;

        /// <summary>
        /// Le gain de stats des luxurosaures du joueur si celui-ci a perdu tous ses vêtements
        /// </summary>
        [SerializeField]
        private FightingStats _veryHornyBonusStats;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Le nombre de luxurosaures du joueur encore en vie
        /// </summary>
        private int _activePlayerLustosaurs;

        /// <summary>
        /// Le nombre de luxurosaures de l'adversaire encore en vie
        /// </summary>
        private int _activeEnemyLustosaurs;

        /// <summary>
        /// Moyenne de différence de niveau entre le joueur et l'ennemi
        /// </summary>
        private int _avgLevelDifference;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Lance un combat avec l'équipe adverse renseignée
        /// </summary>
        /// <param name="enemyTeam">L'équipe ennemie</param>
        public void StartCombat(LustosaurSO[] enemyTeam)
        {
            // Initialise les stats et l'état de jeu

            BattleState = BattleState.Ongoing;
            PlayerSupportStats = EnemySupportStats = FightingStats.Zero;
            _activePlayerLustosaurs = _activeEnemyLustosaurs = 0;
            EXPGained = 0;

            switch (PlayerHeatLevel)
            {
                case 0:
                    HornySupportStats = _hornyBonusStats;
                    break;

                case >= 1:
                    HornySupportStats = _veryHornyBonusStats;
                    break;
            }

            // On copie les luxurosaures au lieu de directement les référencer.
            // Ca nous permet de modifier leurs stats sans avoir à toucher aux originaux.

            PlayerTeam = new LustosaurSO[CombatConstants.NB_PARTICIPATING_LUSTOSAURS];
            EnemyTeam = new LustosaurSO[CombatConstants.NB_PARTICIPATING_LUSTOSAURS];

            int avgPlayerLevels = 0;
            int avgEnemyLevels = 0;
            int count = 0;

            for (int i = 0; i < Mathf.Min(CombatConstants.NB_PARTICIPATING_LUSTOSAURS, _teamMenuManager.PlayerTeam.Count); ++i)
            {
                LustosaurSO lustosaur = _teamMenuManager.PlayerTeam[i];
                PlayerTeam[i] = lustosaur.Clone();
                ++_activePlayerLustosaurs;

                avgPlayerLevels += lustosaur.CurLevel;
                ++count;
            }

            avgPlayerLevels /= count;
            count = 0;

            // Pour l'ennemi, on les mélange avant de les cloner.
            // Ca nous permet d'avoir une chance d'utiliser les luxurosaures
            // en réserve

            List<LustosaurSO> shuffled = new();

            for (int i = 0; i < enemyTeam.Length; ++i)
            {
                if (enemyTeam[i] != null)
                {
                    shuffled.Add(enemyTeam[i]);
                }
            }

            shuffled.Shuffle();

            for (int i = 0; i < Mathf.Min(CombatConstants.NB_PARTICIPATING_LUSTOSAURS, shuffled.Count); ++i)
            {
                LustosaurSO lustosaur = shuffled[i];
                EnemyTeam[i] = lustosaur.Clone();
                ++_activeEnemyLustosaurs;

                avgEnemyLevels += lustosaur.CurLevel;
                ++count;
            }

            avgEnemyLevels /= count;

            // On récupère la différence de niveau moyen entre le joueur et l'ennemi
            // pour le calcul de l'exp.
            // Si le joueur est plus fort que l'ennemi, il n'y aura pas de gain.

            _avgLevelDifference = Mathf.Max(0, avgEnemyLevels - avgPlayerLevels);

            OnCombatStarted?.Invoke();
        }

        /// <summary>
        /// Calcule l'initiative des combattants
        /// </summary>
        public void CalculateInitiative()
        {
            PlayerTotalHP = EnemyTotalHP = 0;

            // On ne prend que les 3 premiers dinos

            for (int i = 0; i < CombatConstants.NB_PARTICIPATING_LUSTOSAURS; ++i)
            {
                if (PlayerTeam[i] != null)
                {
                    PlayerTotalHP += PlayerTeam[i].CurFightingStats.Health;
                }

                if (EnemyTeam[i] != null)
                {
                    EnemyTotalHP += EnemyTeam[i].CurFightingStats.Health;
                }
            }
        }

        /// <summary>
        /// Commence un nouveau tour
        /// </summary>
        public void StartNewTurn()
        {
            PlayerFP += _FPIncreaseOnTurnStart;
            EnemyFP += _FPIncreaseOnTurnStart;

            PlayerSupportStats = (PlayerTeam[1] == null ? FightingStats.Zero : PlayerTeam[1].SupportStats) +
                                              (PlayerTeam[2] == null ? FightingStats.Zero : PlayerTeam[2].SupportStats);

            EnemySupportStats = (EnemyTeam[1] == null ? FightingStats.Zero : EnemyTeam[1].SupportStats) +
                                              (EnemyTeam[2] == null ? FightingStats.Zero : EnemyTeam[2].SupportStats);

            PlayerHasChangedFormationThisTurn = EnemyHasChangedFormationThisTurn = false;
        }

        /// <summary>
        /// Sélectionne un luxurosaure allié
        /// </summary>
        /// <param name="index">La position du luxurosaure dans l'équipe, s'il n'est pas déjà battu</param>
        public void SelectAllyLustosaur(int index)
        {
            int count = 0;

            while (PlayerTeam[index] == null || PlayerTeam[index].CurHealth == 0)
            {
                // On boucle à travers toute la liste jusqu'à avoir fait le tour,
                // car on ne commence pas forcément à 0.
                // Il y aura forcément un luxurosaure valide,
                // car sinon on aura déjà lancé l'écran de victoire/défaite.

                if (count == PlayerTeam.Length)
                {
                    break;
                }

                index = ++index % PlayerTeam.Length;
                ++count;
            }

            // On prend le 1er luxurosaure encore en vie

            SelectedAlly = PlayerTeam[index];
            SelectedAllyIndex = index;
        }

        /// <summary>
        /// Sélectionne un luxurosaure ennemi
        /// </summary>
        /// <param name="index">La position du luxurosaure dans l'équipe, s'il n'est pas déjà battu</param>
        public void SelectEnemyLustosaur(int index)
        {
            int count = 0;

            while (EnemyTeam[index] == null || EnemyTeam[index].CurHealth == 0)
            {
                // On boucle à travers toute la liste jusqu'à avoir fait le tour,
                // car on ne commence pas forcément à 0.
                // Il y aura forcément un luxurosaure valide,
                // car sinon on aura déjà lancé l'écran de victoire/défaite.

                if (count == EnemyTeam.Length)
                {
                    break;
                }

                index = ++index % EnemyTeam.Length;
                ++count;
            }

            // On prend le 1er luxurosaure encore en vie

            SelectedEnemy = EnemyTeam[index];
            SelectedEnemyIndex = index;
        }

        /// <summary>
        /// Sélectionne une attaque
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure attaquant</param>
        /// <param name="index">La position de l'attaque dans la liste</param>
        public void SelectAttack(LustosaurSO lustosaur, int index)
        {
            SelectedAttack = lustosaur.LearnedAttacks[index];
        }

        /// <summary>
        /// Conduit une attaque
        /// </summary>
        /// <param name="attacker">L'attaquant</param>
        /// <param name="defender">Le défenseur</param>
        /// <param name="attack">L'attaque</param>
        /// <param name="isPlayerTurn">true si c'est le tour du joueur</param>
        /// <param name="dmg">Les dégâts infligés</param>
        /// <param name="criticalHit">true s'il s'agit d'un coup critique</param>
        /// <param name="miss">true si l'attaque rate</param>
        public void ConductAttack(LustosaurSO attacker, LustosaurSO defender, AttackSO attack, bool isPlayerTurn, out int dmg, out bool criticalHit, out bool miss)
        {
            FightingStats attackingPlayerStats = isPlayerTurn ? PlayerSupportStats + HornySupportStats : EnemySupportStats;
            FightingStats defendingPlayerStats = isPlayerTurn ? EnemySupportStats : PlayerSupportStats + HornySupportStats;
            bool attackerIsSupport = isPlayerTurn ? SelectedAllyIndex != 0 : SelectedEnemyIndex != 0;
            bool defenderIsSupport = isPlayerTurn ? SelectedEnemyIndex != 0 : SelectedAllyIndex != 0;

            if (isPlayerTurn)
            {
                PlayerFP -= SelectedAttack.Cost;
            }
            else
            {
                EnemyFP -= SelectedAttack.Cost;
            }

            // On calcule le % de chance que l'attaque touche sa cible

            int hitChance = (attack.Accuracy + attacker.CurFightingStats.Accuracy) / 2 + attackingPlayerStats.Accuracy;
            int dodgeChance = defender.CurFightingStats.Evasion + defendingPlayerStats.Evasion;
            int substract = hitChance - dodgeChance;
            int rand = UnityEngine.Random.Range(0, 101);
            miss = rand < substract;

            // On calcule les dégaâts et la chance d'un coup critique

            if (miss)
            {
                dmg = 0;
                criticalHit = false;
                return;
            }

            int totalAttack = (attack.Damage + attacker.CurFightingStats.Attack) / 2 + attackingPlayerStats.Attack;
            int totalDefense = defender.CurFightingStats.Defense + defendingPlayerStats.Defense;
            rand = UnityEngine.Random.Range(0, 101);

            criticalHit = rand < attacker.CurFightingStats.CriticalHitRate + attackingPlayerStats.CriticalHitRate;
            dmg = Mathf.Max(0, Mathf.RoundToInt((totalAttack - totalDefense) * (criticalHit ? CombatConstants.CRITICAL_HIT_DMG_BONUS : 1f)));
            dmg /= attackerIsSupport || defenderIsSupport ? CombatConstants.SUPPORT_ZONE_DMG_REDUCTION : 1;

            // Inflige les dégâts au luxurosaure et le retire du combat si ses PV atteignent 0.
            // Puis on calcule les conditions de victoire

            defender.CurHealth -= dmg;
            defender.CurHealth = Mathf.Max(0, defender.CurHealth);

            if (defender.CurHealth == 0)
            {
                if (isPlayerTurn)
                {
                    EnemyTeam[SelectedEnemyIndex] = null;
                    --_activeEnemyLustosaurs;

                    if (_activeEnemyLustosaurs == 0)
                    {
                        BattleState = BattleState.Victory;
                    }
                }
                else
                {
                    PlayerTeam[SelectedAllyIndex] = null;
                    --_activePlayerLustosaurs;

                    if (_activePlayerLustosaurs == 0)
                    {
                        BattleState = BattleState.Defeat;
                    }
                }
            }
        }

        /// <summary>
        /// Obtient les résistances de chaque ennemi à l'attribut renseigné
        /// </summary>
        /// <param name="attackAttribute">L'attribut de l'attaque</param>
        /// <returns>Les résistances de chaque ennemi</returns>
        public LustosaurResistance[] GetEnemyResistances(ElementalAttribute attackAttribute)
        {
            LustosaurResistance[] resistances = new LustosaurResistance[CombatConstants.NB_PARTICIPATING_LUSTOSAURS];

            for (int i = 0; i < CombatConstants.NB_PARTICIPATING_LUSTOSAURS; ++i)
            {
                LustosaurSO enemy = EnemyTeam[i];

                if (enemy != null)
                {
                    switch (attackAttribute)
                    {
                        case ElementalAttribute.Neutral:
                            resistances[i] = LustosaurResistance.Neutral;
                            break;

                        case ElementalAttribute.Water:
                            resistances[i] = enemy.Attribute switch
                            {
                                ElementalAttribute.Fire => LustosaurResistance.Vulnerable,
                                ElementalAttribute.Earth => LustosaurResistance.Resistant,
                                _ => LustosaurResistance.Neutral,
                            };
                            break;

                        case ElementalAttribute.Fire:
                            resistances[i] = enemy.Attribute switch
                            {
                                ElementalAttribute.Wind => LustosaurResistance.Vulnerable,
                                ElementalAttribute.Water => LustosaurResistance.Resistant,
                                _ => LustosaurResistance.Neutral,
                            };
                            break;

                        case ElementalAttribute.Wind:
                            resistances[i] = enemy.Attribute switch
                            {
                                ElementalAttribute.Earth => LustosaurResistance.Vulnerable,
                                ElementalAttribute.Fire => LustosaurResistance.Resistant,
                                _ => LustosaurResistance.Neutral,
                            };
                            break;

                        case ElementalAttribute.Earth:
                            resistances[i] = enemy.Attribute switch
                            {
                                ElementalAttribute.Water => LustosaurResistance.Vulnerable,
                                ElementalAttribute.Wind => LustosaurResistance.Resistant,
                                _ => LustosaurResistance.Neutral,
                            };
                            break;
                    }
                }
            }

            return resistances;
        }

        /// <summary>
        /// Obtient si le combattant actif a assez de PP pour lancer une autre attaque
        /// </summary>
        /// <param name="isPlayerTurn">true si c'est le tour du joueur</param>
        /// <returns>true s'il peut toujours attaquer</returns>
        public bool ActiveFighterHasEnoughFPToAttack(bool isPlayerTurn)
        {
            LustosaurSO[] activeTeam = isPlayerTurn ? PlayerTeam : EnemyTeam;
            int remainingFP = isPlayerTurn ? PlayerFP : EnemyFP;

            foreach (LustosaurSO lustosaur in activeTeam)
            {
                if (lustosaur != null)
                {
                    foreach (AttackSO attack in lustosaur.LearnedAttacks)
                    {
                        if (attack.Cost <= remainingFP)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Exécute un tour pour l'adversaire
        /// </summary>
        public void ProcessOpponentTurn()
        {
            // On sélectionne un luxurosaure au hasard
            // parmi ceux de l'ennemi qui ont assez de PP pour attaquer

            List<LustosaurSO> validTargets = new();

            foreach (LustosaurSO lustosaur in EnemyTeam)
            {
                if (lustosaur != null)
                {
                    foreach (AttackSO attack in lustosaur.LearnedAttacks)
                    {
                        if (attack.Cost <= EnemyFP)
                        {
                            validTargets.Add(lustosaur);
                        }
                    }
                }
            }

            // On sélectionne un luxurosaure au hasard pour attaquer,
            // et on choisit une attaque au hasard

            SelectedEnemy = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
            SelectedAttack = SelectedEnemy.LearnedAttacks[UnityEngine.Random.Range(0, SelectedEnemy.LearnedAttacks.Count)];

            // On sélectionne une cible au hasard
            // parmi les luxurosaures du joueur

            validTargets.Clear();

            foreach (LustosaurSO lustosaur in EnemyTeam)
            {
                if (lustosaur != null)
                {
                    validTargets.Add(lustosaur);
                }
            }

            SelectedAlly = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
        }

        /// <summary>
        /// Abandonne la partie
        /// </summary>
        public void Submit()
        {
            // On sélectionne un luxurosaure au hasard
            // parmi ceux de l'ennemi

            List<LustosaurSO> validTargets = new();

            foreach (LustosaurSO lustosaur in EnemyTeam)
            {
                if (lustosaur != null)
                {
                    validTargets.Add(lustosaur);
                }
            }

            SelectedEnemy = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];

            // On calcule l'EXP gagnée

            CalculateExpGained(BattleState);
        }

        /// <summary>
        /// Calcule l'exp gagnée
        /// </summary>
        /// <param name="battleState">Issue de la partie</param>
        private void CalculateExpGained(BattleState battleState)
        {
            int expPerEnemyDefeated = CombatConstants.EXP_GAINED_PER_DEFEATED_ENEMY * EnemyTeam.Count(lustosaur => lustosaur == null);
            int expPerLevelDifference = CombatConstants.EXP_GAINED_PER_LEVEL_DIFFERENCE * _avgLevelDifference;

            EXPGained = expPerEnemyDefeated + expPerLevelDifference;
            EXPGained /= battleState == BattleState.Defeat ? CombatConstants.EXP_REDUCTION_COEF_IF_DEFEATED : 1;
        }

        #endregion
    }
}