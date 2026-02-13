using System;
using UnityEngine;

namespace Assets.Scripts.Models.Audio
{
    /// <summary>
    /// Represents the data used to setup an AudioSource
    /// </summary>
    [Serializable]
    public struct AudioClipData
    {
        #region Propriétés

        /// <summary>
        /// The clip to play
        /// </summary>
        public readonly AudioClip Clip => _clip;

        /// <summary>
        /// The default volume level to play this clip at
        /// </summary>
        public readonly float DefaultVolume => _defaultVolume;

        /// <summary>
        /// True if the clip must loop
        /// </summary>
        public readonly bool Loop => _loop;

        #endregion

        #region Variables Unity

        /// <summary>
        /// Le clip audio à jouer
        /// </summary>
        [SerializeField]
        private AudioClip _clip;

        /// <summary>
        /// Volume par défaut
        /// </summary>
        [SerializeField]
        private float _defaultVolume;

        /// <summary>
        /// true si le son doit se jouer en boucle
        /// </summary>
        [SerializeField]
        private bool _loop;

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="clip">Le clip audio à jouer</param>
        /// <param name="defaultVolume">Volume par défaut</param>
        /// <param name="loop">true si le son doit se jouer en boucle</param>
        public AudioClipData(AudioClip clip, float defaultVolume, bool loop)
        {
            _clip = clip;
            _defaultVolume = defaultVolume;
            _loop = loop;
        }

        #endregion
    }
}