// Planet.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class Planet : GameObject2D, ICollidable
    {
        [JsonIgnore] private readonly Vector2 MainBodyPosition = new(0);
        [JsonIgnore] private float mShadowRotation;

        [JsonIgnore] public float Mass => float.PositiveInfinity;

        public Planet(int distanceToStar, float orbitAnle, string textureId, float textureScale)
            : base(Vector2.Zero, textureId, textureScale, 1)
        {
            Velocity = (float)(Constants.GravitationalConstant * Math.Pow(10, 16) / distanceToStar) * 0.00001f;

            Position = Geometry.GetPointOnCircle(MainBodyPosition, distanceToStar, orbitAnle);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, MainBodyPosition) + MathF.PI;
        }


        public override void Update(GameTime gameTime, InputState inputState, GameState gameState, PlanetsystemState planetsystemState)
        {
            base.Update(gameTime, inputState, gameState, planetsystemState);
            MovingDirection = Transformations.RotateVector(Vector2.Normalize(MainBodyPosition - Position), MathHelper.Pi / 2);
            GameObject2DMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, MainBodyPosition) + MathF.PI;
            Rotation -= (float)(MathF.PI / (24 * 60000) * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.planetShadow, Position, TextureOffset, TextureScale * 1.03f, mShadowRotation, TextureDepth + 1, Color.White * 0.875f);
            TextureManager.Instance.DrawGameObject(this);
        }

        public Vector2 GetPositionInOrbit(Vector2 actualPosition)
        {
            var dirToActualPosition = Vector2.Normalize(actualPosition - Position);
            var position = (dirToActualPosition * (BoundedBox.Radius + 500)) + Position;
            return position;
        }
    }
}
