using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects
{
    public class Player : GameObject
    {
        public Player() : base(Vector2.Zero, "ship", 1, 0) {}

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {
            MoveByKeys(inputState.mActionList);
            gameEngine.Camera.SetPosition(Position);
            base.Update(gameTime, inputState, gameEngine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }

        //_____________________________________________________________________________________________________//

        private void MoveByKeys(List<ActionType> actionTypes)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            int speed = 100;

            if (gamePadState.IsConnected)
            {
                Vector2 movement = new(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
                Position += movement * speed;
                return;
            }

            if (actionTypes.Contains(ActionType.MoveUp)) { Position += new Vector2(0, -speed); }
            if (actionTypes.Contains(ActionType.MoveDown)) { Position += new Vector2(0, speed); }
            if (actionTypes.Contains(ActionType.MoveL)) { Position += new Vector2(-speed, 0); }
            if (actionTypes.Contains(ActionType.MoveR)) { Position += new Vector2(speed, 0); }
        }
    }
}
