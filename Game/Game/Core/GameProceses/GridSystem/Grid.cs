// Grid.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;

namespace StellarLiberation.Game.Core.GameProceses.GridSystem
{
    public class Grid
    {
        private int gridSize;

        public Grid(int size)
        {
            gridSize = size;
        }

        public void Draw(Scene scene)
        {
            var screen = scene.Camera2D.Bounds;
            var depth = (int)TextureManager.Instance.MaxLayerDepth;

            var alpha = MathHelper.Clamp(5 * scene.Camera2D.Zoom, 0, 5);
            var color = new Color(alpha, alpha, alpha, alpha);

            for (int x = 0; x < screen.Right; x += gridSize)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Top), new(x, screen.Bottom), color, 1, depth, scene.Camera2D.Zoom);
            }
            for (int y = 0; y < screen.Bottom; y += gridSize)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Left, y), new(screen.Right, y), color, 1, depth, scene.Camera2D.Zoom);
            }
            for (int x = -gridSize; x > screen.Left; x -= gridSize)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(x, screen.Bottom), new(x, screen.Top), color, 1, depth, scene.Camera2D.Zoom);
            }
            for (int y = -gridSize; y > screen.Top; y -= gridSize)
            {
                TextureManager.Instance.DrawAdaptiveLine(new(screen.Right, y), new(screen.Left, y), color, 1, depth, scene.Camera2D.Zoom);
            }
        }
    }
}
