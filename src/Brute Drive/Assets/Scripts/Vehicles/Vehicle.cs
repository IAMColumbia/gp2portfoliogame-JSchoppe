using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BruteDrive;
using BruteDrive.EngineCore;
using BruteDrive.Utilities.CSharp;

namespace BruteDrive.Vehicles
{
    public class Vehicle : ITickable
    {
        protected readonly ITickProvider tickProvider;

        public Vehicle(ITickProvider tickProvider)
        {
            this.tickProvider = tickProvider;
            tickProvider.Tick += Tick;

            steerAngle = 0f;
            speed = 0f;
            forwards = Vector2.up;
        }

        private float steerDegreesPerSecond;
        public float SteerDegreesPerSecond
        {
            get => steerDegreesPerSecond;
            set
            {
                steerDegreesPerSecond = Mathf.Max(0f, value);
            }
        }

        public float MaxSteerDegrees { get; set; }
        public float ForwardsAcceleration { get; set; }
        public float ReverseAcceleration { get; set; }

        private float forwardsMaxSpeed;
        public float ForwardsMaxSpeed
        {
            get => forwardsMaxSpeed;
            set
            {
                forwardsMaxSpeed = Mathf.Max(0f, value);
            }
        }
        private float reverseMaxSpeed;
        public float ReverseMaxSpeed
        {
            get => reverseMaxSpeed;
            set
            {
                reverseMaxSpeed = Mathf.Max(0f, value);
            }
        }

        private Vector2 forwards;

        private float angle;


        private float speed;

        public float Speed
        {
            get => speed;
            set
            {
                // Limit the speed of the vehicle.
                speed = Mathf.Clamp(
                    value, -ReverseMaxSpeed, ForwardsMaxSpeed);
            }
        }

        public float Angle
        {
            get => angle;
            set
            {
                // Keep the angle between 0-360.
                angle = value.WrappedBetween(0f, 360f);
                forwards = new Vector2(
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    Mathf.Cos(angle * Mathf.Deg2Rad));
            }
        }


        public Vector2 Velocity { get; set; }

        public Vector2 Position { get; set; }

        private float steerAngle;

        public IVehicleController Controller { get; set; }
        public IVehicleRenderer Renderer { get; set; }

        public bool HasController => Controller != null;
        public bool HasRenderer => Renderer != null;


        public void Tick(float deltaTime)
        {
            if (HasController) ProcessInput(deltaTime);
            UpdatePosition(deltaTime);
            if (HasRenderer) UpdateRenderer();
        }

        private void UpdatePosition(float deltaTime)
        {
            Position += forwards * deltaTime * Speed;
        }

        private void ProcessInput(float deltaTime)
        {
            float steerTarget = Controller.SteeringAngle * MaxSteerDegrees;

            float steerDelta = SteerDegreesPerSecond * deltaTime;

            if (steerTarget > steerAngle)
                steerAngle = Mathf.Min(steerAngle + steerDelta, steerTarget);
            else if (steerTarget < steerAngle)
                steerAngle = Mathf.Max(steerAngle - steerDelta, steerTarget);

            steerAngle = Mathf.Clamp(steerAngle, -MaxSteerDegrees, MaxSteerDegrees);
            if (Speed > 0f)
                Angle += steerAngle * deltaTime * Speed * 0.4f;
            else
                Angle -= steerAngle * deltaTime * Speed * 0.4f;

            Speed += deltaTime * (
                Controller.GasPedalAmount * ForwardsAcceleration -
                Controller.BrakePedalAmount * ReverseAcceleration);
        }

        private void UpdateRenderer()
        {
            Renderer.Position = Position;
            Renderer.Forwards = forwards;
            Renderer.SteerAngle = steerAngle;
        }
    }
}
