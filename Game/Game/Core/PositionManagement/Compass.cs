// Compass.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.PositionManagement
{
    public class Compass
    {
        private readonly Dictionary<Vector2, string> mKeyValuePairs = new();
        private CircleF mCompass;

        public void Update(Vector2 position, ViewFrustumFilter viewFrustumFilter, List<SpaceShip> objects)
        {
            mKeyValuePairs.Clear();
            mCompass = new CircleF(viewFrustumFilter.ViewFrustum.Center, MathF.Min(viewFrustumFilter.ViewFrustum.Width, viewFrustumFilter.ViewFrustum.Height) * 0.45f );
            foreach (var obj in objects)
            {
                if (viewFrustumFilter.CircleOnWorldView(obj.BoundedBox)) continue;
                var rectangle = viewFrustumFilter.ViewFrustum;
                rectangle.Inflate(-25, -25);
                var pos = Geometry.GetPointOnCircle(mCompass, Geometry.AngleBetweenVectors(position, obj.Position));
                mKeyValuePairs[pos] = obj.TextureId;
            }
        }

        public void Draw()
        {
            TextureManager.Instance.DrawCircle(mCompass.Position, mCompass.Radius, new(20, 20, 10, 10), 1, 1);
            foreach (var pos in mKeyValuePairs.Keys) TextureManager.Instance.Draw(mKeyValuePairs[pos], pos, 20, 20);
        }
    }
}
