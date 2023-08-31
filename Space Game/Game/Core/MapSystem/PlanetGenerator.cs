﻿using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using CelestialOdyssey.GameEngine.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types;
using Microsoft.Xna.Framework;

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
                    Planet planet = GetPlanet(star.Position, oribitRadius, i);
                    planet.AddToSpatialHashing(gameEngine);
                    planets.Add(planet);
                }  
            }
        }

        private static int GetMaxOrbitNumber(Triangular triangularDistribution)
        {
            return (int)triangularDistribution.Sample();
        }

        private static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius, int orbitNumber)
        {
            return orbitNumber switch
            {
                1 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                2 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                3 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Dry(orbitCenter, oribitRadius) }),
                4 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Dry(orbitCenter, oribitRadius), new PlanetTypes.Tessatial(orbitCenter, oribitRadius) }),
                5 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                6 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Stone(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
                7 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
                8 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
                9 => Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Cold(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
                10 => new PlanetTypes.Cold(orbitCenter, oribitRadius),
                _ => null
            };
        }
    }
}
