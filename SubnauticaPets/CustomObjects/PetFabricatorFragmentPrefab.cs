using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Fabricator;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetFabricatorFragmentPrefab
    {
        private static string _prefabId = "PetFabricatorFragment";
        private static string _petFabricatorModelName = "PetFabricatorDamaged";
        public static readonly string NewModelName = "newmodel";
        private static readonly string _oldModelName = "model";

#if SUBNAUTICA
        private static readonly List<Vector3> _coordinatedSpawns = new List<Vector3>
        {
            new Vector3(-172.27f,-43.07f, -234.29f)
        };
#endif
#if SUBNAUTICAZERO
        private static List<Vector3> _coordinatedSpawns = new List<Vector3>
        {
            new Vector3(-171.25f,-41.34f, -234.25f)
        };
#endif
        public static PrefabInfo PrefabInfo;

        public static void InitPrefab()
        {
            CustomPrefab clonePrefab = new CustomPrefab(_prefabId, null, null);

            PrefabTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4") //TechType.GravSphereFragment)
            {
                ModifyPrefab = prefab =>
                {
                    // Add component
                    Log.LogDebug($"PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component...");
                    prefab.AddComponent<PetFabricatorFragment>();
                    Log.LogDebug(
                        $"PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component... Done.");
                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            ModUtils.SetupCoordinatedSpawn(clonePrefab.Info.TechType, _coordinatedSpawns);
            Log.LogDebug($"PetFabricatorFragmentPrefab: Registering {_prefabId}...");
            clonePrefab.Register();
            Log.LogDebug($"PetFabricatorFragmentPrefab: Init Prefab for {_prefabId}. Done.");
            PrefabInfo = clonePrefab.Info;
        }
    }
}
