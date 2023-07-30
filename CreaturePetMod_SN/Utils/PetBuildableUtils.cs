using System.IO;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;
using System.Reflection;

namespace DaftAppleGames.CreaturePetModSn.CustomObjects
{

    public static class PetBuildableUtils
    {
        // Public static references for use by other consumers
        public static PrefabInfo CaveCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo BloodCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo CrabSquidPetBuildablePrefabInfo;
        public static PrefabInfo AlienRobotBuildablePefabInfo;

        public static string SpritePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Sprites";

        /// <summary>
        /// Initialise all Pet buildables
        /// </summary>
        public static void InitPetBuildables()
        {
            Log.LogDebug($"PetBuildableUtils: Current Sprite path is: {SpritePath}");
            CaveCrawlerBuildable.Register();
            BloodCrawlerBuildable.Register();
            CrabSquidBuildable.Register();
            AlienRobotBuildable.Register();
        }

        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("CaveCrawlerPet", "Cave Crawler Pet", "A pet cave crawler.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\CaveCrawler94x110.png"));

            public static void Register()
            {

                // So this can be used by other consumers
                CaveCrawlerPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of the Cave Crawler: WorldEntities/Creatures/CaveCrawler.prefab
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "3e0a11f1-e2b2-4c4f-9a8e-0b0a77dcc065");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("cave_crawler_03").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: CaveCrawlerBuildable cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: CaveCrawlerBuildable found model on {model.name}");
                    }

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.CaveCrawlerDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Blood Crawler Buildable
        /// </summary>
        public static class BloodCrawlerBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("BloodCrawlerPet", "Blood Crawler Pet", "A pet blood crawler.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\BloodCrawler94x110.png"));

            public static void Register()
            {

                // So this can be used by other consumers
                BloodCrawlerPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of the Blood Crawler. WorldEntities/Slots/BloodKelp/BloodKelp_Creature_Unique.prefab
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "314e0fd9-56c5-4f80-9663-15fa077abc15");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("cave_crawler_03").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: BloodCrawlerBuildable cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: BloodCrawlerBuildable found model on {model.name}");
                    }


                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.BloodCrawlerDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Crab Squid Buildable
        /// </summary>
        public static class CrabSquidBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("CrabSquidPet", "Crab Squid Pet", "A pet crab squid.")
                // set the icon to that of the vanilla locker:
                .WithIcon(SpriteManager.Get(TechType.CrabSquid));

            public static void Register()
            {

                // So this can be used by other consumers
                CrabSquidPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of a CrabSquid - WorldEntities/Creatures/CrabSquid.prefab
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "4c2808fe-e051-44d2-8e64-120ddcdc8abb");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: CrabSquidBuilding cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: CrabSquidBuilding found model on {model.name}");
                    }

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.CrabSquidDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Alien Robot Buildable
        /// </summary>
        public static class AlienRobotBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("AlienRobotPet", "Alien Robot Pet", "A pet alien robot.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ImageUtils.LoadSpriteFromFile(SpritePath + "\\AlienRobot94x110.png"));

            public static void Register()
            {

                // So this can be used by other consumers
                AlienRobotBuildablePefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: AlienRobotBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: AlienRobotBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.AlienRobotDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }
    }

}