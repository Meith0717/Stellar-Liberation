// RenderPipeline.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    public class RenderPipeline<T> where T : GameObject2D
    {
        private readonly ViewFrustumFilter mViewFrustumFilter;
        private readonly SpatialHashing<T> mSpatialHashing;
        private List<T> mObjects = new();

        public RenderPipeline(ViewFrustumFilter viewFrustumFilter, SpatialHashing<T> spatialHashing)
        {
            mViewFrustumFilter = viewFrustumFilter;
            mSpatialHashing = spatialHashing;
        }

        public void Update()
        {
            var space = mViewFrustumFilter.WorldFrustum.ToRectangle();
            var edgeDistance = Vector2.Distance(space.Center.ToVector2(), space.Location.ToVector2());
            mObjects = mSpatialHashing.GetObjectsInRadius<T>(space.Center.ToVector2(), (int)edgeDistance);
        }

        public void Render(Scene scene)
        {
            foreach (var obj in mObjects) obj.Draw(scene);
        }
    }
}
