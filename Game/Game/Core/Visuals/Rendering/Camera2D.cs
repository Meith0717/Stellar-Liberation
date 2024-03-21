// Camera2D.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    public class Camera2D
    {
        public Vector2 Position;
        public float Zoom;
        public float Rotation;

        public RectangleF Bounds { get; private set; }
        private readonly RenderPipeline mRenderPipeline;

        public Camera2D()
        {
            Position = Vector2.Zero;
            Zoom = 1;
            Rotation = 0;

            mRenderPipeline = new();
        }

        public void ApplyResolution(Resolution resolution, GameLayer scene)
        {
            var transformationMatrix = Transformations.CreateViewTransformationMatrix(Position, Zoom, Rotation, resolution.Width, resolution.Height);
            Bounds = FrustumCuller.GetFrustum(resolution, transformationMatrix);
            mRenderPipeline.FilterObjs(Bounds, scene.SpatialHashing);
        }

        public bool Contains(Vector2 position) => Bounds.Contains(position);

        public bool Intersects(CircleF circle) => Bounds.Intersects(circle);

        public bool Intersects(RectangleF rectangle) => Bounds.Intersects(rectangle);

        public void Draw(GameLayer scene)
        {
            mRenderPipeline.RenderFiltredObjs(scene);
        }
    }
}
