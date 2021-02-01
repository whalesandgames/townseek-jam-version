using UnityEngine;
using UnityEngine.InputSystem;

namespace WhalesAndGames.MapGame.Singletons
{
    /// <summary>
    /// Manages the Global State of the game, including references to the player's controller.
    /// </summary>
    public class GlobalManager : SingletonBehaviour<GlobalManager>
    {
        #if UNITY_WEBGL
        [DllImport("__Internal")]
        static extern void OpenURLNewTab(string url);
        #endif
        
        [HideInInspector]
        public PlayerInput playerInput;
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);

            playerInput = GetComponent<PlayerInput>();
        }

        /// <summary>
        /// Opens a URL in the user's browser to a given address.
        /// </summary>
        public static void OpenURL(string url)
        {
            // Depending on the current platform use a different share method.
            #if !UNITY_WEBGL
            Application.OpenURL(url);
            #else
            OpenURLNewTab(url);
            #endif
        }
    }
}