using UnityEngine;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Defines an interactable that the player can interact with (including crates, fish, etc.)
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Checks if the player can interact with this interactable.
        /// </summary>
        public bool CheckIfCanInteract();

        /// <summary>
        /// Called when the player is-in range of the interactable.
        /// </summary>
        public void OnInteractableEnter();

        /// <summary>
        /// Called when the player is-off the range of the interactable.
        /// </summary>
        public void OnInteractableExit();
        
        /// <summary>
        /// Called when the player interacts with this interactable.
        /// </summary>
        public void InteractWith();

        /// <summary>
        /// Returns the distance of the interactable and the player.
        /// </summary>
        public Vector2 GetPosition();
    }
}