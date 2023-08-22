using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Utility;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public static class PlanetGenerator
    {
        public static void Generate(List<Star> stars, GameEngine.GameEngine gameEngine ,out List<Planet> planets)
        {
            var triangularDistribution = new Triangular(1, 10, 3);
            planets = new();
            foreach (Star star in stars)
            {
                var maxOrbitNumber = GetMaxOrbitNumber(triangularDistribution);
                for (int i = 1; i <= maxOrbitNumber; i++)
                {
                    var oribitRadius = (int)(star.Width * star.TextureScale) + (20000 * i);
                    Planet planet = new(star.Position, oribitRadius, ContentRegistry.terrestrial1, 10);
                    planet.AddToSpatialHashing(gameEngine);
                    planets.Add(planet);
                }  
            }
        }

        private static int GetMaxOrbitNumber(Triangular triangularDistribution)
        {
            return (int)triangularDistribution.Sample();
        }

        private static Planet GetPlanet(int orbitNumber)
        {
            return orbitNumber switch
            {
                1 => null,
                2 => null,
                3 => null,
                4 => null,
                5 => null,
                6 => null,
                7 => null,
                8 => null,
                9 => null,
                10 => null,
                _ => null
            };
        }
    }
}
