// ExplosionEffect.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public static class ExplosionEffect
    {

        public static void ShipDestroyed(Vector2 position, GameObject2DManager particleManager)
        {
            var size = ExtendetRandom.Random.Next(15, 30) * 0.01f;

            for (int i = 0; i < ExtendetRandom.Random.Next(20, 60); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 200) * .01f;
                var particle = new Particle(position, dir, size, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(300, 500); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 300) * .01f;
                var particle = new Particle(position, dir, size, velocity, new(255, 175, 25), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 300); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(100, 500) * .01f;
                var particle = new Particle(position, dir, size, velocity, new(255, 42, 25), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }

        public static void ShipHit(Vector2 position, Vector2 momentum, GameObject2DManager particleManager)
        {
            var size = ExtendetRandom.Random.Next(1, 10) * 0.01f;

            for (int i = 0; i < ExtendetRandom.Random.Next(0, 3); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + momentum * 0.9f);

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                var particle = new Particle(position, direction, size, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }

        public static void AsteroidHit(Vector2 position, Vector2 momentum, GameObject2DManager particleManager)
        {
            var size = ExtendetRandom.Random.Next(1, 10) * 0.01f;

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 250); i++)
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
