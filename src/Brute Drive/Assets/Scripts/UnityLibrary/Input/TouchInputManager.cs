using System.Collections.Generic;
using System.Linq;
using GameLibrary.Input.Touch;
using UnityEngine;
using UnityLibrary.TickWrappers;
using UnityLibrary.InstanceWrappers;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityLibrary.InstanceWrappers.Input.Touch;

namespace UnityLibrary.Input
{
    /// <summary>
    /// Manages a collection of touches from the Unity Input System.
    /// </summary>
    public sealed class TouchInputManager : MonoBehaviour
    {
        #region Fields
        private List<TouchControl> controls;
        private List<bool> hoveredStates;
        #endregion
        #region Inspector Fields
        [SerializeField] private CanvasNotifierHOTFIX canvasNotifier = default;
        #endregion
        #region Initialization Method
        /// <summary>
        /// Prompts the input manager to retrieve all
        /// touch controls and start monitoring them.
        /// </summary>
        public void Initialize()
        {
            TouchSimulation.Enable();
            EnhancedTouchSupport.Enable();
            // TODO finding controls this way is slow
            // and not deliberate.
            List<TouchControlInstance> wrappers =
                FindObjectsOfType<TouchControlInstance>().ToList();
            // Unwrap all of the editor instances.
            controls = new List<TouchControl>();
            hoveredStates = new List<bool>();
            foreach (UnityEditorWrapper<TouchControl> wrapper in wrappers)
            {
                controls.Add(wrapper.Instance());
                hoveredStates.Add(false);
            }
            UnityTickService.GetProvider(UnityLoopType.Update).Tick += Tick;
            // TODO this is a hotfix.
            canvasNotifier.CanvasChanged += () =>
            {
                foreach (TouchControlInstance control in wrappers)
                    control.OnLayoutChanged();
            };
        }
        #endregion
        #region Tick Implementation
        private void Tick(float deltaTime)
        {
            // Iterate through the registered controls.
            for (int i = 0; i < controls.Count; i++)
            {
                // Test for a hovering touch.
                bool isHovered = false;
                foreach (Finger touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers)
                {
                    if (controls[i].TouchCast(touch.screenPosition))
                    {
                        // If a touch is found, check to either
                        // tick or notify hover began.
                        if (!hoveredStates[i])
                        {
                            hoveredStates[i] = true;
                            if (controls[i] is ITouchHoverListener hoverListener)
                                hoverListener.HoverStarted(touch.screenPosition);
                        }
                        else
                            if (controls[i] is ITouchHoverTickListener tickListener)
                                tickListener.HoverTick(deltaTime, touch.screenPosition);
                        isHovered = true;
                        // Stop testing.
                        break;
                    }
                }
                // If no hits were found, do we need to notify
                // this control?
                if (!isHovered && hoveredStates[i])
                {
                    hoveredStates[i] = false;
                    if (controls[i] is ITouchHoverListener hoverListener)
                        hoverListener.HoverExited();
                }
            }
        }
        #endregion
    }
}
