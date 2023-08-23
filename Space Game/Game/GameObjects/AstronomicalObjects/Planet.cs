using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public class PlanetConfig
    {
        public readonly string TextureId;
        public readonly float TextureScale;

        public PlanetConfig(string textureId, float textureScale)
        {
            TextureId = textureId;
            TextureScale = textureScale;
        }
    }

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
            TextureManager.Instance.DrawAdaptiveCircle(OrbitCenter, OrbitRadius, new Color(5, 5, 5, 5), 1, TextureDepth - 1, engine.Camera.Zoom);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
