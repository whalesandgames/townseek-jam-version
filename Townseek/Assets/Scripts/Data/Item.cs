using System.Collections;
using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Item that is traded with other towns.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Map Game/Item", order = 0)]
    public class Item : ScriptableObject
    {
        [ValueDropdown("GetAllItemCategories", IsUniqueList = true, AppendNextDrawer = true, FlattenTreeView = true)]
        public ItemCategory category;
        [AssetIcon]
        public Sprite icon;
        public int price;
        public bool isSellable = true;
        
        #if UNITY_EDITOR
        /// <summary>
        /// Returns all of the item categories to populate the list.
        /// </summary>
        private static IEnumerable GetAllItemCategories()
        {
            return DataHelpers.ReturnScriptableObjects<ItemCategory>();
        }
        #endif
    }
}