using System.Collections;
using UnityEngine;

namespace MroshawMods.SeaTruckFishScoopMod
{
    public class AquariumEnhancedComponent : MonoBehaviour
    {
        private SeaTruckAquarium[] parentSeaTruckAquariums;

        // Use this for initialization
        void Start()
        {
            RefreshAquariums();
        }

        private void RefreshAquariums()
        {
            parentSeaTruckAquariums = GetComponentsInChildren<SeaTruckAquarium>();
        }
    }
}