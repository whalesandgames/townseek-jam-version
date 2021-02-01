using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Defines a town that can be interacted with when the player is near it.
    /// </summary>
    public class TownBehaviour : MonoBehaviour, IInteractable, IDiscoverable, IAudioEmitter
    {
        [BoxGroup("Town Definition")]
        public Town town;

        [BoxGroup("Town Inventory")]
        [ShowInInspector]
        public Dictionary<Item, int> TownInventory = new Dictionary<Item, int>();
        [BoxGroup("Town Inventory")]
        [ShowInInspector]
        public Dictionary<ItemCategory, float> CategoryPriceFluctuation = new Dictionary<ItemCategory, float>();
        private bool isPlayerInRange = false;

        [FoldoutGroup("Components")]
        [SerializeField]
        private GameObject exclamationMark;
        [FoldoutGroup("Components")]
        [SerializeField]
        private Sprite interactableSprite;
        [FoldoutGroup("Components")]
        [SerializeField]
        private Sprite standbySprite;
        [FoldoutGroup("Components")]
        [SerializeField]
        private TextMeshPro nameText;
        [FoldoutGroup("Components")]
        [FMODUnity.EventRef]
        public string enterSoundEvent;

        private new SpriteRenderer renderer;
        private float lastTimeOpen;

        public delegate void OpenTownPanelEventHandler(TownBehaviour town);
        public event OpenTownPanelEventHandler OpenTownPanel;
        
        public delegate void SetCharacterEventHandler(Sprite sprite, string name);
        public event SetCharacterEventHandler SetCharacter;
        
        public delegate void SetTownInformationEventHandler(string name, string description);
        public event SetTownInformationEventHandler SetTownInformation;
        
        public delegate void SetCharacterTextEventHandler(string text );
        public event SetCharacterTextEventHandler SetCharacterSpeak;
        
        public delegate void UpdateTownInventoryEventHandler(Dictionary<Item, int> townInventory, Dictionary<ItemCategory, float> categoryPriceFluctuation);
        public event UpdateTownInventoryEventHandler UpdateTownInventory;
        
        public delegate void UpdatePlayerInventoryEventHandler(Dictionary<Item, int> playerInventory, Dictionary<ItemCategory, float> categoryPriceFluctuation);
        public event UpdatePlayerInventoryEventHandler UpdatePlayerInventory;

        private DiscoveryTracker discoveryTracker;

        /// <summary>
        /// Initializes this town in the world by setting their inventory.
        /// </summary>
        public void Initialize(DiscoveryTracker dT)
        {
            discoveryTracker = dT;
            renderer = GetComponent<SpriteRenderer>();
            
            foreach (var itemInv in town.itemInventory)
            {
                TownInventory.Add(itemInv.item, itemInv.stock);
            }
            
            foreach (var itemCat in town.categoryModifiers)
            {
                CategoryPriceFluctuation.Add(itemCat.category, itemCat.ReturnRandomPriceFluctuation());
            }

            nameText.text = town.name;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!isPlayerInRange)
            {
                if (Vector2.Distance(GameManager.Instance.playerShip.transform.position, transform.position) < town.playerSpotRadius)
                {
                    PlayerIsInRange();
                }
            }
            else
            {
                if (Vector2.Distance(GameManager.Instance.playerShip.transform.position, transform.position) > town.playerSpotRadius)
                {
                    isPlayerInRange = false;
                }
            }
        }

        /// <summary>
        /// Called whenever the player is in range of this town, changing the price fluctuation.
        /// </summary>
        public void PlayerIsInRange()
        {
            isPlayerInRange = true;

            // Calculates fluctuations for this visit.
            var categoriesToFluctuate = CategoryPriceFluctuation.Keys.ToList();
            foreach (var itemCat in categoriesToFluctuate)
            {
                CategoryPriceFluctuation[itemCat] = town.categoryModifiers.Find(x => x.category == itemCat).ReturnRandomPriceFluctuation();
            }
            
            // Removes items that are not from the store standard stock.
            var itemsToRemove = new List<Item>();
            var itemKeys = TownInventory.Keys.ToList();
            foreach (var item in itemKeys)
            {
                var itemInInventory = town.itemInventory.Find(x => x.item == item);
                if (itemInInventory == null)
                {
                    itemsToRemove.Add(item);
                    continue;
                }

                
            }

            foreach (var item in itemsToRemove)
            {
                TownInventory.Remove(item);
            }
        }

        /// <summary>
        /// Checks if the player can interact with this interactable.
        /// </summary>
        public bool CheckIfCanInteract()
        {
            // Towns are always interactables.
            return true;
        }

        /// <summary>
        /// Called when the player is-in range of the interactable.
        /// </summary>
        public void OnInteractableEnter()
        {
            exclamationMark.SetActive(true);
            renderer.sprite = interactableSprite;
        }

        /// <summary>
        /// Called when the player is-off the range of the interactable.
        /// </summary>
        public void OnInteractableExit()
        {
            exclamationMark.SetActive(false);
            renderer.sprite = standbySprite;
        }

        /// <summary>
        /// Called when the player interacts with this interactable.
        /// </summary>
        public void InteractWith()
        { 
            RestockItems();
            
            SetCharacter?.Invoke(town.characterPortrait, town.characterName);
            SetTownInformation?.Invoke(town.name, town.townDescription);
            SetCharacterSpeak?.Invoke(town.welcomeTalk);
            
            UpdateTownInventory?.Invoke(TownInventory, CategoryPriceFluctuation);
            UpdatePlayerInventory?.Invoke(GameManager.Instance.PlayerInventory, CategoryPriceFluctuation);
            
            OpenTownPanel?.Invoke(this);
            
            FMODUnity.RuntimeManager.PlayOneShot(enterSoundEvent);

            GameManager.Instance.playerCanMove = false;
        }

        /// <summary>
        /// Restocks the item depending on how long the player has been away.
        /// </summary>
        public void RestockItems()
        {
            var currentTime = Time.time;
            var timeForStock = currentTime - lastTimeOpen;
            
            var itemKeys = TownInventory.Keys.ToList();
            foreach (var item in itemKeys)
            {
                // If the item doesn't belong to the default inventory, just skip it.
                var itemInInventory = town.itemInventory.Find(x => x.item == item);
                if (itemInInventory == null)
                {
                    continue;
                }

                // If the store is overstocked (only happens when the player is still near town), just ignore restocking.
                var currentInventory = TownInventory[item];
                if (currentInventory >= itemInInventory.stock)
                {
                    continue;
                }
                
                // Does some restocking.
                var stockUnits = Mathf.FloorToInt(timeForStock / itemInInventory.restockTime) + currentInventory;
                TownInventory[item] = Mathf.Clamp(stockUnits, 0, itemInInventory.stock);
            }
        }

        /// <summary>
        /// Changes the town inventory that is being displayed.
        /// </summary>
        public void SellToPlayer(Item item, int finalPrice)
        {
            if (TownInventory[item] == 0)
            {
                SetCharacterSpeak?.Invoke(town.outOfStockTalk);
                return;
            }
            
            if (!GameManager.Instance.CanPlayerPurchase(finalPrice))
            {
                SetCharacterSpeak?.Invoke(town.noMoneyTalk);
                return;
            }

            GameManager.Instance.ChangePlayerMoney(-finalPrice);
            TownInventory[item]--;
            GameManager.Instance.ChangePlayerInventory(item, 1);
            
            if (TownInventory[item] == 0)
            {
                SetCharacterSpeak?.Invoke(town.outOfStockTalk);

                if (town.itemInventory.Find(x => x.item == item) == null)
                {
                    TownInventory.Remove(item);
                }
            }
            else
            {
                SetCharacterSpeak?.Invoke(town.buyTalk);
            }
            
            UpdateTownInventory?.Invoke(TownInventory, CategoryPriceFluctuation);
            UpdatePlayerInventory?.Invoke(GameManager.Instance.PlayerInventory, CategoryPriceFluctuation);
        }

        /// <summary>
        /// Purchases an item from the player.
        /// </summary>
        public void PurchaseFromPlayer(Item item, int finalPrice)
        {
            GameManager.Instance.ChangePlayerInventory(item, -1);
            GameManager.Instance.ChangePlayerMoney(finalPrice);

            if (TownInventory.ContainsKey(item))
            {
                TownInventory[item]++;
            }
            else
            {
                TownInventory.Add(item, 1);
            }

            SetCharacterSpeak?.Invoke(town.sellTalk);
            
            UpdateTownInventory?.Invoke(TownInventory, CategoryPriceFluctuation);
            UpdatePlayerInventory?.Invoke(GameManager.Instance.PlayerInventory, CategoryPriceFluctuation);
        }

        /// <summary>
        /// Called when town panel is closed.
        /// </summary>
        public void ClosedTownPanel()
        {
            lastTimeOpen = Time.time;
            GameManager.Instance.playerCanMove = true;
        }

        /// <summary>
        /// Called when the player is-in discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryEnter()
        {
            GameManager.DiscoveryTracker.AddNewDiscovery(town);
        }

        /// <summary>
        /// Called when the player is-off the discovery range of the interactable.
        /// </summary>
        public void OnDiscoveryExit()
        {
            GameManager.AudioManager.SetMusicParameter("town", 0);
        }

        /// <summary>
        /// Emits the discovery audio when the player gets close to it.
        /// </summary>
        public void EmitAudio(float distNormalized)
        {
            GameManager.AudioManager.SetMusicParameter("town", town.audioIdentifier);
            GameManager.AudioManager.SetMusicParameter("prox", distNormalized);
        }
        
        /// <summary>
        /// Returns the objects position.
        /// </summary>
        public Vector2 GetPosition()
        {
            return transform.position;
        }
        
        /// <summary>
        /// OnDrawGizmos draws gizmos that are also pickable and always drawn.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (town == null)
            {
                return;
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, town.playerSpotRadius);
        }
    }
}