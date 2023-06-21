using Galaxy_Explovive.Core.GameObjects.Types;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public class StarTypes
    {
        private readonly static StarType M = new("Type-M", .5f, "M", Color.Red);
        private readonly static StarType K = new("Type-K", .55f, "K", Color.OrangeRed);
        private readonly static StarType G = new("Type-G", .6f, "G", Color.Orange);
        private readonly static StarType F = new("Type-F", .65f, "F", Color.White);
        private readonly static StarType A = new("Type-A", .7f, "A", Color.LightBlue);
        private readonly static StarType B = new("Type-B", .75f, "B", Color.LightSkyBlue);
        private readonly static StarType O = new("Type-O", .8f, "O", Color.Blue);
        public readonly static StarType BH = new("Black Hole", 1f, "BH", Color.White);

        public static List<StarType> Types { get; private set; } = new() {
            M, M, M, M, K, K, K, K, G, G, G, G, F, F, F, F, A, A, A, A, B, B, B, B, O, O, O, O, BH
        };
    }
}
