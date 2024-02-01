// DebugSpatialHashing.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;

namespace StellarLiberation.Game.Core.CoreProceses.Debugger
{
    internal static class DebugSpatialHashing
    {
        public static void ObjectsInBucket(GameLayer scene)
        {
            var GameObjects = scene.SpatialHashing.GetObjectsInBucket((int)scene.WorldMousePosition.X, (int)scene.WorldMousePosition.Y);
            Color color = Color.Green;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                var obj = GameObjects[i];
                if (i > 0) color = Color.Blue;
                TextureManager.Instance.DrawAdaptiveLine(scene.WorldMousePosition, obj.Position, color,
                    2, (int)TextureManager.MaxLayerDepth, scene.Camera2D.Zoom);
            }
        }

        public static void Buckets(GameLayer scene, Vector2 mousePosition)
        {
            var screen = scene.Camera2D.Bounds;
            var size = scene.SpatialHashing.CellSize;
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
            TextureManager.Instance.DrawString(FontRegistries.debugFont, mousePosition + new Vector2(1, -5) * 5 / scene.Camera2D.Zoom, hash.ToString(), 1 / scene.Camera2D.Zoom, Color.White);
        }
    }
}
