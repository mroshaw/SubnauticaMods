using Nautilus.Handlers;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{
    internal static class CustomAudioUtils
    {
        private static bool _isFmodReady = false;
        private const string MasterBankAssetName = "PetsBank";
        private const string MasterBankStringAssetName = "PetsBank.strings";

        private static void InitAllFmodBanks()
        {
            if (_isFmodReady)
            {
                return;
            }

            InitFmodBank(MasterBankAssetName);
            InitFmodBank(MasterBankStringAssetName);
        }

        private static void InitFmodBank(string bankName)
        {
            Log.LogDebug($"Loading FMOD Bank from {bankName}...");
            FMOD.Debug.Initialize(FMOD.DEBUG_FLAGS.LOG, FMOD.DEBUG_MODE.TTY, null, null);
            TextAsset bankAsset = CustomAssetBundleUtils.GetObjectFromAssetBundle<TextAsset>(bankName) as TextAsset;
            if (bankAsset == null)
            {
                Log.LogError($"Failed to load {bankName}!");
                return;
            }
            Log.LogDebug($"Loaded {bankName} as {bankAsset.name}");
            FMODUnity.RuntimeManager.LoadBank(bankAsset, true);
            Log.LogDebug($"FMOD Bank Loaded!");
            FMODUnity.RuntimeManager.StudioSystem.getBank($"bank:/bankName", out FMOD.Studio.Bank bank);
            if (!bank.isValid())
            {
                Log.LogError($"The bank {bankName} is not valid!");
            }
            _isFmodReady = true;
        }

        internal static void ConfigureEmitter(FMOD_CustomEmitter emitter, string audioClip, string audioSoundId)
        {
            RegisterSound(audioSoundId, audioClip, Nautilus.Utility.AudioUtils.BusPaths.SurfaceCreatures, 0.1f, 5.0f, 0);
            FMODAsset newAsset = AudioUtils.GetFmodAsset(audioSoundId);
            emitter.SetAsset(newAsset);
        }

        private static void RegisterSound(string id, string clipName, string bus, float minDistance = 10f,
            float maxDistance = 200f, float fadeDuration = 0)
        {
            var sound = Nautilus.Utility.AudioUtils.CreateSound(CustomAssetBundleUtils.GetObjectFromAssetBundle<AudioClip>(clipName) as AudioClip,
                maxDistance >= 0 ? Nautilus.Utility.AudioUtils.StandardSoundModes_3D : Nautilus.Utility.AudioUtils.StandardSoundModes_2D);
            if (maxDistance >= 0)
                sound.set3DMinMaxDistance(minDistance, maxDistance);

            if (fadeDuration > 0)
            {
                sound.AddFadeOut(fadeDuration);
            }

            CustomSoundHandler.RegisterCustomSound(id, sound, bus);
        }

        internal static void GetFMODVersion()
        {

        }
    }
}