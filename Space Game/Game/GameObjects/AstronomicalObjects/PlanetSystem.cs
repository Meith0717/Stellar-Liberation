using CelestialOdyssey.Game.Core.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class PlanetSystem : InteractiveObject
    {
        public PlanetSystem(Vector2 position, string textureId, float textureScale, int textureDepth) 
            : base(position, textureId, textureScale, textureDepth)
        {
        }
    }
}
