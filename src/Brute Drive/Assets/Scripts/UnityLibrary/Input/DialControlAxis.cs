using GameLibrary.Input.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityLibrary.InstanceWrappers;

namespace UnityLibrary.Input
{
    public sealed class DialControlAxis : OnScreenControl
    {
        private void Awake()
        {
            UnityEditorWrapper<TouchControl> controlWrapper =
                gameObject.GetComponent<UnityEditorWrapper<TouchControl>>();
            if (controlWrapper == null)
                UnityEngine.Debug.Log("On Screen Control needs a Dial Hover Control instance.");
            else if (controlWrapper.Instance() is DialHoverTouchControl dialControl)
            {
                dialControl.DialAngleChanged += OnDialAngleChanged;
            }
        }

        private void OnDialAngleChanged(float newAngle)
        {
            if (newAngle > 180f)
                newAngle -= 360f;
            SendValueToControl(Mathf.InverseLerp(-90f, 90f, newAngle) * 2f - 1f);
        }

        protected override sealed string controlPathInternal
        {
            get => inputControlPath;
            set => inputControlPath = value;
        }

        [InputControl(layout = "Axis")]
        [SerializeField] private string inputControlPath = default;
    }
}
