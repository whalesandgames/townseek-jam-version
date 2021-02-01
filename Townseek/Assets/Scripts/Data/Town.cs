using System.Collections.Generic;
using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Town that the player can trade with.
    /// </summary>
    [CreateAssetMenu(fileName = "New Town", menuName = "Map Game/Town", order = 40)]
    public class Town : Discovery
    {
        [Header("Town Appearance")]
        [TextArea]
        public string townDescription;
        [AssetIcon]
        public Sprite townIcon;
        
        [Header("Town Inventory")]
        [ListDrawerSettings(ShowIndexLabels = false)]
        public List<ItemInventory> itemInventory = new List<ItemInventory>();
        [ListDrawerSettings(ShowIndexLabels = false)]
        public List<ItemCategoryModifier> categoryModifiers = new List<ItemCategoryModifier>();
        public float playerSpotRadius;

        [Header("Town Audio")]
        [SerializeField]
        public int audioIdentifier;

        [Header("Town Character")]
        public string characterName;
        public Sprite characterPortrait;
        
        [Header("Town Talk")]
        [TextArea]
        public string welcomeTalk;
        [TextArea]
        public string noMoneyTalk;
        [TextArea]
        public string buyTalk;
        [TextArea]
        public string sellTalk;
        [TextArea]
        public string outOfStockTalk;
    }
}