// ObjectsRenderer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Rendering
{
    public class RenderPipeline<T> where T : GameObject
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
