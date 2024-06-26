// AsteroidGenerator.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    internal static class AsteroidGenerator
    {
        private const int mMargin = 1000;

        public static List<GameObject> GetAsteroidsRing(Vector2 centerPosition, float orbitRadius)
        {
            List<GameObject> asteroids = new();
            var circle = new CircleF(centerPosition, orbitRadius - mMargin);

            for (int i = 0; i < 100; i++)
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
                1 => "asteroid1",
                2 => "asteroid2",
                3 => "asteroid3",
                4 => "asteroid4",
                5 => "asteroid5",
                6 => "asteroid6",
                7 => "asteroid7",
                8 => "asteroid8",
                9 => "asteroid9",
                10 => "asteroid10",
                11 => "asteroid11",
                12 => "asteroid12",
                13 => "asteroid13",
                14 => "asteroid14",
                15 => "asteroid15",
                _ => throw new NotImplementedException()
            };

            return new(position, textureID, ExtendetRandom.Random.Next(10, 50) / 10f);
        }
    }
}
