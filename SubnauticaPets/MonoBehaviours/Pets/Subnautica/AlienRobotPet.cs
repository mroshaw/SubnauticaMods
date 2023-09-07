using DaftAppleGames.SubnauticaPets.CustomObjects;
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

#if SUBNAUTICA
namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class AlienRobotPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "AlienRobotPet";
        public static string TextureName = "AlienRobotTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "AlienRobotPetDna";
        public static string DnaTextureName = "AlienRobotDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 5, probability = 0.4f},
        };

        public static Color PetObjectColor = Color.grey;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 3),
                                new CraftData.Ingredient(DnaBuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Alien Robot scale factor
        public override float ScaleFactor => 1.0f;

        /// <summary>
        /// Add Creature specific components
        /// </summary>
        public override void AddComponents()
        {
            base.AddComponents();
        }

        /// <summary>
        /// Remove Creature specific components
        /// </summary>
        public override void RemoveComponents()
        {
            base.RemoveComponents();
        }

        /// <summary>
        /// Update Creature specific components
        /// </summary>
        public override void UpdateComponents()
        {
            base.UpdateComponents();
        }
    }
}
#endif