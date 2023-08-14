using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.CustomObjects
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
            CustomPrefab customConsole = new CustomPrefab("PetConsole", "Pet Console",
                "A special console for managing and naming pet creatures.",
                ImageUtils.LoadSpriteFromFile($"{SpritePath}\\{PetConsoleSprite}"));

            //               SpriteManager.Get(TechType.PictureFrame));

            // We'll use the PictureFrame as a template
            PrefabTemplate consoleTemplate = new CloneTemplate(customConsole.Info, TechType.PictureFrame)
            {
                // Reconfigure the prefab, once it's been created
                ModifyPrefab = ConfigureConsoleComponents
            };
            customConsole.SetGameObject(consoleTemplate);

            // Set the recipe for the new Console
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
            // We're going to take some of the content from the BaseConsoleUpgrade, mainly to
            // re-purpose the UI


            Log.LogDebug("PetConsolePrefab: Adding PetConsole component...");
            PetConsoleUi petConsole = consoleGameObject.AddComponent<PetConsoleUi>();
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component... Done.");

            Log.LogDebug("PetConsolePrefab: Removing PictureFrame components...");
            ModUtils.DestroyComponentsInChildren<PictureFrame>(consoleGameObject);
            Log.LogDebug("PetConsolePrefab: Removing PictureFrame components... Done.");
        }

    }
}
