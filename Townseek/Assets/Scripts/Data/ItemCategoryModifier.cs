using System.Collections;
using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Item categories define how much a town affects the price of items in stock.
    /// Ideally depending on their motifs and ideals.
    /// </summary>
    [System.Serializable]
    public class ItemCategoryModifier
    {
        [HorizontalGroup]
        [HideLabel]
        [ValueDropdown("GetAllItemCategories", IsUniqueList = true, AppendNextDrawer = true, FlattenTreeView = true)]
        public ItemCategory category;
        [LabelText("PF")]
        [Tooltip("Price Fluctuation (Min-Max)")]
        [HorizontalGroup, LabelWidth(20)]
        public Vector2 priceFluctuation;

        /// <summary>
        /// Returns a fluctuation of the price.
        /// </summary>
        /// <returns></returns>
        public float ReturnRandomPriceFluctuation()
        {
            return Random.Range(priceFluctuation.x, priceFluctuation.y);
        }

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