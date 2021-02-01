using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Defines an entry of how much stock a town has of an item.
    /// </summary>
    [System.Serializable]
    public class ItemInventory
    {
        [HorizontalGroup]
        [HideLabel]
        [ValueDropdown("GetAllItems", IsUniqueList = true, AppendNextDrawer = true, FlattenTreeView = true)]
        public Item item;
        [HorizontalGroup, LabelWidth(40)]
        public int stock;
        [Tooltip("Restock Time")]
        [LabelText("RT")]
        [HorizontalGroup, LabelWidth(20)]
        public int restockTime;
        
        #if UNITY_EDITOR
        /// <summary>
        /// Returns all of the item categories to populate the list.
        /// </summary>
        private static IEnumerable GetAllItems()
        {
            return DataHelpers.ReturnScriptableObjects<Item>();
        }
        #endif
    }
}