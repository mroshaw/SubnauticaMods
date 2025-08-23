using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class PetAnimator : MonoBehaviour
    {
        private Animator _animator;
        private Pet _pet;
        private bool _inRandomAnim = false;

        private static readonly int IsMovingAnimParameter = Animator.StringToHash("IsMoving");
        private static readonly string[] BodyAnims = { "Sit", "Spin", "Roll", "Flinch" };
        private static readonly string[] FaceAnims =  { "Eyes_Annoyed",
                                                "Eyes_Blink",
                                                "Eyes_Cry",
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

        private int[] _bodyAnimHashKeys;
        private int[] _faceAnimHashKeys;

        private int _numBodyAnims;
        private int _numFaceAnims;

        private static readonly int DeadAnim = Animator.StringToHash("Dead");

        private static readonly int EyesDead = Animator.StringToHash("Eyes_Dead");
        private static readonly int EyesSleep= Animator.StringToHash("Eyes_Sleep");
        private static readonly int EyesEcstatic = Animator.StringToHash("Eyes_Excited");
        private static readonly int EyesHappy= Animator.StringToHash("Eyes_Happy");
        private static readonly int EyesSad = Animator.StringToHash("Eyes_Sad");
        private static readonly int EyesDevastated = Animator.StringToHash("Eyes_Trauma");
        private static readonly int EyesNeutral = Animator.StringToHash("Eyes_LookOut");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _pet = GetComponent<Pet>();

            _numBodyAnims = BodyAnims.Length;
            _numFaceAnims = FaceAnims.Length;

            _bodyAnimHashKeys = new int[_numBodyAnims];
            _faceAnimHashKeys = new int[_numFaceAnims];

            for (int currAnim = 0; currAnim < _numBodyAnims; currAnim++)
            {
                _bodyAnimHashKeys[currAnim] = Animator.StringToHash(BodyAnims[currAnim]);
            }

            for (int currAnim = 0; currAnim < _numFaceAnims; currAnim++)
            {
                _faceAnimHashKeys[currAnim] = Animator.StringToHash(FaceAnims[currAnim]);
            }
        }

        private void Update()
        {
            if (!_pet)
            {
                return;
            }

            if (_pet.IsDead)
            {
                _animator.SetBool(DeadAnim, true);
                return;
            }
            if (!_inRandomAnim)
            {
                SetFaceAnimState();
            }
        }

        public void PlayRandomBodyAnim(bool playSound)
        {
            if(!_animator || _bodyAnimHashKeys == null || _bodyAnimHashKeys.Length == 0)
            {
                return;
            }
            int animIndex = Random.Range(0, _numBodyAnims);

            if (_pet && playSound)
            {
                _pet.PlaySound();
            }
            
            LogUtils.LogDebug(LogArea.MonoPets, $"{gameObject.name} is playing random body anim at index: {animIndex} ({BodyAnims[animIndex]})");
            _animator.SetTrigger(_bodyAnimHashKeys[animIndex]);
        }

        public void PlayRandomFaceAnimForDuration(float duration)
        {
            StartCoroutine(PlayRandomFaceAnimAsync(duration));
        }

        public void PlayRandomFaceAnim()
        {
            if(!_animator || _faceAnimHashKeys == null || _faceAnimHashKeys.Length == 0)
            {
                return;
            }
            int animIndex = Random.Range(0, _numFaceAnims);
            LogUtils.LogDebug(LogArea.MonoPets, $"Playing random face anim at index: {animIndex}");
            _animator.Play(_faceAnimHashKeys[animIndex]);
        }

        private IEnumerator PlayRandomFaceAnimAsync(float duration)
        {
            _inRandomAnim = true;
            PlayRandomFaceAnim();
            yield return new WaitForSeconds(duration);
            _inRandomAnim = false;
        }

        private void SetFaceAnimState()
        {
            switch (_pet.Happiness)
            {
                case PetHappiness.Ecstatic:
                    _animator.Play(EyesEcstatic);
                    break;
                case PetHappiness.Happy:
                    _animator.Play(EyesHappy);
                    break;
                case PetHappiness.Neutral:
                    _animator.Play(EyesHappy);
                    break;
                case PetHappiness.Sad:
                    _animator.Play(EyesSad);
                    break;
                case PetHappiness.Devastated:
                    _animator.Play(EyesHappy);
                    break;
                case PetHappiness.Dead:
                    _animator.Play(EyesHappy);
                    break;
            }
        }
        
        /// <summary>
        /// Controls movement animation
        /// </summary>
        public void SetMoving(bool isMoving)
        {
            _animator.SetBool(IsMovingAnimParameter, isMoving);
        }
    }
}