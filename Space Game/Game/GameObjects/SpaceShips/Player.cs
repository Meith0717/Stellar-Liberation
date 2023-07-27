using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Player : SpaceShip
    {
        public Player() : base(new Vector2(1000, 1000), "ship", 1, 0) { }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {

            if (inputState.mGamePadValues.mLeftThumbSticks != Vector2.Zero)
            {
                var targetAngle = Geometry.AngleBetweenVectors(Vector2.Zero, inputState.mGamePadValues.mLeftThumbSticks);
                Rotation += MovementController.GetRotationUpdate(Rotation, targetAngle, 0.02f);
            }

            ManageVelocity(inputState);

            base.Update(gameTime, inputState, gameEngine);
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

            var maxSpeed = 1f;

            if (mToggleMaxSpeed) maxSpeed = mMaxVelocity;

            var maxThumbStickVal = MathF.Max(
                MathF.Abs(inputState.mGamePadValues.mLeftThumbSticks.X),
                MathF.Abs(inputState.mGamePadValues.mLeftThumbSticks.Y)
                ) * maxSpeed;

            if (maxThumbStickVal <= 0 || maxThumbStickVal < Velocity)
            {
                if (mToggleMaxSpeed) return;
                Velocity = (Velocity <= 0)
                    ? 0 : MovementController.GetVelocity(Velocity, -0.05f * maxSpeed);
                return;
            }
            if (maxThumbStickVal < Velocity) return;
            Velocity = (Velocity > maxThumbStickVal)
                ? maxThumbStickVal : MovementController.GetVelocity(Velocity, 0.05f * maxSpeed); ;
        }
    }
}
