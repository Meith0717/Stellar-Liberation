using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.GameEngine.InputManagement.Peripheral
{
    internal class GamePadManager
    {
        private readonly Dictionary<Buttons, ActionType> mActionOnPadPressed, mActionOnPadHold;
        private GamePadState mPreviousState, mState;

        public GamePadManager() 
        {
            mActionOnPadPressed = new()
            {
                { Buttons.Start, ActionType.ESC },
                { Buttons.A, ActionType.ShootProjectile },
            };

            mActionOnPadHold = new() 
            {                
                { Buttons.RightTrigger, ActionType.Accelerate },
                { Buttons.LeftTrigger, ActionType.Decelerate },
                { Buttons.RightThumbstickUp, ActionType.CameraZoomIn },
                { Buttons.RightThumbstickDown, ActionType.CameraZoomOut },
            };
        }

        private List<Buttons> GetHoldButtons(GamePadState gamePadState)
        {
            return Enum.GetValues(typeof(Buttons))
                       .Cast<Buttons>()
                       .Where(b => gamePadState.IsButtonDown(b))
                       .ToList();
        }
        public List<Buttons> GetPressedButtons(GamePadState gamePadState)
        {
            return Enum.GetValues(typeof(Buttons))
                       .Cast<Buttons>()
                       .Where(b => gamePadState.IsButtonDown(b) && !mPreviousState.IsButtonDown(b))
                       .ToList();
        }

        public GamePadValues GetGamePadValues() 
        { 
            GamePadValues values = new();
            values.mRightThumbSticks = new(mState.ThumbSticks.Right.X, -mState.ThumbSticks.Right.Y);
            values.mLeftThumbSticks = new(mState.ThumbSticks.Left.X, -mState.ThumbSticks.Left.Y);
            values.mRightTrigger = mState.Triggers.Right;
            values.mLeftTrigger = mState.Triggers.Left;
            return values;
        }

        public List<ActionType> GetActions()
        {
            List<ActionType> actions = new();
            mPreviousState = mState;
            mState = GamePad.GetState(PlayerIndex.One);
            foreach (Buttons button in GetPressedButtons(mState))
            {
                if (mActionOnPadPressed.TryGetValue(button, out var actionPressed))
                {
                    actions.Add(actionPressed);
                }
            }
            foreach (Buttons button in GetHoldButtons(mState))
            {
                if (mActionOnPadHold.TryGetValue(button, out var actionPressed))
                {
                    actions.Add(actionPressed);
                }
            }
            return actions;
        }
    }
}
