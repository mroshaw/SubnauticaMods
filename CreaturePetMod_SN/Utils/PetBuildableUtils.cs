using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static CraftData;

namespace DaftAppleGames.CreaturePetModSn.Utils
{

    public static class PetBuildableUtils
    {
        // Public static references for use by other consumers
        public static PrefabInfo CaveCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo BloodCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo CrabSquidPetBuildablePrefabInfo;
        public static PrefabInfo AlienRobotBuildablePefabInfo;

        /// <summary>
        /// Initialise all Pet buildables
        /// </summary>
        public static void InitPetBuildables()
        {
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
                .WithIcon(SpriteManager.Get(TechType.CaveCrawler));

            public static void Register()
            {

                // So this can be used by other consumers
                CaveCrawlerPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of the Cave Crawler:
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "7ce2ca9d-6154-4988-9b02-38f670e741b8");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("submarine_locker_04").gameObject;

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.CarbonOld, 3),
                    new Ingredient(PetDnaPrefabUtils.CaveCrawlerDnaPrefabInfo.TechType, 5)));

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
                .WithIcon(SpriteManager.Get(TechType.Shuttlebug));

            public static void Register()
            {

                // So this can be used by other consumers
                BloodCrawlerPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of the Blood Crawler
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "cd34fecd-794c-4a0c-8012-dd81b77f2840");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("submarine_locker_04").gameObject;

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.CarbonOld, 3),
                    new Ingredient(PetDnaPrefabUtils.BloodCrawlerDnaPrefabInfo.TechType, 5)));

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

                // copy the model of a CrabSquid
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "cd34fecd-794c-4a0c-8012-dd81b77f2840");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placced inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("submarine_locker_04").gameObject;

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.CarbonOld, 3),
                    new Ingredient(PetDnaPrefabUtils.CrabSquidDnaPrefabInfo.TechType, 5)));

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
                .WithIcon(SpriteManager.Get(TechType.PrecursorDroid));

            public static void Register()
            {

                // So this can be used by other consumers
                AlienRobotBuildablePefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot
                CloneTemplate caveCrawlerClone = new CloneTemplate(Info, "cd34fecd-794c-4a0c-8012-dd81b77f2840");

                // modify the cloned model:
                caveCrawlerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("submarine_locker_04").gameObject;

                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(caveCrawlerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.CarbonOld, 3),
                    new Ingredient(PetDnaPrefabUtils.AlienRobotDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }
    }

}