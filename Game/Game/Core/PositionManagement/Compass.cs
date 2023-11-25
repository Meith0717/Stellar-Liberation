// Compass.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.PositionManagement
{
    public class Compass
    {
        private readonly Dictionary<Vector2, string> mKeyValuePairs = new();

        public void Update(Vector2 position, ViewFrustumFilter viewFrustumFilter, List<SpaceShip> objects)
        {
            mKeyValuePairs.Clear();
            foreach (var obj in objects)
            {
                if (viewFrustumFilter.CircleOnWorldView(obj.BoundedBox)) continue;
                var rectangle = viewFrustumFilter.ViewFrustum;
                rectangle.Inflate(-10, -10);
                var pos = Geometry.GetPoitOnRectangle(rectangle, Geometry.AngleBetweenVectors(position, obj.Position));
                mKeyValuePairs[pos] = obj.TextureId;
            }
        }

        public void Draw()
        {
            foreach (var pos in mKeyValuePairs.Keys) TextureManager.Instance.Draw(mKeyValuePairs[pos], pos, 20, 20);
        }
    }
}
