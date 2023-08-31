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
            CustomPrefab customFab = new CustomPrefab("PetFabricator", "Pet Fabricator", "A special fabricator for replicating pet creatures from fragments of DNA.",
                ModUtils.GetSpriteFromAssetBundle(PetWorkbenchTexture));

            customFab.CreateFabricator(out CraftTree.Type treeType)
                // Add our Pet Buildables
#if SUBNAUTICA
                .AddCraftNode(PetBuildablePrefab.CaveCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.BloodCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.CrabSquidPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.AlienRobotBuildablePefabInfo.TechType);
#endif
#if SUBNAUTICAZERO
                .AddCraftNode(PetBuildablePrefab.PenglingBabyPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.PenglingAdultPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.SnowStalkerBabyPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.PinnicaridPetBuildablePefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.TrivalveBluePetBuildablePefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.TrivalveYellowBuildablePefabInfo.TechType);
#endif
            FabricatorTemplate fabPrefab = new FabricatorTemplate(customFab.Info, treeType)
            {
                FabricatorModel = FabricatorTemplate.Model.Workbench,
                ModifyPrefab = ConfigureFabComponents

            };
            customFab.SetGameObject(fabPrefab);
            /*
             * This is what the aforementioned json object will look like as a RecipeData object.
             * You may use the CustomPrefab.SetRecipe() to set the recipe to a RecipeData object.
             */
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

            /*
             * Set the recipe.
             */
            customFab.SetRecipe(recipe);

            customFab.SetUnlock(TechType.Workbench)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);

            customFab.Register();
        }

        /// <summary>
        /// Set up our custom "Workbench" component, replacing the default one
        /// </summary>
        /// <param name="fabricatorGameObject"></param>
        private static void ConfigureFabComponents(GameObject fabricatorGameObject)
        {
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component...");
            PetSpawner newPetSpawner = fabricatorGameObject.AddComponent<PetSpawner>();
            newPetSpawner.SkipSpawnObstacleCheck = SubnauticaPetsPlugin.SkipSpawnObstacleCheckConfig.Value;
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component... Done.");

        }
    }
}
