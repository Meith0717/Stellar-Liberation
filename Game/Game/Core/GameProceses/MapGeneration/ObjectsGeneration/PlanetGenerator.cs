// PlanetGenerator.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    public static class PlanetGenerator
    {
        private readonly static List<Registry> ColdTextures = new() { GameSpriteRegistries.cold, GameSpriteRegistries.cold1, GameSpriteRegistries.cold2, GameSpriteRegistries.cold3 };

        private readonly static List<Registry> DryTextures = new() { GameSpriteRegistries.dry, GameSpriteRegistries.dry1, GameSpriteRegistries.dry2, GameSpriteRegistries.dry3 };

        private readonly static List<Registry> StoneTextures = new() { GameSpriteRegistries.stone, GameSpriteRegistries.stone1, GameSpriteRegistries.stone2, GameSpriteRegistries.stone3 };

        private readonly static List<Registry> GasTextures = new() { GameSpriteRegistries.gas, GameSpriteRegistries.gas1, GameSpriteRegistries.gas2, GameSpriteRegistries.gas3 };

        private readonly static List<Registry> WarmTextures = new() { GameSpriteRegistries.warm, GameSpriteRegistries.warm1, GameSpriteRegistries.warm2, GameSpriteRegistries.warm3 };

        private readonly static List<Registry> TerrTextures = new() { GameSpriteRegistries.terrestrial, GameSpriteRegistries.terrestrial1, GameSpriteRegistries.terrestrial2, GameSpriteRegistries.terrestrial3 };

        private readonly static List<Registry> Tropical = new() { GameSpriteRegistries.tropical1, GameSpriteRegistries.tropical2, GameSpriteRegistries.tropical3, GameSpriteRegistries.tropical3 };

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
                < 100 => seededRandom.Next(40, 50) * .1f,
                < 200 => seededRandom.Next(60, 100) * .1f,
                < 250 => seededRandom.Next(45, 55) * .1f,
                < 280 => seededRandom.Next(50, 55) * .1f,
                < 330 => seededRandom.Next(50, 55) * .1f,
                < 500 => seededRandom.Next(50, 55) * .1f,
                < 1000 => seededRandom.Next(45, 50) * .1f,
                _ => seededRandom.Next(40, 45) * .1f
            };

            return new(distanceToStar, angle, texture, size);
        }
    }
}
