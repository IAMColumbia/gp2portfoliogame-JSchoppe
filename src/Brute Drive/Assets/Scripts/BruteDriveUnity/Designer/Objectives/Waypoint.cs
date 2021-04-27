using GameLibrary.Math;
using BruteDriveCore.Objectives;
// TODO find a better way to deal with
// the naming conflict from the core library.
using U = UnityEngine;
using UnityLibrary.TopDown2D;

namespace BruteDriveUnity.Designer.Objectives
{
    public sealed class Waypoint : U.MonoBehaviour, IWaypoint
    {
        public Vector2 Location => transform.position.TopDownFlatten();

        [U.SerializeField] private float radius = 1f;
        [U.SerializeField] private U.Renderer[] waypointRenderers = default;

        [U.SerializeField] private float rotationSpeed = 1f;

        public float Radius
        {
            get => radius;
            set => radius = FloatMath.Max(0f, radius);
        }
        public bool IsRendered
        {
            set
            {
                foreach (U.Renderer renderer in waypointRenderers)
                    renderer.enabled = value;
            }
        }
        public Vector2 Direction
        {
            get => transform.forward.TopDownFlatten();
            set => transform.forward = ((U.Vector2)value).TopDownUnflatten();
        }

        private void Update()
        {
            // Rotate the ring.
            transform.Rotate(U.Vector3.forward, U.Time.deltaTime * rotationSpeed);
        }
    }
}
