// TrailEffect.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects
{
    public class TrailEffect
    {
        public static void Show(Vector2 position, Vector2 movingDir, float velocity, GameTime gameTime, Color color, GameObject2DManager particleManager)
        {
            var size = .1f;

            for (int i = 1; i < gameTime.ElapsedGameTime.TotalMilliseconds * velocity; i += 5)
            {
                var spawnPos = Geometry.GetPointInDirection(position, movingDir, -i);
                var particle = new Particle(spawnPos, Vector2.Zero, size, 0, color, ExtendetRandom.Random.Next(20, 60));
                particleManager.SpawnGameObject2D(particle, false);
            }
        }
    }
}
