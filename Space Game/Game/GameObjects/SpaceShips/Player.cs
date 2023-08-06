using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.WeaponSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {
        [JsonProperty] public CargoHold Inventory { get; private set; }

        public Player() : base(new Vector2(1000, 1000), ContentRegistry.ship.Name, 1) { Inventory = new(16); }

        public new void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine, WeaponManager weaponManager)
        {
            if (inputState.mActionList.Contains(ActionType.FireSecondaryWeapon))
            {
                FireSecondaryWeapon(weaponManager);
            }
            if (inputState.mActionList.Contains(ActionType.FireInitialWeapon))
            {
                FireInitialWeapon(weaponManager);
            }


            if (inputState.mGamePadValues.mLeftThumbSticks != Vector2.Zero)
            {
                var targetAngle = Geometry.AngleBetweenVectors(Vector2.Zero, inputState.mGamePadValues.mLeftThumbSticks);
                Rotation += MovementController.GetRotationUpdate(Rotation, targetAngle, 0.07f);
            }

            ManageVelocity(inputState);
            CollectItems(gameEngine);

            base.Update(gameTime, inputState, gameEngine, weaponManager);
            gameEngine.Camera.SetPosition(Position);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }

        private bool mToggleMaxSpeed;

        private void ManageVelocity(InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.MaxAcceleration)) mToggleMaxSpeed = !mToggleMaxSpeed;

            var stickValue = inputState.mGamePadValues.mLeftThumbSticks.Length();
            var maxSpeed = stickValue * (mToggleMaxSpeed ? mMaxVelocity : 1);

            if (Velocity > maxSpeed)
            {
                if (mToggleMaxSpeed) return;
                Velocity = MovementController.GetVelocity(Velocity, -0.05f);
                Velocity = (Velocity < 0) ? 0 : Velocity;
            } 
            else if (Velocity < maxSpeed)
            {
                Velocity = MovementController.GetVelocity(Velocity, 0.05f);
                Velocity = (Velocity > maxSpeed) ? maxSpeed : Velocity;
            }
        }

        private void CollectItems(GameEngine.GameEngine engine)
        {
            var objects = engine.GetObjectsInRadius<Item>(Position, 500);
            foreach (var item in objects)
            {
                if (!BoundedBox.Contains(item.Position) || !Inventory.AddItem(item)) continue;
                item.Collect();
            }
        }
    }
}
