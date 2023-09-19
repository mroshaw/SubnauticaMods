using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Fabricator;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetFabricatorFragmentPrefab
    {
        private static readonly string PrefabId = "PetFabricatorFragment";
        public static readonly string NewModelName = "newmodel";

#if SUBNAUTICA
        private static readonly List<Vector3> CoordinatedSpawns = new List<Vector3>
        {
            new Vector3(-172.27f,-43.07f, -234.29f),
            new Vector3(-385.8968f,-118.473f, 623.9641f),
            new Vector3(-1603.463f,-342.9196f, 79.65623f),
            new Vector3(-773.6248f,-212.8109f, -729.6456f),
            new Vector3(-31.82343f,-24.19011f, -417.7448f),
            new Vector3(12.00361f,-25.82509f, -243.0502f),
            new Vector3(76.30077f,-24.12408f, -88.81075f),
            new Vector3(82.54358f,-37.00394f, 117.0964f),
            new Vector3(376.3995f,-25.73f, -209.09f)
        };
#endif
#if SUBNAUTICAZERO
        private static readonly List<Vector3> CoordinatedSpawns = new List<Vector3>
        {
            new Vector3(-171.25f,-41.34f, -234.25f)
        };
#endif
        public static PrefabInfo PrefabInfo;

        public static void InitPrefab()
        {
            CustomPrefab clonePrefab = new CustomPrefab(PrefabId, null, null);

            PrefabTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4") //TechType.GravSphereFragment)
            {
                ModifyPrefab = prefab =>
                {
                    // Add component
                    Log.LogDebug("PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component...");
                    prefab.AddComponent<PetFabricatorFragment>();
                    Log.LogDebug(
                        "PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component... Done.");
                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            ModUtils.SetupCoordinatedSpawn(clonePrefab.Info.TechType, CoordinatedSpawns);
            Log.LogDebug($"PetFabricatorFragmentPrefab: Registering {PrefabId}...");
            clonePrefab.Register();
            Log.LogDebug($"PetFabricatorFragmentPrefab: Init Prefab for {PrefabId}. Done.");
            PrefabInfo = clonePrefab.Info;
        }
    }
}
