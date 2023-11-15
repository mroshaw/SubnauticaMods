using DaftAppleGames.SubnauticaPets.Mono.Pets;
using Nautilus.Utility;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    internal static class PetConfig
    {
#if SUBNAUTICA

        /// <summary>
        /// Configure the given Pet Type
        /// </summary>
        /// <param name="petType"></param>
        /// <param name="petGameObject"></param>
        public static Pet ConfigurePet(GameObject petGameObject, PetCreatureType petType, string petName, GameObject parentGameObject)
        {
            switch (petType)
            {
                case PetCreatureType.AlienRobot:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureAlienRobot(petGameObject);
                    break;
                case PetCreatureType.BloodCrawler:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureBloodCrawler(petGameObject);
                    break;
                case PetCreatureType.CaveCrawler:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureCaveCrawler(petGameObject);
                    break;
                case PetCreatureType.CrabSquid:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureCrabSquid(petGameObject);
                    break;
                case PetCreatureType.Cat:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureCat(petGameObject);
                    break;
            }

            return petGameObject.GetComponent<Pet>();
        }

        /// <summary>
        /// Configure common Pet components
        /// </summary>
        /// <param name="petGameObject"></param>
        /// <param name="petType"></param>
        /// <param name="petName"></param>
        /// <param name="parentGameObject"></param>
        public static void ConfigureCommon(GameObject petGameObject, PetCreatureType petType, string petName, GameObject parentGameObject)
        {
            // Add the Pet component
            PetConfigUtils.AddPetComponent(petGameObject, petType, petName, parentGameObject);

            // Add common components
            PetConfigUtils.AddRigidBody(petGameObject);
            PetConfigUtils.AddWorldForces(petGameObject);
            PetConfigUtils.AddPetHandTarget(petGameObject);

            // Reconfigure common components
            PetConfigUtils.ConfigureSkyApplier(petGameObject);
            ModUtils.DestroyComponentsInChildren<Pickupable>(petGameObject);
            PetConfigUtils.ConfigureAnimator(petGameObject);
            PetConfigUtils.ConfigurePetTraits(petGameObject);
        }

        /// <summary>
        /// Configure components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigureAlienRobot(GameObject petGameObject)
        {

        }

        /// <summary>
        /// Configure components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigureBloodCrawler(GameObject petGameObject)
        {

        }

        /// <summary>
        /// Configure components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigureCaveCrawler(GameObject petGameObject)
        {

        }

        /// <summary>
        /// Configure components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigureCrabSquid(GameObject petGameObject)
        {
            ModUtils.DestroyComponentsInChildren<MeleeAttack>(petGameObject);
            ModUtils.DestroyComponentsInChildren<EMPAttack>(petGameObject);
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(petGameObject);
        }

#endif

#if SUBNAUTICAZERO

        /// <summary>
        /// Configure the given Pet Type
        /// </summary>
        /// <param name="petType"></param>
        /// <param name="petGameObject"></param>
        /// <param name="petName"></param>
        /// <param name="parentGameObject"></param>
        public static Pet ConfigurePet(GameObject petGameObject, PetCreatureType petType, string petName, GameObject parentGameObject)
        {
            switch (petType)
            {
                case PetCreatureType.PenglingBaby:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigurePenguinBaby(petGameObject);
                    break;
                case PetCreatureType.PenglingAdult:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigurePenguinAdult(petGameObject);
                    break;
                case PetCreatureType.SnowstalkerBaby:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureSnowStalkerBaby(petGameObject);
                    break;
                case PetCreatureType.Pinnicarid:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigurePinnicarid(petGameObject);
                    break;
                case PetCreatureType.BlueTrivalve:
                case PetCreatureType.YellowTrivalve:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureTrivalve(petGameObject);
                    break;
                case PetCreatureType.Cat:
                    ConfigureCommon(petGameObject, petType, petName, parentGameObject);
                    ConfigureCat(petGameObject);
                    break;
            }

            return petGameObject.GetComponent<Pet>();
        }

        /// <summary>
        /// Configure common Pet components
        /// </summary>
        /// <param name="petGameObject"></param>
        /// <param name="petType"></param>
        /// <param name="petName"></param>
        /// <param name="parentGameObject"></param>
        public static void ConfigureCommon(GameObject petGameObject, PetCreatureType petType, string petName, GameObject parentGameObject)
        {
            // Add the Pet component
            PetConfigUtils.AddPetComponent(petGameObject, petType, petName, parentGameObject);

            // Add common components
            PetConfigUtils.AddRigidBody(petGameObject);
            PetConfigUtils.AddWorldForces(petGameObject);
            PetConfigUtils.AddPetHandTarget(petGameObject);

            // Reconfigure common components
            PetConfigUtils.ConfigureSkyApplier(petGameObject);
            ModUtils.DestroyComponentsInChildren<Pickupable>(petGameObject);
            PetConfigUtils.ConfigureAnimator(petGameObject);
            PetConfigUtils.ConfigurePetTraits(petGameObject);
        }

        /// <summary>
        /// Configure components
        /// </summary>
        public static void ConfigurePenguinBaby(GameObject petGameObject)
        {
            PetConfigUtils.PreventFloatingOnDeath(petGameObject);
            PetConfigUtils.ConfigureSwimming(petGameObject);
        }

        /// <summary>
        /// Configure components
        /// </summary>
        public static void ConfigurePenguinAdult(GameObject petGameObject)
        {
            PetConfigUtils.PreventFloatingOnDeath(petGameObject);
            PetConfigUtils.ConfigureSwimming(petGameObject);
        }

        /// <summary>
        /// Configure components
        /// </summary>
        public static void ConfigureSnowStalkerBaby(GameObject petGameObject)
        {
            PetConfigUtils.PreventFloatingOnDeath(petGameObject);
            PetConfigUtils.ConfigureMovement(petGameObject);
            PetConfigUtils.ConfigureSwimming(petGameObject);
        }

        /// <summary>
        /// Configure Pinnicarid components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigurePinnicarid(GameObject petGameObject)
        {
            PetConfigUtils.PreventFloatingOnDeath(petGameObject);
            PetConfigUtils.ConfigureSwimming(petGameObject);
        }

        /// <summary>
        /// Configure components
        /// </summary>
        public static void ConfigureTrivalve(GameObject petGameObject)
        {
            ModUtils.DestroyComponentsInChildren<LargeWorldEntity>(petGameObject);
            PetConfigUtils.PreventFloatingOnDeath(petGameObject);
            PetConfigUtils.ConfigureSwimming(petGameObject);
        }
#endif
        /// <summary>
        /// Configure Cat components
        /// </summary>
        public static void ConfigureCat(GameObject petGameObject)
        {
            PetConfigUtils.AddPrefabIdentifier(petGameObject);
            PetConfigUtils.AddSimpleMovement(petGameObject);
            MaterialUtils.ApplySNShaders(petGameObject);
        }
    }
}
