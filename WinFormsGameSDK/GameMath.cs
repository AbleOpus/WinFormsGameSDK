using System;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Provides drawing-related math functionality.
    /// </summary>
    public static class GameMath
    {
        /// <summary>
        /// Gets whether the specified angle is within the specified angle range.
        /// </summary>
        /// <param name="angle">The angle to test.</param>
        /// <param name="startAngle">The start angle to test against.</param>
        /// <param name="endAngle">The end angle to test against.</param>
        /// <returns>True, if the angle is within range, otherwise false.</returns>
        public static bool AngleWithinRange(float angle, float startAngle, float endAngle)
        {
            if (startAngle.Equals(endAngle))
            {
                return angle.Equals(endAngle);
            }

            if (endAngle < startAngle)
            {
                // Offset range so it is not in the wonky area.
                float offsetAmount = 180 - startAngle;
                //float range = a1 + a2;
                startAngle = -179;
                endAngle += offsetAmount;
                // Offset angle so it is not in the wonky range.

                if (angle > 0)
                {
                    float angleOffset = 180 - angle;
                    angle = -179;
                    angle += (offsetAmount + angleOffset);
                }

                return angle > startAngle && angle < endAngle;
            }

            if (endAngle > startAngle)
            {
                return angle > startAngle && angle < endAngle;
            }
            else
            {
                return angle < startAngle && angle > endAngle;
            }
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        public static float DegreeToRadian(float degrees)
        {
            return (float)Math.PI * degrees / 180.0f;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        public static float RadianToDegree(float radians)
        {
            return radians * (180.0f / (float)Math.PI);
        }
    }
}
