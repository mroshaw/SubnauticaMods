using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using DaftAppleGames.CreaturePetModSn.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.CustomObjects
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
                .AddCraftNode(PetBuildablePrefab.CaveCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.BloodCrawlerPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.CrabSquidPetBuildablePrefabInfo.TechType)
                .AddCraftNode(PetBuildablePrefab.AlienRobotBuildablePefabInfo.TechType);

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
            newPetSpawner.SkipSpawnObstacleCheck = CreaturePetModSnPlugin.SkipSpawnObstacleCheckConfig.Value;
            Log.LogDebug("PetFabricatorUtils: Adding PetSpawner component... Done.");

        }
    }
}
