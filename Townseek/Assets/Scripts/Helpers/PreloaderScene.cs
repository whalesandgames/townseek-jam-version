using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhalesAndGames.MapGame.Helpers
{
    /// <summary>
    /// Preloader scene that loads in the banks from FMOD before launching the rest of the game.
    /// </summary>
    public class PreloaderScene : MonoBehaviour
    {
        [BoxGroup("Loading Strings")]
        [SerializeField]
        private TextMeshProUGUI loadingText;
        [BoxGroup("Loading Strings")]
        [SerializeField]
        private string[] loadingStrings;

        [BoxGroup("Loading Objects")]
        [SerializeField]
        private GameObject disclaimerGameObject;
        [BoxGroup("Loading Objects")]
        [SerializeField]
        private GameObject webDisclaimerGameObject;

        private bool isLoading = false;
    
        private IEnumerator Start()
        {
            var rT = Random.Range(0, loadingStrings.Length);
            loadingText.text = loadingStrings[rT];
            
            while (!FMODUnity.RuntimeManager.HasBankLoaded("Master"))
            {
                yield return null;
            }

            #if UNITY_WEBGL
                disclaimerGameObject.SetActive(false);
                webDisclaimerGameObject.SetActive(true);
            #else
                ContinueToGame();
            #endif
        }

        /// <summary>
        /// Continues to the game.
        /// </summary>
        public void ContinueToGame()
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            SceneManager.LoadScene("Game");
        }
    }   
}
