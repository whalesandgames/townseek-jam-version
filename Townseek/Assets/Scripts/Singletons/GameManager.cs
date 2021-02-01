using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.UI;

namespace WhalesAndGames.MapGame.Singletons
{
    /// <summary>
    /// Manages the state of the game and the player's inventory.
    /// </summary>
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [BoxGroup("Game State")]
        public GameState gameState = GameState.Title;
        [BoxGroup("Game State")]
        public bool playerCanMove = true;

        [BoxGroup("Game Elements")]
        public GameObject playerShip;
        
        [BoxGroup("Player Inventory")]
        public int money = 100;
        [BoxGroup("Player Inventory")]
        public List<StartingItem> startingItems = new List<StartingItem>();
        [BoxGroup("Player Inventory")]
        [ReadOnly]
        [ShowInInspector]
        public Dictionary<Item, int> PlayerInventory = new Dictionary<Item, int>();

        public delegate void MoneyChangedEventHandler(float money);
        public event MoneyChangedEventHandler PlayerMoneyChanged;

        public delegate void PlayerInventoryChangedEventHandler(Dictionary<Item, int> playerInventory);

        public event PlayerInventoryChangedEventHandler PlayerInventoryChanged;
        
        public delegate void PressedToStartEventHandler();

        public event PressedToStartEventHandler PressedToStart;

        // Audio
        public static DiscoveryTracker DiscoveryTracker;
        public static AudioManager AudioManager;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            AudioManager = GetComponentInChildren<AudioManager>();
            DiscoveryTracker = GetComponentInChildren<DiscoveryTracker>();

            CanvasManager.Instance.Initialize(this, DiscoveryTracker);
            DiscoveryTracker.Initialize();
        }
        
        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            PlayerMoneyChanged?.Invoke(money);
            
            GlobalManager.Instance.playerInput.actions["PressToStart"].performed += PressToStart;
        }
        
        /// <summary>
        /// Player pressed to start, which sets the Game State as tutorial.
        /// </summary>
        public void PressToStart()
        {
            AudioManager.PlayDiscoveryEvent();
            
            gameState = GameState.Tutorial;
            PressedToStart?.Invoke();
            
            GlobalManager.Instance.playerInput.actions["PressToStart"].performed -= PressToStart;
        }

        /// <summary>
        /// Player pressed to start, which sets the Game State as tutorial.
        /// </summary>
        public void PressToStart(InputAction.CallbackContext obj)
        {
            AudioManager.PlayDiscoveryEvent();
            
            gameState = GameState.Tutorial;
            PressedToStart?.Invoke();
            
            GlobalManager.Instance.playerInput.actions["PressToStart"].performed -= PressToStart;
        }

        /// <summary>
        /// Starts the game by setting the game state and summoning the player ship.
        /// </summary>
        public void StartGame()
        {
            var playerShipAnimator = playerShip.GetComponent<Animator>();
            playerShipAnimator.SetTrigger("Start");

            gameState = GameState.Playing;
            playerCanMove = true;
        }

        /// <summary>
        /// Returns if the player has enough money for a transaction.
        /// </summary>
        public bool CanPlayerPurchase(int transaction)
        {
            return money > transaction;
        }

        /// <summary>
        /// Changes the money of the player based on the given value.
        /// </summary>
        public void ChangePlayerMoney(int value)
        {
            AudioManager.PlayCoinEvent();
            
            money += value;
            PlayerMoneyChanged?.Invoke(money);
        }
        
        /// <summary>
        /// Changes the player inventory.
        /// </summary>
        public void ChangePlayerInventory(Item item, int quantity)
        {
            if (PlayerInventory.ContainsKey(item))
            {
                PlayerInventory[item] += quantity;   
            }
            else
            {
                PlayerInventory.Add(item, quantity);
            }

            if (PlayerInventory[item] == 0)
            {
                PlayerInventory.Remove(item);
            }
            
            PlayerInventoryChanged?.Invoke(PlayerInventory);
        }

        /// <summary>
        /// Toggles if the player can move.
        /// </summary>
        public void TogglePlayerCanMove(bool enabled)
        {
            playerCanMove = enabled;
        }
    }
    
    /// <summary>
    /// Defines a struct of starting items.
    /// </summary>
    [System.Serializable]
    public struct StartingItem
    {
        public Item item;
        public int quantity;
    }
}