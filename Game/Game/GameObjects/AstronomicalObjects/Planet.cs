﻿// Planet.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
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
        [JsonProperty]
        public Vector2 OrbitCenter { get; private set; }
        [JsonProperty]
        public int OrbitRadius { get; private set; }
        [JsonProperty]
        public float OrbitRadians { get; private set; }
        [JsonIgnore]
        private float mShadowRotation;

        public Planet(Vector2 orbitCenter, int orbitRadius, string textureId, float textureScale)
            : base(Vector2.Zero, textureId, textureScale, 1)
        {
            OrbitCenter = orbitCenter;
            OrbitRadius = orbitRadius;
            OrbitRadians = ExtendetRandom.Random.NextSingle() * (MathF.PI * 2);

            Position = Geometry.GetPointOnCircle(OrbitCenter, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, OrbitCenter) + MathF.PI;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            RemoveFromSpatialHashing(scene);
            OrbitRadians -= 0.00001f;
            Position = Geometry.GetPointOnCircle(OrbitCenter, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, OrbitCenter) + MathF.PI;
            Rotation -= 0.0001f;
            AddToSpatialHashing(scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.planetShadow, Position, TextureOffset, TextureScale * 1.05f, mShadowRotation, TextureDepth + 1, Color.White);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
