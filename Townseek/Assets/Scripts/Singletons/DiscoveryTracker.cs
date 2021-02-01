using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using WhalesAndGames.MapGame.Data;
using WhalesAndGames.MapGame.Logic;
using WhalesAndGames.MapGame.UI;

namespace WhalesAndGames.MapGame.Singletons
{
    /// <summary>
    /// Tracks and Manages all of the Discoveries, namely displaying information about the towns.
    /// </summary>
    public class DiscoveryTracker : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        public List<Discovery> PlayerDiscoveries = new List<Discovery>();

        [ReadOnly]
        public List<TownBehaviour> listOfTowns = new List<TownBehaviour>();
        [ReadOnly]
        public List<LandmarkBehaviour> listOfLandmarks = new List<LandmarkBehaviour>();

        private int discoveriesFound = 0;
        private int totalDiscoveries = 0;

        public delegate void NewDiscoveryNotificationEventHandler(Discovery discovery);
        public event NewDiscoveryNotificationEventHandler NewDiscoveryNotification;

        public delegate void UpdateDiscoveryTotalEventHandler(int discoveriesFound, int totalDiscoveries);
        public event UpdateDiscoveryTotalEventHandler UpdateDiscoveriesTotal;

        /// <summary>
        /// Initializes the Discovery Tracker by tracking discoveries.
        /// </summary>
        public void Initialize()
        {
            foreach (var tB in listOfTowns)
            {
                tB.Initialize(this);
            }
            
            totalDiscoveries = listOfTowns.Count + listOfLandmarks.Count;
            UpdateDiscoveriesTotal?.Invoke(discoveriesFound, totalDiscoveries);
        }

        /// <summary>
        /// Adds a new discovery to the discoveries that the player has found.
        /// </summary>
        public void AddNewDiscovery(Discovery discovery)
        {
            if (PlayerDiscoveries.Contains(discovery))
            {
                return;
            }
            
            discoveriesFound += 1;
            PlayerDiscoveries.Add(discovery);
            
            NewDiscoveryNotification?.Invoke(discovery);
            UpdateDiscoveriesTotal?.Invoke(discoveriesFound, totalDiscoveries);
            
            GameManager.AudioManager.PlayDiscoveryEvent();
        }
        
        #if UNITY_EDITOR
        /// <summary>
        /// Adds the existing towns and landmarks to the lists, caching them before the player gets into play mode.
        /// </summary>
        [Button("Index Discoveries", ButtonSizes.Large)]
        private void UpdateLists()
        {
            listOfTowns.Clear();
            listOfLandmarks.Clear();
            
            listOfTowns = FindObjectsOfType<TownBehaviour>().ToList();
            listOfLandmarks = FindObjectsOfType<LandmarkBehaviour>().ToList();
        }
        #endif
    }
}