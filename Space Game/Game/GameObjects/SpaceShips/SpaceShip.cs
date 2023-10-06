﻿using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : GameObject
    {
        [JsonIgnore] 
        public float Velocity { get; set; } = 0;
        [JsonIgnore] 
        public SpaceShip Target { get; set; }
        [JsonProperty]
        public PlanetSystem ActualSystem { get; private set; }

        [JsonIgnore]
        public SensorArray SensorArray { get; protected set; } = new(2000000, 1000);
        [JsonIgnore] 
        public SublightEngine SublightEngine { get; protected set; } = new(50);
        [JsonIgnore] 
        public HyperDrive HyperDrive { get; protected set; } = new(6000, 100);
        [JsonIgnore] 
        public WeaponSystem WeaponSystem { get; protected set; }
        [JsonProperty] 
        public DefenseSystem DefenseSystem { get; protected set; } = new(1000, 1000, 0, 1);


        public SpaceShip(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale, 10) { }

        public void SetActualSystem(PlanetSystem planetSystem)
        {
            ActualSystem = planetSystem;
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            RemoveFromSpatialHashing(sceneLayer);
            Position += Geometry.CalculateDirectionVector(Rotation) * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            AddToSpatialHashing(sceneLayer);

            HyperDrive.Update(gameTime, this);
            WeaponSystem.Update(gameTime, inputState, sceneLayer);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, sceneLayer);
            CheckForHit();
            base.Update(gameTime, inputState, sceneLayer);
        }

        private void CheckForHit()
        {
            var projectileInRange = SensorArray.SortedObjectsInRange.OfType<Projectile>();
            if (!projectileInRange.Any()) return;
            foreach (var projectile in projectileInRange)
            {
                if (!projectile.BoundedBox.Intersects(BoundedBox) || this == projectile.Origin) continue;
                projectile.HasHit = true;
                DefenseSystem.GetDamage(projectile.ShieldDamage, projectile.HullDamage);
            }
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            DefenseSystem.DrawShields(this);
        }
    }
}
