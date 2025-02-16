using UnityEngine;
using UnityEngine.EventSystems;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Provides functionality for to interact with the Pet Console
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
        public override void OnSelect(bool lockMovement)
        {
            base.OnSelect(true);
            _player = Player.main;
            GamepadInputModule.current.SetCurrentGrid(_panel);
        }

        /// <summary>
        /// Implements OnDeselct method
        /// </summary>
        public override void OnDeselect()
        {
            base.OnDeselect();
            _player = null;
        }

        /// <summary>
        /// Implementation of OnButtonHover
        /// </summary>
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
