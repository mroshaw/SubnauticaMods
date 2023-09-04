using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static DaftAppleGames.SubnauticaPets.Utils.UiUtils;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// This static class provides methods to create our "Buildable Pets", via the
    /// new Pet Fabricator.
    /// </summary>
    public static class PetBuildablePrefab
    {
        // Public static references for use by other consumers
#if SUBNAUTICA
        public static PrefabInfo CaveCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo BloodCrawlerPetBuildablePrefabInfo;
        public static PrefabInfo CrabSquidPetBuildablePrefabInfo;
        public static PrefabInfo AlienRobotBuildablePefabInfo;
#endif

#if SUBNAUTICAZERO
        public static PrefabInfo PenglingBabyPetBuildablePrefabInfo;
        public static PrefabInfo PenglingAdultPetBuildablePrefabInfo;
        public static PrefabInfo SnowStalkerBabyPetBuildablePrefabInfo;
        public static PrefabInfo PinnicaridPetBuildablePefabInfo;
        public static PrefabInfo TrivalveBluePetBuildablePefabInfo;
        public static PrefabInfo TrivalveYellowBuildablePefabInfo;
#endif
        /// <summary>
        /// Initialise all Pet buildables
        /// </summary>
        public static void InitPetBuildables()
        {
#if SUBNAUTICA
            CaveCrawlerBuildable.Register();
            BloodCrawlerBuildable.Register();
            CrabSquidBuildable.Register();
            AlienRobotBuildable.Register();
#endif
#if SUBNAUTICAZERO
            PenglingBabyBuildable.Register();
            PenglingAdultBuildable.Register();
            SnowStalkerBabyBuildable.Register();
            PinnicaridBuildable.Register();
            TrivalveYellowBuildable.Register();
            TrivalveBluePetBuildable.Register();
#endif
        }

#if SUBNAUTICA
        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("CaveCrawlerPet", "Cave Crawler Pet", "A pet cave crawler.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(CaveCrawlerTexture));

            public static void Register()
            {
                // So this can be used by other consumers
                CaveCrawlerPetBuildablePrefabInfo = Info;
                // Create prefab:
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
                prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Gold, 3),
                    new CraftData.Ingredient(PetDnaPrefab.CaveCrawlerDnaPrefabInfo.TechType, 5)));
                // Finally, register it into the game
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
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(BloodCrawlerTexture));

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
                prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Gold, 3),
                    new CraftData.Ingredient(PetDnaPrefab.BloodCrawlerDnaPrefabInfo.TechType, 5)));

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
                prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Gold, 3),
                    new CraftData.Ingredient(PetDnaPrefab.CrabSquidDnaPrefabInfo.TechType, 5)));

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
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(AlienRobotTexture));

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
                prefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Gold, 3),
                    new CraftData.Ingredient(PetDnaPrefab.AlienRobotDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

#endif

#if SUBNAUTICAZERO
        /// <summary>
        /// Baby Pengling Buildable
        /// </summary>
        public static class PenglingBabyBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("PenglingBabyPet", "Pengling Baby Pet", "A pet baby Pengling.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PenglingBabyTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                PenglingBabyPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate babyPenglingClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                babyPenglingClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: PenglingBabyBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: PenglingBabyBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(babyPenglingClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.PenglingBabyDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Pengling Adult Buildable
        /// </summary>
        public static class PenglingAdultBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("PenglingAdultPet", "Pengling Adult Pet", "A pet adult Pengling.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PenglingAdultTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                PenglingAdultPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate adultPenglingClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                adultPenglingClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: PenglingAdultBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: PenglingAdultBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(adultPenglingClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.PenglingAdultDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Snow Stalker Baby Buildable
        /// </summary>
        public static class SnowStalkerBabyBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("SnowStalkerBabyPet", "Snow Stalker Baby Pet", "A pet baby Snowstalker.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(SnowStalkerBabyTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                SnowStalkerBabyPetBuildablePrefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate babySnowStalkerClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                babySnowStalkerClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: SnowStalkerBabyBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: SnowStalkerBabyBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(babySnowStalkerClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.SnowStalkerBabyDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Pinnicarid Buildable
        /// </summary>
        public static class PinnicaridBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("PinnicaridPet", "Pinnicarid Pet", "A pet Pinnicarid.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(PinnicaridTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                PinnicaridPetBuildablePefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate pinnicaridClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                pinnicaridClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: PinnicaridBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: PinnicaridBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(pinnicaridClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.PinnicaridDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Yellow Trivalve Buildable
        /// </summary>
        public static class TrivalveYellowBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("YellowTrivalvePet", "Yellow Trivalve Pet", "A pet yellow Trivalve.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(TrivalveYellowTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                TrivalveYellowBuildablePefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate trivalveYellowClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                trivalveYellowClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: TrivalveYellowBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: TrivalveYellowBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(trivalveYellowClone);


                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.TrivalveYellowDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }

        /// <summary>
        /// Blue Trivalve Buildable
        /// </summary>
        public static class TrivalveBluePetBuildable
        {
            public static PrefabInfo Info { get; } = PrefabInfo
                .WithTechType("BlueTrivalvePet", "Blue Trivalve Pet", "A pet yellow Trivalve.")
                // set the icon to that of the vanilla locker:
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(TrivalveBlueTexture));

            public static void Register()
            {

                // So this can be used by other consumers
                TrivalveBluePetBuildablePefabInfo = Info;

                // create prefab:
                CustomPrefab prefab = new CustomPrefab(Info);

                // copy the model of an Alien Robot - WorldEntities/Creatures/Precursor_Droid.prefab
                CloneTemplate trivalveBlueClone = new CloneTemplate(Info, "4fae8fa4-0280-43bd-bcf1-f3cba97eed77");

                // modify the cloned model:
                trivalveBlueClone.ModifyPrefab += obj =>
                {
                    // allow it to be placed inside bases and submarines on the ground, and can be rotated:
                    ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                    // find the object that holds the model:
                    GameObject model = obj.transform.Find("models").gameObject;
                    if (!model)
                    {
                        Log.LogDebug("PetBuildableUtils: BlueTrivalveBuildable - cannot find object model in prefab!");
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildableUtils: BlueTrivalveBuildable found model on {model.name}");
                    }
                    // add all components necessary for it to be built:
                    PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
                };

                // assign the created clone model to the prefab itself:
                prefab.SetGameObject(trivalveBlueClone);

                // set recipe:
                prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Gold, 3),
                    new Ingredient(PetDnaPrefab.TrivalveBlueDnaPrefabInfo.TechType, 5)));

                // finally, register it into the game:
                prefab.Register();
            }
        }
#endif
    }
}