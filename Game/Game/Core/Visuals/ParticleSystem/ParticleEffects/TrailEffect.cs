// TrailEffect.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public class TrailEffect
    {
        public static void Show(Vector2 position, Vector2 movingDir, float velocity, GameTime gameTime, Color color, Queue<ParticleEmitor> particleEmitors, float multiplier)
        {
            for (int i = 1; i < gameTime.ElapsedGameTime.TotalMilliseconds * velocity * multiplier; i += 5)
            {
                var spawnPos = Geometry.GetPointInDirection(position, movingDir, -i);
                particleEmitors.Enqueue(new(spawnPos, Vector2.Zero, 0, color, ExtendetRandom.Random.Next(100, 300)));
            }
        }
    }
}
