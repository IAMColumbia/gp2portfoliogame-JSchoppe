using GameLibrary.Math;
using GameLibrary.Transforms;

namespace GameLibrary.Indicators
{
    // TODO subclass this so it can be animated over time.
    /// <summary>
    /// A compass indicator that points towards an object.
    /// </summary>
    public class CompassIndicator
    {
        #region Fields
        // Dependencies.
        private IOrientedTransform3D compassTransform;
        private ITickProvider tickProvider;
        // State.
        private bool isTicking;
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new compass indicator; initializing its values.
        /// </summary>
        public CompassIndicator()
        {
            isTicking = false;
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// The transform for the compass.
        /// </summary>
        public IOrientedTransform3D CompassTransform
        {
            get => compassTransform;
            set
            {
                // Update the ticking state of
                // this object to respond to the new
                // injected value.
                if (value != compassTransform)
                {
                    if (value is null)
                    {
                        if (isTicking)
                        {
                            isTicking = false;
                            tickProvider.Tick -= Tick;
                        }
                    }
                    else if (!isTicking && !(tickProvider is null))
                    {
                        isTicking = true;
                        tickProvider.Tick += Tick;
                    }
                    compassTransform = value;
                }
            }
        }
        /// <summary>
        /// Ticks this compass to point towards a direction.
        /// </summary>
        public ITickProvider TickProvider
        {
            get => tickProvider;
            set
            {
                // Update the ticking state of
                // this object to respond to the new
                // injected provider.
                if (value != tickProvider)
                {
                    if (!(tickProvider is null) && isTicking)
                    {
                        isTicking = false;
                        tickProvider.Tick -= Tick;
                    }
                    if (!(value is null) && !(compassTransform is null))
                    {
                        isTicking = true;
                        tickProvider.Tick += Tick;
                    }
                    tickProvider = value;
                }
            }
        }
        #endregion
        #region Compass Properties
        /// <summary>
        /// The current target for the compass to point at.
        /// </summary>
        public Vector3 Target { get; set; }
        #endregion
        #region Tick Compass Implementation
        private void Tick(float deltaTime)
        {
            // Orient the arrow towards the target.
            compassTransform.Forwards = 
                (Target - compassTransform.Position).GetNormalized();
        }
        #endregion
    }
}
