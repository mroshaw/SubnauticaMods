using System.IO;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using Nautilus.Assets.Gadgets;
using System.Reflection;
using Nautilus.Utility;

namespace DaftAppleGames.CreaturePetModSn.Utils
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class PetDnaPrefabUtils : MonoBehaviour
    {
        // Static references for consumers
        public static PrefabInfo CaveCrawlerDnaPrefabInfo;
        public static PrefabInfo BloodCrawlerDnaPrefabInfo;
        public static PrefabInfo CrabSquidDnaPrefabInfo;
        public static PrefabInfo AlienRobotDnaPrefabInfo;

        public static string SpritePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Sprites";

        /// <summary>
        /// Register all prefabs
        /// </summary>
        public static void InitPetPrefabs()
        {
            InitCaveCrawlerDnaPrefab();
            InitBloodCrawlerDnaPrefab();
            InitCrabSquidDnaPrefab();
            InitAlienRobotDnaPrefab();
        }

        /// <summary>
        /// Register Cave Crawler DNA prefab
        /// </summary>
        private static void InitCaveCrawlerDnaPrefab()
        {
            PrefabInfo caveCrawlerDnaInfo = PrefabInfo.WithTechType("CaveCrawlerDna", "Cave Crawler DNA", "DNA from a Cave Crawler.");
            caveCrawlerDnaInfo.WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\DnaStrand94x110.png"));
            CaveCrawlerDnaPrefabInfo = caveCrawlerDnaInfo;
            CustomPrefab caveCrawlerDna = new CustomPrefab(caveCrawlerDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(caveCrawlerDnaInfo, TechType.ReefbackDNA)
            {
                // Callback to change all material colors of this clone to blue.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.blue))
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
            bloodCrawlerDnaInfo.WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\DnaStrand94x110.png"));
            BloodCrawlerDnaPrefabInfo = bloodCrawlerDnaInfo;
            CustomPrefab bloodCrawlerDna = new CustomPrefab(bloodCrawlerDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(bloodCrawlerDnaInfo, TechType.ReefbackDNA)
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
            crabSquidDnaInfo.WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\DnaStrand94x110.png"));
            CrabSquidDnaPrefabInfo = crabSquidDnaInfo;
            CustomPrefab crabSquidDna = new CustomPrefab(crabSquidDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(crabSquidDnaInfo, TechType.ReefbackDNA)
            {
                // Callback to change all material colors of this clone to green.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.green))
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
            alienRobotDnaInfo.WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\DnaStrand94x110.png"));
            AlienRobotDnaPrefabInfo = alienRobotDnaInfo;
            CustomPrefab alienRobotDna = new CustomPrefab(alienRobotDnaInfo);
            PrefabTemplate cloneTemplate = new CloneTemplate(alienRobotDnaInfo, TechType.ReefbackDNA)
            {
                // Callback to change all material colors of this clone to grey.
                ModifyPrefab = prefab => prefab.GetComponentsInChildren<Renderer>().ForEach(r => r.materials.ForEach(m => m.color = Color.grey))
            };
            alienRobotDna.SetGameObject(cloneTemplate);
            alienRobotDna.SetSpawns(new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_Grass, count = 4, probability = 0.1f },
                new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_CaveFloor, count = 1, probability = 0.4f });

            alienRobotDna.Register();
        }
    }
}
