using CelestialOdyssey.Core.GameEngine.Content_Management;
using MonoGame.Extended;
using System.Collections.Generic;

namespace CelestialOdyssey.GameEngine
{
    public static class Rendering
    {

        public static void DrawGameObject<T>(GameEngine engine, T obj) where T : GameObjects.GameObject
        {
            if (obj.BoundedBox.Radius == 0) throw new System.Exception($"BoundedBox Radius is Zero {obj}");
            if (engine.FrustumCuller.CircleOnWorldView(obj.BoundedBox))
            {
                obj.Draw(engine);
            }
        }

        public static void DrawGameObjects<T>(GameEngine engine, HashSet<T> objects) where T : GameObjects.GameObject
        {
            foreach (T obj in objects)
            {
                if (obj.BoundedBox.Radius == 0) throw new System.Exception($"BoundedBox Radius is Zero {obj}");
                if (engine.FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                {
                    obj.Draw(engine);
                }
            }
        }
    }
}
