using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : GameObject
    {
        public float Velocity { get; set; } = 0;
        [JsonIgnore] public SensorArray SensorArray { get; private set; } = new(30000000);
        [JsonIgnore] public SublightEngine SublightEngine { get; private set; } = new(200);
        [JsonIgnore] public HyperDrive HyperDrive { get; private set; } = new(6000, 100);
        [JsonProperty] public DefenseSystem DefenseSystem { get; private set; } = new(100, 100, 1, 1);

        public SpaceShip(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale, 10) { }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            RemoveFromSpatialHashing(sceneLayer);
            Position += Geometry.CalculateDirectionVector(Rotation) * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            AddToSpatialHashing(sceneLayer);

            HyperDrive.Update(gameTime, this);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(Position, sceneLayer);
            base.Update(gameTime, inputState, sceneLayer);
        }
    }
}
