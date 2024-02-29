// ExplosionEffect.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public static class ExplosionEffect
    {
        public static void ShipDestroyed(Vector2 position, Vector2 momentum, GameObject2DManager particleManager, float multiplier)
        {
            var size = ExtendetRandom.Random.Next(15, 30) * 0.007f;

            momentum = momentum * (float)ExtendetRandom.Random.NextDouble();

            for (int i = 0; i < ExtendetRandom.Random.Next(20, 60) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 200) * .01f;
                var particle = new Particle(position, dir + momentum, size, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 1000));
                particleManager.SpawnGameObject2D(particle, false);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(300, 500) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 300) * .01f;
                var particle = new Particle(position, dir + momentum, size, velocity, new(255, 175, 25), ExtendetRandom.Random.Next(150, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 300) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(100, 500) * .01f;
                var particle = new Particle(position, dir + momentum, size, velocity, new(255, 42, 25), ExtendetRandom.Random.Next(250, 3000));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }

        public static void ShipHit(Vector2 position, Vector2 momentum, GameObject2DManager particleManager, float multiplier)
        {
            var size = ExtendetRandom.Random.Next(1, 10) * 0.01f;

            for (int i = 0; i < ExtendetRandom.Random.Next(0, 3) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + momentum * 0.9f);

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                var particle = new Particle(position, direction, size, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }

        public static void AsteroidHit(Vector2 position, Vector2 momentum, GameObject2DManager particleManager, float multiplier)
        {
            var size = ExtendetRandom.Random.Next(1, 10) * 0.01f;

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 250) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + momentum * 0.8f);

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                var particle = new Particle(position, direction, size, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }
    }
}
