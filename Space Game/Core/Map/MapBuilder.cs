using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Core.Maths;
using Galaxy_Explovive.Game.GameObjects;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.Map
{
    internal class MapBuilder
    {
        private static MapBuilder mInstance = null;
        public static MapBuilder Instance { get { return mInstance ??= new MapBuilder(); }}

        public List<PlanetSystem> Generate(int SystemAmount, Vector2 MapSize)
        {
            int counter = 0;
            List<PlanetSystem> map = new List<PlanetSystem>();
            while (counter < SystemAmount)
            {
                Vector2 position = new Vector2(Globals.mRandom.Next(-(int)MapSize.X, (int)MapSize.X), Globals.mRandom.Next(-(int)MapSize.Y, (int)MapSize.Y));
                List<Star> neighbourSystem = ObjectLocator.Instance.GetObjectsInRadius(position, 20000).OfType<Star>().ToList();
                if (neighbourSystem.Count > 0)
                {
                    continue;
                }
                map.Add(new PlanetSystem(position));
                counter++;
            }
            return map;
        }
    }
}
