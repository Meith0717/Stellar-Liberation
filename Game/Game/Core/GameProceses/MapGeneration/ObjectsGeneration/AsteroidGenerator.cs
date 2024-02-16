// AsteroidGenerator.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    internal static class AsteroidGenerator
    {
        private const int mMargin = 1000;

        public static List<Asteroid> GetAsteroidsRing(Vector2 centerPosition, float orbitRadius)
        {
            List<Asteroid> asteroids = new();
            var circle = new CircleF(centerPosition, orbitRadius - mMargin);

            for (int i = 0; i < 500; i++)
            {
                var position = ExtendetRandom.NextVectorOnBorder(circle);
                circle.Radius += i;
                asteroids.Add(GenerateAsteroid(position));
            }
            return asteroids;
        }

        public static Asteroid GenerateAsteroid(Vector2 position)
        {
            string textureID = ExtendetRandom.Random.Next(1, 15) switch
            {
                1 => GameSpriteRegistries.asteroid1,
                2 => GameSpriteRegistries.asteroid2,
                3 => GameSpriteRegistries.asteroid3,
                4 => GameSpriteRegistries.asteroid4,
                5 => GameSpriteRegistries.asteroid5,
                6 => GameSpriteRegistries.asteroid6,
                7 => GameSpriteRegistries.asteroid7,
                8 => GameSpriteRegistries.asteroid8,
                9 => GameSpriteRegistries.asteroid9,
                10 => GameSpriteRegistries.asteroid10,
                11 => GameSpriteRegistries.asteroid11,
                12 => GameSpriteRegistries.asteroid12,
                13 => GameSpriteRegistries.asteroid13,
                14 => GameSpriteRegistries.asteroid14,
                15 => GameSpriteRegistries.asteroid15,
                _ => throw new NotImplementedException()
            };

            return new(position, textureID, ExtendetRandom.Random.Next(10, 50) / 10f);
        }
    }
}
