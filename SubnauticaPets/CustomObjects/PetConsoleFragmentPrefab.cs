using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetConsoleFragmentPrefab
    {
        private static readonly string _prefabId = "PetConsoleFragment";
        private static readonly string _petConsolePrefabName = "PetConsoleDamaged";
        private static readonly string _modelName = "model";
#if SUBNAUTICA
        private static readonly List<Vector3> _coordinatedSpawns = new List<Vector3>
        {
            new Vector3(-171.25f,-41.34f, -234.25f),
            new Vector3(-47.14f, -29.15f, -409.04f),
            new Vector3(-163.87f, -40.96f, -250.69f )
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
                        Log.LogDebug($"PetConsoleFragmentPrefab: Destroying old model.");
                        Object.Destroy(oldModelGameObject);
                    }
                    else
                    {
                        Log.LogDebug($"PetConsoleFragmentPrefab: Old model not found.");
                    }
                    // Replace model
                    GameObject damagedConsoleGameObject =
                        ModUtils.GetGameObjectInstanceFromAssetBundle(_petConsolePrefabName);

                    GameObject modelGameObject = damagedConsoleGameObject.FindChild(_modelName);

                    // Add new model
                    Log.LogDebug($"PetConsoleFragmentPrefab: InitPrefab is setting the model for {prefab.name} to {modelGameObject.name}...");
                    modelGameObject.transform.SetParent(prefab.transform);
                    modelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    modelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    MaterialUtils.ApplySNShaders(modelGameObject);

                    // Add component
                    Log.LogDebug($"PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component...");
                    prefab.AddComponent<PetConsoleFragment>();
                    Log.LogDebug(
                        $"PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component... Done.");
                }
            };
            clonePrefab.SetGameObject(cloneTemplate);
            ModUtils.SetupCoordinatedSpawn(clonePrefab.Info.TechType, _coordinatedSpawns);
            Log.LogDebug($"PetConsoleFragmentPrefab: Registering {_prefabId}...");
            clonePrefab.Register();
            Log.LogDebug($"PetConsoleFragmentPrefab: Init Prefab for {_prefabId}. Done.");
            PrefabInfo = clonePrefab.Info;
        }
    }
}
