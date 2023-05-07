using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Game.GameObjects;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

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
                List<Star> neighbourSystem = ObjectLocator.Instance.GetObjectsInRadius(position, Globals.mPlanetSystemDistanceRadius).OfType<Star>().ToList();
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
