using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;

namespace CreaturePetMod_BZ
{
	/// <summary>
	/// Class for patching the specific movement of SnowStalkerBaby pets
	/// </summary>
	class SnowStalkderMovementPatcher
	{
		/// <summary>
		/// Patches surface movement
		/// </summary>
		[HarmonyPatch(typeof(MoveOnSurface))]
		[HarmonyPatch("Perform")]
		private class MoveOnSurfacePatch
		{
			/// <summary>
			/// Overrides the Perform method specifically for SnowStlkerBaby pets, forcing the GoToInternal behaviour
			/// </summary>
			/// <param name="__instance"></param>
			/// <param name="time"></param>
			/// <param name="deltaTime"></param>
			/// <returns></returns>
			[HarmonyPrefix]
			private static bool PerformOverride(MoveOnSurface __instance, float time, float deltaTime)
			{
				// Get parent Creature and check that it's a Pet
				Creature creature = __instance.creature;
				if (creature.GetType() != typeof(SnowStalkerBaby) || !PetUtils.IsCreaturePet(creature))
				{
					// Invoke the "unpatched" method
					return true;
				}

				if (__instance.timeNextTarget <= time)
				{
					__instance.desiredPosition = __instance.FindRandomPosition();
					__instance.timeNextTarget = time + __instance.updateTargetInterval + __instance.updateTargetRandomInterval * UnityEngine.Random.value;
					// Enforce the "GoToInternal" behaviuour
					__instance.walkBehaviour.GoToInternal(__instance.desiredPosition, (__instance.desiredPosition - creature.transform.position).normalized, __instance.moveVelocity);
					Logger.Log(Logger.Level.Debug, $"{creature.GetType()} is walking to random target");
				}
				return false;
			}
		}
	}
}
