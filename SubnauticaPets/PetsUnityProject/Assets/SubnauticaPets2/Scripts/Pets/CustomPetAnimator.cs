using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class CustomPetAnimator : MonoBehaviour
    {
        private Animator _animator;
        private static string[] bodyAnims = { "Sit", "Spin", "Roll", "Flinch" };
        private static string[] faceAnims =  { "Eyes_Annoyed",
                                                "Eyes_Blink",
                                                "Eyes_Cry",
                                                "Eyes_Dead",
                                                "Eyes_Excited",
                                                "Eyes_Happy",
                                                "Eyes_LookDown",
                                                "Eyes_LookIn",
                                                "Eyes_LookOut",
                                                "Eyes_LookUp",
                                                "Eyes_Rabid",
                                                "Eyes_Sad",
                                                "Eyes_Shrink",
                                                "Eyes_Sleep",
                                                "Eyes_Spin",
                                                "Eyes_Squint",
                                                "Eyes_Trauma",
                                                "Sweat_L",
                                                "Sweat_R",
                                                "Teardrop_L",
                                                "Teardrop_R" };

        private int[] bodyAnimHashKeys;
        private int[] faceAnimHashKeys;

        private int _numBodyAnims;
        private int _numFaceAnims;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _numBodyAnims = bodyAnims.Length;
            _numFaceAnims = faceAnims.Length;


            bodyAnimHashKeys = new int[_numBodyAnims];
            faceAnimHashKeys = new int[_numFaceAnims];

            for (int currAnim = 0; currAnim < _numBodyAnims; currAnim++)
            {
                bodyAnimHashKeys[currAnim] = Animator.StringToHash(bodyAnims[currAnim]);
            }

            for (int currAnim = 0; currAnim < _numFaceAnims; currAnim++)
            {
                faceAnimHashKeys[currAnim] = Animator.StringToHash(faceAnims[currAnim]);
            }
        }

        public void PlayRandomBodyAnim()
        {
            int animIndex = Random.Range(0, _numBodyAnims);
            LogUtils.LogDebug(LogArea.MonoPets, $"Playing random body anim at index: {animIndex}");
            _animator.SetTrigger(bodyAnimHashKeys[animIndex]);
        }
        public void PlayRandomFaceAnim()
        {
            int animIndex = Random.Range(0, _numFaceAnims);
            LogUtils.LogDebug(LogArea.MonoPets, $"Playing random face anim at index: {animIndex}");
            _animator.Play(faceAnimHashKeys[animIndex]);
        }
    }
}