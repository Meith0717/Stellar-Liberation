// Planet.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    [Collidable]
    public class Planet : GameObject2D
    {
        [JsonProperty] public float OrbitRadians;
        [JsonIgnore] private float mShadowRotation;
        [JsonProperty] private readonly int OrbitRadius;
        [JsonIgnore] private readonly Vector2 mMainBodyPosition;

        public Planet(int distanceToStar, float orbitAnle, string textureId, float textureScale)
            : base(Vector2.Zero, textureId, textureScale, 1)
        {
            mMainBodyPosition = Vector2.Zero;
            OrbitRadius = distanceToStar;
            OrbitRadians = orbitAnle;

            Position = Geometry.GetPointOnCircle(mMainBodyPosition, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, mMainBodyPosition) + MathF.PI;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            base.Update(gameTime, inputState, scene);
            OrbitRadians -= 0.00001f;
            Position = Geometry.GetPointOnCircle(mMainBodyPosition, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, mMainBodyPosition) + MathF.PI;
            Rotation -= 0.0001f;
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.planetShadow, Position, TextureOffset, TextureScale * 1.05f, mShadowRotation, TextureDepth + 1, Color.White);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
