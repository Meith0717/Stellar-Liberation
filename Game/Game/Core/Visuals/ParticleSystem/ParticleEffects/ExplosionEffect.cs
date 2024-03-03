// ExplosionEffect.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public static class ExplosionEffect
    {
        public static void ShipDestroyed(Vector2 position, ParticleManager particleManager, float multiplier)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(20, 60) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 200) * .01f;
                particleManager.Add(position, dir, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 1000));
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(300, 500) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 300) * .01f;
                particleManager.Add(position, dir, velocity, new(255, 175, 25), ExtendetRandom.Random.Next(150, 2000));
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 300) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(100, 500) * .01f;
                particleManager.Add(position, dir, velocity, new(255, 42, 25), ExtendetRandom.Random.Next(250, 3000));
            }
        }

        public static void ShipHit(Vector2 position, Vector2 momentum, ParticleManager particleManager, float multiplier)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(0, 3) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + momentum * 0.9f);

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                particleManager.Add(position, direction, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
            }
        }

        public static void AsteroidHit(Vector2 position, Vector2 momentum, ParticleManager particleManager, float multiplier)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(100, 250) * multiplier; i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + momentum * 0.8f);

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                particleManager.Add(position, direction, velocity, new(189, 195, 199), ExtendetRandom.Random.Next(50, 2000));
            }
        }
    }
}
