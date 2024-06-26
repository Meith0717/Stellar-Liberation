// PlanetGenerator.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MonoGame.Extended;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    public static class PlanetGenerator
    {
        private readonly static List<string> ColdTextures = new() { "cold", "cold1", "cold2", "cold3" };

        private readonly static List<string> DryTextures = new() { "dry", "dry1", "dry2", "dry3" };

        private readonly static List<string> StoneTextures = new() { "stone", "stone1", "stone2", "stone3" };

        private readonly static List<string> GasTextures = new() { "gas", "gas1", "gas2", "gas3" };

        private readonly static List<string> WarmTextures = new() { "warm0", "warm1", "warm2", "warm3" };

        private readonly static List<string> TerrTextures = new() { "terrestrial", "terrestrial1", "terrestrial2", "terrestrial3" };

        private readonly static List<string> Tropical = new() { "tropical1", "tropical2", "tropical3", "tropical3" };

        public static double GetTemperature(int starTemp, int distanceToStar) => starTemp / Math.Pow(distanceToStar / 5000d, 1.2);

        public static Planet GetPlanet(Random seededRandom, int starTemp, int distanceToStar)
        {
            var kelvin = GetTemperature(starTemp, distanceToStar);
            var angle = seededRandom.NextAngle();
            string texture = kelvin switch
            {
                < 100 => ColdTextures[seededRandom.Next(3)],
                < 200 => GasTextures[seededRandom.Next(3)],
                < 250 => StoneTextures[seededRandom.Next(3)],
                < 280 => TerrTextures[seededRandom.Next(3)],
                < 330 => Tropical[seededRandom.Next(3)],
                < 500 => DryTextures[seededRandom.Next(3)],
                < 1000 => StoneTextures[seededRandom.Next(3)],
                _ => WarmTextures[seededRandom.Next(3)]
            };

            float size = kelvin switch
            {
                < 100 => seededRandom.Next(40, 50) * .05f,
                < 200 => seededRandom.Next(60, 100) * .05f,
                < 250 => seededRandom.Next(45, 55) * .05f,
                < 280 => seededRandom.Next(50, 55) * .05f,
                < 330 => seededRandom.Next(50, 55) * .05f,
                < 500 => seededRandom.Next(50, 55) * .05f,
                < 1000 => seededRandom.Next(45, 50) * .05f,
                _ => seededRandom.Next(40, 45) * .05f
            };

            return new(distanceToStar, angle, texture, size);
        }
    }
}
