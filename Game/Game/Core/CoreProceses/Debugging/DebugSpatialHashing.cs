// DebugSpatialHashing.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.Core.CoreProceses.Debugging
{
    internal static class DebugSpatialHashing
    {
        public static void ObjectsInBucket(GameLayer scene)
        {
            RectangleF rectangle = new(scene.WorldMousePosition, new(10000, 10000));
            TextureManager.Instance.DrawAdaptiveRectangleF(rectangle, Color.Orange, 1, 1, scene.Camera2D.Zoom);
            var GameObjects1 = scene.SpatialHashing.GetObjectsInRectangle<GameObject2D>(rectangle);
            foreach (var obj in GameObjects1)
            {
                TextureManager.Instance.DrawAdaptiveLine(scene.WorldMousePosition, obj.Position, Color.Orange,
                    2, TextureManager.MaxLayerDepth, scene.Camera2D.Zoom);
            }
            TextureManager.Instance.DrawAdaptiveCircle(scene.WorldMousePosition, 10000, Color.Blue, 1 , 1, scene.Camera2D.Zoom);
            var GameObjects = scene.SpatialHashing.GetObjectsInRadius<GameObject2D>(scene.WorldMousePosition, 10000);
            foreach (var obj in GameObjects)
            {
                TextureManager.Instance.DrawAdaptiveLine(scene.WorldMousePosition, obj.Position, Color.Blue,
                    2, TextureManager.MaxLayerDepth, scene.Camera2D.Zoom);
            }
        }

        public static void Buckets(GameLayer scene, Vector2 mousePosition)
        {
            var screen = scene.Camera2D.Bounds;
            var size = scene.SpatialHashing.mCellSize;
            var depth = (int)TextureManager.MaxLayerDepth;

            for (int x = 0; x < screen.Right; x += size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Top), new(x, screen.Bottom), new Color(100, 100, 100, 100), 1, depth, scene.Camera2D.Zoom);
            }
            for (int y = 0; y < screen.Bottom; y += size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Left, y), new(screen.Right, y), new Color(100, 100, 100, 100), 1, depth, scene.Camera2D.Zoom);
            }
            for (int x = -size; x > screen.Left; x -= size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Bottom), new(x, screen.Top), new Color(100, 100, 100, 100), 1, depth, scene.Camera2D.Zoom);
            }
            for (int y = -size; y > screen.Top; y -= size)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Right, y), new(screen.Left, y), new Color(100, 100, 100, 100), 1, depth, scene.Camera2D.Zoom);
            }

            var hash = scene.SpatialHashing.Hash((int)mousePosition.X, (int)mousePosition.Y);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, mousePosition + new Vector2(1, -5) * 5 / scene.Camera2D.Zoom, $"ID: {hash}", .7f / scene.Camera2D.Zoom, Color.White);
        }
    }
}
