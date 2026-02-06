using System;
using Assets.Scripts.Models.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Logique du bouton des améliorations
/// </summary>
public class UpgradeOptionInstance : MonoBehaviour
{
    #region Evénements

    /// <summary>
    /// Appelée quand le bouton est cliqué
    /// </summary>
    public Action<UpgradeOptionInstance> OnClick { get; set; }

    #endregion

    #region Propriétés

    /// <summary>
    /// L'amélioration à appliquer
    /// </summary>
    public PlayerUpgradeSO Upgrade => _upgrade;

    #endregion

    #region Variables Unity

    /// <summary>
    /// Icône de l'amélioration
    /// </summary>
    [SerializeField]
    private Image _icon;

    /// <summary>
    /// Label du nom de l'amélioration
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _nameLabel;

    /// <summary>
    /// Label du nom de l'amélioration
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _costLabel;

    #endregion

    #region Variables d'instance

    /// <summary>
    /// L'amélioration à appliquer
    /// </summary>
    private PlayerUpgradeSO _upgrade;

    #endregion

    #region Méthodes publiques

    /// <summary>
    /// Assigne l'amélioration à appliquer
    /// </summary>
    /// <param name="upgrade">L'amélioration à appliquer</param>
    public void SetCurUpgrade(PlayerUpgradeSO upgrade)
    {
        _upgrade = upgrade;
        _icon.sprite = upgrade.Sprite;
        _nameLabel.SetText(upgrade.name);
        _costLabel.SetText($"{upgrade.Cost} EXP");
    }

    /// <summary>
    /// Envoie le contenu de l'upgrade à la vue pour la màj
    /// </summary>
    public void SendUpgradeToView()
    {
        OnClick?.Invoke(this);
    }

    #endregion
}
