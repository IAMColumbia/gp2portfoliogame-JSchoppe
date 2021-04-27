using System;
using System.Collections.Generic;
using GameLibrary;
using GameLibrary.Math;
using BruteDriveCore.Vehicles;

namespace BruteDriveCore.Objectives
{
    /// <summary>
    /// An objective where vehicles must go through a linear set of waypoints.
    /// </summary>
    public sealed class WaypointsObjective : Objective
    {
        #region Fields
        private ITickProvider tickProvider;
        private IWaypoint[] waypoints;
        private int waypointIndex;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new waypoints objective with the given tick provider and waypoints.
        /// </summary>
        /// <param name="tickProvider">Provides the tick function to check vehicle proximity to the waypoint.</param>
        /// <param name="waypoints">The waypoints for the objective.</param>
        public WaypointsObjective(ITickProvider tickProvider, IWaypoint[] waypoints)
        {
            this.tickProvider = tickProvider;
            this.waypoints = waypoints;
            TriggeringVehicles = new List<Vehicle>();
            // Flag a potential crash from calling constructor with
            // an empty collection of waypoints.
            if (waypoints is null || waypoints.Length is 0)
                throw new ArgumentException(
                    "Waypoint objective must have at least one waypoint!",
                    "waypoints");
        }
        #endregion
        #region Waypoint Properties
        /// <summary>
        /// The current waypoint this objective is on.
        /// </summary>
        public IWaypoint CurrentWaypoint => waypoints[waypointIndex];
        /// <summary>
        /// Vehicles that can trigger this waypoint.
        /// </summary>
        public List<Vehicle> TriggeringVehicles { get; }
        #endregion
        #region Objective Methods
        /// <summary>
        /// Starts the waypoint objective.
        /// </summary>
        public override void StartObjective()
        {
            waypointIndex = 0;
            // Show the first waypoint.
            waypoints[0].IsRendered = true;
            // Hide all waypoints after the first.
            for (int i = 1; i < waypoints.Length; i++)
                waypoints[i].IsRendered = false;
            // Start ticking to check against waypoints.
            if (!InProgress)
                tickProvider.Tick += Tick;
            base.StartObjective();
        }
        /// <summary>
        /// Cancels the waypoint objective.
        /// </summary>
        public override void CancelObjective()
        {
            // Stop the objective if in progress.
            if (InProgress)
            {
                tickProvider.Tick -= Tick;
                // Hide all waypoints.
                for (int i = 0; i < waypoints.Length; i++)
                    waypoints[i].IsRendered = false;
            }
            base.CancelObjective();
        }
        #endregion
        #region Tick Routine
        private void Tick(float deltaTime)
        {
            // Check each vehicle to see if it close enough
            // to trigger the waypoints.
            foreach (Vehicle vehicle in TriggeringVehicles)
            {
                Vector2 delta = vehicle.Location - waypoints[waypointIndex].Location;
                if (delta.x * delta.x + delta.y * delta.y <
                    waypoints[waypointIndex].Radius * waypoints[waypointIndex].Radius)
                {
                    // If a vehicle is close enough, advance the
                    // waypoint, changing the waypoints render state.
                    waypoints[waypointIndex].IsRendered = false;
                    waypointIndex++;
                    // Are there more waypoints?
                    if (waypointIndex < waypoints.Length)
                        waypoints[waypointIndex].IsRendered = true;
                    // If not the objective is complete.
                    else
                    {
                        tickProvider.Tick -= Tick;
                        ObjectiveComplete();
                    }
                    break;
                }
            }
        }
        #endregion
    }
}
