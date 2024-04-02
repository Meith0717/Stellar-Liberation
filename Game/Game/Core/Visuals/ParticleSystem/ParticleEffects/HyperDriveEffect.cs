// HyperDriveEffect.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public class HyperDriveEffect
    {
        public static void Charge(Vector2 position, Queue<ParticleEmitor> particleEmitors)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(0, 3); i++)
            {
                var spawnPos = ExtendetRandom.NextVectorOnBorder(new(position, ExtendetRandom.Random.Next(600, 800)));
                var direction = Vector2.Normalize(position - spawnPos);
                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;

                particleEmitors.Enqueue(new(spawnPos, direction, velocity, new(214, 234, 248), ExtendetRandom.Random.Next(50, 800)));
            }
        }

        public static void Stop(Vector2 position, Queue<ParticleEmitor> particleEmitors)
        {
            for (int i = 0; i < ExtendetRandom.Random.Next(10, 30); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var direction);
                var velocity = ExtendetRandom.Random.Next(50, 100) * .01f;

                particleEmitors.Enqueue(new(position, direction, velocity, new(214, 234, 248), ExtendetRandom.Random.Next(50, 800)));
            }
        }
    }
}
