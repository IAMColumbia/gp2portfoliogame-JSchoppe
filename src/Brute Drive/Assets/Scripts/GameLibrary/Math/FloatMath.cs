// TODO engine configuration should be centralized.
// Toggle based on the engine.
#define ENGINE_UNITY
/*
#define ENGINE_MONOGAME
*/

namespace GameLibrary.Math
{
    #region Utility Class Header
    /// <summary>
    /// Contains utilities for floating point arithmetic.
    /// </summary>
    public static partial class FloatMath
    {
        #region Constants
        /// <summary>
        /// Number of radians in half a circle.
        /// </summary>
        public const float PI = 3.1415927f;
        /// <summary>
        /// Number of radians in a complete circle.
        /// </summary>
        public const float TAU = 6.2831853f;
        /// <summary>
        /// Conversion factor for degrees to radians.
        /// </summary>
        public const float DEG_2_RAD = 0.0174533f;
        /// <summary>
        /// Conversion factor for radians to degrees.
        /// </summary>
        public const float RAD_2_DEG = 57.2957795f;
        #endregion
        #region Math Methods
        /// <summary>
        /// Gets the absolute value of this value.
        /// </summary>
        /// <param name="value">The value to take the absolute value of.</param>
        /// <returns>A positive version of this value.</returns>
        public static partial float Abs(float value);
        /// <summary>
        /// Calculates the square root of the provided value.
        /// </summary>
        /// <param name="value">The value to take the square root of.</param>
        /// <returns>The square root.</returns>
        public static partial float Sqrt(float value);
        /// <summary>
        /// Calculates the sine ratio for the angle in radians.
        /// </summary>
        /// <param name="radians">The angle to take the sine of.</param>
        /// <returns>A value between 0-1.</returns>
        public static partial float SinRad(float radians);
        /// <summary>
        /// Calculates the sine ratio for the angle in degrees.
        /// </summary>
        /// <param name="degrees">The angle to take the sine of.</param>
        /// <returns>A value between 0-1.</returns>
        public static partial float SinDeg(float degrees);
        /// <summary>
        /// Calculates the cosine ratio for the angle in radians.
        /// </summary>
        /// <param name="radians">The angle to take the cosine of.</param>
        /// <returns>A value between 0-1.</returns>
        public static partial float CosRad(float radians);
        /// <summary>
        /// Calculates the cosine ratio for the angle in degrees.
        /// </summary>
        /// <param name="degrees">The angle to take the cosine of.</param>
        /// <returns>A value between 0-1.</returns>
        public static partial float CosDeg(float degrees);
        #endregion
        #region Interpolation Methods
        /// <summary>
        /// Linearly interpolates between two given points given an interpolant.
        /// </summary>
        /// <param name="start">The start of the interpolation range.</param>
        /// <param name="end">The end of the interpolation range.</param>
        /// <param name="interpolant">The interpolant where 0 corresponds to the start and 1 to the end.</param>
        /// <returns>The interpolated value between start and end.</returns>
        public static partial float Lerp(float start, float end, float interpolant);
        /// <summary>
        /// Finds the interpolant in the range from the given value.
        /// </summary>
        /// <param name="start">The start of the interpolation range.</param>
        /// <param name="end">The end of the interpolation range.</param>
        /// <param name="value">The value to find the interpolant of.</param>
        /// <returns>The interpolant that yields this value.</returns>
        public static partial float InverseLerp(float start, float end, float value);
        /// <summary>
        /// Maps a value from one interpolation range to another.
        /// </summary>
        /// <param name="fromMin">The start of the current interpolation range.</param>
        /// <param name="fromMax">The end of the current interpolation range.</param>
        /// <param name="toMin">The start of the desired interpolation range.</param>
        /// <param name="toMax">The end of the desired interpolation range.</param>
        /// <param name="value">The value relative to the current range.</param>
        /// <returns>The new value with interpolant shifted into the to range.</returns>
        public static partial float Map(float fromMin, float fromMax, float toMin, float toMax, float value);
        #endregion
        #region Utility Methods
        /// <summary>
        /// Retrieves the minimum of all provided values.
        /// </summary>
        /// <param name="values">The values to find the minimum in.</param>
        /// <returns>The minimum value in the collection.</returns>
        public static partial float Min(params float[] values);
        /// <summary>
        /// Retrieves the maximum of all provided values.
        /// </summary>
        /// <param name="values">The values to find the maximum in.</param>
        /// <returns>The maximum value in the collection.</returns>
        public static partial float Max(params float[] values);
        /// <summary>
        /// Clamps a value within the given range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <returns></returns>
        public static partial float Clamp(float value, float min, float max);
        /// <summary>
        /// Moves towards the target value, stopping if reaching the value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="delta">The delta towards the target.</param>
        /// <returns>A value closer to or at the target.</returns>
        public static partial float MoveTowards(float current, float target, float delta);
        #endregion
    }
    #endregion
    #region Engine Independent Implementation
    public static partial class FloatMath
    {
        public static partial float Abs(float value)
        {
            return (value > 0f) ? value : -value;
        }
        public static partial float SinDeg(float degrees)
        {
            return SinRad(degrees * DEG_2_RAD);
        }
        public static partial float CosDeg(float degrees)
        {
            return CosRad(degrees * DEG_2_RAD);
        }
        public static partial float InverseLerp(float start, float end, float interpolant)
        {
            return start + (end - start) * interpolant;
        }
        public static partial float Map(float fromMin, float fromMax, float toMin, float toMax, float value)
        {
            return Lerp(toMin, toMax, InverseLerp(fromMin, fromMax, value));
        }
        public static partial float MoveTowards(float current, float target, float delta)
        {
            if (delta > Abs(target - current))
                return target;
            else
                return current + ((target > current) ? delta : -delta);
        }
    }
    #endregion
#if ENGINE_UNITY
    #region Unity Implementation
    public static partial class FloatMath
    {
        public static partial float Sqrt(float value)
        {
            return UnityEngine.Mathf.Sqrt(value);
        }
        public static partial float Min(params float[] values)
        {
            return UnityEngine.Mathf.Min(values);
        }
        public static partial float Max(params float[] values)
        {
            return UnityEngine.Mathf.Max(values);
        }
        public static partial float Clamp(float value, float min, float max)
        {
            return UnityEngine.Mathf.Clamp(value, min, max);
        }
        public static partial float SinRad(float radians)
        {
            return UnityEngine.Mathf.Sin(radians);
        }
        public static partial float CosRad(float radians)
        {
            return UnityEngine.Mathf.Cos(radians);
        }
        public static partial float Lerp(float start, float end, float interpolant)
        {
            return UnityEngine.Mathf.LerpUnclamped(start, end, interpolant);
        }
    }
    #endregion
#endif
}
