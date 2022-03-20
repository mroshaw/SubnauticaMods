using ECCLibrary;
using UnityEngine;
using CreaturePetMod_SN;
using Logger = QModManager.Utility.Logger;

public class DypThePenguin : ECCLibrary.CreatureAsset
{
    public string prefabId;
    public DypThePenguin(string classId, string friendlyName, string description, GameObject model, Texture2D spriteTexture) : base(classId, friendlyName, description, model, spriteTexture)
    {
    }
    public override BehaviourType BehaviourType => BehaviourType.Crab;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Near;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(10f, 10f, 10f), 3f, 1f, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.DeadMeat;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        // Add TrailManager
        Logger.Log(Logger.Level.Debug, $"Setting up custom behaviour...");

        // Get a handle on the GameObject to simplify things
        GameObject go = this.GetGameObject();

        // Remove the Creature component
        Logger.Log(Logger.Level.Debug, $"Removing Creature...");
        Creature creature = go.GetComponent<Creature>();
        GameObject.Destroy(creature);

        // Add CreaturePet Component
        Logger.Log(Logger.Level.Debug, $"Adding CreaturePet...");
        CreaturePet creaturePet = go.AddComponent<CreaturePet>();
        creaturePet.SetPetDetails(QMod.Config.PetName.ToString(), GetPrefabId());

        // Remove the old MoveOnSurface component
        Logger.Log(Logger.Level.Debug, $"Getting old MoveOnSurface components...");
        MoveOnSurface moveOnSurfaceOld = go.GetComponent<MoveOnSurface>();
        WalkBehaviour walkBehaviourOld = go.GetComponent<WalkBehaviour>();
        OnSurfaceTracker onSurfaceTrackerOld= go.GetComponent<OnSurfaceTracker>();
        OnSurfaceMovement onSurfaceMovementOld = go.GetComponent<OnSurfaceMovement>();

        Logger.Log(Logger.Level.Debug, $"Destroying old MoveOnSurface components...");
        GameObject.Destroy(onSurfaceMovementOld);
        GameObject.Destroy(onSurfaceTrackerOld);
        GameObject.Destroy(walkBehaviourOld);
        GameObject.Destroy(moveOnSurfaceOld);

        ////////////////////////////////////////////////////
        /// ADD AND CONFIGURE COMPONENTS
        ////////////////////////////////////////////////////

        // Add PetMoveOnSurface component and required additional components and references
        Logger.Log(Logger.Level.Debug, $"Adding PetMoveOnSurface...");
        PetMoveOnSurface petMoveOnSurface = go.AddComponent<PetMoveOnSurface>();
        Logger.Log(Logger.Level.Debug, $"Adding SplineFollowing...");
        SplineFollowing splineFollowing = go.AddComponent<SplineFollowing>();
        Logger.Log(Logger.Level.Debug, $"Adding PetAvoidEdges...");
        PetAvoidEdges petAvoidEdges = go.AddComponent<PetAvoidEdges>();
        Logger.Log(Logger.Level.Debug, $"Adding PetGravity...");
        PetGravity petGravity = go.AddComponent<PetGravity>();
        Logger.Log(Logger.Level.Debug, $"Adding OnSurfaceMovement...");
        OnSurfaceMovement onSurfaceMovement = go.AddComponent<OnSurfaceMovement>();
        Logger.Log(Logger.Level.Debug, $"Adding WalkBehavior...");
        WalkBehaviour walkBehaviour = go.AddComponent<WalkBehaviour>();
        Logger.Log(Logger.Level.Debug, $"Adding OnSurfaceTracker...");
        OnSurfaceTracker onSurfaceTracker = go.AddComponent<OnSurfaceTracker>();
        Logger.Log(Logger.Level.Debug, $"Adding Pet...");
        Pet pet = go.AddComponent<Pet>();

        // Pet
        Logger.Log(Logger.Level.Debug, $"Configuring Pet...");
        pet.onSurfaceTracker = onSurfaceTracker;

        // PetMoveOnSurface
        Logger.Log(Logger.Level.Debug, $"Configuring PetMoveOnSurface...");
        petMoveOnSurface.walkBehaviour = walkBehaviour;
        petMoveOnSurface.onSurfaceTracker = onSurfaceTracker;

        // Walkbehaviour
        Logger.Log(Logger.Level.Debug, $"Configuring WalkBehaviour...");
        walkBehaviour.allowSwimming = false;
        walkBehaviour.onSurfaceMovement = onSurfaceMovement;
        walkBehaviour.onSurfaceTracker = onSurfaceTracker;
        walkBehaviour.splineFollowing = splineFollowing;

        // OnSurfaceMovement
        Logger.Log(Logger.Level.Debug, $"Configuring OnSurfaceMovement...");
        onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;

        // Locomotion
        Logger.Log(Logger.Level.Debug, $"Configuring Locomotion...");
        Locomotion locomotion = go.GetComponent<Locomotion>();
        locomotion.canWalkOnSurface = true;

        // RigidBody
        Logger.Log(Logger.Level.Debug, $"Configuring RigidBody...");
        Rigidbody rigidBody = go.GetComponent<Rigidbody>();
        rigidBody.mass = 20.0f;
   
        // PetAvoidEdges
        Logger.Log(Logger.Level.Debug, $"Configuring PetAvoidEdges...");
        petAvoidEdges.onSurfaceTracker = onSurfaceTracker;
        petAvoidEdges.walkBehaviour = walkBehaviour;
        petAvoidEdges.rigidbody = rigidBody;

        // PetGravity
        Logger.Log(Logger.Level.Debug, $"Configuring PetGravity...");
        petGravity.petRigidBody = rigidBody;
        petGravity.onSurfaceTracker = onSurfaceTracker;
        petGravity.pet = pet;

        ////////////////////////////////////////////////////
        /// REMOVE COMPONENTS
        ////////////////////////////////////////////////////

        // Remove Creature, as we now have Pet
        GameObject.Destroy(creature);
        Logger.Log(Logger.Level.Debug, $"Destroying Creature...");
        
        // Remove SwimRandom
        Logger.Log(Logger.Level.Debug, $"Destroying SwimRandom...");
        SwimRandom swimRandom = go.GetComponent<SwimRandom>();
        GameObject.Destroy(swimRandom);

        // Reset the Creature actions, now that we've set up our components
        Logger.Log(Logger.Level.Debug, $"Resetting Actions...");
        pet.ScanCreatureActions();

        Logger.Log(Logger.Level.Debug, $"Done setting up creature!");
    }

    /// <summary>
    /// Set creature health
    /// </summary>
    /// <param name="liveMixinData"></param>
    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        Logger.Log(Logger.Level.Debug, $"Setting health...");
        liveMixinData.maxHealth = 20f;
    }

    /// <summary>
    /// Updates the PrefabId of the Game Object. Used for loading and saving
    /// </summary>
    private string GetPrefabId()
    {
        return this.GetGameObject().GetComponent<PrefabIdentifier>().Id;
    }
}
