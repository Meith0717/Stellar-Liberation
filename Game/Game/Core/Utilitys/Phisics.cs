// Phisics.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.Utilitys
{
    public static class Phisics
    {
        public static double CalculateDecelerationDistance(double initialVelocity, double deceleration)
        {
            // Calculate the deceleration distance here and return the result
            double distance = (initialVelocity * initialVelocity) / (2 * deceleration);
            return distance;
        }
    }
}
