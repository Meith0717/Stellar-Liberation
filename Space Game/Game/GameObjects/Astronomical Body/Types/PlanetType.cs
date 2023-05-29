

namespace Galaxy_Explovive.Core.GameObjects.Types
{
    public class PlanetType
    {
        public float Size { get; private set; }
        public string Texture { get; private set; }

        public PlanetType(float size, string texture)
        {
            Size = size;
            Texture = texture;
        }
    }
}
