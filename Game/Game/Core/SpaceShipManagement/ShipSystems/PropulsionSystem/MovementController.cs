// MovementController.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.Utilitys;
using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem
{
    public static class MovementController
    {

        public static float GetRotationUpdate(float currentRotation, Vector2 currentPosition, Vector2 targetPosition)
        {
            var targetRotation = Geometry.AngleBetweenVectors(currentPosition, targetPosition);
            float delta = Geometry.AngleDegDelta(Geometry.RadToDeg(currentRotation), Geometry.RadToDeg(targetRotation));
            delta = Geometry.DegToRad(delta);
            return delta switch { 
                float.NaN => 0, 
                _ => delta };
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

        public static float GetVelocity(float currentVelocity, float targetVelocity, float acceleration)
        {
            var a = MathF.Abs(acceleration);
            if (targetVelocity < currentVelocity) a = -acceleration;

            var newVelocity = currentVelocity + a;

            return a switch
            {
                < 0 => MathF.Max(0, newVelocity),
                > 0 => MathF.Min(newVelocity, targetVelocity),
                _ => currentVelocity
            };
        }
    }
}
