namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours
{

    /// <summary>
    /// Class to create a new Pet Fabricator
    /// </summary>
    public class PetWorkbench : Workbench
    {
        public override void Start()
        {
            base.Start();
        }

        public override void OnCraftingEnd()
        {
            // Spawn the pet here. Don't call the base end function
        }
    }
}