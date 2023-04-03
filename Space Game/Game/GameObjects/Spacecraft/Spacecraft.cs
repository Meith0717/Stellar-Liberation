using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;


namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public Vector2 TargetPosition { get; private set; }
        public float Velocity {private get; set; }

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);

            List<ActionType> actions = inputState.mActionList;
            if (actions.Contains(ActionType.MoveUp)) { Position -= new Vector2(0, Velocity); }
            if (actions.Contains(ActionType.MoveDown)) { Position += new Vector2(0, Velocity); }
            if (actions.Contains(ActionType.MoveLeft)) { Position -= new Vector2(Velocity, 0); }
            if (actions.Contains(ActionType.MoveRight)) { Position += new Vector2(Velocity, 0); }
        }
    }
}
