// ExplosionEffect.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.ParticleSystem.ParticleEffects
{
    public static class ExplosionEffect
    {

        private static readonly List<Color> mColors = new() { Color.Brown, Color.Red, Color.RosyBrown, Color.Orange, Color.MonoGameOrange, Color.Gray };

        public static void ShipDestroyed(Vector2 position, ParticleManager particleManager)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(20, 60); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 200) * .01f;
                var particle = new Particle(position, dir, 0.15f, velocity, Color.LightGray, ExtendetRandom.Random.Next(50, 2000));
                particleManager.AddParticle(particle);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(300, 500); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(10, 300) * .01f;
                var particle = new Particle(position, dir, 0.1f, velocity, new(255, 151, 0), ExtendetRandom.Random.Next(50, 2000));
                particleManager.AddParticle(particle);
            }

            for (int i = 0; i < ExtendetRandom.Random.Next(100, 300); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var velocity = ExtendetRandom.Random.Next(100, 500) * .01f;
                var particle = new Particle(position, dir, 0.15f, velocity, new(255, 108, 0), ExtendetRandom.Random.Next(50, 2000));
                particleManager.AddParticle(particle);
            }
        }

        public static void ShipHit(Vector2 position, Vector2 momentum, ParticleManager particleManager)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(0, 3); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + (momentum * 0.9f));

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                var particle = new Particle(position, direction, 0.1f, velocity, Color.Gray, ExtendetRandom.Random.Next(50, 2000));
                particleManager.AddParticle(particle);
            }
        }

        public static void AsteroidHit(Vector2 position, Vector2 momentum, ParticleManager particleManager)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(50, 150); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);
                var direction = Vector2.Normalize(dir + (momentum * 0.8f));

                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;
                var particle = new Particle(position, direction, 0.15f, velocity, Color.LightGray, ExtendetRandom.Random.Next(50, 2000));
                particleManager.AddParticle(particle);
            }
        }
    }
}
