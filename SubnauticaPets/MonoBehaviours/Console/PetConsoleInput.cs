using UnityEngine;
using UnityEngine.EventSystems;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Console
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class PetConsoleInput : uGUI_InputGroup, IEventSystemHandler, uGUI_IButtonReceiver, IPointerHoverHandler
    {
        private static string hoverTextKey = "PetConsole";
        private Player player;
        private float terminationSqrDistance = 4.0f;

        private uGUI_NavigableControlGrid panel;
        private RectTransform rt;

        /// <summary>
        /// Unity Awake method.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            terminationSqrDistance = Mathf.Pow(3f, 2f);
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            panel = GetComponent<uGUI_NavigableControlGrid>();
            rt = panel.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Unity Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (focused && player != null && (player.transform.position - rt.position).sqrMagnitude >= terminationSqrDistance)
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
            Log.LogDebug($"PetConsoleInput: OnSelect called with lockMovement: {lockMovement}");
            base.OnSelect(true);
            player = Player.main;
            GamepadInputModule.current.SetCurrentGrid(panel);
        }

        /// <summary>
        /// Implements OnDeselct method
        /// </summary>
        public override void OnDeselect()
        {
            Log.LogDebug($"PetConsoleInput: DeSelect called");

            base.OnDeselect();
            player = null;
        }

        /// <summary>
        /// Implementation of OnButtonHover
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerHover(PointerEventData eventData)
        {
            if (enabled && !selected)
            {
                HandReticle.main.SetText(HandReticle.TextType.Hand, hoverTextKey, true, uGUI.button0);
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
