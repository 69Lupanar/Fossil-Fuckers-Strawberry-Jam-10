using System;
using Assets.Scripts.Models.Dinos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.UI
{
    /// <summary>
    /// Représente une attaque dans le menu de sélection en combat
    /// </summary>
    public class AttackSlotInstance : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand on clique sur le bouton
        /// </summary>
        private Action<int> _onAttackBtnClick { get; set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Icône de l'attribut
        /// </summary>
        [SerializeField]
        private Image _attributeIcon;

        /// <summary>
        /// Label du nom de l'attaque
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _nameLabel;

        /// <summary>
        /// Label des dégâts de l'attaque
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _dmgLabel;

        /// <summary>
        /// Label de la précision de l'attaque
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _accLabel;

        /// <summary>
        /// Label du coût de l'attaque
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI _costLabel;

        /// <summary>
        /// bouton
        /// </summary>
        [SerializeField]
        private Button _btn;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Initialise les données
        /// </summary>
        /// <param name="attack">L'attaque correspondante</param>
        /// <param name="onClick">Appelée quand on clique sur le bouton</param>
        public void SetData(AttackSO attack, Action<int> onClick)
        {
            _attributeIcon.gameObject.SetActive(attack.AttributeSprite != null);
            _attributeIcon.sprite = attack.AttributeSprite;
            _nameLabel.SetText(attack.name);
            _dmgLabel.SetText(attack.Damage.ToString());
            _accLabel.SetText(attack.Accuracy.ToString());
            _costLabel.SetText(attack.Cost.ToString());
            _onAttackBtnClick = onClick;
        }

        /// <summary>
        /// Rend le bouton interactif ou non
        /// </summary>
        /// <param name="interactable">true pour le rendre interactif</param>
        public void SetInteractable(bool interactable)
        {
            _btn.interactable = interactable;
        }

        /// <summary>
        /// Appelée quand on clique sur le bouton
        /// </summary>
        public void OnClick()
        {
            _onAttackBtnClick?.Invoke(transform.GetSiblingIndex());
        }

        #endregion
    }
}