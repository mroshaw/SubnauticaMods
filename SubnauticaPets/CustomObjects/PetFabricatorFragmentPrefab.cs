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
        private static readonly string _modelName = "model";

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

            PrefabTemplate cloneTemplate = new CloneTemplate(clonePrefab.Info, TechType.GravSphereFragment)
            {
                ModifyPrefab = prefab =>
                {
                    // Remove old model
                    GameObject oldModelGameObject = prefab.FindChild(_modelName);
                    if (oldModelGameObject != null)
                    {
                        Log.LogDebug($"PetFabricatorFragmentPrefab: Destroying old model...");
                       Object.Destroy(oldModelGameObject);
                    }
                    else
                    {
                        Log.LogDebug($"PetFabricatorFragmentPrefab: Old model not found.");
                    }

                    // Replace model
                    GameObject damagedConsoleGameObject =
                        ModUtils.GetGameObjectInstanceFromAssetBundle(_petFabricatorModelName);

                    GameObject modelGameObject = damagedConsoleGameObject.FindChild(_modelName);

                    // Add new model
                    Log.LogDebug($"PetFabricatorFragmentPrefab: InitPrefab is setting the model for {prefab.name} to {
                        modelGameObject.name}...");
                    modelGameObject.transform.SetParent(prefab.transform);
                    modelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    modelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    MaterialUtils.ApplySNShaders(modelGameObject);

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
