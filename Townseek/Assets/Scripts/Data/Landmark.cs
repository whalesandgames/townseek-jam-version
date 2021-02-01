using System.Collections.Generic;
using AssetIcons;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Defines a landmark in the map.
    /// </summary>
    [CreateAssetMenu(fileName = "New Landmark", menuName = "Map Game/Landmark", order = 41)]
    public class Landmark : Discovery
    {
        [Header("Town Appearance")]
        [TextArea]
        public List<string> landmarkDescriptions;
        [AssetIcon]
        public Sprite landmarkIcon;
    }
}