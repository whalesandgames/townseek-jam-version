using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Defines an audio event zone that plays sound effects when the player is close to it.
    /// </summary>
    public class AudioAtmosZone : MonoBehaviour, IAudioEmitter
    {
        [Header("Parameter")]
        [ValueDropdown("GetAllAudioParameters", IsUniqueList = true, AppendNextDrawer = true, FlattenTreeView = true)]
        [SerializeField]
        private AudioParameter audioParameter;

        /// <summary>
        /// Emits the audio by calling parameters in the Audio Manager.
        /// </summary>
        public void EmitAudio(float distNormalized)
        {
            if (audioParameter == null)
            {
                return;
            }
            
            GameManager.AudioManager.SetAtmosParameter(audioParameter.parameterName, distNormalized);
        }

        /// <summary>
        /// Returns the distance of this emitter and the player.
        /// </summary>
        public Vector2 GetPosition()
        {
            return transform.position;
        }
        
        #if UNITY_EDITOR
        /// <summary>
        /// Returns all of the item categories to populate the list.
        /// </summary>
        private static IEnumerable GetAllAudioParameters()
        {
            return DataHelpers.ReturnScriptableObjects<AudioParameter>();
        }
        #endif
    }
}