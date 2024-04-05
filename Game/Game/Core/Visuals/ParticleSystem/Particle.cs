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
        public Vector2 mPosition;
        private Vector2 mMovingDirection;
        private float mVelocity;
        private Color mColor;
        private int mDisposeTime;
        private float mSize;

        public bool IsDisposed => mDisposeTime <= 0;

        public Particle(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime, float size)
            => Populate(position, movementDirection, velocity, color, dispodeTime, size);

        public void Populate(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime, float size)
        {
            mPosition = position;
            mMovingDirection = movementDirection;
            mVelocity = velocity;
            mColor = color;
            mDisposeTime = dispodeTime;
            mSize = size;
        }

        public void Update(GameTime gameTime)
        {
            mDisposeTime -= gameTime.ElapsedGameTime.Milliseconds;
            mPosition += mMovingDirection * mVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Draw()
            => TextureManager.Instance.Draw(GameSpriteRegistries.particle, mPosition, .1f * mSize, 0f, 9, mColor);
    }
}
