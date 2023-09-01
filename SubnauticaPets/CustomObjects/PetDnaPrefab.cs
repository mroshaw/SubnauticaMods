using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static DaftAppleGames.SubnauticaPets.Utils.UiUtils;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static class for creating the "Pet DNA" collectable objects
    /// </summary>
    internal class PetDnaPrefab
    {
        // Static references for consumers
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
#if SUBNAUTICA
            InitCaveCrawlerDnaPrefab();
            InitBloodCrawlerDnaPrefab();
            InitCrabSquidDnaPrefab();
            InitAlienRobotDnaPrefab();
#endif
#if SUBNAUTICAZERO
            InitSnowStalkerBabyDnaPrefab();
            InitPenglingBabyDnaPrefab();
            InitPenglingAdultDnaPrefab();
            InitPinnicaridDnaPrefab();
            InitTrivalveBlueDnaPrefab();
            InitTrivalveYellowDnaPrefab();
#endif
        }

        #region SUBNAUTICAPETMETHODS
#if SUBNAUTICA
        /// <summary>
        /// Register Cave Crawler DNA prefab
        /// </summary>
        private static void InitCaveCrawlerDnaPrefab()
        {
            PrefabInfo caveCrawlerDnaInfo = PrefabInfo.WithTechType("CaveCrawlerDna", "Cave Crawler DNA", "DNA from a Cave Crawler.");
            caveCrawlerDnaInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaCaveCrawlerTexture));
            CaveCrawlerDnaPrefabInfo = caveCrawlerDnaInfo;
            CustomPrefab caveCrawlerDna = new CustomPrefab(caveCrawlerDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(caveCrawlerDnaInfo, TechType.BonesharkEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            caveCrawlerDna.SetGameObject(cloneTemplate);
            caveCrawlerDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            caveCrawlerDna.Register();
        }

        /// <summary>
        /// Register Blood Crawler DNA prefab
        /// </summary>
        private static void InitBloodCrawlerDnaPrefab()
        {
            PrefabInfo bloodCrawlerDnaInfo = PrefabInfo.WithTechType("BloodCrawlerDna", "Blood Crawler DNA", "DNA from a Blood Crawler.");
            bloodCrawlerDnaInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaBloodCrawlerTexture));
            BloodCrawlerDnaPrefabInfo = bloodCrawlerDnaInfo;
            CustomPrefab bloodCrawlerDna = new CustomPrefab(bloodCrawlerDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(bloodCrawlerDnaInfo, TechType.CrabsnakeEgg)
            {
                // Callback to change all material colors of this clone to red.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.red))
            };
            bloodCrawlerDna.SetGameObject(cloneTemplate);
            bloodCrawlerDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            bloodCrawlerDna.Register();
        }

        /// <summary>
        /// Register Crab Squid DNA prefab
        /// </summary>
        private static void InitCrabSquidDnaPrefab()
        {
            PrefabInfo crabSquidDnaInfo = PrefabInfo.WithTechType("CrabSquidDna", "Crab Squid DNA", "DNA from a Crab Squid.");
            crabSquidDnaInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaCrabSquidTexture));
            CrabSquidDnaPrefabInfo = crabSquidDnaInfo;
            CustomPrefab crabSquidDna = new CustomPrefab(crabSquidDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(crabSquidDnaInfo, TechType.CrabsquidEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.blue))
            };
            crabSquidDna.SetGameObject(cloneTemplate);
            crabSquidDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            crabSquidDna.Register();
        }

        /// <summary>
        /// Register Alien Robot DNA prefab
        /// </summary>
        private static void InitAlienRobotDnaPrefab()
        {
            PrefabInfo alienRobotDnaInfo = PrefabInfo.WithTechType("AlienRobotDna", "Alien Robot DNA", "DNA from an Alien Robot.");
            alienRobotDnaInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaAlienRobotTexture));
            AlienRobotDnaPrefabInfo = alienRobotDnaInfo;
            CustomPrefab alienRobotDna = new CustomPrefab(alienRobotDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(alienRobotDnaInfo, TechType.CrashEgg)
            {
                // Callback to change all material colors of this clone to grey.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.grey))
            };
            alienRobotDna.SetGameObject(cloneTemplate);
            alienRobotDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            alienRobotDna.Register();
        }
#endif
        #endregion

        #region BELOW ZERO METHODS
#if SUBNAUTICAZERO
        /// <summary>
        /// Register Snow Stalker Baby DNA prefab
        /// </summary>
        private static void InitSnowStalkerBabyDnaPrefab()
        {
            SnowStalkerBabyDnaPrefabInfo = PrefabInfo.WithTechType("SnowStalkerBabyDna", "Snow Stalker Baby DNA", "DNA from a baby Snow Stalker.");
            SnowStalkerBabyDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaSnowStalkerBabyTexture));
            CustomPrefab snowStalkerBabyDna = new CustomPrefab(SnowStalkerBabyDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(SnowStalkerBabyDnaPrefabInfo, TechType.ArcticRayEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            snowStalkerBabyDna.SetGameObject(cloneTemplate);
            snowStalkerBabyDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            snowStalkerBabyDna.Register();
        }

        /// <summary>
        /// Register Pengling Baby DNA prefab
        /// </summary>
        private static void InitPenglingBabyDnaPrefab()
        {
            PenglingBabyDnaPrefabInfo = PrefabInfo.WithTechType("PenglingBabyDna", "Pengling Baby DNA", "DNA from a baby Pengling.");
            PenglingBabyDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaPenglingBabyTexture));

            CustomPrefab penglingBabyDna = new CustomPrefab(PenglingBabyDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(PenglingBabyDnaPrefabInfo, TechType.BrinewingEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            penglingBabyDna.SetGameObject(cloneTemplate);
            penglingBabyDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            penglingBabyDna.Register();
        }

        /// <summary>
        /// Register Pengling Adult DNA prefab
        /// </summary>
        private static void InitPenglingAdultDnaPrefab()
        {
            PenglingAdultDnaPrefabInfo = PrefabInfo.WithTechType("PenglingAdultDna", "Adult Pengling DNA", "DNA from an adult Pengling.");
            PenglingAdultDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaPenglingAdultTexture));

            CustomPrefab penglingAdultDna = new CustomPrefab(PenglingAdultDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(PenglingAdultDnaPrefabInfo, TechType.BruteSharkEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            penglingAdultDna.SetGameObject(cloneTemplate);
            penglingAdultDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            penglingAdultDna.Register();
        }

        /// <summary>
        /// Register Pinnicarid DNA prefab
        /// </summary>
        private static void InitPinnicaridDnaPrefab()
        {
            PinnicaridDnaPrefabInfo = PrefabInfo.WithTechType("PinnicaridDna", "Pinnicarid DNA", "DNA from a Pinnicarid.");
            PinnicaridDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaPinnicaridTexture));
            CustomPrefab pinnicaridDna = new CustomPrefab(PinnicaridDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(PinnicaridDnaPrefabInfo, TechType.LavaZoneEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            pinnicaridDna.SetGameObject(cloneTemplate);
            pinnicaridDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            pinnicaridDna.Register();
        }

        /// <summary>
        /// Register Blue Trivalve DNA prefab
        /// </summary>
        private static void InitTrivalveBlueDnaPrefab()
        {
            TrivalveBlueDnaPrefabInfo = PrefabInfo.WithTechType("TrivalveBlueDna", "Blue Trivalve DNA", "DNA from a blue Trivalve.");
            TrivalveBlueDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaTrivalveBlueTexture));
            CustomPrefab trivalveBlueDna = new CustomPrefab(TrivalveBlueDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(TrivalveBlueDnaPrefabInfo, TechType.TrivalveBlueEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            trivalveBlueDna.SetGameObject(cloneTemplate);
            trivalveBlueDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            trivalveBlueDna.Register();
        }

        /// <summary>
        /// Register Yellow Trivalve DNA prefab
        /// </summary>
        private static void InitTrivalveYellowDnaPrefab()
        {
            TrivalveYellowDnaPrefabInfo = PrefabInfo.WithTechType("TrivalveYellowDna", "Yellow Trivalve DNA", "DNA from a yellow Trivalve.");
            TrivalveYellowDnaPrefabInfo.WithIcon(ModUtils.GetSpriteFromAssetBundle(DnaTrivalveYellowTexture));
            CustomPrefab trivalveYellowDna = new CustomPrefab(TrivalveYellowDnaPrefabInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(TrivalveYellowDnaPrefabInfo, TechType.TrivalveYellowEgg)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.magenta))
            };
            trivalveYellowDna.SetGameObject(cloneTemplate);
            trivalveYellowDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            trivalveYellowDna.Register();
        }
#endif

        #endregion

    }
}
