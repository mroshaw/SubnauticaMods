using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Linq;

namespace CreaturePetMod_BZ
{
	class DebugCreature
	{
		// Creature
		// Token: 0x06000D01 RID: 3329 RVA: 0x0004BD80 File Offset: 0x00049F80
		[HarmonyPatch(typeof(Creature))]
		[HarmonyPatch("ChooseBestAction")]
		private class DegubChooseBestAction
		{
			[HarmonyPrefix]
			private static bool DebugChooseBestAction(Creature __instance, float time, ref CreatureAction __result)
			{

				Logger.Log(Logger.Level.Debug, $"Getting best action for: {__instance.GetType()}. Time is: {time}");
				if (__instance.actions.Count == 0)
				{
					__result = null;
				}
				float num = 0f;
				CreatureAction creatureAction = null;
				if (__instance.prevBestAction != null)
				{
					creatureAction = __instance.prevBestAction;
					num = creatureAction.Evaluate(time);
					Logger.Log(Logger.Level.Debug, $"Had previous action: {__instance.prevBestAction.GetType()}. Evaluate (num) returned: {num}");
					creatureAction.timeLastChecked = time;
				}
				int num2 = __instance.indexLastActionChecked + 1;
				if (num2 >= __instance.actions.Count)
				{
					num2 = 0;
				}
				__instance.indexLastActionChecked = num2;
				CreatureAction creatureAction2 = __instance.prevBestAction;
				for (int i = 0; i < __instance.actions.Count; i++)
				{
					CreatureAction creatureAction3 = __instance.actions[i];
					if (!(creatureAction3 == __instance.prevBestAction) && (i == num2 || creatureAction3.NeedsToBeChecked(time)))
					{
						Logger.Log(Logger.Level.Debug, $"Assessing: {creatureAction3.GetType()} with MaxEvaluatePriority: {creatureAction3.GetMaxEvaluatePriority()}");
						creatureAction3.timeLastChecked = time;
						Logger.Log(Logger.Level.Debug, $"1");
						if (creatureAction3.GetMaxEvaluatePriority() > num)

							Logger.Log(Logger.Level.Debug, $"2");
						{
							float num3 = creatureAction3.Evaluate(time);
							Logger.Log(Logger.Level.Debug, $"Evaluate returned num3: {num3} against num: {num}");
							Logger.Log(Logger.Level.Debug, $"3");
							if (num3 > num)
							{
								num = num3;
								Logger.Log(Logger.Level.Debug, $"4");
								creatureAction = creatureAction3;
							}
						}
					}
				}
				Logger.Log(Logger.Level.Debug, $"5");
				if (creatureAction)
				{
					Logger.Log(Logger.Level.Debug, $"Best action found: {creatureAction.GetType()}");
				}
				else
				{
					Logger.Log(Logger.Level.Debug, $"Best action is null");
				}

				__result = creatureAction;
				return false;
			}
		}


		// [HarmonyPatch(typeof(SwimWalkCreatureController))]
		// [HarmonyPatch("set_state")]
		internal class DebugSwimWalkCreatureController
		{
			// [HarmonyPrefix]
			private static void DebugSetState(SwimWalkCreatureController __instance, SwimWalkCreatureController.State value)
			{
				__instance._state = value;
				bool flag = __instance._state == SwimWalkCreatureController.State.Swim;
				__instance.animateByVelocity.enabled = flag;
				Behaviour[] array = __instance.swimBehaviours;
				for (int i = 0; i < array.Length; i++)
				{
					Logger.Log(Logger.Level.Debug, $"Swimbehaviour: {array[i].GetType()}");
					array[i].enabled = flag;
				}
				array = __instance.walkBehaviours;
				for (int i = 0; i < array.Length; i++)
				{
					Logger.Log(Logger.Level.Debug, $"Walkbehaviour: {array[i].GetType()}");
					array[i].enabled = !flag;
				}
				if (__instance.swimColliders != null)
				{
					Collider[] array2 = __instance.swimColliders;
					for (int i = 0; i < array2.Length; i++)
					{
						Logger.Log(Logger.Level.Debug, $"Swimcollider: {array2[i].GetType()}");
						array2[i].enabled = flag;
					}
				}
				if (__instance.walkColliders != null)
				{
					Collider[] array2 = __instance.walkColliders;
					for (int i = 0; i < array2.Length; i++)
					{
						Logger.Log(Logger.Level.Debug, $"Walkcollider: {array2[i].GetType()}");
						array2[i].enabled = !flag;
					}
				}
				__instance.creature.ScanCreatureActions();
				__instance.locomotion.freezeHorizontalRotation = !flag;
				__instance.animator.SetBool("in_water", flag);
				__instance.landCreatureGravity.SetEnabled(!flag);
				__instance.stateChangedEvent.Trigger(__instance._state);
			}
		}

		[HarmonyPatch(typeof(CreatureAction))]
		[HarmonyPatch("Evaluate")]
		private class DebugActionEvaluate
		{
			[HarmonyPostfix]
			private static void DebugGetEvaluatePriority(CreatureAction __instance, ref float __result)
			{
				// Get parent Creature
				Creature creature = __instance.creature;
				if (creature.GetType() == typeof(SnowStalkerBaby) && __instance.GetType() == typeof(AvoidObstaclesOnLand))
				{
					//
					Logger.Log(Logger.Level.Debug, $"Creature: {creature.GetType()}, CreatureAction has: {__instance.evaluatePriority}, {__instance.priorityMultiplier}. Result was: {__result}");
					__result = 0.923f;
				}
			}
		}



		// Creature
		// Token: 0x06000D01 RID: 3329 RVA: 0x0004BD80 File Offset: 0x00049F80
		[HarmonyPatch(typeof(AvoidObstaclesOnLand))]
		[HarmonyPatch("Evaluate")]
		private class DegubAvoidObstacles
		{
			[HarmonyPrefix]
			private static bool DebugEvaluate(AvoidObstaclesOnLand __instance, float time, ref float __result)
			{
				Logger.Log(Logger.Level.Debug, $"In Evaluate");
				// Get parent Creature
				Creature creature = __instance.creature;
				Logger.Log(Logger.Level.Debug, $"AvoidObstacles for {creature.GetType()}. TimeStartAvoidance: {__instance.timeStartAvoidance}, AvoidanceDuration: {__instance.avoidanceDuration}, TimeNextScan: {__instance.timeNextScan}. Time: {time}");
				if (time < __instance.timeStartAvoidance + __instance.avoidanceDuration)
				{
					Logger.Log(Logger.Level.Debug, $"Returning GetEvaluatePriority");
					__result = __instance.GetEvaluatePriority();
				}
				if (time > __instance.timeNextScan)
				{
					Logger.Log(Logger.Level.Debug, $"Doing a Scan...");
					__instance.timeNextScan = time + __instance.scanInterval;
					Transform transform = __instance.creature.transform;
					RaycastHit hit;
					if (Physics.Raycast(transform.TransformPoint(__instance.positionOffset), transform.forward, out hit, __instance.scanDistance, -5, QueryTriggerInteraction.Ignore) && __instance.IsObstacle(hit))
					{
						Logger.Log(Logger.Level.Debug, $"Raycast hit!");
						__instance.moveDirectionFound = false;
						if (creature.GetType() == typeof(SnowStalkerBaby))
						{
							Logger.Log(Logger.Level.Debug, $"Snowstalker Raycast Hit!");
							__result = 0.923f;
							return false;
						}
						else
						{
							__result = __instance.GetEvaluatePriority();
							return false;
						}
					}
					else
					{
						Logger.Log(Logger.Level.Debug, $"Raycast did not hit");
					}
				}
				__result = 0f;
				return false;
			}
		}
	}
}

