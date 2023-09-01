using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.MovementSystems;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

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

        public Player(Vector2 position) : base(position, ContentRegistry.ship.Name, 10) { Inventory = new(16); }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            if (inputState.mActionList.Contains(ActionType.FireInitialWeapon))
            {
                if (GetTarget(out var target))
                {
                    WeaponSlot1.Fire(target);
                    WeaponSlot2.Fire(target);
                    WeaponSlot3.Fire(target);
                    WeaponSlot4.Fire(target);
                }
            }
            var targetAngle = Geometry.AngleBetweenVectors(Position, sceneLayer.WorldMousePosition);
            Rotation += MovementController.GetRotationUpdate(Rotation, targetAngle, 0.07f);

            ManageVelocity(inputState);
            WeaponSlot1.Update(this, gameTime, inputState, sceneLayer);
            WeaponSlot2.Update(this, gameTime, inputState, sceneLayer);
            WeaponSlot3.Update(this, gameTime, inputState, sceneLayer);
            WeaponSlot4.Update(this, gameTime, inputState, sceneLayer);
            CollectItems(sceneLayer);

            base.Update(gameTime, inputState, sceneLayer);
            sceneLayer.Camera.SetPosition(Position);
        }


        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
        }

        private void ManageVelocity(InputState inputState)
        {
            var sublightVelocity = 10;

            if (inputState.mActionList.Contains(ActionType.Deacceleration))
            {
                Velocity = MovementController.GetVelocity(Velocity, -0.2f);
                Velocity = (Velocity < 0) ? 0 : Velocity;
            }
            if (inputState.mActionList.Contains(ActionType.Acceleration))
            {
                Velocity = (Velocity <= 0) ? 1 : Velocity;
                Velocity = MovementController.GetVelocity(Velocity, 0.2f);
                Velocity = (Velocity > sublightVelocity) ? sublightVelocity : Velocity;
            }
        }

        private void CollectItems(SceneLayer sceneLayer)
        {
            var objects = sceneLayer.GetObjectsInRadius<Item>(Position, 500);
            foreach (var item in objects)
            {
                if (!BoundedBox.Contains(item.Position) || !Inventory.AddItem(item)) continue;
                item.Collect();
            }
        }
    }
}
