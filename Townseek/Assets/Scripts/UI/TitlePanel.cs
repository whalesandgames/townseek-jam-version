using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Title panel to the game that shows the player information.
    /// </summary>
    public class TitlePanel : MonoBehaviour
    {
        [BoxGroup("Tutorial Window")]
        [SerializeField]
        private GameObject tutorialWindow;
        private Animator tutorialWindowAnimator;

        [BoxGroup("Banner and Start Button")]
        [SerializeField]
        private Button clickToStartButton;
        [BoxGroup("Banner and Start Button")]
        [SerializeField]
        private GameObject pressBanner;
        private Animator pressBannerAnimator;

        private Animator animator;
        private GameManager gameManager;
        
        /// <summary>
        /// Initializes this panel.
        /// </summary>
        public void Initialize(GameManager gM)
        {
            gameManager = gM;

            animator = GetComponent<Animator>();
            tutorialWindowAnimator = tutorialWindow.GetComponent<Animator>();
            pressBannerAnimator = pressBanner.GetComponent<Animator>();

            gM.PressedToStart += OpenTutorialWindow;
        }
        
        /// <summary>
        /// Opens the tutorial window.
        /// </summary>
        public void OpenTutorialWindow()
        {
            clickToStartButton.gameObject.SetActive(false);
            
            pressBannerAnimator.SetTrigger("Close");
            tutorialWindow.SetActive(true);
        }

        /// <summary>
        /// Since the Event System was constantly firing overlap events, this is a workaround.
        /// </summary>
        public void StartGameButton()
        {
            gameManager.PressToStart();
        }

        /// <summary>
        /// The player has confirmed the tutorial window.
        /// </summary>
        public void UnderstoodTutorialWindow()
        {
            IEnumerator Coroutine()
            {
                tutorialWindowAnimator.SetTrigger("Close");
                yield return new WaitForSeconds(0.75f);
                animator.SetTrigger("Close");
                yield return new WaitForSeconds(1f);

                CanvasManager.Instance.StartCanvasCorner();
                
                gameManager.StartGame();
                gameObject.SetActive(false);
            }

            StartCoroutine(Coroutine());
        }
    }
}