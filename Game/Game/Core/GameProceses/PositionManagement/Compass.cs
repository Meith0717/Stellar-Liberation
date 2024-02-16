// Compass.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    public class Compass
    {
        private readonly Dictionary<Vector2, string> mKeyValuePairs = new();
        private CircleF mCompass;

        public void Update(Vector2 position, GraphicsDevice graphicsDevice, List<GameObject2D> objects)
        {
            mKeyValuePairs.Clear();
            mCompass = new CircleF(graphicsDevice.Viewport.Bounds.Center, MathF.Min(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height) * 0.45f);
            foreach (var obj in objects)
            {
                if (graphicsDevice.Viewport.Bounds.Intersects(obj.BoundedBox)) continue;
                if (obj is LaserProjectile || obj is Particle || obj is Asteroid) continue;
                var rectangle = graphicsDevice.Viewport.Bounds;
                rectangle.Inflate(-25, -25);
                var pos = Geometry.GetPointOnCircle(mCompass, Geometry.AngleBetweenVectors(position, obj.Position));
                mKeyValuePairs[pos] = obj.TextureId;
            }
        }

        public void Draw()
        {
            TextureManager.Instance.DrawCircle(mCompass.Position, mCompass.Radius, new(2, 2, 2, 2), 5, 1);
            foreach (var pos in mKeyValuePairs.Keys) TextureManager.Instance.Draw(mKeyValuePairs[pos], pos - new Vector2(10, 10), 20, 20);
        }
    }
}
