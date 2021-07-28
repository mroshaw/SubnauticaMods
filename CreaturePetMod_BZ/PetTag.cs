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
    /// This is effectively a "dummy" component, that allows us to "tag" a creature
    /// as a pet. We can then look for this component in a GameObject to distinguish between
    /// a spawned creature and a spawned pet
    /// </summary>
    class PetTag : MonoBehaviour
    {
        public bool IsPet;
        public string PetName;
        public PetTag ()
        {
           IsPet = true;
        }
    }
}
