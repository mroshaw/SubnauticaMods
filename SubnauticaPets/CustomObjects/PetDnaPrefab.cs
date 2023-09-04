using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static DaftAppleGames.SubnauticaPets.Utils.UiUtils;
using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static class for creating the "Pet DNA" collectable objects
    /// </summary>
    internal class PetDnaPrefab
    {
        // Static references for consumers
        public static string DnaModelObjectName = "DNAModel";
        public static GameObject DnaModelPrefab = null;
#if SUBNAUTICA
        public static PrefabInfo CaveCrawlerDnaPrefabInfo;
        public static PrefabInfo BloodCrawlerDnaPrefabInfo;
        public static PrefabInfo CrabSquidDnaPrefabInfo;
        public static PrefabInfo AlienRobotDnaPrefabInfo;
#endif
#if SUBNAUTICAZERO
        public static PrefabInfo PenglingBabyDnaPrefabInfo;
        public static PrefabInfo PenglingAdultDnaPrefabInfo;
        public static PrefabInfo SnowStalkerBabyDnaPrefabInfo;
        public static PrefabInfo PinnicaridDnaPrefabInfo;
        public static PrefabInfo TrivalveBlueDnaPrefabInfo;
        public static PrefabInfo TrivalveYellowDnaPrefabInfo;
#endif

        /// <summary>
        /// Register all prefabs
        /// </summary>
        public static void InitPetPrefabs()
        {
            // Get and init the DNA model prefab
            SetDnaModelObjectPrefab();
#if SUBNAUTICA
            // Cave Crawler
            CaveCrawlerDnaPrefabInfo = InitPrefab("CaveCrawlerDnaSample",
                "Cave Crawler DNA Sample",
                "A sample of DNA from a Cave Crawler",
                ModUtils.GetSpriteFromAssetBundle(DnaCaveCrawlerTexture),
                Color.yellow,
                new LootDistributionData.BiomeData
                {
                biome = BiomeType.SafeShallows_Grass,
                count = 10,
                probability = 0.8f
                });
            // Blood Crawler
            BloodCrawlerDnaPrefabInfo = InitPrefab("BloodCrawlerDnaSample",
                "Blood Crawler DNA Sample", "A sample of DNA from a Blood Crawler",
                ModUtils.GetSpriteFromAssetBundle(DnaBloodCrawlerTexture),
                Color.red,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_SandFlat,
                    count = 10,
                    probability = 0.8f
                });
            // Crab Squid
            CrabSquidDnaPrefabInfo = InitPrefab("CrabSquidDnaSample",
                "Crab Squid DNA Sample",
                "A sample of DNA from a Crab Squid",
                ModUtils.GetSpriteFromAssetBundle(DnaCrabSquidTexture),
                Color.green,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_UniqueCreature,
                    count = 10,
                    probability = 0.8f
                });
            // Alien Robot
            AlienRobotDnaPrefabInfo = InitPrefab("AlienRobotDnaSample",
                "Alien Robot DNA Sample",
                "A sample of DNA from an Alien Robot",
                ModUtils.GetSpriteFromAssetBundle(DnaAlienRobotTexture),
                Color.grey,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_EscapePod,
                    count = 10,
                    probability = 0.8f
                });
#endif
#if SUBNAUTICAZERO
            // Snow Stalker Baby
            SnowStalkerBabyDnaPrefabInfo = InitPrefab("SnowStalkerBabyDnaSample",
                "Snow Stalker Baby DNA Sample",
                "A sample of DNA from a Baby Snow Stalker",
                ModUtils.GetSpriteFromAssetBundle(DnaSnowStalkerBabyTexture),
                Color.white,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
            // Pengling Adult
            PenglingAdultDnaPrefabInfo = InitPrefab("PenglingAdultDnaSample",
                "Pengling Adult DNA Sample",
                "A sample of DNA from an Adult Pengling",
                ModUtils.GetSpriteFromAssetBundle(DnaPenglingAdultTexture),
                Color.grey,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
            // Pengling Baby
            PenglingBabyDnaPrefabInfo = InitPrefab("PenglingBabyDnaSample",
                "Pengling Baby DNA Sample",
                "A sample of DNA from a Baby Pengling",
                ModUtils.GetSpriteFromAssetBundle(DnaPenglingBabyTexture),
                Color.cyan,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
            // Pinnicarid
            PinnicaridDnaPrefabInfo = InitPrefab("PinnicaridDnaSample",
                "Pinnicarid DNA Sample",
                "A sample of DNA from a Pinnicarid",
                ModUtils.GetSpriteFromAssetBundle(DnaPinnicaridTexture),
                Color.magenta,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
            // Yellow Trivalve
            TrivalveBlueDnaPrefabInfo = InitPrefab("YellowTrivalveDnaSample",
                "Yellow Trivalve DNA Sample",
                "A sample of DNA from a yellow Trivalve",
                ModUtils.GetSpriteFromAssetBundle(DnaTrivalveYellowTexture),
                Color.yellow,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
            // Blue Trivalve
            TrivalveYellowDnaPrefabInfo = InitPrefab("BlueTrivalveDnaSample",
                "Blue Trivalve DNA Sample",
                "A sample of DNA from a blue Trivalve",
                ModUtils.GetSpriteFromAssetBundle(DnaTrivalveBlueTexture),
                Color.blue,
                new LootDistributionData.BiomeData
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 10,
                    probability = 0.8f
                });
#endif
            SetFixedSpawns();
        }

        /// <summary>
        /// Generic method to set up the DNA prefabs
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="lootBiome"></param>
        /// <returns></returns>
        private static PrefabInfo InitPrefab(string classId, string displayName, string description, Sprite sprite,
            Color color, LootDistributionData.BiomeData lootBiome)
        {
            Log.LogDebug($"PetDnaPrefab: Init Prefab for {classId}...");

            CustomPrefab testClone = new CustomPrefab(
               classId,
               displayName,
               description,
               sprite
            );

            PrefabTemplate cloneTemplate = new CloneTemplate(testClone.Info, TechType.Titanium)
            {
                ModifyPrefab = prefab =>
                {
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {prefab.name}... using {DnaModelPrefab.name}");
                    GameObject newModel = Object.Instantiate(DnaModelPrefab);
                    // Add new model
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}...");
                    newModel.transform.SetParent(prefab.transform);
                    newModel.transform.localPosition = new Vector3(0, 0, 0);
                    newModel.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    // Set model color
                    newModel.FindChild("Ends").GetComponent<MeshRenderer>().material.color = color;
                    // Add the rotate script to the DNA model
                    RotateModel rotateModel = newModel.FindChild("DNA").AddComponent<RotateModel>();
                    rotateModel.RotationSpeed = 0.01f;
                    // Resize
                    prefab.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    // Disable the old model
                    prefab.FindChild("model").SetActive(false);
                    Log.LogDebug($"PetDnaPrefab: InitPrefab is setting the model for {prefab.name} to {newModel.name}. Done.");
                }
            };
            testClone.SetGameObject(cloneTemplate);

            testClone.SetSpawns(lootBiome);
            testClone.Register();
            Log.LogDebug($"PetDnaPrefab: Init Prefab for {classId}. Done.");
            return testClone.Info;
        }

        /// <summary>
        /// Get's and inits the DNA model object from the Asset Bundle
        /// </summary>
        /// <returns></returns>
        private static void SetDnaModelObjectPrefab()
        {
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is locating model in Asset Bundle...");
            GameObject prefab = ModUtils.GetGameObjectPrefabFromAssetBundle(DnaModelObjectName);
            if (prefab == null)
            {
                Log.LogDebug($"PetDnaPrefab: GetDnaModelObjectPrefab couldn't find {DnaModelObjectName}");
                return;
            }
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is updating material shader properties...");
            MaterialUtils.ApplySNShaders(prefab);
            DnaModelPrefab = prefab;
            Log.LogDebug("PetDnaPrefab: GetDnaModelObjectPrefab is done.");
        }

        /// <summary>
        /// Spawns Pet DNA in some fixed locations
        /// </summary>
        public static void SetFixedSpawns()
        {
            Log.LogDebug("PetDnaPrefab: SetFixedSpawns adding coordinated spawns...");
            List<SpawnInfo> spawnInfos = new List<SpawnInfo>()
            {
#if SUBNAUTICA
                new SpawnInfo("CaveCrawlerDnaSample", new Vector3(-131.93f, -19.82f, -246.48f),
                    Vector3.up * 90f),
                new SpawnInfo("AlienRobotDnaSample", new Vector3(-132.63f, -19.82f, -246.48f),
                Vector3.up * 90f),
                new SpawnInfo("BloodCrawlerDnaSample", new Vector3(-131.93f, -19.32f, -246.48f),
                    Vector3.up * 90f),
                new SpawnInfo("CrabSquidDnaSample", new Vector3(-132.63f, -19.32f, -246.48f),
                    Vector3.up * 90f)

#endif
#if SUBNAUTICAZERO
                new SpawnInfo("SnowStalkerBabyDnaSample", new Vector3(617.56f, -178.31f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo("PenglingAdultDnaSample", new Vector3(616.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
                new SpawnInfo("PenglingBabyDnaSample", new Vector3(617.56f, -177.51f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo("PinnicaridDnaSample", new Vector3(616.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
                new SpawnInfo("YellowTrivalveDnaSample", new Vector3(618.56f, -178.31f, -456.59f),
                    Vector3.up * 90f),
                new SpawnInfo("BlueTrivalveDnaSample", new Vector3(618.76f, -177.51f, -455.89f),
                    Vector3.up * 90f),
#endif
            };
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(spawnInfos);
            Log.LogDebug("PetDnaPrefab: SetFixedSpawns adding coordinated spawns... Done.");
        }
    }
}
