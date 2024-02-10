// RenderPipeline.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    /// <summary>
    /// A generic class managing a list of 2D game objects and providing rendering functionality.
    /// </summary>
    public class RenderPipeline
    {
        /// <summary>
        /// List of 2D game objects managed by the render pipeline.
        /// </summary>
        private List<GameObject2D> mObjects = new();

        /// <summary>
        /// Filters 2D game objects based on a specified view frustum using spatial hashing.
        /// </summary>
        public void FilterObjs(RectangleF viewFrustum, SpatialHashing spatialHashing)
        {
            var space = viewFrustum.ToRectangle();
            var edgeDistance = Vector2.Distance(space.Center.ToVector2(), space.Location.ToVector2());
            mObjects = spatialHashing.GetObjectsInRadius<GameObject2D>(space.Center.ToVector2(), (int)edgeDistance);
        }

        /// <summary>
        /// Renders the filtered 2D game objects in a given scene.
        /// </summary>
        public void RenderFiltredObjs(GameLayer scene)
        {
            foreach (var obj in mObjects) obj.Draw(scene);
        }
    }
}
