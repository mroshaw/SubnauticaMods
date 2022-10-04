using MroshawMods.Checks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Logger = QModManager.Utility.Logger;
using Random = System.Random;

namespace MroshawMods.CreaturePetMod_BZ
{
    /// <summary>
    /// This Component allows us to "tag" a creature as a Pet
    /// We can then look for this component in a GameObject to distinguish between
    /// a spawned creature and a spawned pet.
    /// We can also use this for future functionality and attributes.
    /// </summary>
    internal class CreaturePet : MonoBehaviour
    {
        // Selection of animations on petting
        //  { "peck", "flutter" };
        //  { "shudder", "call", "trip" };
        //  { "chilling", "standUpSniff", "standUpHowl", "dryFur" };
        private static readonly string[] PenglingAdultAnims = { "peck", "flutter", "bite" };
        private static readonly string[] PenglingBabyAnims = { "shudder", "call", "trip", "bite", "fidget", "flinch" };
        private static readonly string[] SnowStalkerJuvenileAnims = { "dryFur", "roar", "bite", "flinch", "fidget", "standUpSniff", "standUpHowl", "threaten" };
        private static readonly string[] SnowStalkerBabyAnims = { "dryFur", "growl", "bite", "roar", "fidget", "standUpSniff", "standUpHowl" };
        private static readonly string[] PinnacaridAnims = { "flutter", "beg", "bite", "fidget", "flinch", "call" };
        private static readonly string[] TrivalveAnims = { };

        private PetDetails _petDetails;
         private Creature _creature;
        private Animator _animator;
        public TechType petTechType;
        public PetCreatureType petCreatureType;
        private MoveOnSurface _moveOnSurface;
        private TrivalvePlayerInteraction _trivalvePlayerInteraction;
        private string _prefabId;

        public PetDetails ConfigurePet(PetCreatureType petType, string name)
        {
            // Set main components
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Setting up main components...");
            _creature = GetComponent<Creature>();
            if (!_creature)
            {
                Logger.Log(Logger.Level.Error, "ConfigurePet: CreaturePet cannot find Creature component!");
                return null;
            }
            _animator = _creature.GetAnimator();
            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                Logger.Log(Logger.Level.Debug, "ConfigurePet: Can't find MoveOnFurface component!");
            }

            _trivalvePlayerInteraction = GetComponentInChildren<TrivalvePlayerInteraction>();
            if(!_trivalvePlayerInteraction)
            {
                Logger.Log(Logger.Level.Debug, "ConfigurePet: Can't find TrivalvePlayerInteraction component. Probably not a Trivalve.");
            }
            petTechType = CraftData.GetTechType(gameObject);
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Setting up main components... Done");

            // Set Pet Details
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Setting up PetDetails...");
            _petDetails = new PetDetails();
            _petDetails.PetName = name;
            _petDetails.PetType = petType;

            Logger.Log(Logger.Level.Debug, "ConfigurePet: Get PrefabId");
            PrefabIdentifier prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
            if (prefabIdentifier)
            {
                _prefabId = prefabIdentifier.Id;
                _petDetails.PrefabId = _prefabId;
            }
            else
            {
                Logger.Log(Logger.Level.Error, "ConfigurePet: Cannot find PrefabIdentifier on GameObject!");
            }
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Setting up PetDetails... Done");

            if (petType == PetCreatureType.Unknown)
            {
                petCreatureType = GetPetTypeByTechType();
            }
            else
            {
                petCreatureType = petType;
            }

            // Configure default traits
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Configurng traits...");
            ConfigurePetTraits();
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Configurng traits... Done");

            Logger.Log(Logger.Level.Debug, "ConfigurePet: Configure PetCreature...");
            ConfigurePetCreature();
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Configure PetCreature... Done");

            // Clean up the left over NavMesh components
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Cleaning up the Mesh...");
            CleanUpMesh();
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Cleaning up the Mesh... Done");

            // Refresh Actions based on amended Component allocation
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Scanning creature actions...");
            _creature.ScanCreatureActions();
            Logger.Log(Logger.Level.Debug, "ConfigurePet: Scanning creature actions... Done.");

            Logger.Log(Logger.Level.Debug, "ConfigurePet: All done! :)");
            LongJohnSilverCheck();
            return _petDetails;
        }

        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        private void CleanUpMesh()
        {
            // Remove NavMesh components
            MoveOnNavMesh navMeshComp = gameObject.GetComponent<MoveOnNavMesh>();
            Logger.Log(Logger.Level.Debug, "... CleanUpMesh: Destroying MoveOnNavMesh");
            if (navMeshComp)
            {
                Destroy(navMeshComp);
            }
            else
            {
                Logger.Log(Logger.Level.Error, "... CleanUpMesh: Couldn't find MoveOnNavMesh!");
            }

            NavMeshFollowing navMeshFollowComp = gameObject.GetComponent<NavMeshFollowing>();
            Logger.Log(Logger.Level.Debug, "... CleanUpMesh: Destroying NavMeshFollowing");
            if (navMeshFollowComp)
            {
                Destroy(navMeshFollowComp);
            }
            else
            {
                Logger.Log(Logger.Level.Error, "... CleanUpMesh: Couldn't find NavMeshFollowing!");
            }

            // Destroy the NavMesh Agent
            NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            Logger.Log(Logger.Level.Debug, "... Destroying NavMeshAgent");
            if(navMeshAgent)
            {
                Destroy(navMeshAgent);

                // Remove NavMesh behaviour
                Logger.Log(Logger.Level.Debug, "... Removing NavMesh Behaviour");
                SwimWalkCreatureController swimWalkCreatureController = gameObject.GetComponent<SwimWalkCreatureController>();
                swimWalkCreatureController.walkBehaviours = RemoveBehaviourItem(swimWalkCreatureController.walkBehaviours, typeof(UnityEngine.AI.NavMeshAgent));
            }
            else
            {
                Logger.Log(Logger.Level.Error, "... CleanUpMesh: Couldn't find NavMeshAgent!");
            }
        }

        /// <summary>
        /// Configures the Pet
        /// </summary>
        private void ConfigurePetCreature()
        {
            // Get the CreatureType
            Logger.Log(Logger.Level.Debug, $"... ConfigurePetCreature: Configuring {petCreatureType}...");

            // Configure base creature
            // Configure base creature behaviours

            // Set the name for easy debugging
            gameObject.name = $"{petCreatureType}(Pet)";

            // Prevent Pet from swimming in interiors   
            Logger.Log(Logger.Level.Debug, "... ConfigurePetCreature:  LandCreatureGravity...");
            LandCreatureGravity landCreatureGravity = gameObject.GetComponent<LandCreatureGravity>();
            landCreatureGravity.forceLandMode = true;
            landCreatureGravity.enabled = true;

            // Remove the CreatureDeath component, to prevent floating on death
            Logger.Log(Logger.Level.Debug, "... ConfigurePetCreature:  CreatureDeath...");
            CreatureDeath creatureDeath = gameObject.GetComponent<CreatureDeath>();
            Destroy(creatureDeath);

            // Configure the Pickupable component
            if (petCreatureType != PetCreatureType.BlueTrivalve && petCreatureType != PetCreatureType.YellowTrivalve)
            {
                Logger.Log(Logger.Level.Debug, "... ConfigurePetCreature:  Pickupable...");
                Pickupable pickupable = gameObject.GetComponent<Pickupable>();
                if (!pickupable)
                {
                    pickupable = gameObject.AddComponent<Pickupable>();
                }
                pickupable.isPickupable = false;
            }

            // Add creature specific config
            switch (petCreatureType)
            {
                case PetCreatureType.SnowstalkerBaby:
                    ConfigureSnowStalkerBaby();
                    break;
             //   case PetCreatureType.SnowstalkerJuvinile:
             //       ConfigureSnowStalkerJuvenile();
             //       break;
                case PetCreatureType.PenglingBaby:
                    ConfigurePenglingBaby();
                    break;

                case PetCreatureType.PenglingAdult:
                    ConfigurePenglingAdult();
                    break;
                case PetCreatureType.Pinnicarid:
                    ConfigurePinnicarid();
                    break;
                case PetCreatureType.BlueTrivalve:
                case PetCreatureType.YellowTrivalve:
                    ConfigureTrivalve();
                    break;

                default:
                    Logger.Log(Logger.Level.Debug, $"... Invalid Pet Type: {petCreatureType}");
                    break;
            }
            Logger.Log(Logger.Level.Debug, "... ConfigurePetCreature:  Done");
        }

        /// <summary>
        /// Check if player has been a naughty boy... SQWAAAK!
        /// </summary>
        private void LongJohnSilverCheck()
        {
            if(PirateCheck.IsPirate())
            {
                StartCoroutine(SetPetResting(5.0f));
            }
        }

        /// <summary>
        /// Hes not dead, he's resting!
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator SetPetResting(float delay)
        {
            yield return new WaitForSeconds(delay);
            Kill();
            ErrorMessage.AddMessage($"Arrr, yer Pet be restin'! Plunder ye a legit copy of the game, if it pleases ye!");
        }

        /// <summary>
        /// Causes the pet to wait for a number of seconds, then walk to the player's position
        /// </summary>
        public void WalkToPlayerWithDelay()
        {
            Logger.Log(Logger.Level.Debug, $"{GetPetType()} is walking to towards player");
            StartCoroutine(WalkToPlayAsync(QMod.Config.beckonDelay));
        }

        /// <summary>
        /// Async method to walk pet to players position after given delay
        /// </summary>
        /// <param name="timeToWait"></param>
        /// <returns></returns>
        private IEnumerator WalkToPlayAsync(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            _moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position, (Player.main.transform.position - transform.position).normalized, _moveOnSurface.moveVelocity);
        }

        /// <summary>
        /// Get the PetCreatureType by TechType. Needed if we don't have the PetType
        /// available.
        /// </summary>
        /// <returns></returns>
        private PetCreatureType GetPetTypeByTechType()
        {
            switch (petTechType)
            {
                case TechType.SnowStalkerBaby:
                    return PetCreatureType.SnowstalkerBaby;
                case TechType.Penguin:
                    return PetCreatureType.PenglingAdult;
                case TechType.PenguinBaby:
                    return PetCreatureType.PenglingBaby;
                case TechType.Pinnacarid:
                    return PetCreatureType.Pinnicarid;
             //   case TechType.SnowStalker:
             //       return PetCreatureType.SnowstalkerJuvinile;
                default:
                    return PetCreatureType.Unknown;
            }
        }

        /// <summary>
        /// Configure a SnowStalkerBaby pet
        /// </summary>
        private void ConfigureSnowStalkerBaby()
        {
            SnowStalkerBaby snowStalkerPet = GetComponent<SnowStalkerBaby>();
            Logger.Log(Logger.Level.Debug, $"... Configuring SnowStalkerBaby: {GetPetName()}");

            // Add a SurfaceMovement component, get that little bugger moving around!
            Logger.Log(Logger.Level.Debug, $"... Configuring movement components ...");
            OnSurfaceTracker onSurfaceTracker = gameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = gameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = gameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = gameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = gameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalkerPet.onSurfaceMovement = onSurfaceMovement;
            Logger.Log(Logger.Level.Debug, $"... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            Logger.Log(Logger.Level.Debug, $"... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = gameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = gameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalkerPet;
            avoidObstaclesOnSurface.creature = snowStalkerPet;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            Logger.Log(Logger.Level.Debug, $"... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            Logger.Log(Logger.Level.Debug, $"... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = gameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = gameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            Logger.Log(Logger.Level.Debug, $"... Configuring SwimRandom and LastTarget... Done");


            // Play first anim
            if (_animator)
            {
                _animator.SetTrigger("dryFur");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, $"... Animator not set! WARNING!");
            }
        }

        /// <summary>
        /// Configure Snowstalker Juvenile pet
        /// </summary>
        /// <param name="petCreatureGO"></param>
        private void ConfigureSnowStalkerJuvenile()
        {
            SnowStalker snowStalkerPet = gameObject.GetComponent<SnowStalker>();
            Logger.Log(Logger.Level.Debug, $"... Configuring SnowStalker Juvenile: {snowStalkerPet.name}...");
            Logger.Log(Logger.Level.Debug, $"... Configuring SnowStalker Juvenile: {snowStalkerPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("dryFur");
        }


        /// <summary>
        /// Configure Pengling Baby specific behaviours
        /// </summary>
        private void ConfigurePenglingBaby()
        {
            PenguinBaby penglingPet = gameObject.GetComponent<PenguinBaby>();
            Logger.Log(Logger.Level.Debug, $"... Configuring Pengling Baby: {penglingPet.name}...");
            Logger.Log(Logger.Level.Debug, $"... Configuring Pengling Baby: {penglingPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");

        }

        /// <summary>
        /// Configure Pengling Adult specific behaviours
        /// </summary>
        private void ConfigurePenglingAdult()
        {
            Penguin penglingPet = gameObject.GetComponent<Penguin>();
            Logger.Log(Logger.Level.Debug, $"... Configuring Pengling: {penglingPet.name}...");
            Logger.Log(Logger.Level.Debug, $"... Configuring Pengling: {penglingPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");
        }

        /// <summary>
        /// Configure Pengling Adult specific behaviours
        /// </summary>
        private void ConfigurePinnicarid()
        {
            Pinnacarid pinnicaridPet = gameObject.GetComponent<Pinnacarid>();
            Logger.Log(Logger.Level.Debug, $"... Configuring Pinnicarid: {pinnicaridPet.name}...");
            Logger.Log(Logger.Level.Debug, $"... Configuring Pinnicarid: {pinnicaridPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");
        }

        /// <summary>
        /// Configure Trivalve specific behaviours
        /// </summary>
        private void ConfigureTrivalve()
        {
            Trivalve trivalvePet = gameObject.GetComponent<Trivalve>();
            Logger.Log(Logger.Level.Debug, $"... Configuring Trivalve: {trivalvePet.name}...");
            Logger.Log(Logger.Level.Debug, $"... Configuring Trivalve: {trivalvePet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");
        }

        /// <summary>
        /// Configure the Pet Details
        /// </summary>
        /// <param name="petName"></param>
        /// <param name="petType"></param>
        public void SetPetDetails(string petName, PetCreatureType petType)
        {
            if (_petDetails == null)
            {
                _petDetails = new PetDetails();
            }
            else
            {
                _petDetails.PetName = petName;
                _petDetails.PetType = petType;
            }
        }


        /// <summary>
        /// Returns the PetDetails PetName
        /// </summary>
        /// <returns></returns>
        public string GetPetName()
        {
            return _petDetails.PetName;
        }

        /// <summary>
        /// Returns the PetDetails Creature Type
        /// </summary>
        /// <returns></returns>
        public PetCreatureType GetPetType()
        {
            return _petDetails.PetType;
        }

        /// <summary>
        /// Returns the PetDetails PrefabId
        /// </summary>
        /// <returns></returns>
        public string GetPetPrefabId()
        {
            return _petDetails.PrefabId;
        }

        /// <summary>
        /// Returns the PetDetails object
        /// </summary>
        /// <returns></returns>
        internal PetDetails GetPetDetailsObject()
        {
            return _petDetails;
        }

        /// <summary>
        /// Returns the PetDetails IsAlive state
        /// </summary>
        /// <returns></returns>
        internal bool IsPetAlive()
        {
            return _petDetails.IsAlive;
        }

        /// <summary>
        /// To be called when the creature dies "naturally"
        /// </summary>
        public void Dead()
        {
            QMod.PetDetailsHashSet.Remove(_petDetails);
            // Stop floating away!
            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                Logger.Log(Logger.Level.Error, "Dead: Preventing floating away...");
                rigidBody.mass = 10.0f;
            }
            else
            {
                Logger.Log(Logger.Level.Error, "Dead: Couldn't find RigidBody!");
            }

            // So long, fuzzball.
            ErrorMessage.AddMessage($"One of your pets has died! Farewell, {_petDetails.PetName}!");
            // Wait 5 seconds then dispose of the body
            StartCoroutine(DestroyAsync(5, gameObject));
        }

        /// <summary>
        /// Kills the pet
        /// </summary>
        public void Kill()
        {
            // Get LiveMixIn component and kill
            Creature creature = GetComponentInParent<Creature>();
            LiveMixin liveMixin = GetComponentInParent<LiveMixin>();

            // Kill the Creature
            Logger.Log(Logger.Level.Debug, $"Killing {creature.GetType()} ({GetPetName()})");
            liveMixin.Kill();
            _petDetails.IsAlive = false;

            // ErrorMessage.AddMessage($"You killed {_petDetails.PetName}!");

            // Wait 5 seconds then dispose of the body
            StartCoroutine(DestroyAsync(5, gameObject));
        }

        /// <summary>
        /// Triggers a random animation on petting a pet
        /// </summary>
        public void PetWithAnimation()
        {
            string petAnimation;
            int index;
            Random random = new Random();

            // Pick a random animation and play it
            switch (petCreatureType)
            {
                case PetCreatureType.PenglingAdult:
                    index = random.Next(PenglingAdultAnims.Length);
                    petAnimation =  PenglingAdultAnims[index];
                    break;
                case PetCreatureType.PenglingBaby:
                    index = random.Next(PenglingBabyAnims.Length);
                    petAnimation =  PenglingBabyAnims[index];
                    break;
                case PetCreatureType.SnowstalkerBaby:
                    index = random.Next(SnowStalkerBabyAnims.Length);
                    petAnimation = SnowStalkerBabyAnims[index];
                    break;
                // case PetCreatureType.SnowstalkerJuvinile:
                //    index = random.Next(SnowStalkerJuvenileAnims.Length);
                //    petAnimation = SnowStalkerJuvenileAnims[index];
                //    break;

                case PetCreatureType.Pinnicarid:
                    index = random.Next(PinnacaridAnims.Length);
                    petAnimation = PinnacaridAnims[index];
                    break;
                case PetCreatureType.Unknown:
                default:
                    Logger.Log(Logger.Level.Debug, $"Random animation: Invalid Tech Type. {petTechType.ToString()}");
                    return;
            }

            _animator.SetTrigger(petAnimation);
            Logger.Log(Logger.Level.Debug, $"{GetPetName()}: Random animation: {petAnimation}");
        }

        /// <summary>
        /// Configure basic Creature component traits
        /// </summary>
        internal void ConfigurePetTraits()
        {
            _creature.Friendliness.Value = 1.0f;
            _creature.Happy.Value = 1.0f;
            _creature.Aggression.Value = 0.0f;
            _creature.Scared.Value = 0.0f;
            _creature.Curiosity.Value = 1.0f;
            _creature.Hunger.Value = 1.0f;
            _creature.Tired.Value = 0.0f;
        }

        /// <summary>
        /// Destory GameObject after number of seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private static IEnumerator DestroyAsync(float seconds, GameObject gameObject)
        {
            yield return new WaitForSeconds(seconds);
            Logger.Log(Logger.Level.Debug, $"GameObject destroyed.");
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// Remove the given behaviour from the behavior array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="typeToRemove"></param>
        /// <returns></returns>
        private static Behaviour[] RemoveBehaviourItem(Behaviour[] array, Type typeToRemove)
        {
            Logger.Log(Logger.Level.Debug, $"... Removing behaviour: {typeToRemove}");
            List<Behaviour> behaviourList = new List<Behaviour>(array);
            Behaviour behaviorToRemove = behaviourList.Find(x => x.GetType() == typeToRemove);
            behaviourList.Remove(behaviorToRemove);
            Logger.Log(Logger.Level.Debug, $"... Behaviour removed: {typeToRemove}");
            return behaviourList.ToArray();
        }

        public void AddConstructable()
        {
            Logger.Log(Logger.Level.Debug, $"Adding Constructable...");
            Constructable constructable = gameObject.AddComponent<Constructable>();
            Logger.Log(Logger.Level.Debug, $"Adding Constructable... Done.");
            Logger.Log(Logger.Level.Debug, $"Configuring Constructable...");
            // constructable.
            Logger.Log(Logger.Level.Debug, $"Configuring Constructable... Done.");
        }

        /// <summary>
        /// Overrides the TrivalvePlayerInteraction "IsAllowedToInteract" method
        /// </summary>
        /// <param name="swimWalkState"></param>
        /// <returns></returns>
        public bool AllowedToInteract(SwimWalkCreatureController.State swimWalkState)
        {
            if (_trivalvePlayerInteraction.state != TrivalvePlayerInteraction.State.None)
            {
                // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: State is not None!");
                return false;
            }
            if (PlayerCinematicController.cinematicModeCount > 0)
            {
                // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Player cinematic count is not 0!");
                return false;
            }
            if (!_trivalvePlayerInteraction.liveMixin.IsAlive())
            {
                // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Trivalve is not alive!");
                return false;
            }
            Player localPlayerComp = global::Utils.GetLocalPlayerComp();
            if (localPlayerComp == null)
            {
                // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Can't find LocalPlayerComp!");
                return false;
            }
            if (swimWalkState == SwimWalkCreatureController.State.Swim)
            {
                if (!localPlayerComp.IsSwimming())
                {
                    // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Trivale is swimming and Player is not!!");
                    return false;
                }
            }
            else
            {
                if (!_trivalvePlayerInteraction.trivalve.onSurfaceTracker.onSurface)
                {
                    // Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Trivalve not on surface!");
                    return false;
                }

                // Commenting this out, as it's an element of the original code, but always returns false. I'm assuming because IsGrounded
                // doesn't take into account being in a base.
                /*
                if (localPlayerComp.motorMode != Player.MotorMode.Run || !localPlayerComp.groundMotor.IsGrounded())
                {
                    Logger.Log(Logger.Level.Debug, $"CreaturePet.AllowedToInteract: Player is not grounded or player is running!");
                    return false;
                }
                */
            }
            return true;
        }
    }
}
