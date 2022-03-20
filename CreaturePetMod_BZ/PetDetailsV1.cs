namespace MroshawMods.CreaturePetMod_BZ
{
    /// <summary>
    /// LEGACY CLASS, USED TO MIGRATE OLD SAVE FILES
    /// Class for storing custom pet details.
    /// Having this separate from CreaturePet allows us to store instances
    /// in our HashSet and Serialize for save and loading
    /// </summary>
    public class PetDetailsV1
    {
        public readonly string PrefabId;
        public readonly string PetName;
        private bool IsAlive;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public PetDetailsV1()
        {
            PrefabId = "";
            PetName = "";
            IsAlive = true;
        }

        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="prefabId"></param>
        /// <param name="petName"></param>
        public PetDetailsV1(string prefabId, string petName)
        {
            PrefabId = prefabId;
            PetName = petName;
            IsAlive = true;
        }

        /// <summary>
        /// Override the HashSet Equals so we don't duplicate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is PetDetails q && q.PrefabId == PrefabId && q.PetName == PetName;
        }

        /// <summary>
        /// Further supports dedupe of the Hashset
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return PrefabId.GetHashCode() ^ PetName.GetHashCode();
        }
    }
}
