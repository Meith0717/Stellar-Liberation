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

        public static void Emit(Vector2 position, Vector2 momentum, ParticleManager particleManager)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(300, 500); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var particle = new Particle(position, Vector2.Normalize(dir + momentum), 0.2f, ExtendetRandom.Random.Next(10, 200) * .01f, new(255, 151, 0), ExtendetRandom.Random.Next(100, 2000));
                particleManager.AddParticle(particle);
            }
            for (int i = 0; i < ExtendetRandom.Random.Next(100, 300); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var dir);

                var particle = new Particle(position, Vector2.Normalize(dir + momentum), 0.2f, ExtendetRandom.Random.Next(100, 300) * .01f, new(255, 108, 0), ExtendetRandom.Random.Next(100, 2000));
                particleManager.AddParticle(particle);
            }
        }
    }
}
