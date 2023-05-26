using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Core
{
    public class Globals
    {
        // Instances
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static Camera2d Camera2d { get; set; }
        public static GameLayer GameLayer { get; set; }
        public static DebugSystem DebugSystem { get; set; }

        // Values
        public static float SubLightVelocity = 0.01f;
        public static int MouseSpatialHashingRadius = 10000;
        public static Color HoverColor = new Color(50, 50, 50);
        public static bool mRayTracing = false;
        public static int mPlanetSystemDistanceRadius = 10000;
    }
}
