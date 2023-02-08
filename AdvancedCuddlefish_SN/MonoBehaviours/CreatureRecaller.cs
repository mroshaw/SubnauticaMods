using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static WorldStreaming.BatchOctreesStreamer;

namespace DaftAppleGames.EnhancedCuddlefish_SN.MonoBehaviours
{
    /// <summary>
    /// MonoBehaviour to recall all CreatureRecallListeners to current transform location
    /// </summary>
    internal class CreatureRecaller : MonoBehaviour
    {
        private CreatureRecallListener[] _allCreatureRecallListeners;

        public void Start()
        {

        }

        /// <summary>
        /// Refresh the list of Recall Listeners
        /// </summary>
        private void RefreshCreatureRecallListeners()
        {
            EnhancedCuddlefishPlugin.Log.LogDebug("Refreshing CreatureRecallListeners...");
            _allCreatureRecallListeners = FindObjectsOfType<CreatureRecallListener>();
            EnhancedCuddlefishPlugin.Log.LogDebug($"Found {_allCreatureRecallListeners.Length} CreatureRecallListeners.");
        }

        /// <summary>
        /// Public method to recall all Listeners to current transform location
        /// </summary>
        public void RecallAllCreatures()
        {
            RefreshCreatureRecallListeners();
            EnhancedCuddlefishPlugin.Log.LogDebug($"Recalling all RecallCreatureListeners ({_allCreatureRecallListeners.Length})");
            float buffer = 1.0f;

            foreach (CreatureRecallListener listener in _allCreatureRecallListeners)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug($"Recalling {listener.gameObject.name}...");
                Vector3 targetPosition = transform.position + (Vector3.forward * buffer);
                listener.RecallCreature(targetPosition);
                buffer++;
                EnhancedCuddlefishPlugin.Log.LogDebug($"{listener.gameObject.name} recalled.");
            }
        }
    }
}
