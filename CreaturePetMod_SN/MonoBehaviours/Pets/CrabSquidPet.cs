using UnityEngine;
using static DaftAppleGames.CreaturePetMod_SN.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetMod_SN.MonoBehaviours.Pets
{
    /// <summary>
    /// Implements CrabSquid specific Pet functionality
    /// </summary>
    internal class CrabSquidPet : Pet
    {

        // Crab Squid scale factor
        private float _scaleFactor = 0.08f;

        /// <summary>
        /// Add Creature specific components
        /// </summary>
        public override void AddComponents()
        {
            base.AddComponents();
        }

        /// <summary>
        /// Remove Creature specific components
        /// </summary>
        public override void RemoveComponents()
        {
            base.RemoveComponents();
        }

        /// <summary>
        /// Update Creature specific components
        /// </summary>
        public override void UpdateComponents()
        {
            // Set the scale of the CrabSquid
            Log.LogDebug($"CrabSquidPet: Setting transform scale to {_scaleFactor}");
            gameObject.transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
            Log.LogDebug($"CrabSquidPet: Setting transform scale to {_scaleFactor}. Done");

            base.UpdateComponents();
        }
    }
}