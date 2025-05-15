using DaftAppleGames.SubnauticaPets.BaseParts;
using DaftAppleGames.SubnauticaPets.Extensions;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    internal static class PetConsolePrefab
    {
        public static PrefabInfo Info;

        /// <summary>
        /// Register Pet Console
        /// </summary>
        public static void Register()
        {
            // Unlock at start if in Creative mode
            Info = PrefabInfo
                .WithTechType("PetConsole", null, null, unlockAtStart: false)
                .WithIcon(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetConsoleIconTexture") as Sprite);
            CustomPrefab consolePrefab = new CustomPrefab(Info);

            // We'll use the PictureFrame as a template
            PrefabTemplate consoleTemplate = new CloneTemplate(consolePrefab.Info, TechType.PictureFrame)
            {
                // Reconfigure the prefab, once it's been created
                ModifyPrefab = prefabGameObject =>
                {
                    ConfigurePrefab(prefabGameObject);
                }
            };
            consolePrefab.SetGameObject(consoleTemplate);

            // Define the recipe for the new Console, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
            {
                recipe = new RecipeData(
                    new CraftData.Ingredient(TechType.Titanium, 3),
                    new CraftData.Ingredient(TechType.ComputerChip, 1),
                    new CraftData.Ingredient(TechType.CopperWire, 2),
                    new CraftData.Ingredient(TechType.Glass, 1));
            }
            else
            {
                // Only costs 1 titanium in "Easy" mode
                recipe = new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1));
            }

            // Set the recipe.
            consolePrefab.SetRecipe(recipe);

            consolePrefab.SetUnlock(Info.TechType)
                .WithAnalysisTech(CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetConsoleDataBankPopupImageTexture") as Sprite, null,
                    null)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithEncyclopediaEntry("Tech/Habitats",
                    CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetConsoleDataBankPopupImageTexture") as Sprite,
                    CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>("PetConsoleDataBankMainImageTexture") as Texture2D);

            consolePrefab.Register();
        }

        private static void ConfigurePrefab(GameObject prefabGameObject)
        {
            prefabGameObject.SetActive(false);
            prefabGameObject.ApplyNewMeshTexture("PetConsoleTexture", "submarine_Picture_Frame");

            prefabGameObject.DestroyComponentsInChildren<PictureFrame>();

            // Set up the UI
            PetConsole petConsole = prefabGameObject.AddComponent<PetConsole>();

            GameObject screen = prefabGameObject.transform.Find("Screen").gameObject;
            screen.SetActive(false);

            CreatePetConsoleUi(prefabGameObject, out Button renameButton,
                out Button killButton, out Button killConfirmButton,
                out Button killAllButton, out Button killAllConfirmButton,
                out TMP_InputField petNameTextInput, out GameObject petsScrollViewContent, out Button petListButtonTemplate);
            petConsole.renameButton = renameButton;
            petConsole.killButton = killButton;
            petConsole.killConfirmButton = killConfirmButton;
            petConsole.killAllButton = killAllButton;
            petConsole.killAllConfirmButton = killAllConfirmButton;
            petConsole.petNameTextInput = petNameTextInput;
            petConsole.petListButtonTemplate = petListButtonTemplate;
            petConsole.petsScrollViewContent = petsScrollViewContent;
        }

        /// <summary>
        /// Create the UI
        /// </summary>
        public static void CreatePetConsoleUi(GameObject targetGameObject,
            out Button renameButton,
            out Button killButton, out Button killConfirmButton,
            out Button killAllButton, out Button killAllConfirmButton,
            out TMP_InputField petNameTextInput, out GameObject petsScrollViewContent, out Button petListButtonTemplate)
        {
            // Get MoonpoolUpgradeConsole prefab instance as a base for copying out controls
            GameObject consolePrefabGameObject = Base.pieces[(int)Base.Piece.MoonpoolUpgradeConsoleShort].prefab.gameObject;
            GameObject consoleClone = GameObject.Instantiate(consolePrefabGameObject);

            GameObject editScreenGameObject = consoleClone.FindChild("EditScreen");
            editScreenGameObject.transform.SetParent(targetGameObject.transform);
            editScreenGameObject.transform.localPosition = new Vector3(0, 0, 0.02f);
            editScreenGameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
            editScreenGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject.Destroy(consoleClone);

            // Init UI
            GameObject newScreenGameObject = CreateConsoleScreen(editScreenGameObject);
            CreateConsoleButtons(editScreenGameObject, newScreenGameObject,
                out killButton, out killConfirmButton,
                out killAllButton, out killAllConfirmButton,
                out renameButton);
            CreateTextEntry(editScreenGameObject, newScreenGameObject, out petNameTextInput);
            UiUtils.CreateScrollView(editScreenGameObject, newScreenGameObject, "Pet List Scroll View", new Vector3(-145, -50, 0), new Vector2(360.0f, 200.0f), out petsScrollViewContent);
            editScreenGameObject.AddComponent<PetConsoleInput>();
            CreatePetListButtonTemplate(editScreenGameObject, targetGameObject, out petListButtonTemplate);
            // AddRotatingIcon(editScreenGameObject);
        }

        /// <summary>
        /// Create the new button controls
        /// </summary>
        private static void CreateConsoleButtons(GameObject sourceUiScreen, GameObject targetUiScreen,
            out Button killButton, out Button killConfirmButton,
            out Button killAllButton, out Button killAllConfirmButton,
            out Button renameButton)
        {
            // Rename button
            renameButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "RenamePetButton", "Button_Rename", targetUiScreen,
                new Vector3(160, 20, 0), false);

            // Kill button
            killButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillPetButton", "Button_Kill", targetUiScreen,
                new Vector3(160, -50, 0), false);

            // Kill confirm button
            killConfirmButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsConfirmButton", "Button_AreYouSure", targetUiScreen,
                new Vector3(160, -50, 0), true);

            // Kill All button
            killAllButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsButton", "Button_KillAll", targetUiScreen,
                new Vector3(160, -120, 0), false);

            // Kill All Confirm button
            killAllConfirmButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsConfirmButton", "Button_AreYouSure", targetUiScreen,
                new Vector3(160, -120, 0), true);

            // Set up the colour and state of the 'Confirm' buttons
            ConfigureConfirmButton(killConfirmButton);
            ConfigureConfirmButton(killAllConfirmButton);
        }

        private static void ConfigureConfirmButton(Button confirmButton)
        {
            confirmButton.GetComponent<Image>().color = Color.red;
            confirmButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            confirmButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create the new Text Entry controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private static void CreateTextEntry(GameObject sourceUiScreen, GameObject targetUiScreen, out TMP_InputField inputField)
        {
            // Rename pet label
            GameObject petNameLabel = UiUtils.CreateLabel(sourceUiScreen, "Name Label", "PetNameLabel", "Label_PetName",
                targetUiScreen, new Vector3(-180, 100, 0));
            // Rename pet field
            inputField = UiUtils.CreateTextEntry(sourceUiScreen, "InputField", "PetNameField", "Tip_ClickToEdit",
                targetUiScreen, new Vector3(110, 100, 0));
        }

        private static void CreatePetListButtonTemplate(GameObject sourceUiScreen, GameObject targetUiScreen, out Button petListButtonTemplate)
        {
            petListButtonTemplate = UiUtils.CreateButton(sourceUiScreen, "Button",
                $"SelectPetButtonTemplate", $"Template",
                targetUiScreen, new Vector3(0, 0, 0), true);
            petListButtonTemplate.gameObject.SetActive(false);
        }

        /// <summary>
        /// Adds a little rotating icon to the top left of the console
        /// </summary>
        private static void AddRotatingIcon(GameObject targetGameObject)
        {
            GameObject iconGameObject = new GameObject("ConsoleIcon")
            {
                transform =
                {
                    localPosition = new Vector3(-50, 25, 0),
                    localRotation = new Quaternion(0, 0, 0, 0),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            };

            iconGameObject.transform.SetParent(targetGameObject.transform);

            Image iconImage = iconGameObject.AddComponent<Image>();
            iconImage.sprite = CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>("PetConsoleRotatingIconTexture") as Sprite;
            RotateIcon iconRotate = iconGameObject.AddComponent<RotateIcon>();

        }

        /// <summary>
        /// Disables "original" UI elements in the source UI
        /// Returns a "new" clean one for use in the mod
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="newScreenName"></param>
        /// <param name="activeScreenName"></param>
        /// <param name="inactiveScreenName"></param>
        public static GameObject CreateConsoleScreen(GameObject sourceUi)
        {
            // Disable "Screen", "Active" and "Inactive"
            GameObject activeScreen = sourceUi.transform.Find("Active").gameObject;
            GameObject inactiveScreen = sourceUi.transform.Find("Inactive").gameObject;
            activeScreen.SetActive(false);
            inactiveScreen.SetActive(false);
            SubNameInput subNameInput = sourceUi.GetComponent<SubNameInput>();
            Object.Destroy(subNameInput);
            GameObject newScreen = Object.Instantiate(activeScreen);
            newScreen.name = "PetConsolePanel";
            newScreen.transform.SetParent(sourceUi.transform);
            newScreen.transform.position = inactiveScreen.transform.position;
            newScreen.transform.rotation = inactiveScreen.transform.rotation;
            newScreen.transform.localScale = inactiveScreen.transform.localScale;

            // Deactivate all existing content
            foreach (Transform child in newScreen.transform)
            {
                child.gameObject.SetActive(false);
            }
            Image backgroundImage = newScreen.GetComponent<Image>();
            if (backgroundImage)
            {
                backgroundImage.enabled = false;
            }
            newScreen.SetActive(true);
            return newScreen;
        }
    }
}