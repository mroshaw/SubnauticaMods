using System;
using System.Collections;
using System.Collections.Generic;
using mset;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace DaftAppleGames.CreaturePetMod_BZ
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetCreatureType { SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, BlueTrivalve, YellowTrivalve, Unknown }
    // SnowstalkerJuvinile

    // Some pet names to choose from
    public enum PetCreatureName { Anise, Beethoven, Bitey, Buddy, Cheerio, Clifford, Denali, Fuzzy, Gandalf, Hera, Jasper, Juju, Kovu, Lassie, Lois, Meera, Mochi, Oscar, Picasso, Ranger, Sampson, Shadow, Sprinkles, Stinky, Tobin, Wendy, Zola }

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

        // Public attributes
        public TechType PetTechType;
        public PetCreatureType PetCreatureType;

        // Useful component refs
        private PetDetails _petDetails;
        private Creature _creature;
        private Animator _animator;
        private MoveOnSurface _moveOnSurface;
        private TrivalvePlayerInteraction _trivalvePlayerInteraction;
        private string _prefabId;


        /// <summary>
        /// Main method to set up a newly spawned pet
        /// </summary>
        /// <param name="petType"></param>
        /// <param name="petName"></param>
        /// <returns></returns>
        public PetDetails ConfigurePet(PetCreatureType petType, string petName)
        {
            // Set main components
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Setting up main components...");
            _creature = GetComponent<Creature>();
            if (!_creature)
            {
                CreaturePetPluginBz.Log.LogError("ConfigurePet: CreaturePet cannot find Creature component!");
                return null;
            }
            _animator = _creature.GetAnimator();
            _moveOnSurface = GetComponent<MoveOnSurface>();
            if (!_moveOnSurface)
            {
                CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Can't find MoveOnFurface component!");
            }

            _trivalvePlayerInteraction = GetComponentInChildren<TrivalvePlayerInteraction>();
            if(!_trivalvePlayerInteraction)
            {
                CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Can't find TrivalvePlayerInteraction component. Probably not a Trivalve.");
            }
            PetTechType = CraftData.GetTechType(gameObject);
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Setting up main components... Done");

            // Set Pet Details
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Setting up PetDetails...");
            _petDetails = new PetDetails
            {
                PetName = petName,
                PetType = petType
            };

            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Get PrefabId");
            PrefabIdentifier prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
            if (prefabIdentifier)
            {
                _prefabId = prefabIdentifier.Id;
                _petDetails.PrefabId = _prefabId;
            }
            else
            {
                CreaturePetPluginBz.Log.LogError("ConfigurePet: Cannot find PrefabIdentifier on GameObject!");
            }
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Setting up PetDetails... Done");

            if (petType == PetCreatureType.Unknown)
            {
                PetCreatureType = GetPetTypeByTechType();
            }
            else
            {
                PetCreatureType = petType;
            }

            // Configure default traits
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Configurng traits...");
            ConfigurePetTraits();
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Configurng traits... Done");

            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Configure PetCreature...");
            ConfigurePetCreature();
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Configure PetCreature... Done");

            // Clean up the left over NavMesh components
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Cleaning up the Mesh...");
            CleanUpMesh();
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Cleaning up the Mesh... Done");

            // Refresh Actions based on amended Component allocation
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Scanning creature actions...");
            _creature.ScanCreatureActions();
            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: Scanning creature actions... Done.");

            CreaturePetPluginBz.Log.LogDebug("ConfigurePet: All done! :)");
            LongJohnSilverCheck();
            return _petDetails;
        }

        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        private void CleanUpMesh()
        {
            CreaturePetPluginBz.Log.LogDebug("... CleanUpMesh: Destroying MoveOnNavMesh");

            // Remove NavMesh components
            MoveOnNavMesh navMeshComp = gameObject.GetComponent<MoveOnNavMesh>();
            if (navMeshComp)
            {
                Destroy(navMeshComp);
            }
            else
            {
                CreaturePetPluginBz.Log.LogError("... CleanUpMesh: Couldn't find MoveOnNavMesh!");
            }

            NavMeshFollowing navMeshFollowComp = gameObject.GetComponent<NavMeshFollowing>();
            CreaturePetPluginBz.Log.LogDebug("... CleanUpMesh: Destroying NavMeshFollowing");
            if (navMeshFollowComp)
            {
                Destroy(navMeshFollowComp);
            }
            else
            {
                CreaturePetPluginBz.Log.LogError("... CleanUpMesh: Couldn't find NavMeshFollowing!");
            }

            // Destroy the NavMesh Agent
            NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            CreaturePetPluginBz.Log.LogDebug("... Destroying NavMeshAgent");
            if(navMeshAgent)
            {
                Destroy(navMeshAgent);

                // Remove NavMesh behaviour
                CreaturePetPluginBz.Log.LogDebug("... Removing NavMesh Behaviour");
                SwimWalkCreatureController swimWalkCreatureController = gameObject.GetComponent<SwimWalkCreatureController>();
                swimWalkCreatureController.walkBehaviours = RemoveBehaviourItem(swimWalkCreatureController.walkBehaviours, typeof(NavMeshAgent));
            }
            else
            {
                CreaturePetPluginBz.Log.LogError("... CleanUpMesh: Couldn't find NavMeshAgent!");
            }
        }

        /// <summary>
        /// Configures the Pet
        /// </summary>
        private void ConfigurePetCreature()
        {
            // Get the CreatureType
            CreaturePetPluginBz.Log.LogDebug( $"... ConfigurePetCreature: Configuring {PetCreatureType}...");

            // Configure base creature
            // Configure base creature behaviours

            // Set the petName for easy debugging
            gameObject.name = $"{PetCreatureType}(Pet)";

            // Prevent Pet from swimming in interiors   
            CreaturePetPluginBz.Log.LogDebug("... ConfigurePetCreature:  LandCreatureGravity...");
            LandCreatureGravity landCreatureGravity = gameObject.GetComponent<LandCreatureGravity>();
            landCreatureGravity.forceLandMode = true;
            landCreatureGravity.enabled = true;

            // Remove the CreatureDeath component, to prevent floating on death
            CreaturePetPluginBz.Log.LogDebug("... ConfigurePetCreature:  CreatureDeath...");
            CreatureDeath creatureDeath = gameObject.GetComponent<CreatureDeath>();
            Destroy(creatureDeath);

            // Configure the Pickupable component
            if (PetCreatureType != PetCreatureType.BlueTrivalve && PetCreatureType != PetCreatureType.YellowTrivalve)
            {
                CreaturePetPluginBz.Log.LogDebug("... ConfigurePetCreature:  Pickupable...");
                Pickupable pickupable = gameObject.GetComponent<Pickupable>();
                if (!pickupable)
                {
                    pickupable = gameObject.AddComponent<Pickupable>();
                }
                pickupable.isPickupable = false;
            }

            // Add creature specific config
            switch (PetCreatureType)
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
                    CreaturePetPluginBz.Log.LogDebug( $"... Invalid Pet Type: {PetCreatureType}");
                    break;
            }
            CreaturePetPluginBz.Log.LogDebug("... ConfigurePetCreature:  Done");
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
            ErrorMessage.AddMessage("Arrr, yer Pet be restin'! Plunder ye a legit copy of the game, if it pleases ye!");
        }

        /// <summary>
        /// Causes the pet to wait for a number of seconds, then walk to the player's position
        /// </summary>
        public void WalkToPlayerWithDelay()
        {
            CreaturePetPluginBz.Log.LogDebug( $"{GetPetType()} is walking to towards player");
            StartCoroutine(WalkToPlayAsync(CreaturePetPluginBz.BeckonDelayConfig.Value));
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
            switch (PetTechType)
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
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring SnowStalkerBaby: {GetPetName()}");

            // Add a SurfaceMovement component, get that little bugger moving around!
            CreaturePetPluginBz.Log.LogDebug( "... Configuring movement components ...");
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
            CreaturePetPluginBz.Log.LogDebug( "... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            CreaturePetPluginBz.Log.LogDebug( "... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = gameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = gameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalkerPet;
            avoidObstaclesOnSurface.creature = snowStalkerPet;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            CreaturePetPluginBz.Log.LogDebug( "... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            CreaturePetPluginBz.Log.LogDebug( "... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = gameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = gameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            CreaturePetPluginBz.Log.LogDebug( "... Configuring SwimRandom and LastTarget... Done");


            // Play first anim
            if (_animator)
            {
                _animator.SetTrigger("dryFur");
            }
            else
            {
                CreaturePetPluginBz.Log.LogDebug( "... Animator not set! WARNING!");
            }
        }

        /// <summary>
        /// Configure Snowstalker Juvenile pet
        /// </summary>
        /// <param petName="petCreatureGO"></param>
        private void ConfigureSnowStalkerJuvenile()
        {
            SnowStalker snowStalkerPet = gameObject.GetComponent<SnowStalker>();
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring SnowStalker Juvenile: {snowStalkerPet.name}...");
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring SnowStalker Juvenile: {snowStalkerPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("dryFur");
        }


        /// <summary>
        /// Configure Pengling Baby specific behaviours
        /// </summary>
        private void ConfigurePenglingBaby()
        {
            PenguinBaby penglingPet = gameObject.GetComponent<PenguinBaby>();
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pengling Baby: {penglingPet.name}...");
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pengling Baby: {penglingPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");

        }

        /// <summary>
        /// Configure Pengling Adult specific behaviours
        /// </summary>
        private void ConfigurePenglingAdult()
        {
            Penguin penglingPet = gameObject.GetComponent<Penguin>();
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pengling: {penglingPet.name}...");
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pengling: {penglingPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");
        }

        /// <summary>
        /// Configure Pengling Adult specific behaviours
        /// </summary>
        private void ConfigurePinnicarid()
        {
            Pinnacarid pinnicaridPet = gameObject.GetComponent<Pinnacarid>();
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pinnicarid: {pinnicaridPet.name}...");
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Pinnicarid: {pinnicaridPet.name}... Done");

            // Play first anim
            _animator.SetTrigger("flutter");
        }

        /// <summary>
        /// Configure Trivalve specific behaviours
        /// </summary>
        private void ConfigureTrivalve()
        {
            Trivalve trivalvePet = gameObject.GetComponent<Trivalve>();
            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Trivalve: {trivalvePet.name}...");

            // Force parenting to base section
            GameObject baseGameObject = FindObjectOfType<Base>().gameObject;
            if (baseGameObject)
            {
                Sky baseSky = baseGameObject.GetComponent<Sky>();
                SkyApplier skyApplier = gameObject.GetComponent<SkyApplier>();
                if (skyApplier)
                {
                    CreaturePetPluginBz.Log.LogDebug($"... Configuring Trivalve: Setting Base SkyApplier..");
                    skyApplier.SetCustomSky(baseSky);
                }
            }

            CreaturePetPluginBz.Log.LogDebug( $"... Configuring Trivalve: {trivalvePet.name}... Done");

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
            CreaturePetPluginBz.PetDetailsHashSet.Remove(_petDetails);
            // Stop floating away!
            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                CreaturePetPluginBz.Log.LogError("Dead: Preventing floating away...");
                rigidBody.mass = 10.0f;
            }
            else
            {
                CreaturePetPluginBz.Log.LogError("Dead: Couldn't find RigidBody!");
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
            CreaturePetPluginBz.Log.LogDebug( $"Killing {creature.GetType()} ({GetPetName()})");
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
            switch (PetCreatureType)
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
                    CreaturePetPluginBz.Log.LogDebug( $"Random animation: Invalid Tech Type. {PetTechType.ToString()}");
                    return;
            }

            _animator.SetTrigger(petAnimation);
            CreaturePetPluginBz.Log.LogDebug( $"{GetPetName()}: Random animation: {petAnimation}");
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
            CreaturePetPluginBz.Log.LogDebug( "GameObject destroyed.");
            Destroy(gameObject);
        }

        /// <summary>
        /// Remove the given behaviour from the behavior array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="typeToRemove"></param>
        /// <returns></returns>
        private static Behaviour[] RemoveBehaviourItem(Behaviour[] array, Type typeToRemove)
        {
            CreaturePetPluginBz.Log.LogDebug( $"... Removing behaviour: {typeToRemove}");
            List<Behaviour> behaviourList = new List<Behaviour>(array);
            Behaviour behaviorToRemove = behaviourList.Find(x => x.GetType() == typeToRemove);
            behaviourList.Remove(behaviorToRemove);
            CreaturePetPluginBz.Log.LogDebug( $"... Behaviour removed: {typeToRemove}");
            return behaviourList.ToArray();
        }

        public void AddConstructable()
        {
            CreaturePetPluginBz.Log.LogDebug( "Adding Constructable...");
            Constructable constructable = gameObject.AddComponent<Constructable>();
            CreaturePetPluginBz.Log.LogDebug( "Adding Constructable... Done.");
            CreaturePetPluginBz.Log.LogDebug( "Configuring Constructable...");
            // constructable.
            CreaturePetPluginBz.Log.LogDebug( "Configuring Constructable... Done.");
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
                // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: State is not None!");
                return false;
            }
            if (PlayerCinematicController.cinematicModeCount > 0)
            {
                // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Player cinematic count is not 0!");
                return false;
            }
            if (!_trivalvePlayerInteraction.liveMixin.IsAlive())
            {
                // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivalve is not alive!");
                return false;
            }
            Player localPlayerComp = Utils.GetLocalPlayerComp();
            if (localPlayerComp == null)
            {
                // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Can't find LocalPlayerComp!");
                return false;
            }
            if (swimWalkState == SwimWalkCreatureController.State.Swim)
            {
                if (!localPlayerComp.IsSwimming())
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivale is swimming and Player is not!!");
                    return false;
                }
            }
            else
            {
                if (!_trivalvePlayerInteraction.trivalve.onSurfaceTracker.onSurface)
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivalve not on surface!");
                    return false;
                }

                // Commenting this out, as it's an element of the original code, but always returns false. I'm assuming because IsGrounded
                // doesn't take into account being in a base.
                /*
                if (localPlayerComp.motorMode != Player.MotorMode.Run || !localPlayerComp.groundMotor.IsGrounded())
                {
                    CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Player is not grounded or player is running!");
                    return false;
                }
                */
            }
            return true;
        }
    }
}
