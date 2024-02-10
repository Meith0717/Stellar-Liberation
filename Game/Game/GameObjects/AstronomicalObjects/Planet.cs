// Planet.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics;
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
        [JsonIgnore] private readonly Vector2 MainBodyPosition = Vector2.Zero;
        [JsonIgnore] private float mShadowRotation;
        [JsonProperty] private readonly int OrbitRadius;

        public Planet(int distanceToStar, float orbitAnle, string textureId, float textureScale)
            : base(Vector2.Zero, textureId, textureScale, 1)
        {
            OrbitRadius = distanceToStar;
            Velocity = (float)(Constants.GravitationalConstant * Math.Pow(10, 16)/distanceToStar);

            Position = Geometry.GetPointOnCircle(MainBodyPosition, OrbitRadius, orbitAnle);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, MainBodyPosition) + MathF.PI;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            base.Update(gameTime, inputState, scene);
            MovingDirection = Transformations.RotateVector(Vector2.Normalize(MainBodyPosition - Position), MathHelper.Pi / 2);
            GameObject2DMover.Move(gameTime, this, scene.SpatialHashing);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, MainBodyPosition) + MathF.PI;
            Rotation -= (float)(0.000005 * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.planetShadow, Position, TextureOffset, TextureScale * 1.05f, mShadowRotation, TextureDepth + 1, Color.White);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
