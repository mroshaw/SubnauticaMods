using DaftAppleGames.SubnauticaPets.BaseParts;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    internal class PetConsoleFragmentPrefab
    {
        public static PrefabInfo Info;

        /// <summary>
        /// Initialise Pet Console Fragment prefab
        /// </summary>
        public static void Register()
        {
            Info = PrefabInfo
                .WithTechType("PetConsoleFragment", null, null, unlockAtStart: false);
            CustomPrefab consoleFragmentPrefab = new CustomPrefab(Info);

            // Base upgrade console fragment
            CloneTemplate cloneTemplate = new CloneTemplate(Info, "7eaf11d3-5b65-4325-a249-d69c7cc838b0")

            {
                ModifyPrefab = obj =>
                {
                    if (!obj)
                    {
                        LogUtils.LogError(LogArea.Prefabs, $"PetConsoleFragmentPrefab cloned obj is null!");
                    }
                    LogUtils.LogDebug(LogArea.Prefabs, $"ConsoleFragmentPrefab cloned. Obj is: {obj.name}");

                    obj.SetActive(false);

                    GameObject damagedConsoleGameObject =
                        CustomAssetBundleUtils.GetPrefabInstanceFromAssetBundle("PetConsoleDamaged", true);
                    GameObject newModelGameObject = damagedConsoleGameObject.FindChild("newmodel");

                    if (!newModelGameObject)
                    {
                        LogUtils.LogError(LogArea.Prefabs, $"PetConsoleFragmentPrefab: Unable to find model in new Asset!");
                    }

                    // Find old model and replace
                    GameObject oldModelGameObject = obj.FindChild("model");

                    if (!oldModelGameObject)
                    {
                        LogUtils.LogError(LogArea.Prefabs, $"PetConsoleFragmentPrefab: Couldn't find old model! All children:");
                    }

                    LogUtils.LogDebug(LogArea.Prefabs, "Found old model!");

                    newModelGameObject.transform.SetParent(oldModelGameObject.transform.parent);
                    newModelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    newModelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    oldModelGameObject.SetActive(false);

                    // Configure
                    MaterialUtils.ApplySNShaders(newModelGameObject);
                    PrefabUtils.AddBasicComponents(obj, "PetConsoleFragment", Info.TechType, LargeWorldEntity.CellLevel.Medium);
                    PrefabUtils.AddResourceTracker(obj, TechType.Fragment);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.UpdatePickupable(obj, false);
                    PrefabConfigUtils.SetRigidBodyKinematic(obj, true);
                    obj.AddComponent<PetConsoleFragment>();
                }
            };

            LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: SetGameObject...");
            consoleFragmentPrefab.SetGameObject(cloneTemplate);

            LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: SetSpawns...");

            SpawnLocation[] spawnLocations =
            {
                    new SpawnLocation(new Vector3(-49.88f, -30.49f, -407.04f), new Vector3(75.39f, 133.30f, 184.94f)), // warp -49.88 -30.49f -407.04 -- good place for samples!
                    new SpawnLocation(new Vector3(288.63f,-105.24f, 414.90f), new Vector3(275.50f, 103.87f ,81.24f)), // warp 288.63 -105.24 414.90
                    new SpawnLocation(new Vector3(74.52f,-48.40f, 389.12f), new Vector3(290.15f, 73.77f ,242.85f)), // warp 74.52 -48.40 389.12
                    new SpawnLocation(new Vector3(-398.39f,-140.44f, 666.46f), new Vector3(295.26f, 18.29f ,333.94f)), // warp -398.39 -140.44 666.46
                    new SpawnLocation(new Vector3(-1632.70f,-358.51f, 77.22f), new Vector3(286.08f, 56.34f ,149.44f)), // warp -1632.70 -358.51 77.22
                    new SpawnLocation(new Vector3(-507.13f,-98.74f, -56.38f), new Vector3(89.73f, 86.47f ,13.13f)), // warp -507.13 -98.74 -56.38
                    new SpawnLocation(new Vector3(-632.11f, -111.64f, -37.28f), new Vector3(278.92f, 172.84f ,321.07f)), // warp -632.11  -111.64 -37.28
                    new SpawnLocation(new Vector3(-1452.19f, -348.24f, 768.21f), new Vector3(68.13f, 56.04f ,153.21f)), // warp -1452.19 -348.24 768.21
                    new SpawnLocation(new Vector3(81.92f,-35.90f, 128.91f), new Vector3(290.41f, 250.65f ,255.06f)), // warp 81.92 -35.90 128.91
                    new SpawnLocation(new Vector3(392.70f,-28.90f, -175.90f), new Vector3(285.41f, 124.50f ,73.68f)), // warp 392.70 -28.90 -175.90
                };

            consoleFragmentPrefab.SetSpawns(spawnLocations);
            LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: CreateFragment...");
            consoleFragmentPrefab.CreateFragment(PetConsolePrefab.Info.TechType, 5.0f, 3, "PetConsole", true, true);
            LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: Register...");
            consoleFragmentPrefab.Register();
        }

    }
}