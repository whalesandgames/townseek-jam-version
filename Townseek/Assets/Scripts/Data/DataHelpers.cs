using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Helps perform various functions across the project.
    /// </summary>
    public class DataHelpers : MonoBehaviour
    {
        #if UNITY_EDITOR
        /// <summary>
        /// Find all Scriptable Object of a certain type.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable ReturnScriptableObjects<T>()
        {
            return UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(x)));
        }
        #endif
    }
}