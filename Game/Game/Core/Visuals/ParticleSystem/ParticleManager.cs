// ParticleManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Visuals.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem
{
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


        public void Add(Vector2 position, Vector2 movementDirection, float velocity, Color color, int dispodeTime)
        {
            if (mCachedParticles.Count == 0)
            {
                mParticles.Add(new(position, movementDirection, velocity, color, dispodeTime));
                return;
            }
            var cachedParticle = mCachedParticles.First();
            mCachedParticles.RemoveFirst();
            cachedParticle.Populate(position, movementDirection, velocity, color, dispodeTime);
            mParticles.Add(cachedParticle);
        }

        private void Remove(Particle particle)
        {
            if (!mParticles.Remove(particle)) return;
            mCachedParticles.AddLast(particle);
        }

        public void Update(GameTime gameTime)
        {
            var particles = mParticles.ToList();
            foreach ( var particle in particles)
            {
                particle.Update(gameTime);
                if (!particle.IsDisposed) continue;
                Remove(particle);
            }
        }

        public void Draw(Camera2D camera2D)
        {
            foreach (var particle in mParticles)
            {
                if (!camera2D.Contains(particle.Position)) continue;
               particle.Draw();
            }
        }
    }
}
