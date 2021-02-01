using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Shows what the player currently has in the inventory.
    /// </summary>
    public class PlayerInventoryPanel : MonoBehaviour
    {
        [BoxGroup("Panel")]
        [SerializeField]
        private GameObject playerInventoryWindow;
        private Animator playerInventoryWindowAnimator;
        
        [BoxGroup("Inventory")]
        [SerializeField]
        private RectTransform playerInventoryTransform;
        [BoxGroup("Inventories")]
        [SerializeField]
        private GameObject itemListingPrefab;

        private GameManager gameManager;
        
        /// <summary>
        /// Initializes this panel.
        /// </summary>
        public void Initialize(GameManager gM)
        {
            gameManager = gM;
            gM.PlayerInventoryChanged += UpdatePlayerInventory;

            playerInventoryWindowAnimator = playerInventoryWindow.GetComponent<Animator>();
        }

        /// <summary>
        /// Opens the player inventory panel.
        /// </summary>
        public void OpenPlayerInventory()
        {
            gameManager.playerCanMove = false;
            
            playerInventoryWindow.SetActive(true);
            playerInventoryTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Updates the content of the player's inventory.
        /// </summary>
        public void UpdatePlayerInventory(Dictionary<Item, int> playerInventory)
        {
            foreach (Transform t in playerInventoryTransform)
            {
                Destroy(t.gameObject);
            }
            
            foreach (var item in playerInventory)
            {
                var newItemListing = Instantiate(itemListingPrefab, playerInventoryTransform);
                
                newItemListing.transform.Find("Item Icon").GetComponent<Image>().sprite = item.Key.icon;
                newItemListing.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text = item.Key.name;
                var priceText = newItemListing.transform.Find("Item Price/Item Price Value").GetComponent<TextMeshProUGUI>().text = item.Key.price.ToString();
                
                newItemListing.transform.Find("Item Stock").GetComponent<TextMeshProUGUI>().text = $"x{item.Value}";
            }
        }

        /// <summary>
        /// Closes the player inventory panel.
        /// </summary>
        public void ClosePlayerInventory()
        {
            gameManager.playerCanMove = true;
            playerInventoryWindowAnimator.SetTrigger("Close");
        }
    }
}