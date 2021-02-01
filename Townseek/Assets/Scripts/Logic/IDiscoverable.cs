using UnityEngine;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Marks something as being able to be discovered, and uses that definition for audio as well.
    /// </summary>
    public interface IDiscoverable
    {
        /// <summary>
        /// Called when the player is-in discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryEnter();

        /// <summary>
        /// Called when the player is-off the discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryExit();

        /// <summary>
        /// Returns the distance of the discovery and the player.
        /// </summary>
        public Vector2 GetPosition();
    }
}