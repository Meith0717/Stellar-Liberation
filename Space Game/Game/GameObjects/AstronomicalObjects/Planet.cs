using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.GameEngine.Content_Management;
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
        [JsonIgnore]
        private float mShadowRotation;

        public Planet(Vector2 orbitCenter, int orbitRadius, string textureId, float textureScale) 
            : base(Vector2.Zero, textureId, textureScale, 1)
        {
            OrbitCenter = orbitCenter;
            OrbitRadius = orbitRadius;
            OrbitRadians = Utility.Random.NextSingle() * (MathF.PI * 2);

            Position = Geometry.GetPointOnCircle(OrbitCenter, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, OrbitCenter) + MathF.PI;
            UpdateBoundBox();
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            RemoveFromSpatialHashing(sceneLayer);
            OrbitRadians -= 0.001f;
            Position = Geometry.GetPointOnCircle(OrbitCenter, OrbitRadius, OrbitRadians);
            mShadowRotation = Geometry.AngleBetweenVectors(Position, OrbitCenter) + MathF.PI;
            Rotation -= 0.001f;
            AddToSpatialHashing(sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.Draw(ContentRegistry.planetShadow, Position, TextureOffset, TextureScale, mShadowRotation, TextureDepth + 1, Color.White);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
