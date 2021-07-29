using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// This Component allows us to "tag" a creature as a Pet
    /// We can then look for this component in a GameObject to distinguish between
    /// a spawned creature and a spawned pet.
    /// We can also use this for future functionality and attributes.
    /// </summary>
    class CreaturePet : MonoBehaviour
    {
        private PetDetails PetDetails;
        public bool IsPet;

        /// <summary>
        /// creaturePet constructor
        /// </summary>
        public CreaturePet ()
        {
            IsPet = true;
            PetDetails = null;
        }

        public void SetPetDetails(string petName, string prefabId)
        {
            if (PetDetails == null)
            {
                PetDetails = new PetDetails();
            }
            PetDetails.PetName = petName;
            PetDetails.PrefabId = prefabId;
        }

        public string GetPetName()
        {
            return PetDetails.PetName;
        }

        public string GetPetPrefabId()
        {
            return PetDetails.PrefabId;
        }

        public PetDetails GetPetDetailsObject()
        {
            return PetDetails;
        }
    }
}
