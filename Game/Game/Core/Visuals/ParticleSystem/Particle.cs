// Particle.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem
{
    internal class Particle
    {
        public Vector2 Position;
        private Vector2 MovingDirection;
        private float Velocity;
        private Color Color;
        private int DisposeTime;

        public bool IsDisposed => DisposeTime <= 0;

        public Particle(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime) 
            => Populate(position, movementDirection, velocity, color, dispodeTime);  

        public void Populate(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime)
        {
            Position = position;
            MovingDirection = movementDirection;
            Velocity = velocity;
            Color = color;
            DisposeTime = dispodeTime;
        }

        public void Update(GameTime gameTime)
        {
            DisposeTime -= gameTime.ElapsedGameTime.Milliseconds;
            Position += MovingDirection * Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Draw()
            =>TextureManager.Instance.Draw(GameSpriteRegistries.particle, Position, .1f, 0f, 9, Color);
    }
}
