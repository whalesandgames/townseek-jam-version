using FMODUnity;
using UnityEngine;
using WhalesAndGames.MapGame.UI;

namespace WhalesAndGames.MapGame.Singletons
{
    /// <summary>
    /// Handles the game's music, sound effects and ambiance.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private StudioEventEmitter musicEmitter;
        [SerializeField]
        private StudioEventEmitter atmosEvent;
        [SerializeField]
        private StudioEventEmitter coinEvent;
        [SerializeField]
        private StudioEventEmitter discoveryEvent;

        private bool isMuted = false;
        
        /// <summary>
        /// Sets a music parameter in the 
        /// </summary>
        public void SetMusicParameter(string parameter, float value)
        {
            musicEmitter.SetParameter(parameter, value);
        }

        /// <summary>
        /// Sets the value of an atmos parameter.
        /// </summary>
        public void SetAtmosParameter(string parameter, float value)
        {
            atmosEvent.SetParameter(parameter, value);
        }

        /// <summary>
        /// Plays the coins sounds when trading is made.
        /// </summary>
        public void PlayCoinEvent()
        {
            coinEvent.Play();
        }
        
        /// <summary>
        /// Plays the discovery sound when the player finds a new location in the map.
        /// </summary>
        public void PlayDiscoveryEvent()
        {
            discoveryEvent.Play();
        }

        /// <summary>
        /// Toggles the mute off and on.
        /// </summary>
        public bool ToggleMute()
        {
            isMuted = !isMuted;
            
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("STvolume", isMuted ? 0 : 100);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFXvolume", isMuted ? 0 : 100);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("General Volume", isMuted ? 0 : 100);

            return isMuted;
        }
    }
}