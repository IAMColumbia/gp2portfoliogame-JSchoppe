namespace BruteDriveCore.Objectives
{
    #region Handler Delegates
    /// <summary>
    /// Handler for when an objective has been completed.
    /// </summary>
    /// <param name="completedObjective">The completed objective.</param>
    public delegate void ObjectiveCompletedHandler(Objective completedObjective);
    /// <summary>
    /// Handler for when an objective has been failed (must be restarted).
    /// </summary>
    /// <param name="failedObjective">The objective that was failed.</param>
    public delegate void ObjectiveFailedHandler(Objective failedObjective);
    #endregion
    /// <summary>
    /// Base class for all objectives.
    /// </summary>
    public abstract class Objective
    {
        #region Objective State Change Events
        /// <summary>
        /// Called when this objective has been completed.
        /// </summary>
        public event ObjectiveCompletedHandler Completed;
        /// <summary>
        /// Called when this objective has been failed.
        /// </summary>
        public event ObjectiveFailedHandler Failed;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes the objective as dormant/not in progress.
        /// </summary>
        public Objective()
        {
            InProgress = false;
        }
        #endregion
        #region Objective Properties
        /// <summary>
        /// Whether this objective is currently active and in progress.
        /// </summary>
        public bool InProgress { get; private set; }
        #endregion
        #region Objective Methods
        /// <summary>
        /// Starts the objective, initializing it.
        /// </summary>
        public virtual void StartObjective()
        {
            InProgress = true;
        }
        /// <summary>
        /// Cancels the objective.
        /// </summary>
        public virtual void CancelObjective()
        {
            InProgress = false;
        }
        #endregion
        #region Subclass Utility Methods
        /// <summary>
        /// Call when the objective condition has been completed.
        /// </summary>
        protected void ObjectiveComplete()
        {
            InProgress = false;
            Completed?.Invoke(this);
        }
        /// <summary>
        /// Call when the objective condition has been irreversibly failed.
        /// </summary>
        protected void ObjectiveFailed()
        {
            InProgress = false;
            Failed?.Invoke(this);
        }
        #endregion
    }
}
