using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DaftAppleGames.SubnauticaPets.Mono.BaseParts
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class PetConsoleInput : uGUI_InputGroup, IEventSystemHandler, uGUI_IButtonReceiver, IPointerHoverHandler
    {
        private static readonly string HoverTextKey = "PetConsole";
        private Player _player;
        private float _terminationSqrDistance = 4.0f;

        private uGUI_NavigableControlGrid _panel;
        private RectTransform _rt;

        /// <summary>
        /// Unity Awake method.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            _terminationSqrDistance = Mathf.Pow(3f, 2f);
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            _panel = GetComponent<uGUI_NavigableControlGrid>();
            _rt = _panel.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Unity Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (focused && _player != null && (_player.transform.position - _rt.position).sqrMagnitude >= _terminationSqrDistance)
            {
                Deselect();
            }
        }

        /// <summary>
        /// Implements OnSelect method
        /// </summary>
        /// <param name="lockMovement"></param>
        public override void OnSelect(bool lockMovement)
        {
            // base.OnSelect(lockMovement);
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsoleInput: OnSelect called with lockMovement: {lockMovement}");
            base.OnSelect(true);
            _player = Player.main;
            GamepadInputModule.current.SetCurrentGrid(_panel);
        }

        /// <summary>
        /// Implements OnDeselct method
        /// </summary>
        public override void OnDeselect()
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleInput: DeSelect called");

            base.OnDeselect();
            _player = null;
        }

        /// <summary>
        /// Implementation of OnButtonHover
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerHover(PointerEventData eventData)
        {
            if (enabled && !selected)
            {
                HandReticle.main.SetText(HandReticle.TextType.Hand, HoverTextKey, true, uGUI.button0);
                HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
                HandReticle.main.SetIcon(HandReticle.IconType.Interact, 1f);
            }
        }

        /// <summary>
        /// Implementation of OnButtonDown
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool OnButtonDown(GameInput.Button button)
        {
            if (button == uGUI.button1)
            {
                Deselect();
                return true;
            }
            return false;
        }
    }
}
