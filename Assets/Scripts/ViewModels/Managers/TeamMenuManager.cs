using System;
using Assets.Scripts.Models.Dinos;
using UnityEngine;

namespace Assets.Scripts.ViewModels.Managers
{
    /// <summary>
    /// Gère l'équipe du luxurosaures du joueur
    /// </summary>
    public class TeamMenuManager : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelée quand un Luxurosaure est créé
        /// </summary>
        public Action<LustosaurSO> OnLustosaurCreated { get; set; }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ajoute un nouveau luxurosaure à l'équipe.
        /// Si l'équipe est pleine, il sera envoyé à la réserve.
        /// Si la réserve est pleine, il sera défaussé.
        /// </summary>
        /// <param name="lustosaur">Le luxurosaure à ajouter</param>
        public void AddLustosaur(LustosaurSO lustosaur)
        {
            OnLustosaurCreated?.Invoke(lustosaur);
        }

        #endregion
    }
}