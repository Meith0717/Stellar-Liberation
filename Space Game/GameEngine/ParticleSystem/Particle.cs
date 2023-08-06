using CelestialOdyssey.GameEngine.GameObjects;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.GameEngine.ParticleSystem
{
    public class Particle : GameObject
    {
        public Particle(Vector2 position, string textureId, float textureScale, int textureDepth) 
            : base(position, "pixle", textureScale, textureDepth)
        {
        }
    }
}
