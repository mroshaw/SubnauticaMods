using ECCLibrary;
using UnityEngine;

public class Ingenalvus : ECCLibrary.CreatureAsset
{

    public Ingenalvus(string classId, string friendlyName, string description, GameObject model, Texture2D spriteTexture) : base(classId, friendlyName, description, model, spriteTexture)
    {
    }
    public override BehaviourType BehaviourType => BehaviourType.SmallFish;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Near;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(10f, 10f, 10f), 3f, 1f, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.SmallFish;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        // Add TrailManager

    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = 20f;
    }

}
