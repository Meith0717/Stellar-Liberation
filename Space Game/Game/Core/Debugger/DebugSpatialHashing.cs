using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.DebugSystem
{
    internal static class DebugSpatialHashing
    {
        public static void ObjectsInBucket(Scene scene)
        {
            var GameObjects = scene.SpatialHashing.GetObjectsInBucket((int)scene.WorldMousePosition.X, (int)scene.WorldMousePosition.Y);
            Color color = Color.Green;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                var obj = GameObjects[i];
                if (i > 0) color = Color.Blue;
                TextureManager.Instance.DrawAdaptiveLine(scene.WorldMousePosition, obj.Position, color,
                    2, (int)TextureManager.Instance.MaxLayerDepth, scene.Camera.Zoom);
            }
        }

        public static void Buckets(Scene scene, Vector2 mousePosition)
        {
            var screen = scene.FrustumCuller.WorldFrustum;
            var size = scene.SpatialHashing.CellSize;
            var depth = (int)TextureManager.Instance.MaxLayerDepth;

            for (int x = 0; x < screen.Right; x += size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Top), new(x, screen.Bottom), new Color(100, 100, 100, 100), 1, depth, scene.Camera.Zoom);
            }
            for (int y = 0; y < screen.Bottom; y += size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Left, y), new(screen.Right, y), new Color(100, 100, 100, 100), 1, depth, scene.Camera.Zoom);
            }
            for (int x = -size; x > screen.Left; x -= size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Bottom), new(x, screen.Top), new Color(100, 100, 100, 100), 1, depth, scene.Camera.Zoom);
            }
            for (int y = -size; y > screen.Top; y -= size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Right, y), new(screen.Left, y), new Color(100, 100, 100, 100), 1, depth, scene.Camera.Zoom);
            }

            var hash = scene.SpatialHashing.Hash((int)mousePosition.X, (int)mousePosition.Y);
            TextureManager.Instance.DrawString("text", mousePosition + (new Vector2(1, -5) * 5 / scene.Camera.Zoom), hash.ToString(), 1 / scene.Camera.Zoom, Color.White);
        }
    }
}
