#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static DaftAppleGames.SubnauticaPets.Utils.UiUtils;
namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// Static utilities class for creating the new Pet Fabricator
    /// </summary>
    internal static class PetFabricatorPrefab
    {
        /// <summary>
        /// Makes the new Pet Fabricator available for use.
        /// </summary>
        public static void InitPetFabricator()
        {
            CustomPrefab customFab = new CustomPrefab("PetFabricator",
                null,
                null,
                ModUtils.GetSpriteFromAssetBundle(PetFabricator.PetFabricatorIconTexture));

            customFab.CreateFabricator(out CraftTree.Type treeType)
                // Add our Pet Buildables
#if SUBNAUTICA
                .AddCraftNode(CaveCrawlerPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(BloodCrawlerPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(CrabSquidPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(AlienRobotPet.BuildablePrefabInfo.TechType);
#endif
#if SUBNAUTICAZERO
                .AddCraftNode(PenglingBabyPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(PenglingAdultPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(SnowStalkerBabyPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(PinnicaridPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(TrivalveYellowPet.BuildablePrefabInfo.TechType)
                .AddCraftNode(TrivalveBluePet.BuildablePrefabInfo.TechType);
#endif
            FabricatorTemplate fabPrefab = new FabricatorTemplate(customFab.Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench,
                ModifyPrefab = ConfigureFabComponents

            };
            customFab.SetGameObject(fabPrefab);

            // Define the recipe
            RecipeData recipe = new RecipeData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Nickel, 1),
                    new Ingredient(TechType.Copper, 1)
                }
            };

            // Set the recipe
            customFab.SetRecipe(recipe);
            customFab.SetUnlock(PetFabricatorFragmentPrefab.PrefabInfo.TechType, 1);
            customFab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
            customFab.Register();
            
            PetFabricator.PrefabInfo = customFab.Info;
        }

        /// <summary>
        /// Set up our custom "Workbench" component, replacing the default one
        /// </summary>
        /// <param name="fabricatorGameObject"></param>
        private static void ConfigureFabComponents(GameObject fabricatorGameObject)
        {
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component...");
            PetSpawner newPetSpawner = fabricatorGameObject.AddComponent<PetSpawner>();
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component... Done.");

            Log.LogDebug("PetFabricatorUtils: Adding PetFabricator component...");
            PetFabricator newPetFabricator = fabricatorGameObject.AddComponent<PetFabricator>();
            Log.LogDebug("PetFabricatorUtils: Adding PetFabricator component... Done.");
        }
    }
}