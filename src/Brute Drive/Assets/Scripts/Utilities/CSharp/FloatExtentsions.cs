﻿using System;

namespace BruteDrive.Utilities.CSharp
{
    /// <summary>
    /// Provides extensions for the floating point data type.
    /// </summary>
    public static class FloatExtentsions
    {
        #region Range Wrapping
        /// <summary>
        /// Gets the floating point value wrapped around into the given range.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The minimum wrap value.</param>
        /// <param name="max">The maximum wrap value.</param>
        /// <returns>A value between min and max that maps to the input value.</returns>
        public static float WrappedBetween(this float value, float min, float max)
        {
            // Error checking.
            if (max <= min)
#if DEBUG
                throw new ArgumentException("Max must be greater than min!", "max");
#else
                return value; 
#endif
            // Localize the remainder.
            float step = max - min;
            value -= min;
            // Use modulo accounting for negative step.
            // Move the result back into the range by adding min back.
            return value % step + ((value < 0f) ? step : 0f) + min;
        }
        #endregion
    }
}
