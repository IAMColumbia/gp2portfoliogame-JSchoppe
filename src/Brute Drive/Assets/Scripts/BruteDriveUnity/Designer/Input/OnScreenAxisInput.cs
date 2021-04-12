using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

namespace BruteDriveUnity.Designer.Input
{
    /// <summary>
    /// A stick control displayed on screen and moved around by touch or other pointer
    /// input.
    /// </summary>
    [AddComponentMenu("Input/On-Screen Axis")]
    public sealed class OnScreenAxisInput : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            startPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Get the current position snapped to the drag axis.
            Vector2 position = Vector3.Project(
                eventData.position - startPosition, dragAxis);



            float input = Mathf.Min(Vector2.Distance(position, startPosition), dragRange);

            if ((position.normalized + dragAxis).sqrMagnitude < 1f)
                input *= -1f;
            

            SendValueToControl(input);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Reset the control.
            SendValueToControl(0f);
        }

        private void Awake()
        {
            // Save time by normalizing once in initialization.
            dragAxis = dragAxis.normalized;
        }

        [FormerlySerializedAs("movementRange")]
        [SerializeField]
        private float dragRange = 50;

        [SerializeField] private Vector2 dragAxis = Vector2.right;

        [InputControl(layout = "Axis")]
        [SerializeField] private string inputControlPath = default;

        private Vector2 startPosition;

        protected override sealed string controlPathInternal
        {
            get => inputControlPath;
            set => inputControlPath = value;
        }
    }
}
