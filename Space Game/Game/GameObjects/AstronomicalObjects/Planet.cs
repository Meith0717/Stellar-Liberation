﻿using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class Planet : GameObject
    {
        [JsonProperty]
        public Vector2 OrbitCenter { get; private set; }
        [JsonProperty]
        public int OrbitRadius { get; private set; }
        [JsonProperty]
        public float OrbitRadians { get; private set; }


        public Planet(Vector2 orbitCenter, int orbitRadius, string textureId, float textureScale) 
            : base(Vector2.Zero, textureId, textureScale, 2)
        {
            OrbitCenter = orbitCenter;
            OrbitRadius = orbitRadius;
            OrbitRadians = Utility.Random.NextSingle() * MathF.PI * 2;

            Position = Geometry.GetPointOnCircle(OrbitCenter, OrbitRadius, OrbitRadians);
            UpdateBoundBox();
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}