using Sirenix.OdinInspector;
using UnityEngine;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Behaviour for a landmark that exists in the map for the player to examine.
    /// </summary>
    public class LandmarkBehaviour : MonoBehaviour, IInteractable, IDiscoverable
    {
        [BoxGroup("Landmark Definition")]
        public Landmark landmark;
        
        [FoldoutGroup("Components")]
        [SerializeField]
        private GameObject questionMark;
        
        public delegate void SetLandmarkExamineEventHandler(string text);
        public event SetLandmarkExamineEventHandler SetLandermarkExamine;
        
        /// <summary>
        /// Called when the player is-in discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryEnter()
        {
            GameManager.DiscoveryTracker.AddNewDiscovery(landmark);
        }

        /// <summary>
        /// Called when the player is-off the discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryExit()
        {
            // Do nothing?
        }

        /// <summary>
        /// Checks if the player can interact with this interactable.
        /// </summary>
        public bool CheckIfCanInteract()
        {
            return true;
        }

        /// <summary>
        /// Called when the player is-in range of the interactable.
        /// </summary>
        public void OnInteractableEnter()
        {
            questionMark.SetActive(true);
        }

        /// <summary>
        /// Called when the player interacts with this interactable.
        /// </summary>
        public void OnInteractableExit()
        {
            questionMark.SetActive(false);
        }

        /// <summary>
        /// Called when the player is-off the range of the interactable.
        /// </summary>
        public void InteractWith()
        {
            var randomDescription = Random.Range(0, landmark.landmarkDescriptions.Count);
            var landmarkDescription = landmark.landmarkDescriptions[randomDescription];

            SetLandermarkExamine?.Invoke(landmarkDescription);
        }

        /// <summary>
        /// Returns the objects position.
        /// </summary>
        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}