using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {
        [JsonProperty] public CargoHold Inventory { get; private set; }
        [JsonProperty] private Weapon WeaponSlot1 = new PhotonPhaser(new(200, 0));
        [JsonProperty] private Weapon WeaponSlot2 = new PhotonPhaser(new(-200, 0));
        [JsonProperty] private Weapon WeaponSlot3 = new PhotonTorpedo(new(210, 50));
        [JsonProperty] private Weapon WeaponSlot4 = new PhotonTorpedo(new(210, -50));
        [JsonProperty] private List<SolarSystem> Targets;

        public Player(Vector2 position) : base(position, ContentRegistry.ship.Name, 1) { Inventory = new(16); }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.FireInitialWeapon))
            {
                if (GetTarget(out var target))
                {
                    WeaponSlot1.Fire(target, GameLayer);
                    WeaponSlot2.Fire(target, GameLayer);
                    WeaponSlot3.Fire(target, GameLayer);
                    WeaponSlot4.Fire(target, GameLayer);
                }
            }
            var targetAngle = Geometry.AngleBetweenVectors(Position, GameLayer.WorldMousePosition);
            Rotation += MovementController.GetRotationUpdate(Rotation, targetAngle, 0.07f);

            ManageVelocity(inputState);
            WeaponSlot1.Update(this, gameTime, inputState);
            WeaponSlot2.Update(this, gameTime, inputState);
            WeaponSlot3.Update(this, gameTime, inputState);
            WeaponSlot4.Update(this, gameTime, inputState);
            CollectItems();

            base.Update(gameTime, inputState);
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this);
        }

        private void ManageVelocity(InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.Deacceleration))
            {
                Velocity = MovementController.GetVelocity(Velocity, -0.1f);
                Velocity = (Velocity < 0) ? 0 : Velocity;
            }
            if (inputState.mActionList.Contains(ActionType.Acceleration))
            {
                Velocity = (Velocity <= 0) ? 1 : Velocity;
                Velocity = MovementController.GetVelocity(Velocity, 0.1f);
                Velocity = (Velocity > mMaxVelocity) ? mMaxVelocity : Velocity;
            }
        }

        private void CollectItems()
        {
            var objects = GameLayer.GetObjectsInRadius<Item>(Position, 500);
            foreach (var item in objects)
            {
                if (!BoundedBox.Contains(item.Position) || !Inventory.AddItem(item)) continue;
                item.Collect();
            }
        }
    }
}
