using System;
using UnityEngine;
using Febucci.UI;
using Febucci.UI.Core;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Does speaking sounds on text that is animated by the Text Animator.
    /// </summary>
    [RequireComponent(typeof(TAnimPlayerBase))]
    public class TextAnimatorSounds : MonoBehaviour
    {
        [Header("Speaking Sounds")]
        public bool autoStart;
        [FMODUnity.EventRef]
        public string speakingSoundEvent;

        private FMOD.Studio.EventInstance speakingEmitter;
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (speakingSoundEvent != string.Empty)
            {
                speakingEmitter = FMODUnity.RuntimeManager.CreateInstance(speakingSoundEvent);
            }

            if (autoStart)
            {
                GetComponent<TAnimPlayerBase>()?.onTypewriterStart.AddListener(OnStart);
            }
        }

        /// <summary>
        /// Called when a character is typed on screen.
        /// </summary>
        public void OnStart()
        {
            speakingEmitter.setParameterByName("end", 0);
            speakingEmitter.start();
            
            GetComponent<TAnimPlayerBase>()?.onTextShowed.AddListener(OnFinish);
        }

        /// <summary>
        /// Called when a character is over.
        /// </summary>
        private void OnFinish()
        {
            speakingEmitter.setParameterByName("end", 1);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            speakingEmitter.setParameterByName("end", 3);
        }
    }
}