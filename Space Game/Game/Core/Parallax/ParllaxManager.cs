﻿using CelestialOdyssey.Core.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Parallax
{
    internal class ParllaxManager
    {
        private readonly List<ParllaxBackground> mBackdrounds = new();

        public void Update(Vector2 cameraMovement)
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Update(cameraMovement * 0.0035f);
            }
        }

        public void Draw()
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Draw();
            }
        }

        public void Add(ParllaxBackground backdround)
        {
            mBackdrounds.Add(backdround);
        }

        public void OnResolutionChanged(GraphicsDevice graphicsDevice)
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.OnResolutionChanged(graphicsDevice);
            }
        }
    }
}
