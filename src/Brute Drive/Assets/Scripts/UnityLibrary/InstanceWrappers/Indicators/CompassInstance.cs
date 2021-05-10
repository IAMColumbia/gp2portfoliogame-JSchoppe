using GameLibrary.Math;
using GameLibrary.Transforms;
using GameLibrary.Indicators;
using UnityLibrary.TickWrappers;

namespace UnityLibrary.InstanceWrappers.Indicators
{
    // TODO consider how oriented transform 3d can be implemented once
    // and reused. Architecture around this could be better.
    /// <summary>
    /// A Unity instance of the compass indicator.
    /// </summary>
    public sealed class CompassInstance : UnityEditorWrapper<CompassIndicator>, IOrientedTransform3D
    {
        #region IOrientedTransform Implmentation
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector3 Forwards
        {
            get => transform.forward;
            set => transform.forward = value;
        }
        #endregion
        #region Compass Indicator Initialization
        public override CompassIndicator Initialize()
            => new CompassIndicator()
            {
                TickProvider = UnityTickService.GetProvider(UnityLoopType.Update),
                CompassTransform = this
            };
        #endregion
    }
}
