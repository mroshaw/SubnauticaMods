using DaftAppleGames.SubnauticaPets.Pets;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static CraftData;
namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PrefabConfigUtilsPlatform
    {
        public static void RegisterCustomPet(PrefabInfo prefabInfo, string classId, string bundlePrefabName, TechType techType, TechType dnaTechType, bool includeDnaTechType = false)
        {
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            GameObject prefabGameObject = ModUtils.GetGameObjectPrefabFromAssetBundle(bundlePrefabName);

            GameObject model = prefabGameObject.transform.Find("model").gameObject;
            Transform petEyes = prefabGameObject.transform.Find("Eyes");
            SimpleMovement simpleMovement = prefabGameObject.AddComponent<SimpleMovement>();
            simpleMovement.Eyes = petEyes;

            PrefabUtils.AddBasicComponents(prefabGameObject, classId, prefabInfo.TechType, LargeWorldEntity.CellLevel.Medium);
            PrefabUtils.AddConstructable(prefabGameObject, prefabInfo.TechType, ConstructableFlags.Base, model);
            PrefabUtils.AddVFXFabricating(prefabGameObject, "model", -0.2f, 0.9f, new Vector3(0.0f, 0.0f, 0.0f), 0.7f, new Vector3(0.0f, 0.0f, 0.0f));

            prefabGameObject.GetComponent<LargeWorldEntity>().enabled = false;
            MaterialUtils.ApplySNShaders(prefabGameObject);
            PrefabConfigUtils.AddPetComponent(prefabGameObject);
            PrefabConfigUtils.AddCustomPetComponents(prefabGameObject);
            PrefabConfigUtils.AddPetHandTarget(prefabGameObject);
            prefab.SetGameObject(prefabGameObject);

            // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
            {
                if (includeDnaTechType)
                {
                    recipe = new RecipeData(
                        new Ingredient(TechType.Gold, 1),
                        new Ingredient(TechType.Titanium, 1),
                        new Ingredient(TechType.Salt, 1),
                        new Ingredient(dnaTechType, 2));
                }
                else
                {
                    recipe = new RecipeData(
                        new Ingredient(TechType.Gold, 1),
                        new Ingredient(TechType.Titanium, 1),
                        new Ingredient(TechType.Salt, 1));
                }
            }
            else
            {
                recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
            }
            CraftingGadget crafting = prefab.SetRecipe(recipe);
            prefab.Register();
        }

        /// <summary>
        /// Destroy the EmpAttack component
        /// </summary>
        public static void DestroyEmpAttack(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyEmpAttack started...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(targetGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyEmpAttack done.");
        }

        /// <summary>
        /// Destroy the AttackLastTarget component
        /// </summary>
        public static void DestroyAttackLastTarget(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget started...");
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(targetGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget done.");
        }

    }
}