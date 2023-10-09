using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : GameObject
    {
        [JsonIgnore] public float Velocity { get; set; } = 0;
        [JsonIgnore] public SpaceShip Target { get; set; }
        [JsonIgnore] public SensorArray SensorArray { get; protected set; } = new(2000000, 1000);
        [JsonIgnore] public SublightEngine SublightEngine { get; protected set; } = new(100);
        [JsonIgnore] public HyperDrive HyperDrive { get; protected set; } = new(6000, 100);
        [JsonIgnore] public Weapons WeaponSystem { get; protected set; }
        [JsonProperty] public DefenseSystem DefenseSystem { get; protected set; } = new(100, 100, 0, 1);
        [JsonIgnore] public PlanetSystem ActualPlanetSystem { get; set; }


        public SpaceShip(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale, 10) { }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            Position += Geometry.CalculateDirectionVector(Rotation) * (float)(Velocity * gameTime.ElapsedGameTime.Milliseconds);
            AddToSpatialHashing(scene);

            HyperDrive.Update(gameTime, this);
            SublightEngine.Update(this);
            WeaponSystem.Update(gameTime, inputState, this, sceneManagerLayer, scene);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, ActualPlanetSystem, scene);
            CheckForHit(scene);
            base.Update(gameTime, inputState, sceneManagerLayer , scene);
        }

        private void CheckForHit(Scene scene)
        {
            var projectileInRange = scene.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            foreach (var projectile in projectileInRange)
            {
                if (!BoundedBox.Intersects(projectile.BoundedBox) || this == projectile.Origine) continue;

                if (projectile.Origine is Enemy && this is Enemy) continue;  


                projectile.HasHit = true;
                DefenseSystem.GetDamage(projectile.ShieldDamage, projectile.HullDamage);
            }
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            DefenseSystem.DrawShields(this);
            WeaponSystem.Draw(sceneManagerLayer, scene);
        }
    }
}
