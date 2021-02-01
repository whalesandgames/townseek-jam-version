using UnityEngine;

namespace WhalesAndGames.MapGame.Helpers
{
    /// <summary>
    /// Animation events allow animations to perform specific actions.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorEvents : MonoBehaviour
    {
        /// <summary>
        /// Destroys this game object.
        /// </summary>
        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Destroys the parent game object.
        /// </summary>
        public void DestroyParentGameObject()
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

        /// <summary>
        /// Disables this game object.
        /// </summary>
        public void DisableGameObject()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Disables the Animator itself. This will stop Animated Canvas from setting themselves as dirty.
        /// </summary>
        public void DisableAnimator()
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}