using UnityEngine;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Makes an object be able to emit audio based on the player's position.
    /// </summary>
    public interface IAudioEmitter
    {
        /// <summary>
        /// Emits the audio by calling parameters in the Audio Manager.
        /// </summary>
        public void EmitAudio(float distNormalized);
        
        /// <summary>
        /// Returns the distance of this emitter and the player.
        /// </summary>
        public Vector2 GetPosition();
    }
}