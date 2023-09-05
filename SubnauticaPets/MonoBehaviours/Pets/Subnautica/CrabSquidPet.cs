#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements CrabSquid specific Pet functionality
    /// </summary>
    internal class CrabSquidPet : Pet
    {
        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "CrabSquidPet";
        public static string DisplayName = Language.main.Get("DisplayName_CrabSquid");
        public static string Description = Language.main.Get("Description_CrabSquid");
        public static string TextureName = "CrabSquidTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "CrabSquidDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_CrabSquidDna");
        public static string DnaDescription = Language.main.Get("Description_CrabSquidDna");
        public static string DnaTextureName = "CrabSquidDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
        };

        public static Color PetObjectColor = Color.blue;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 3),
                new CraftData.Ingredient(BuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Crab Squid scale factor
        public override float ScaleFactor => 0.07f;

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
            Log.LogDebug("CrabSquidPet: Destroying components...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(gameObject);
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(gameObject);
            // ModUtils.DestroyComponentsInChildren<SwimBehaviour>(gameObject);
            Log.LogDebug("CrabSquidPet: Destroying components... Done."); 
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