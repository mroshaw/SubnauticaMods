
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using DaftAppleGames.SubnauticaPets.BaseParts;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

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
                null,
                null,
                ModUtils.GetSpriteFromAssetBundle(PetConsole.PetConsoleIconTexture));

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
            customConsole.SetUnlock(PetConsoleFragmentPrefab.PrefabInfo.TechType, 1);
            customConsole.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
            customConsole.Register();

            PetConsole.PrefabInfo = customConsole.Info;
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
