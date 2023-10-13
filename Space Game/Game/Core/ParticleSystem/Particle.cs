// Particle.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ParticleSystem
{
    public class Particle : GameObject
    {
        public Particle(Vector2 position, string textureId, float textureScale, int textureDepth)
            : base(position, "pixle", textureScale, textureDepth)
        {
        }
    }
}
