using DaftAppleGames.SubnauticaPets.Extensions;
using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static CraftData;
namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static utilities class for prefab configuration tasks specifically for the Subnautica game
    /// </summary>
    internal static class PrefabConfigUtilsPlatform
    {
        public static void RegisterCustomPet(PrefabInfo prefabInfo, string classId, string bundlePrefabName,
            string audioClipName,
            TechType techType, TechType dnaTechType)
        {
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            GameObject prefabGameObject = CustomAssetBundleUtils.GetObjectFromAssetBundle<GameObject>(bundlePrefabName) as GameObject;

            GameObject model = prefabGameObject.transform.Find("model").gameObject;
            Transform petEyes = prefabGameObject.transform.Find("Eyes");
            SimpleMovement simpleMovement = prefabGameObject.AddComponent<SimpleMovement>();

            // Standard components
            PrefabUtils.AddBasicComponents(prefabGameObject, classId, prefabInfo.TechType, LargeWorldEntity.CellLevel.Medium);
            PrefabUtils.AddConstructable(prefabGameObject, prefabInfo.TechType, ConstructableFlags.Base, model);
            PrefabUtils.AddVFXFabricating(prefabGameObject, "model", -0.2f, 0.9f, new Vector3(0.0f, 0.0f, 0.0f), 0.7f, new Vector3(0.0f, 0.0f, 0.0f));
            prefabGameObject.GetComponent<LargeWorldEntity>().enabled = false;
            MaterialUtils.ApplySNShaders(prefabGameObject);

            // Custom Pet Components
            PrefabConfigUtils.AddPetComponent(prefabGameObject);
            PrefabConfigUtils.AddCustomPetComponents(prefabGameObject, audioClipName, AudioUtils.BusPaths.SurfaceCreatures, 10.0f);
            PrefabConfigUtils.AddPetHandTarget(prefabGameObject);
            AddSubnauticaPetComponents(prefabGameObject);
            
            // ONLY required for Subnautica, to prevent Pets from falling into MoonPools
            prefabGameObject.SetLayer("Vehicle", true);
            
            prefab.SetGameObject(prefabGameObject);

            // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (SubnauticaPetsPlugin.ModConfig.ModMode == ModMode.Adventure)
            {
                if (dnaTechType != TechType.None)
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
            targetGameObject.DestroyComponentsInChildren<EMPAttack>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyEmpAttack done.");
        }

        /// <summary>
        /// Destroy the AttackLastTarget component
        /// </summary>
        public static void DestroyAttackLastTarget(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget started...");
            targetGameObject.DestroyComponentsInChildren<AttackLastTarget>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "DestroyAttackLastTarget done.");
        }

        /// <summary>
        /// Custom components for all Subnautica game pets
        /// </summary>
        public static void AddSubnauticaPetComponents(GameObject targetGameObject)
        {
            AddCleanUpSerializer(targetGameObject);
        }
        
        /// <summary>
        /// Ensures the Pet is on the correct Layer for MoonPool collision detection
        /// </summary>
        public static void AddCleanUpSerializer(GameObject targetGameObject)
        {
            targetGameObject.EnsureComponent<CleanUpSerializer>();
        }
    }
}