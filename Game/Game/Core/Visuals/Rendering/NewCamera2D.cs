﻿// NewCamera2D.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    public class NewCamera2D
    {
        public Vector2 Position;
        public float Zoom;
        public float Rotation;

        public RectangleF Bounds { get; private set; }
        private readonly RenderPipeline<GameObject2D> mRenderPipeline;

        public NewCamera2D()
        {
            Position = Vector2.Zero;
            Zoom = 1;
            Rotation = 0;

            mRenderPipeline = new();
        }

        public void Update(GraphicsDevice graphicsDevice, Scene scene)
        {
            var transformationMatrix = Transformations.CreateViewTransformationMatrix(Position, Zoom, Rotation, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            Bounds = FrustumCuller.GetFrustum(graphicsDevice, transformationMatrix);
            mRenderPipeline.FilterObjs(Bounds, scene.SpatialHashing);
        }

        public bool Contains(Vector2 position) => Bounds.Contains(position);

        public bool Intersects(CircleF circle) => Bounds.Intersects(circle);

        public bool Intersects(RectangleF rectangle) => Bounds.Intersects(rectangle);

        public void Draw(Scene scene)
        {
            mRenderPipeline.RenderFiltredObjs(scene);
        }
    }
}
