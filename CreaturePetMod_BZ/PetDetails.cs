using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Class for storing custom pet details.
    /// Having this separate from CreaturePet allows us to store instances
    /// in our HashSet and Serialize for save and loading
    /// </summary>
    class PetDetails
    {
        public string PrefabId;
        public string PetName;

        public PetDetails()
        {
            PrefabId = "";
            PetName = "";
        }

        public PetDetails (string prefabId, string petName)
        {
            PrefabId = prefabId;
            PetName = petName;
        }

        /// <summary>
        /// OVerride the HashSet Equals so we don't duplicate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PetDetails q = obj as PetDetails;
            return q != null && q.PrefabId == this.PrefabId && q.PetName == this.PetName;
        }

        /// <summary>
        /// Further supports dedupe of the Hashset
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PrefabId.GetHashCode() ^ this.PetName.GetHashCode();
        }
    }
}
