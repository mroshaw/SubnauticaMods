using System.Collections;
using UnityEngine;
using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace MroshawMods.SeaTruckFishScoopMod
{
    public class FishScoopComponent : MonoBehaviour
    {
        private bool isOn;
        private SeaTruckMotor parentSeaTruck;
        private GameObject[] aquariumGos;

        // Use this for initialization
        void Start()
        {
            parentSeaTruck = GetComponent<SeaTruckMotor>();
            isOn = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Check for "toggle fish scoop" keypress
            if (Input.GetKeyUp(QModHelper.Config.ToggleFishScoop))
            {
                Logger.Log(Logger.Level.Debug, $"Toggle keypress detected");
                // Only toggle when pilotinbg Seatruck
                if (!Player.main.IsPilotingSeatruck())
                {
                    Logger.Log(Logger.Level.Debug, "Toggle: Not piloting. Abort.");
                    return;
                }

                // Toggle scoop
                AquariumsMod.ToggleFishScoop(QModHelper.Config.EnableFishScoop);
            }

            // Check for "purge aquariums" keypress
            if (Input.GetKeyUp(QModHelper.Config.ReleaseFishKey))
            {
                Logger.Log(Logger.Level.Debug, "Purge keypress detected");
                // Only allow when pilotinbg Seatruck
                if (!Player.main.IsPilotingSeatruck())
                {
                    Logger.Log(Logger.Level.Debug, "Purge: Not piloting. Abort.");
                    return;
                }
                Logger.Log(Logger.Level.Debug, "Attempting to purge Aquariums...");
                AquariumsMod.PurgeAllFish();
                Logger.Log(Logger.Level.Debug, "Aquariums purged!");
            }
        }

        private void UpdateAquariums()
        {
            
        }
    }
}