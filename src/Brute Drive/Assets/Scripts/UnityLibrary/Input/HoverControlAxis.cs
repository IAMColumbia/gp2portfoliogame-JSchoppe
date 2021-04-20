using GameLibrary.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityLibrary.InstanceWrappers;

namespace UnityLibrary.Input
{

    public sealed class HoverControlAxis : OnScreenControl
    {
        private void Awake()
        {
            UnityEditorWrapper<TouchControl> controlWrapper =
                gameObject.GetComponent<UnityEditorWrapper<TouchControl>>();
            if (controlWrapper == null)
                UnityEngine.Debug.Log("On Screen Control needs a Hover Control instance.");
            else if (controlWrapper.Instance() is HoverTouchControl hoverControl)
            {
                hoverControl.HoverStateChanged += OnHoverStateChanged;
            }
        }

        private void OnHoverStateChanged(bool isHovered)
        {
            SendValueToControl(isHovered ? 1f : 0f);
        }

        protected override sealed string controlPathInternal
        {
            get => inputControlPath;
            set => inputControlPath = value;
        }

        [InputControl(layout = "Button")]
        [SerializeField] private string inputControlPath = default;
    }
}
