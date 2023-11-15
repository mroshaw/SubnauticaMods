using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetConsoleFragmentPrefab
    {
        // Public PrefabInfo, for anything that needs it
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType(PrefabClassId, null, null, unlockAtStart: false);

        // Prefab Class Id
        private const string PrefabClassId = "PetConsoleFragment";

        // Asset bundle references
        private const string PetConsolePrefabName = "PetConsoleDamaged";
        private const string PetConsolePopupImageTexture = "PetConsoleDataBankPopupImageTexture";

        // Model references
        private const string NewModelName = "newmodel";
        public const string OldModelName = "model";

        #region Spawn Locations
#if SUBNAUTICA
        private static readonly SpawnLocation[] SpawnLocations =
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

        /*
        warp -49.88  -29.49 -407.04
        warp 288.63 -104.24  414.90
        warp 74.52 -47.40 389.12
        warp -398.39 -139.44 666.46
        warp -1632.70 -357.51 77.22
        warp -507.13 -97.74 -56.38
        warp -632.11 -110.64 -37.28

        warp -1452.52 -349.30 757.0
        warp 75.05555 -34.76138 124.0958
        warp 390.811 -21.42437 -175.1048
         */
#endif
#if SUBNAUTICAZERO

        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-171.25f,-41.34f, -234.25f), new Vector3(0f, 0f, 0f)),
        };
#endif
        #endregion

        /// <summary>
        /// Initialise the Pet Console fragment prefab
        /// </summary>
        public static void InitPrefab()
        {
            CustomPrefab consoleFragmentPrefab = new CustomPrefab(Info);

            CloneTemplate cloneTemplate = new CloneTemplate(consoleFragmentPrefab.Info, TechType.GravSphereFragment)
            {
                ModifyPrefab = prefab =>
                {
                    #region Replace Model
                    // Replace model
                    GameObject damagedConsoleGameObject =
                        ModUtils.GetGameObjectInstanceFromAssetBundle(PetConsolePrefabName, true);

                    GameObject modelGameObject = damagedConsoleGameObject.FindChild(NewModelName);

                    // Add new model
                    LogUtils.LogDebug(LogArea.Prefabs, 
                        $"PetConsoleFragmentPrefab: InitPrefab is setting the model for {prefab.name} to {modelGameObject.name}...");
                    modelGameObject.transform.SetParent(prefab.transform);
                    modelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    modelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    // Remove old model
                    GameObject oldModelGameObject = prefab.FindChild(OldModelName);
                    if (oldModelGameObject != null)
                    {
                        LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: Destroying old model.");
                        Object.Destroy(oldModelGameObject);
                    }
                    else
                    {
                        LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: Old model not found.");
                    }
                    #endregion
                    #region Configure Prefab
                    MaterialUtils.ApplySNShaders(modelGameObject);
                    PrefabUtils.AddBasicComponents(prefab, PrefabClassId, Info.TechType, LargeWorldEntity.CellLevel.Medium);
                    ResourceTracker resourceTracker = PrefabUtils.AddResourceTracker(prefab, TechType.Fragment);

                    // Add component
                    LogUtils.LogDebug(LogArea.Prefabs, "PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component...");
                    prefab.AddComponent<PetConsoleFragment>();
                    LogUtils.LogDebug(LogArea.Prefabs, 
                        "PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component... Done.");
                    #endregion
                }
            };
            consoleFragmentPrefab.SetGameObject(cloneTemplate);
            consoleFragmentPrefab.SetSpawns(SpawnLocations);

            // Set up as a scannable fragment
            consoleFragmentPrefab.CreateFragment(PetConsolePrefab.Info.TechType, 5.0f, 3,
                PetConsolePrefab.PetConsoleEncyKey, true, true);

            LogUtils.LogDebug(LogArea.Prefabs, $"PetConsoleFragmentPrefab: Registering {PrefabClassId}...");
            consoleFragmentPrefab.Register();
            LogUtils.LogDebug(LogArea.Prefabs, $"PetConsoleFragmentPrefab: Init Prefab for {PrefabClassId}. Done.");
        }
    }
}
