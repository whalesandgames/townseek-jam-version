using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Logic;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Controls the town panel that shows information about a town.
    /// </summary>
    public class TownPanel : MonoBehaviour
    {
        [BoxGroup("Panel")]
        [SerializeField]
        private GameObject townWindow;
        private Animator townWindowAnimator;
        
        [BoxGroup("Town Descriptions")]
        [SerializeField]
        private TextMeshProUGUI townNameText;
        [BoxGroup("Town Descriptions")]
        [SerializeField]
        private TextMeshProUGUI townDescriptionText;
        
        [BoxGroup("Character and Talk")]
        [SerializeField]
        private Image characterPortraitImage;
        [BoxGroup("Character and Talk")]
        [SerializeField]
        private TextMeshProUGUI characterNameText;
        [BoxGroup("Character and Talk")]
        [SerializeField]
        private TextMeshProUGUI characterSpeakText;

        [BoxGroup("Inventories")]
        [SerializeField]
        private RectTransform townInventoryTransform;
        [BoxGroup("Inventories")]
        [SerializeField]
        private RectTransform playerInventoryTransform;
        [BoxGroup("Inventories")]
        [SerializeField]
        private GameObject itemListingPrefab;
        [BoxGroup("Inventories")]
        [SerializeField]
        private Material grayscaleUIMaterial;

        private DiscoveryTracker discoveryTracker;
        private TownBehaviour activeTown;

        /// <summary>
        /// Initializes this Canvas Panel with the elements necessary.
        /// </summary>
        public void Initialize(DiscoveryTracker dT)
        {
            discoveryTracker = dT;

            foreach (var tb in dT.listOfTowns)
            {
                tb.OpenTownPanel += OpenTownPanel;
                tb.SetCharacter += SetCharacter;
                tb.SetTownInformation += SetTownInformation;
                tb.SetCharacterSpeak += SetCharacterSpeak;

                tb.UpdateTownInventory += DrawTownInventory;
                tb.UpdatePlayerInventory += DrawPlayerInventory;
            }

            townWindowAnimator = townWindow.GetComponent<Animator>();
        }
        
        /// <summary>
        /// Opens the town panel by setting it to active and playing animations.
        /// </summary>
        public void OpenTownPanel(TownBehaviour town)
        {
            if (townWindow.activeSelf)
            {
                townWindow.SetActive(false);
            }
            
            activeTown = town;
            townWindow.SetActive(true);

            townInventoryTransform.anchoredPosition = Vector2.zero;
            playerInventoryTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Sets the character portrait to match the town open.
        /// </summary>
        public void SetCharacter(Sprite sprite, string name)
        {
            characterPortraitImage.sprite = sprite;
            characterPortraitImage.SetNativeSize();

            characterNameText.text = name;
        }
        
        /// <summary>
        /// Sets the town information with its name and description.
        /// </summary>
        public void SetTownInformation(string name, string description)
        {
            townNameText.text = name;
            townDescriptionText.text = description;
        }

        /// <summary>
        /// Sets the character text, called depending on the actions that the player has taken.
        /// </summary>
        public void SetCharacterSpeak(string text)
        {
            characterSpeakText.text = text;
            characterSpeakText.ForceMeshUpdate();
        }

        /// <summary>
        /// Draws the town inventory, and the stock of each item.
        /// </summary>
        public void DrawTownInventory(Dictionary<Item, int> townInventory, Dictionary<ItemCategory, float> categoryPriceFluctuation)
        {
            foreach (Transform t in townInventoryTransform)
            {
                Destroy(t.gameObject);
            }
            
            foreach (var item in townInventory)
            {
                var newItemListing = Instantiate(itemListingPrefab, townInventoryTransform);
                
                var finalPrice = item.Key.price;
                if (categoryPriceFluctuation.ContainsKey(item.Key.category))
                {
                    finalPrice = Mathf.RoundToInt(finalPrice * categoryPriceFluctuation[item.Key.category]);
                }
                
                var itemSprite = newItemListing.transform.Find("Item Icon").GetComponent<Image>();
                itemSprite.sprite = item.Key.icon;
                
                newItemListing.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text = item.Key.name;
            
                var priceText = newItemListing.transform.Find("Item Price/Item Price Value").GetComponent<TextMeshProUGUI>();
                priceText.text = finalPrice.ToString();

                if (finalPrice == item.Key.price)
                {
                    priceText.color = Color.white;
                }
                else if (finalPrice < item.Key.price)
                {
                    priceText.color = new Color(0.7f, 0.86f, 0.63f);
                }
                else if (finalPrice > item.Key.price)
                {
                    priceText.color = new Color(0.74f, 0.41f, 0.41f);
                }
            
                newItemListing.transform.Find("Item Stock").GetComponent<TextMeshProUGUI>().text = $"x{item.Value}";

                var actionButton = newItemListing.transform.Find("Item Button").GetComponent<Button>();
                actionButton.onClick.AddListener(delegate { BuyItem(item.Key, finalPrice); });
                
                // Gray-scales the UI and the Icon if the items has no stock.
                if (item.Value == 0)
                {
                    itemSprite.material = grayscaleUIMaterial;
                    newItemListing.GetComponent<Image>().material = grayscaleUIMaterial;
                    actionButton.GetComponent<Image>().material = grayscaleUIMaterial;
                }
            }
        }

        /// <summary>
        /// Draws the player inventory and what it holds.
        /// </summary>
        public void DrawPlayerInventory(Dictionary<Item, int> playerInventory, Dictionary<ItemCategory, float> categoryPriceFluctuation)
        {
            foreach (Transform t in playerInventoryTransform)
            {
                Destroy(t.gameObject);
            }
            
            foreach (var item in playerInventory)
            {
                var newItemListing = Instantiate(itemListingPrefab, playerInventoryTransform);
                
                var finalPrice = item.Key.price;
                if (categoryPriceFluctuation.ContainsKey(item.Key.category))
                {
                    finalPrice = Mathf.RoundToInt(finalPrice * categoryPriceFluctuation[item.Key.category]);
                }

                newItemListing.transform.Find("Item Icon").GetComponent<Image>().sprite = item.Key.icon;
                newItemListing.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text = item.Key.name;
            
                var priceText = newItemListing.transform.Find("Item Price/Item Price Value").GetComponent<TextMeshProUGUI>();
                priceText.text = finalPrice.ToString();

                if (finalPrice == item.Key.price)
                {
                    priceText.color = Color.white;
                }
                else if (finalPrice > item.Key.price)
                {
                    priceText.color = new Color(0.7f, 0.86f, 0.63f);
                }
                else if (finalPrice < item.Key.price)
                {
                    priceText.color = new Color(0.74f, 0.41f, 0.41f);
                }
            
                newItemListing.transform.Find("Item Stock").GetComponent<TextMeshProUGUI>().text = $"x{item.Value}";

                var actionButton = newItemListing.transform.Find("Item Button").GetComponent<Button>();
                actionButton.onClick.AddListener(delegate { SellItem(item.Key, finalPrice); });

                actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell";
            }
        }

        /// <summary>
        /// Called when the player presses a town inventory to buy an item.
        /// </summary>
        public void BuyItem(Item item, int finalPrice)
        {
            activeTown?.SellToPlayer(item, finalPrice);
        }

        /// <summary>
        /// Called when the player presses an own inventory to sell an item.
        /// </summary>
        public void SellItem(Item item, int finalPrice)
        {
            activeTown?.PurchaseFromPlayer(item, finalPrice);
        }

        /// <summary>
        /// Closes the town panel.
        /// </summary>
        public void CloseTownPanel()
        {
            activeTown?.ClosedTownPanel();
            activeTown = null;
            
            townWindowAnimator.SetTrigger("Close");
        }
    }
}