using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Febucci.UI.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Manages the canvas that showcase 
    /// </summary>
    public class CanvasManager : SingletonBehaviour<CanvasManager>
    {
        [BoxGroup("Stats Displays")]
        [SerializeField]
        private GameObject canvasCorner;
        private Animator canvasCornerAnimator;
        [BoxGroup("Stats Displays")]
        [SerializeField]
        private TextMeshProUGUI goldValueText;
        [BoxGroup("Stats Displays")]
        [SerializeField]
        private TextMeshProUGUI discoveriesTotalText;

        [BoxGroup("Discovery Window")]
        [SerializeField]
        private GameObject discoveryNotification;
        [BoxGroup("Discovery Window")]
        [SerializeField]
        private TextMeshProUGUI discoveryTypeText;
        [BoxGroup("Discovery Window")]
        [SerializeField]
        private TextMeshProUGUI discoveryNameText;
        
        [BoxGroup("Examine Bubble")]
        [SerializeField]
        private GameObject examineBubble;
        [BoxGroup("Examine Bubble")]
        [SerializeField]
        private TextMeshProUGUI examineText;

        private Animator examineBubbleAnimator;

        // References
        private GameManager gameManager;
        private TitlePanel titlePanel;
        private TownPanel townPanel;
        private PlayerInventoryPanel playerInventoryPanel;

        /// <summary>
        /// Initializes the Canvas Manager with the elements necessary.
        /// </summary>
        public void Initialize(GameManager gM, DiscoveryTracker dT)
        {
            IEnumerator Coroutine()
            {
                examineBubble.SetActive(true);
                discoveryNotification.SetActive(true);
                
                discoveryTypeText.ForceMeshUpdate();
                examineText.ForceMeshUpdate();
                
                yield return new WaitForEndOfFrame();

                discoveryNotification.GetComponent<Animator>().enabled = true;
                discoveryNotification.SetActive(false);
                examineBubble.SetActive(false);
            };
            
            gameManager = gM;

            titlePanel = GetComponentInChildren<TitlePanel>();
            townPanel = GetComponentInChildren<TownPanel>();
            playerInventoryPanel = GetComponentInChildren<PlayerInventoryPanel>();
            examineBubbleAnimator = examineBubble.GetComponent<Animator>();
            canvasCornerAnimator = canvasCorner.GetComponent<Animator>();
            
            gM.PlayerMoneyChanged += UpdateGoldValueText;
            dT.NewDiscoveryNotification += NewDiscoveryNotification;
            examineText.GetComponent<TAnimPlayerBase>()?.onTextShowed.AddListener(OnExamineFinish);
            
            titlePanel.Initialize(gM);
            townPanel.Initialize(dT);
            playerInventoryPanel.Initialize(gM);
            
            foreach (var lb in dT.listOfLandmarks)
            {
                lb.SetLandermarkExamine += ExamineLandmark;
            }
            
            dT.UpdateDiscoveriesTotal += UpdateDiscoveriesTotal;
            StartCoroutine(Coroutine());
        }
        
        /// Opens the Canvas Corner Window in the corner.
        /// </summary>
        public void StartCanvasCorner()
        {
            canvasCornerAnimator.SetTrigger("Start");
        }

        /// <summary>
        /// Updates the gold value in the text with the player's current money.
        /// </summary>
        public void UpdateGoldValueText(float money)
        {
            goldValueText.text = money.ToString("N0");
        }

        /// <summary>
        /// Updates the total number of discoveries shown.
        /// </summary>
        public void UpdateDiscoveriesTotal(int discoveriesFound, int totalDiscoveries)
        {
            discoveriesTotalText.text = $"{discoveriesFound}/{totalDiscoveries}";
        }

        /// <summary>
        /// Called when a landmark is examined.
        /// </summary>
        public void ExamineLandmark(string description)
        {
            if (examineBubble.activeSelf)
            {
                examineBubble.SetActive(false);
            }
            
            examineBubble.SetActive(true);
            examineText.GetComponent<TextAnimatorSounds>().OnStart();
            
            examineText.text = description;
        }
        
        /// <summary>
        /// Called when the player finishes examining a landmark.
        /// </summary>
        public void OnExamineFinish()
        {
            examineBubbleAnimator.SetTrigger("Finished");
        }

        /// <summary>
        /// Shows a notification that the player has found a new discovery.
        /// </summary>
        public void NewDiscoveryNotification(Discovery discovery)
        {
            if (discoveryNotification.activeSelf)
            {
                discoveryNotification.SetActive(false);
            }
            
            discoveryNotification.SetActive(true);
            
            discoveryTypeText.text = $"<bounce>New {discovery.GetType().Name} Discovered!</bounce>";
            discoveryNameText.text = discovery.name;
        }

        /// <summary>
        /// Opens a URL in the user's browser to a given address.
        /// </summary>
        public void OpenURL(string url)
        {
            GlobalManager.OpenURL(url);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}