// ParticleManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Visuals.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem
{
    public readonly struct ParticleEmitor(Vector2 Position, Vector2 MovementDirection, float Velocity, Color Color, int DispodeTime, float Size = 1)
    {
        public readonly Vector2 Position = Position;
        public readonly Vector2 MovementDirection = MovementDirection;
        public readonly float Velocity = Velocity;
        public readonly Color Color = Color;
        public readonly int DispodeTime = DispodeTime;
        public readonly float Size = Size;
    }

    public class ParticleManager
    {
        private readonly HashSet<Particle> mParticles;
        private readonly LinkedList<Particle> mCachedParticles;

        public int Count => mParticles.Count;

        public ParticleManager()
        {
            mParticles = new();
            mCachedParticles = new();
        }

        public void Update(GameTime gameTime, Queue<ParticleEmitor> particleEmitors)
        {
            while (particleEmitors.Count > 0)
            {
                var particleEmitor = particleEmitors.Dequeue();
                Add(particleEmitor.Position, particleEmitor.MovementDirection, particleEmitor.Velocity, particleEmitor.Color, particleEmitor.DispodeTime, particleEmitor.Size);
            }

            var particles = mParticles.ToList();
            foreach (var particle in particles)
            {
                particle.Update(gameTime);
                if (!particle.IsDisposed) continue;
                Remove(particle);
            }
        }

        private void Add(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime, float size)
        {
            if (mCachedParticles.Count == 0)
            {
                mParticles.Add(new(position, movementDirection, velocity, color, dispodeTime, size));
                return;
            }
            var cachedParticle = mCachedParticles.First();
            mCachedParticles.RemoveFirst();
            cachedParticle.Populate(position, movementDirection, velocity, color, dispodeTime, size);
            mParticles.Add(cachedParticle);
        }

        private void Remove(Particle particle)
        {
            if (!mParticles.Remove(particle)) return;
            mCachedParticles.AddLast(particle);
        }

        public void Clear()
        {
            mParticles.Clear();
            mCachedParticles.Clear();
        }

        public void Draw(Camera2D camera2D)
        {
            foreach (var particle in mParticles)
            {
                if (!camera2D.Contains(particle.mPosition)) continue;
                particle.Draw();
            }
        }
    }
}
