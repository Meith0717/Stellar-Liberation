using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.GameObjects.Types
{
    public class StarType
    {
        public float Size { get; private set; }
        public string Texture { get; private set; }
        public Color StarColor { get; private set; }
        public Color LightColor { get; private set; }

        public StarType(float size, string texture, Color starColor)
        {
            Size = size;
            Texture = texture;
            StarColor = starColor;
            LightColor = new Color(
                (starColor.R > 0) ? 2 : 0,
                (starColor.G > 0) ? 2 : 0,
                (starColor.B > 0) ? 2 : 0,
                0
                );
        }
    }
}
