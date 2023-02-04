namespace DaftAppleGames.CreaturePetMod_BZ
{
    /// <summary>
    /// Class for storing custom pet details.
    /// Having this separate from CreaturePet allows us to store instances
    /// in our HashSet and Serialize for save and loading
    /// </summary>
    public class PetDetails
    {
        public string PrefabId { get; set; }
        public string PetName { get; set; }
        public bool IsAlive { get; set; }
        public PetCreatureType PetType { get; set; }

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PetDetails()
        {
            PrefabId = "";
            PetName = "";
            IsAlive = true;
            PetType = PetCreatureType.Unknown;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefabId"></param>
        /// <param name="petName"></param>
        /// <param name="petType"></param>
        public PetDetails (string prefabId, string petName, PetCreatureType petType)
        {
            PrefabId = prefabId;
            PetName = petName;
            IsAlive = true;
            PetType = petType;
        }

        /// <summary>
        /// Records the death of a Pet
        /// </summary>
        public void KillPet()
        {
            IsAlive = false;
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
