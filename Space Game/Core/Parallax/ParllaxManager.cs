﻿using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Effects
{
    internal class ParllaxManager
    {
        private readonly List<ParllaxBackground> mBackdrounds = new();

        public void Update(Vector2 cameraMovement, float cameraZoom)
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Update(cameraMovement * cameraZoom);
            }
        }

        public void Draw(TextureManager textureManager)
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Draw(textureManager);
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
