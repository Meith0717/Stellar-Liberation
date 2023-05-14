using Galaxy_Explovive.Core.GameObjects.Types;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public class StarTypes
    {
        private static StarType M = new(.5f, "M", Color.Red);
        private static StarType K = new(.55f, "K", Color.OrangeRed);
        private static StarType G = new(.6f, "G", Color.Orange);
        private static StarType F = new(.65f, "F", Color.White);
        private static StarType A = new(.7f, "A", Color.LightBlue);
        private static StarType B = new(.75f, "B", Color.LightSkyBlue);
        private static StarType O = new(.8f, "O", Color.Blue);

        public static List<StarType> types { get; private set; } = new() { M, K, G, F, A, B, O };
    }
}
