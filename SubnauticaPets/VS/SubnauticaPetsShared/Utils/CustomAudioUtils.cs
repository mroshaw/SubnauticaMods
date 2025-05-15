using FMOD;
using Nautilus.Handlers;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static wrapper methods around the FMOD classes
    /// </summary>
    internal static class CustomAudioUtils
    {
        internal static void ConfigureEmitter(FMOD_CustomEmitter emitter, string audioClipName, string busPath, float volume)
        {
            RegisterSound(audioClipName, busPath, 0.1f, 15.0f, 0);
            FMODAsset newAsset = AudioUtils.GetFmodAsset(audioClipName);
            emitter.SetAsset(newAsset);
            SetEmitterVolume(emitter, volume);
        }

        private static void RegisterSound(string clipName, string bus, float minDistance = 10f,
            float maxDistance = 200f, float fadeDuration = 0)
        {
            var sound = AudioUtils.CreateSound(CustomAssetBundleUtils.GetObjectFromAssetBundle<AudioClip>(clipName) as AudioClip,
                maxDistance >= 0 ? AudioUtils.StandardSoundModes_3D : AudioUtils.StandardSoundModes_2D);
            if (maxDistance >= 0)
                sound.set3DMinMaxDistance(minDistance, maxDistance);

            if (fadeDuration > 0)
            {
                sound.AddFadeOut(fadeDuration);
            }
            CustomSoundHandler.RegisterCustomSound(clipName, sound, bus);
        }

        private static void SetEmitterVolume(FMOD_CustomEmitter emitter, float volume)
        {
            if (!emitter.evt.hasHandle())
            {
                emitter.CacheEventInstance();
            }

            if (!emitter.evt.hasHandle())
            {
                Log.LogDebug($"FMOD Emitter has no handle!");
                return;
            }

            RESULT result = emitter.evt.getVolume(out float currentVolume, out float finalVolume);
            Log.LogDebug($"Result of GetVolume is: {result.ToString()}");
            Log.LogDebug($"Emitter current volume is: {currentVolume}, final volume is: {finalVolume}. Setting volume to: {volume}");
            result = emitter.evt.setVolume(volume);
            Log.LogDebug($"Result of SetVolume is: {result.ToString()}");
            emitter.evt.getVolume(out currentVolume, out finalVolume);
            Log.LogDebug($"Emitter new volume is: {currentVolume}, final volume is: {finalVolume}");

        }
    }
}