using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    public static class MovementController
    {
        /// <summary>
        /// Calculates the rotation update needed to smoothly transition from the current rotation
        /// to the target rotation, applying a scaling factor for finer control over the update rate.
        /// </summary>
        /// <param name="currentRotation">The current rotation angle in radians.</param>
        /// <param name="targetRotation">The target rotation angle in radians.</param>
        /// <param name="rotationSmoothingFactor">A factor that controls the rate of rotation update.
        /// Higher values result in faster updates, while lower values produce smoother transitions.</param>
        /// <returns>
        /// The rotation update value, scaled by the provided smoothing factor for better control over the transition.
        /// If the input angles are NaN (Not a Number), the function returns 0 to prevent undesired behavior.
        /// </returns>
        public static float GetRotationUpdate(float currentRotation, Vector2 currentPosition, Vector2 targetPosition, float rotationSmoothingFactor)
        {
            var targetRotation = Geometry.AngleBetweenVectors(currentPosition, targetPosition);
            float delta = Geometry.AngleDegDelta(Geometry.RadToDeg(currentRotation), Geometry.RadToDeg(targetRotation));
            delta = Geometry.DegToRad(delta);
            return delta switch
            {
                float.NaN => 0,
                _ => delta * rotationSmoothingFactor,
            };
        }

        public static float GetRotationUpdate(float currentRotation, float targetRotation, float rotationSmoothingFactor)
        {
            float delta = Geometry.AngleDegDelta(Geometry.RadToDeg(currentRotation), Geometry.RadToDeg(targetRotation));
            delta = Geometry.DegToRad(delta);
            return delta switch
            {
                float.NaN => 0,
                _ => delta * rotationSmoothingFactor,
            };
        }

        /// <summary>
        /// Calculates the new velocity based on the current velocity and acceleration applied.
        /// </summary>
        /// <param name="currentVelocity">The current velocity of the object.</param>
        /// <param name="acceleration">The acceleration applied to the object.</param>
        /// <returns>The new velocity after applying the given acceleration.</returns>
        public static float GetVelocity(float currentVelocity, float acceleration)
        {
            return currentVelocity + acceleration;
        }
    }
}
