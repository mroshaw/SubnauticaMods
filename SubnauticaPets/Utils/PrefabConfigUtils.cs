﻿using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using Nautilus.Utility;
using UnityEngine;
#if SUBNAUTICAZERO
using UnityEngine.AI;
#endif
namespace DaftAppleGames.SubnauticaPets.Utils
{
    internal static class PrefabConfigUtils
    {
        /// <summary>
        /// Adds the basic components required for every Pet to function
        /// </summary>
        public static void AddBasicPetComponents()
        {

        }

        /// <summary>
        /// Adds the specified child pet class component to the given creature GameObject
        /// based on the given PetCreatureType
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <returns></returns>
        public static void AddPetComponent(GameObject targetGameObject)
        {
            Pet pet = targetGameObject.GetComponent<Pet>();
            if (pet == null)
            {
                pet = targetGameObject.AddComponent<Pet>();
            }
        }

        /// <summary>
        /// Adds a Capsule Collider to DNA prefab
        /// </summary>
        /// <param name="targetGameObject"></param>
        public static void AddDnaCapsuleCollider(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddDnaCapsuleCollider started...");

            Collider collider = targetGameObject.GetComponentInChildren<Collider>(true);
            if (collider)
            {
                Object.Destroy(collider);
                CapsuleCollider newCollider = collider.gameObject.AddComponent<CapsuleCollider>();
                newCollider.center = new Vector3(0, 0, 0);
                newCollider.radius = 0.18f;
                newCollider.height = 0.73f;
                newCollider.direction = 1;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddDnaCapsuleCollider done.");
        }

        /// <summary>
        /// Configure the animator component
        /// </summary>
        public static void ConfigureAnimator(GameObject targetGameObject, bool isEnabled)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureAnimator started...");
            Creature creature = targetGameObject.GetComponent<Creature>();
            Animator animator = creature.GetAnimator();
            animator.enabled = isEnabled;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureAnimator done.");
        }

        /// <summary>
        /// Adds the ScaleOnStart component
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="scaleFactor"></param>
        public static void AddScaleOnStart(GameObject targetGameObject, float scaleFactor)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddScaleOnStart started...");
            ScaleOnStart scaleOnStart = targetGameObject.AddComponent<ScaleOnStart>();
            scaleOnStart.Scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddScaleOnStart done.");
        }

        /// <summary>
        /// Add VFX Fabricator component
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="pathToModel"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="posOffset"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="eulerOffset"></param>
        public static void AddVFXFabricating(GameObject targetGameObject, string pathToModel, float minY, float maxY,Vector3 posOffset, float scaleFactor, Vector3 eulerOffset)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddVFXFabricating started...");
            GameObject modelGameObject = targetGameObject.GetComponentInChildren<Animator>().gameObject;
            if (modelGameObject != null)
            {
                PrefabUtils.AddVFXFabricating(modelGameObject, pathToModel, minY, maxY, posOffset, scaleFactor,
                    eulerOffset);
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddVFXFabricating done.");
        }

        /// <summary>
        /// Adds a Prefab Identifier component
        /// </summary>
        /// <param name="targetGameObject"></param>
        public static void AddPrefabIdentifier(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddPrefabIdentifier started...");
            PrefabIdentifier prefabIdentifier = targetGameObject.GetComponent<PrefabIdentifier>();
            if (!prefabIdentifier)
            {
                targetGameObject.AddComponent<PrefabIdentifier>();
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddPrefabIdentifier done.");
        }

        /// <summary>
        /// Sets all Mesh Renderers to the given colour
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="color"></param>
        public static void SetMeshRenderersColor(GameObject targetGameObject, string modelGameObjectName, Color color)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetMeshRenderersColor started...");
            targetGameObject.FindChild(modelGameObjectName).GetComponent<MeshRenderer>().material.color = color;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetMeshRenderersColor done.");
        }

        /// <summary>
        /// Adds a RotateModel component
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="modelGameObjectName"></param>
        public static void AddRotateModel(GameObject targetGameObject, string modelGameObjectName)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddRotateModel started...");
            GameObject dnaGameObject = targetGameObject.transform.Find(modelGameObjectName).gameObject;
            RotateModel rotateModel = dnaGameObject.AddComponent<RotateModel>();
            rotateModel.RotationSpeed = 0.1f;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddRotateModel done.");
        }

        /// <summary>
        /// Adds a FixSkyApplier component
        /// </summary>
        /// <param name="targetGameObject"></param>
        public static void AddFixSkyApplier(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddFixSkyApplier started...");
            FixSkyApplier fixSkyApplier = targetGameObject.GetComponent<FixSkyApplier>();
            if (fixSkyApplier == null)
            {
                fixSkyApplier = targetGameObject.AddComponent<FixSkyApplier>();
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddFixSkyApplier done.");
        }

        /// <summary>
        /// Adds a TechTag component
        /// </summary>
        public static void AddTechTag(GameObject targetGameObject, TechType techType)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddTechTag started...");
            TechTag techTag = targetGameObject.GetComponent<TechTag>();
            if (techTag == null)
            {
                techTag = targetGameObject.AddComponent<TechTag>();
            }
            techTag.type = techType;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddTechTag done");
        }

        /// <summary>
        /// Updates the Pickupable component
        /// </summary>
        public static void UpdatePickupable(GameObject targetGameObject, bool isPickupable)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "UpdatePickupable started...");
            // Prevent fragments from being phsyically picked up
            Pickupable pickupable = targetGameObject.GetComponent<Pickupable>();
            if (pickupable)
            {
                pickupable.isPickupable = isPickupable;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "UpdatePickupable done.");
        }

        /// <summary>
        /// Adds the SimpleMovement component
        /// </summary>
        public static void AddSimpleMovement(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddSimpleMovement started...");
            // Add simple movement component
            SimpleMovement movement = targetGameObject.GetComponent<SimpleMovement>();
            if (movement == null)
            {
                movement = targetGameObject.AddComponent<SimpleMovement>();
                movement.MoveSpeed = 1.0f;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddSimpleMovement done.");
        }

        /// <summary>
        /// Add the WorldForces component
        /// </summary>
        public static void AddWorldForces(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddWorldForces started...");
            WorldForces worldForces = targetGameObject.GetComponent<WorldForces>();
            if (worldForces == null)
            {
                worldForces = targetGameObject.AddComponent<WorldForces>();
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddWorldForces done.");
        }

        /// <summary>
        /// Add the PetHandTarget component
        /// </summary>
        public static void AddPetHandTarget(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddPetHandTarget started...");
            targetGameObject.AddComponent<PetHandTarget>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddPetHandTarget done.");
        }

        /// <summary>
        /// Sets the pet scale
        /// </summary>
        public static void SetScale(GameObject targetGameObject, Vector3 scaleFactor)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetScale started...");
            targetGameObject.transform.localScale = scaleFactor;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetScale done.");
        }

        /// <summary>
        /// Configure Pet Traits for "friendly" creatures
        /// </summary>
        public static void ConfigurePetTraits(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigurePetTraits started...");
            Creature creature = targetGameObject.GetComponent<Creature>();
            if (creature)
            {
                creature.Friendliness.Value = 1.0f;
                creature.Happy.Value = 1.0f;
                creature.Aggression.Value = 0.0f;
                creature.Scared.Value = 0.0f;
                creature.Curiosity.Value = 1.0f;
                creature.Hunger.Value = 1.0f;
                creature.Tired.Value = 0.0f;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigurePetTraits done.");
        }

        /// <summary>
        /// Adds a RigidBody, if not one already
        /// </summary>
        public static void AddRigidBody(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddRigidBody started...");
            Rigidbody rigidbody = targetGameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = targetGameObject.AddComponent<Rigidbody>();
                rigidbody.mass = 0.5f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                rigidbody.isKinematic = false;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddRigidBody done.");
        }


        /// <summary>
        /// Update the state of the RigidBody
        /// </summary>
        public static void SetRigidBodyKinematic(GameObject targetGameObject, bool isKinematic)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetRigidBodyKinematic started...");
            Rigidbody rigidbody = targetGameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = isKinematic;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "SetRigidBodyKinematic done.");
        }

        /// <summary>
        /// Adds the Freeze On Settle component
        /// </summary>
        public static void AddFreezeOnSettle(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddFreezeOnSettle started...");
            FreezeOnSettle freeze = targetGameObject.GetComponent<FreezeOnSettle>();
            if (freeze == null)
            {
                freeze = targetGameObject.AddComponent<FreezeOnSettle>();
                freeze.CheckType = FreezeCheckType.Velocity;
                freeze.VelocityThreshold = 0.025f;
                freeze.RayCastDistance = 5f;
                freeze.StartDelay = 2.0f;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddFreezeOnSettle done.");
        }

        /// <summary>
        /// Adds the Align to Floor component
        /// </summary>
        public static void AddAlignToFloor(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddAlignToFloor started...");
            AlignToFloorOnStart alignToFloor = targetGameObject.GetComponent<AlignToFloorOnStart>();
            if (alignToFloor == null)
            {
                alignToFloor = targetGameObject.AddComponent<AlignToFloorOnStart>();
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "AddAlignToFloor done.");
        }

        /// <summary>
        /// Resize the box collider
        /// </summary>
        public static void ResizeCollider(GameObject targetGameObject, Vector3 colliderCenter, Vector3 colliderSize)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ResizeCollider started...");
            BoxCollider collider = targetGameObject.GetComponentInChildren<BoxCollider>(true);
            if (collider)
            {
                collider.center = colliderCenter;
                collider.size = colliderSize;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ResizeCollider done.");
        }

        /// <summary>
        /// Deletes the old model
        /// </summary>
        public static void RemoveOldModel(GameObject targetGameObject, string modelNameHint)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "RemoveOldModel started...");
            GameObject oldModelGameObject = targetGameObject.FindChild(modelNameHint);
            if (oldModelGameObject != null)
            {
                Object.Destroy(oldModelGameObject);
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "RemoveOldModel done.");
        }

        /// <summary>
        /// Configures the Sky and SkyApplier, to ensure
        /// creature mesh shaders don't look "dull".
        /// </summary>
        public static void ConfigureSkyApplier(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSkyApplier started...");
            Pet pet = targetGameObject.GetComponent<Pet>();

            SkyApplier skyApplier = targetGameObject.GetComponent<SkyApplier>();
            if (!skyApplier)
            {
                skyApplier = targetGameObject.AddComponent<SkyApplier>();
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier added SkyApplier component.");
            
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier setting SkyApplier Sky.");
            // skyApplier.SetSky(Skies.BaseInterior);

            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier updating renderers...");
            Renderer[] creatureRenderers = targetGameObject.GetComponentsInChildren<Renderer>(true);
            LogUtils.LogDebug(LogArea.PetConfigUtils, $"Pet: ConfigureSkyApplier found {creatureRenderers.Length} renderers...");
            // skyApplier.anchorSky = Skies.Auto;
            // skyApplier.emissiveFromPower = false;
            skyApplier.dynamic = false;
            if (creatureRenderers.Length > 0)
            {
                skyApplier.renderers = creatureRenderers;
            }

#if SUBNAUTICAZERO
            skyApplier.Initialize();
#endif
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSkyApplier done.");
        }

        /// <summary>
        /// Prevents a Pet from floating on death
        /// </summary>
        public static void PreventFloatingOnDeath(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "PreventFloatingOnDeath started...");
            // Remove the CreatureDeath component, to prevent floating on death
            targetGameObject.DestroyComponentsInChildren<CreatureDeath>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "PreventFloatingOnDeath done.");
        }

        /// <summary>
        /// Remove the given behaviour from the behavior array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="typeToRemove"></param>
        /// <returns></returns>
        private static Behaviour[] RemoveBehaviourItem(Behaviour[] array, System.Type typeToRemove)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "RemoveBehaviourItem started...");
            List<Behaviour> behaviourList = new List<Behaviour>(array);
            Behaviour behaviorToRemove = behaviourList.Find(x => x.GetType() == typeToRemove);
            behaviourList.Remove(behaviorToRemove);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "RemoveBehaviourItem done.");
            return behaviourList.ToArray();
        }


#if SUBNAUTICAZERO
        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        public static void CleanNavUpMesh(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "CleanNavUpMesh started...");
            // Remove NavMesh components
            targetGameObject.DestroyComponentsInChildren<MoveOnNavMesh>();
            targetGameObject.DisableComponentsInChildren<NavMeshFollowing>();
            targetGameObject.DisableComponentsInChildren<NavMeshAgent>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "CleanNavUpMesh done.");
        }

        /// <summary>
        /// Configure Swimming components
        /// </summary>
        /// <param name="targetGameObject"></param>
        public static void ConfigureSwimming(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSwimming started...");
            // Prevent Pet from swimming in interiors   
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... ConfigurePetCreature:  LandCreatureGravity...");
            LandCreatureGravity landCreatureGravity = targetGameObject.GetComponent<LandCreatureGravity>();
            landCreatureGravity.forceLandMode = true;
            landCreatureGravity.enabled = true;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSwimming done.");
        }

        /// <summary>
        /// Override the SnowStalker movement
        /// </summary>
        public static void ConfigureMovement(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureMovement started...");
            SnowStalkerBaby snowStalker = targetGameObject.GetComponent<SnowStalkerBaby>();

            // Add a SurfaceMovement component, get that little bugger moving around!
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ...");
            OnSurfaceTracker onSurfaceTracker = targetGameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = targetGameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = targetGameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = targetGameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = targetGameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalker.onSurfaceMovement = onSurfaceMovement;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = targetGameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = targetGameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalker;
            avoidObstaclesOnSurface.creature = snowStalker;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = targetGameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = targetGameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureMovement started... Done.");
        }
#endif
#if SUBNAUTICA

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
#endif
}
}