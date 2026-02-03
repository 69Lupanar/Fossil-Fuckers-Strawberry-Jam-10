namespace Assets.Scripts.Models.EventArgs
{
    /// <summary>
    /// Infos sur le changment de points d'exp du joueur
    /// </summary>
    public class EXPChangedEventArgs : System.EventArgs
    {
        #region Propriétés

        /// <summary>
        /// Le montant actuel du joueur
        /// </summary>
        public int CurEXP { get; private set; }

        /// <summary>
        /// Le montant gagné (ou perdu, si négatif)
        /// </summary>
        public int Amount { get; private set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Le montant actuel du joueur
        /// </summary>
        /// <param name="curEXP">Le montant actuel du joueur</param>
        /// <param name="amount">Le montant gagné (ou perdu, si négatif)</param>
        public EXPChangedEventArgs(int curEXP, int amount)
        {
            CurEXP = curEXP;
            Amount = amount;
        }
        #endregion
    }
}