using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;
using SpawnLocation = Nautilus.Assets.SpawnLocation;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    public static class FragmentPrefabs
    {
        /// <summary>
        /// Initialise all Fragment Prefabs
        /// </summary>
        public static void RegisterAll()
        {
            PetConsoleFragmentPrefab.Register();
            PetFabricatorFragmentPrefab.Register();
        }

        /// <summary>
        /// Pet Fabricator Fragment
        /// </summary>
        public static class PetFabricatorFragmentPrefab
        {
            public static PrefabInfo Info;

            public static void Register()
            {
                Info = PrefabInfo
                    .WithTechType("PetFabricatorFragment", null, null, unlockAtStart: false);
                CustomPrefab fabricatorFragmentPrefab = new CustomPrefab(Info);

                CloneTemplate cloneTemplate = new CloneTemplate(Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4")
                {
                    ModifyPrefab = obj =>
                    {
                        obj.SetActive(false);
                        // Add components
                        PrefabUtils.AddBasicComponents(obj, "PetFabricatorFragment", Info.TechType, LargeWorldEntity.CellLevel.Medium);
                        PrefabUtils.AddResourceTracker(obj, TechType.Fragment);
                        PrefabConfigUtils.ConfigureSkyApplier(obj);
                        PrefabConfigUtils.UpdatePickupable(obj, false);
                        PrefabConfigUtils.SetRigidBodyKinematic(obj, true);
                        PrefabConfigUtils.ResizeCollider(obj, new Vector3(0.0f, 0.61f, 0.24f), new Vector3(1.02f, 1.2f, 0.52f));
                        obj.AddComponent<PetFabricatorFragment>();
                    }
                };

                fabricatorFragmentPrefab.SetGameObject(cloneTemplate);
#if SUBNAUTICA
                SpawnLocation[] spawnLocations =
                {
                    new SpawnLocation(new Vector3(-172.27f, -43.07f, -234.29f), new Vector3(346.22f, 345.14f, 8.72f)),
                    new SpawnLocation(new Vector3(-385.88f, -124.79f, 623.95f), new Vector3(8.71f, 0.62f, 2.86f)),
                    new SpawnLocation(new Vector3(-1603.49f, -355.97f, 79.63f), new Vector3(5.06f, 2.32f, 354.08f)),
                    new SpawnLocation(new Vector3(-773.64f, -224.83f, -729.66f), new Vector3(13.85f, 0.46f, 354.31f)),
                    new SpawnLocation(new Vector3(-31.72f, -32.77f, -418.56f), new Vector3(348.20f, 355.20f, 347.04f)),
                    new SpawnLocation(new Vector3(12.01f, -28.85f, -243.06f), new Vector3(11.23f, 0.61f, 1.94f)),
                    new SpawnLocation(new Vector3(76.28f, -30.01f, -88.79f), new Vector3(290.52f, 172.81f, 186.37f)),
                    new SpawnLocation(new Vector3(82.50f, -40.76f, 117.07f), new Vector3(3.60f, 357.73f, 330.99f)),
                    new SpawnLocation(new Vector3(376.36f, -26.60f, -209.09f), new Vector3(292.15f, 131.21f, 226.02f))
                };
#endif
#if SUBNAUTICAZERO
                SpawnLocation[] spawnLocations =
                {
                    new SpawnLocation(new Vector3(-171.25f,-41.34f, -234.25f), new Vector3(0f, 0f, 0f))
                };
#endif
                fabricatorFragmentPrefab.SetSpawns(spawnLocations);
                fabricatorFragmentPrefab.CreateFragment(BaseModulePrefabs.PetFabricatorPrefab.Info.TechType, 5.0f, 3, "PetFabricator", true, true);
                fabricatorFragmentPrefab.Register();
            }
        }

        /// <summary>
        /// Pet Console Fragment
        /// </summary>
        public static class PetConsoleFragmentPrefab
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

                CloneTemplate cloneTemplate = new CloneTemplate(consoleFragmentPrefab.Info, TechType.GravSphereFragment)
                {
                    ModifyPrefab = obj =>
                    {
                        obj.SetActive(false);

                        GameObject damagedConsoleGameObject =
                            ModUtils.GetGameObjectInstanceFromAssetBundle("PetConsoleDamaged", true);

                        GameObject newGodelGameObject = damagedConsoleGameObject.FindChild("newmodel");

                        // Find old model and replace
                        GameObject oldModelGameObject = obj.FindChild("model");
                        newGodelGameObject.transform.SetParent(oldModelGameObject.transform);
                        newGodelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                        newGodelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                        // Configure
                        MaterialUtils.ApplySNShaders(newGodelGameObject);
                        PrefabUtils.AddBasicComponents(obj, "PetConsoleFragment", Info.TechType, LargeWorldEntity.CellLevel.Medium);
                        PrefabUtils.AddResourceTracker(obj, TechType.Fragment);
                        PrefabConfigUtils.ConfigureSkyApplier(obj);
                        PrefabConfigUtils.UpdatePickupable(obj, false);
                        PrefabConfigUtils.SetRigidBodyKinematic(obj, true);
                        obj.AddComponent<PetConsoleFragment>();
                    }
                };

                consoleFragmentPrefab.SetGameObject(cloneTemplate);
#if SUBNAUTICA
                SpawnLocation[] spawnLocations =
                {
                    new SpawnLocation(new Vector3(-49.88f, -30.49f, -407.04f), new Vector3(75.39f, 133.30f, 184.94f)),
                    new SpawnLocation(new Vector3(288.63f,-105.24f, 414.90f), new Vector3(275.50f, 103.87f ,81.24f)),
                    new SpawnLocation(new Vector3(74.52f,-48.40f, 389.12f), new Vector3(290.15f, 73.77f ,242.85f)),
                    new SpawnLocation(new Vector3(-398.39f,-140.44f, 666.46f), new Vector3(295.26f, 18.29f ,333.94f)),
                    new SpawnLocation(new Vector3(-1632.70f,-358.51f, 77.22f), new Vector3(286.08f, 56.34f ,149.44f)),
                    new SpawnLocation(new Vector3(-507.13f,-98.74f, -56.38f), new Vector3(89.73f, 86.47f ,13.13f)),
                    new SpawnLocation(new Vector3(-632.11f, -111.64f, -37.28f), new Vector3(278.92f, 172.84f ,321.07f)),
                    new SpawnLocation(new Vector3(-1452.19f, -348.24f, 768.21f), new Vector3(68.13f, 56.04f ,153.21f)),
                    new SpawnLocation(new Vector3(81.92f,-35.90f, 128.91f), new Vector3(290.41f, 250.65f ,255.06f)),
                    new SpawnLocation(new Vector3(392.70f,-28.90f, -175.90f), new Vector3(285.41f, 124.50f ,73.68f)),
                };
#endif
#if SUBNAUTICAZERO

                SpawnLocation[] spawnLocations =
                {
                    new SpawnLocation(new Vector3(-171.25f, -41.34f, -234.25f), new Vector3(0f, 0f, 0f)),
                };
#endif
                consoleFragmentPrefab.SetSpawns(spawnLocations);
                consoleFragmentPrefab.CreateFragment(BaseModulePrefabs.PetConsolePrefab.Info.TechType, 5.0f, 3, "PetConsole", true, true);
                consoleFragmentPrefab.Register();
            }
        }
    }
}
