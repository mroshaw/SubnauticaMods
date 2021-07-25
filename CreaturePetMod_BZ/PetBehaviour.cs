using System;
using System.Collections.Generic;
using Logger = QModManager.Utility.Logger;
using UnityEngine;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Allows us to configure standard and pet specific behaviours to our pet instance
    /// </summary>
    static class PetBehaviour
    {
        /// <summary>
        /// Configure base creature behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigureBasePet(GameObject petCreatureGameObject)
        {
            // Configure base creature behaviours
            Creature creaturePet = petCreatureGameObject.GetComponent<Creature>();

            creaturePet.Friendliness.Value = 1.0f;
            creaturePet.Aggression.Value = 0.0f;
            creaturePet.Scared.Value = 0.0f;
            creaturePet.Hunger.Value = 0.0f;
        }

        /// <summary>
        /// Configure Snow Stalker Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigureSnowStalkerBaby(GameObject petCreatureGameObject)
        {
            SnowStalkerBaby snowStalkerPet = petCreatureGameObject.GetComponent<SnowStalkerBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring SnowStalker: {snowStalkerPet.name}");

            // Shake down!
            snowStalkerPet.GetAnimator().SetTrigger("dryFur");
        }

        /// <summary>
        /// Conifugre Pengling Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigurePenglingBaby(GameObject petCreatureGameObject)
        {
            PenguinBaby penglingPet = petCreatureGameObject.GetComponent<PenguinBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling: {penglingPet.name}");
        }
    }
}
