using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types
{
    public class PlanetConfig
    {
        public readonly string TextureId;
        public readonly float TextureScale;

        public PlanetConfig(string textureId, float textureScale)
        {
            TextureId = textureId;
            TextureScale = textureScale;
        }
    }

    internal class PlanetTypes
    {
        
    }
}
