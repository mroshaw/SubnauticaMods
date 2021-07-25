using System;
using System.Collections.Generic;
using Logger = QModManager.Utility.Logger;
using UnityEngine;

namespace CreaturePetMod_BZ
{
    static class PetBehaviour
    {

        internal static void ConfigureBasePet(GameObject petCreatureGameObject)
        {
            // Configure base creature behaviours
            Creature creaturePet = petCreatureGameObject.GetComponent<Creature>();
            SwimWalkCreatureController swimWalk = creaturePet.GetComponent<SwimWalkCreatureController>();
            Logger.Log(Logger.Level.Debug, $"Creature is: {swimWalk.state}");
            swimWalk.state = SwimWalkCreatureController.State.Walk;

            creaturePet.Friendliness.Value = 1.0f;
            creaturePet.Aggression.Value = 0.0f;
            creaturePet.Scared.Value = 0.0f;
            creaturePet.Hunger.Value = 0.0f;
        }

        internal static void ConfigureSnowStalkerBaby(GameObject petCreatureGameObject)
        {
            SnowStalkerBaby snowStalkerPet = petCreatureGameObject.GetComponent<SnowStalkerBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring SnowStalker: {snowStalkerPet.name}");

            CreatureAction moveOnSurface = snowStalkerPet.GetComponent<MoveOnSurface>();
            if (moveOnSurface)
            {
                Logger.Log(Logger.Level.Debug, $"Found move on surface component");
                moveOnSurface.TryStartAction();
            }

            List<CreatureAction> allActions = snowStalkerPet.actions;
            List<CreatureAction> actionsToRemove = new List<CreatureAction>();

            // Find undesireable actions, in order to remove them
            Logger.Log(Logger.Level.Debug, $"There are {allActions.Count} actions");
            foreach (CreatureAction action in allActions)
            {
                System.Type actionType = action.GetType();
                Logger.Log(Logger.Level.Debug, $"Found: {actionType}");
                // Remove the Leash and Swim actions
                if (actionType == typeof(StayAtLeashPosition) || actionType == typeof(GoToDivePoint))
                {
                    Logger.Log(Logger.Level.Debug, $"Found: {actionType} and added to remove list");
                    actionsToRemove.Add(action);
                }
                if (actionType == typeof(MoveOnSurface))
                {
                    Logger.Log(Logger.Level.Debug, $"Force move on surface action");
                    snowStalkerPet.TryStartAction(action);
                    action.Perform(99.99f, 1.0f);
                }
            }

            // Now remove them
            Logger.Log(Logger.Level.Debug, $"Removing actions");
            foreach (CreatureAction actionToRemove in actionsToRemove)
            {
                Logger.Log(Logger.Level.Debug, $"Removed: {actionToRemove.GetType()}");
                allActions.Remove(actionToRemove);
            }
            Logger.Log(Logger.Level.Debug, $"There are now {allActions.Count} actions");
            // Shake down!
            snowStalkerPet.GetAnimator().SetTrigger("dryFur");
        }

        internal static void ConfigurePenglingBaby(GameObject petCreatureGameObject)
        {
            PenguinBaby penglingPet = petCreatureGameObject.GetComponent<PenguinBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling: {penglingPet.name}");
        }
    }
}
