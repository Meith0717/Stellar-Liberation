using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.GameObjects.Weapons;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Player : SpaceShip
    {

        private float mAcceleration;
        private float mDeacceleration;

        public Player() : base(Vector2.Zero, "ship", 1, 0) { }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine, WeaponManager weaponManager)
        {
            if (SpaceshipsInRadius(gameEngine, out var spaceShips))
            {
                if (inputState.mActionList.Contains(ActionType.ShootProjectile))
                {
                    weaponManager.AddProjectile(this, spaceShips[0]);
                }
            }

            if (inputState.mGamePadValues.mLeftThumbSticks != Vector2.Zero)
            {
                var targetAngle = Geometry.AngleBetweenVectors(Vector2.Zero, inputState.mGamePadValues.mLeftThumbSticks);
                Rotation += ManageRotation(Rotation, targetAngle);
            }

            Velocity += ControllVelocity(inputState);
            Velocity = (Velocity < 0) ? 0 : Velocity;
            Velocity = (Velocity > mMaxVelocity) ? mMaxVelocity : Velocity;

            base.Update(gameTime, inputState, gameEngine, weaponManager);
            gameEngine.Camera.SetPosition(Position);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }

        
        private float ControllVelocity(InputState inputState) 
        {
            var rightTrigger = inputState.mGamePadValues.mRightTrigger;
            var leftTrigger = inputState.mGamePadValues.mLeftTrigger;
            if (inputState.mActionList.Contains(ActionType.Accelerate)) return 0.1f * rightTrigger;
            if (inputState.mActionList.Contains(ActionType.Decelerate)) return -0.1f * leftTrigger;
            return 0;
        }
    }
}
