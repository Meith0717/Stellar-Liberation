// ParallaxController.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.Visuals.Rendering;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Visuals.ParallaxSystem
{
    internal class ParallaxController
    {
        private readonly List<ParllaxBackground> mBackdrounds = new();
        private readonly Camera2D mCamera2D;
        private Vector2 mLastPosition;

        public ParallaxController(Camera2D camera2D)
        {
            mCamera2D = camera2D;
            mLastPosition = mCamera2D.Position;
        }

        public void Update()
        {
            var movement = Vector2.Negate(mLastPosition - mCamera2D.Position);
            mLastPosition = mCamera2D.Position;
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Update(movement * mCamera2D.Zoom);
            }
        }

        public void Draw()
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Draw();
            }
        }

        public void Add(ParllaxBackground backdround) => mBackdrounds.Add(backdround);

        public void OnResolutionChanged(GraphicsDevice graphicsDevice)
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.OnResolutionChanged(graphicsDevice);
            }
        }
    }
}
