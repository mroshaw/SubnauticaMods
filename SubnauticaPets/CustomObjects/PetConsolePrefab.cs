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
    /// Static class for creating the Pet Console prefab
    /// </summary>
    internal static class PetConsolePrefab
    {
        /// <summary>
        /// Makes the new Pet Console available for use.
        /// </summary>
        public static void InitPetConsole()
        {
            // Create our custom prefab
            CustomPrefab customConsole = new CustomPrefab("PetConsole",
                Language.main.Get("DisplayName_PetConsole"),
                Language.main.Get("Description_PetConsole"),
                ModUtils.GetSpriteFromAssetBundle(PetConsoleTexture));

            // We'll use the PictureFrame as a template
            PrefabTemplate consoleTemplate = new CloneTemplate(customConsole.Info, TechType.PictureFrame)
            {
                // Reconfigure the prefab, once it's been created
                ModifyPrefab = ConfigureConsoleComponents
            };
            customConsole.SetGameObject(consoleTemplate);

            // Define the recipe for the new Console
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

            // Set the recipe.
            customConsole.SetRecipe(recipe);
            customConsole.SetUnlock(TechType.Workbench)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
            customConsole.Register();
        }

        /// <summary>
        /// Set up our custom "Console" component, replacing the default one
        /// </summary>
        /// <param name="consoleGameObject"></param>
        private static void ConfigureConsoleComponents(GameObject consoleGameObject)
        {
            // Add the PetConsole component, which will provision the UI
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component...");
            PetConsole petConsole = consoleGameObject.AddComponent<PetConsole>();
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component... Done.");
        }

    }
}
