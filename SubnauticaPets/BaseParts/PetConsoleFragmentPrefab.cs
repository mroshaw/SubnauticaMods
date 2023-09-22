using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Utility;
using UWE;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetConsoleFragmentPrefab
    {
        private static readonly string PrefabId = "PetConsoleFragment";
        private static readonly string PetConsolePrefabName = "PetConsoleDamaged";
        private static readonly string NewModelName = "newmodel";
        public static readonly string OldModelName = "model";

        // Ency keys
        private static readonly string PetConsoleEncyPath = "Tech/Habitats";
        private static readonly string PetConsoleEncyKey = "PetConsole";

        // Asset Bundle refs for Databank
        private static readonly string PetConsoleMainImageTexture = "PetConsoleDataBankMainImageTexture";
        private static readonly string PetConsolePopupImageTexture = "PetConsoleDataBankPopupImageTexture";

#if SUBNAUTICA
        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-47.14f, -29.15f, -409.04f), new Vector3(0f, 0f, 0f))
        };

        /*
           new Vector3(-163.87f, -40.96f, -250.69f ),
            new Vector3(390.811f,-21.42437f, -175.1048f),
            new Vector3(288.8913f,-91.88315f, 413.5513f),
            new Vector3(75.05555f,-34.76138f, 124.0958f),
            new Vector3(74.77833f,-44.48608f, 387.945f),
            new Vector3(-398.011f,-134.0287f, 664.6798f),
            new Vector3(-1632.773f,-349.8546f, 75.00484f),
            new Vector3(-506.9559f,-94.37725f, -55.21287f),
            new Vector3(-1452.52f, -349.30f, 757.0f),
            new Vector3(-632.39f, -109.14f, -33.36f)
        */
#endif
#if SUBNAUTICAZERO

        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-171.25f,-41.34f, -234.25f), new Vector3(0f, 0f, 0f))
        };
#endif
        public static PrefabInfo PrefabInfo;

        /// <summary>
        /// Initialise the Pet Console fragment prefab
        /// </summary>
        public static void InitPrefab()
        {
            CustomPrefab consoleFragmentPrefab = new CustomPrefab(PrefabId, null, null);

            PrefabTemplate cloneTemplate = new CloneTemplate(consoleFragmentPrefab.Info, TechType.GravSphereFragment)
            {
                ModifyPrefab = prefab =>
                {
                    // Replace model
                    GameObject damagedConsoleGameObject =
                        ModUtils.GetGameObjectInstanceFromAssetBundle(PetConsolePrefabName);

                    GameObject modelGameObject = damagedConsoleGameObject.FindChild(NewModelName);

                    // Add new model
                    Log.LogDebug(
                        $"PetConsoleFragmentPrefab: InitPrefab is setting the model for {prefab.name} to {modelGameObject.name}...");
                    modelGameObject.transform.SetParent(prefab.transform);
                    modelGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    modelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    // Remove old model
                    GameObject oldModelGameObject = prefab.FindChild(OldModelName);
                    if (oldModelGameObject != null)
                    {
                        Log.LogDebug("PetConsoleFragmentPrefab: Destroying old model.");
                        Object.Destroy(oldModelGameObject);
                    }
                    else
                    {
                        Log.LogDebug("PetConsoleFragmentPrefab: Old model not found.");
                    }

                    MaterialUtils.ApplySNShaders(modelGameObject);

                    // Add component
                    Log.LogDebug("PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component...");
                    prefab.AddComponent<PetConsoleFragment>();
                    Log.LogDebug(
                        "PetConsoleFragmentPrefab: InitPrefab adding PetConsoleFragment component... Done.");
                }
            };
                
            consoleFragmentPrefab.SetGameObject(cloneTemplate);
            SetupScanning(consoleFragmentPrefab);
            ModUtils.SetupCoordinatedSpawn(consoleFragmentPrefab, SpawnLocations);
            Log.LogDebug($"PetConsoleFragmentPrefab: Registering {PrefabId}...");
            consoleFragmentPrefab.Register();
            Log.LogDebug($"PetConsoleFragmentPrefab: Init Prefab for {PrefabId}. Done.");
            PrefabInfo = consoleFragmentPrefab.Info;
        }

        /// <summary>
        /// Set up scanning and databank entries
        /// </summary>
        /// <param name="consoleFragmentPrefab"></param>
        private static void SetupScanning(CustomPrefab consoleFragmentPrefab)
        {
            // Set up Scanning Gadget
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry...");
            ScanningGadget scanningGadget = consoleFragmentPrefab.SetUnlock(TechType.None);
            scanningGadget.WithScannerEntry(scanTime: 4.0f, isFragment:true, encyKey: PetConsoleEncyKey, destroyAfterScan: true);
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry... Done.");

            // Set up Databank
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry...");
            PDAHandler.AddEncyclopediaEntry(PetConsoleEncyKey, PetConsoleEncyPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(PetConsoleMainImageTexture),
                ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture));
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry... Done.");
        }
    }
}
