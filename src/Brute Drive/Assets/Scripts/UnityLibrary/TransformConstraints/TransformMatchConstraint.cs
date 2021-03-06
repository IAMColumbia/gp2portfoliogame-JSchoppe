﻿using UnityEngine;
using UnityLibrary.TickWrappers;

namespace UnityLibrary.TransformConstraints
{
    /// <summary>
    /// Constrain a transform so that some if its values
    /// match that of another transform.
    /// </summary>
    public sealed class TransformMatchConstraint : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Constraint References")]
        [Tooltip("The parent transform that will drive this constraint.")]
        [SerializeField] private Transform parent = default;
        [Header("Constraint Attributes")]
        [Tooltip("The update loop that the matching code runs in.")]
        [SerializeField] private UnityLoopType gameLoopType = UnityLoopType.Update;
        [Header("Position Constraints")]
        [SerializeField] private bool matchPositionX = false;
        [SerializeField] private bool matchPositionY = false;
        [SerializeField] private bool matchPositionZ = false;
        [Header("Rotation Constraints")]
        [SerializeField] private bool matchRotationX = false;
        [SerializeField] private bool matchRotationY = false;
        [SerializeField] private bool matchRotationZ = false;
        [Header("Local Scale Constraints")]
        [SerializeField] private bool matchScaleX = false;
        [SerializeField] private bool matchScaleY = false;
        [SerializeField] private bool matchScaleZ = false;
        #endregion
        #region Update Matching Implementation
        private void Start()
        {
            UnityTickService.GetProvider(gameLoopType).Tick += MatchTransform;
        }
        private void OnDestroy()
        {
            UnityTickService.GetProvider(gameLoopType).Tick -= MatchTransform;
        }
        private void MatchTransform(float deltaTime)
        {
            // Match the transform values conditionally.
            transform.position = new Vector3
            {
                x = matchPositionX ? parent.position.x : transform.position.x,
                y = matchPositionY ? parent.position.y : transform.position.y,
                z = matchPositionZ ? parent.position.z : transform.position.z
            };
            transform.eulerAngles = new Vector3
            {
                x = matchRotationX ? parent.eulerAngles.x : transform.eulerAngles.x,
                y = matchRotationY ? parent.eulerAngles.y : transform.eulerAngles.y,
                z = matchRotationZ ? parent.eulerAngles.z : transform.eulerAngles.z
            };
            transform.localScale = new Vector3
            {
                x = matchScaleX ? parent.localScale.x : transform.localScale.x,
                y = matchScaleY ? parent.localScale.y : transform.localScale.y,
                z = matchScaleZ ? parent.localScale.z : transform.localScale.z
            };
        }
        #endregion
    }
}
