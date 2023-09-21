using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using System.Collections.Generic;
using Nautilus.Assets.Gadgets;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetFabricatorFragmentPrefab
    {
        private static readonly string PrefabId = "PetFabricatorFragment";
        public static readonly string NewModelName = "newmodel";

        // Ency keys
        private static readonly string PetFabricatorEncyKey = "PetFabricator";
        private static readonly string PetFabricatorEncyPath = "Tech/Habitats";
        // Asset Bundle refs
        private static readonly string PetFabricatorMainImageTexture = "PetFabricatorDataBankMainImageTexture";
        private static readonly string PetFabricatorPopupImageTexture = "PetFabricatorDataBankPopupImageTexture";


        private static bool _isInitialised = false;

#if SUBNAUTICA
        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-172.27f,-43.07f, -234.29f), new Vector3(0f, 0f, 0f))
        };

        /*
            new Vector3(-172.27f,-43.07f, -234.29f) /*,
            new Vector3(-385.8968f,-118.473f, 623.9641f),
            new Vector3(-1603.463f,-342.9196f, 79.65623f),
            new Vector3(-773.6248f,-212.8109f, -729.6456f),
            new Vector3(-31.82343f,-24.19011f, -417.7448f),
            new Vector3(12.00361f,-25.82509f, -243.0502f),
            new Vector3(76.30077f,-24.12408f, -88.81075f),
            new Vector3(82.54358f,-37.00394f, 117.0964f),
            new Vector3(376.3995f,-25.73f, -209.09f) 
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
        /// Initialise the Pet Fabricator fragment prefab
        /// </summary>
        public static void InitPrefab()
        {
            Log.LogDebug("PetFabricatorFragmentPrefab: InitPrefab...");
            if (_isInitialised)
            {
                Log.LogError("PetFabricatorFragmentPrefab: ... InitPrefab called more than once!!!");
            }
            _isInitialised = true;

            CustomPrefab fabricatorFragmentPrefab = new CustomPrefab(PrefabId, null, null);

            PrefabTemplate cloneTemplate = new CloneTemplate(fabricatorFragmentPrefab.Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4") //TechType.GravSphereFragment)
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
            fabricatorFragmentPrefab.SetGameObject(cloneTemplate);
            ModUtils.SetupCoordinatedSpawn(fabricatorFragmentPrefab, SpawnLocations);
            Log.LogDebug($"PetFabricatorFragmentPrefab: Registering {PrefabId}...");
            fabricatorFragmentPrefab.Register();
            Log.LogDebug($"PetFabricatorFragmentPrefab: Init Prefab for {PrefabId}. Done.");
            PrefabInfo = fabricatorFragmentPrefab.Info;
        }

        /// <summary>
        /// Set up scanning and databank entries
        /// </summary>
        /// <param name="consoleFragmentPrefab"></param>
        private static void SetupScanning(CustomPrefab fabricatorFragmentPrefab)
        {
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry...");
            ScanningGadget scanningGadget = new ScanningGadget(fabricatorFragmentPrefab, TechType.None);
            scanningGadget.WithScannerEntry(3.0f, true, PetFabricatorEncyKey, true);
            ModUtils.ConfigureDatabankEntry(PetFabricatorEncyKey, PetFabricatorEncyPath, PetFabricatorMainImageTexture, PetFabricatorPopupImageTexture);
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry... Done.");
        }
    }
}
