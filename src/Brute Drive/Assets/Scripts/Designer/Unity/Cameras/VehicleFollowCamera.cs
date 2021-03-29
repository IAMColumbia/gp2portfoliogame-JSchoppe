using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BruteDrive.Vehicles;
using BruteDrive.Designer.Unity.Vehicles;

namespace BruteDrive.Designer.Unity.Cameras
{
    public sealed class VehicleFollowCamera : MonoBehaviour
    {
        [Header("Target Reference")]
        [Tooltip("The vehicle that this camera will follow.")]
        [SerializeField] private VehicleInstance vehicleInstance = default;
        [Header("Camera Parameters")]
        [Tooltip("The elevation to point the camera at the vehicle location.")]
        [SerializeField] private float lookElevation = 6f;
        [Tooltip("The required speed before the camera will react.")]
        [SerializeField] private float speedThreshold = 0.5f;
        [Tooltip("The speed at which the camera will rotate when the vehicle is turning.")]
        [SerializeField] private float degreesPerSecond = 180f;
        [Header("Camera Parameters - Min Speed")]
        [Tooltip("The distance from the vehicle at minimum speed zero.")]
        [SerializeField] private float minBoomDistance = 8f;
        [Tooltip("The height from the vehicle at minimum speed zero.")]
        [SerializeField] private float minBoomHeight = 6f;
        [Header("Camera Parameters - Max Speed")]
        [Tooltip("The distance from the vehicle at the maximum vehicle speed.")]
        [SerializeField] private float maxBoomDistance = 12f;
        [Tooltip("The height from the vehicle at the maximum vehicle speed.")]
        [SerializeField] private float maxBoomHeight = 8f;

        public Vehicle Vehicle { get; set; }

        private float rotationDegrees;

        private void Awake()
        {
#if DEBUG
            // Warn designer of bad data.
            if (vehicleInstance == null)
                Debug.LogError("Camera needs a vehicle to follow!", this);
#endif
            rotationDegrees = 180f;
            // Retrieve the underlying vehicle instance.
            Vehicle = vehicleInstance.Instance();
        }

        private void Update()
        {
            UpdateRotationDegrees();

            Vector3 target = new Vector3(Vehicle.Position.x, 0f, Vehicle.Position.y);

            float boomInterpolant = Mathf.Clamp01(
                Mathf.InverseLerp(
                    speedThreshold,
                    Vehicle.ForwardsMaxSpeed,
                    Vehicle.Speed));

            float boomDistance = Mathf.Lerp(minBoomDistance, maxBoomDistance, boomInterpolant);

            transform.position = target + new Vector3(
                Mathf.Sin((Vehicle.Angle + rotationDegrees) * Mathf.Deg2Rad) * boomDistance,
                Mathf.Lerp(minBoomHeight, maxBoomHeight, boomInterpolant),
                Mathf.Cos((Vehicle.Angle + rotationDegrees) * Mathf.Deg2Rad) * boomDistance);

            transform.LookAt(target + Vector3.up * lookElevation);
        }

        private void UpdateRotationDegrees()
        {
            if (Vehicle.Speed < -speedThreshold)
                rotationDegrees = Mathf.Max(0f,
                    rotationDegrees - Time.deltaTime * degreesPerSecond);
            else
                rotationDegrees = Mathf.Min(180f,
                    rotationDegrees + Time.deltaTime * degreesPerSecond);
        }
    }
}
