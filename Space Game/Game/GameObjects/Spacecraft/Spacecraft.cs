using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public Vector2 TargetPosition { get; set; }
        private bool IsSelect { get; set; } = false;
        public float Velocity {private get; set; }
        public bool IsMoving { get; private set; }

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
            IsMoving = false;
            GetTargetPosition(inputState);
            Move();
            CheckForSelection(inputState);
        }

        private void CheckForSelection(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover) { IsSelect = !IsSelect; }
            if (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover) { IsSelect = false; }
        }
        private void GetTargetPosition(InputState inputState)
        {
            if (!IsSelect) { return; }

            Vector2 MousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            List<InteractiveObject> GameObjects = Globals.mGameLayer.GetObjectsInRadius(MousePosition, 200).OfType<InteractiveObject>().ToList(); ;
            foreach (InteractiveObject gameObject in GameObjects)
            {
                if (inputState.mMouseActionType == MouseActionType.LeftClick && gameObject.IsHover) 
                { 
                    TargetPosition = gameObject.Position;
                }
            } 
            
        }
        private void Move()
        {
            Vector2 DirectionToTarget = Vector2.Subtract(TargetPosition, Position);
            if (DirectionToTarget.Length() < 10) { return; }
            IsMoving = true;
            Vector2 directionVector = DirectionToTarget.NormalizedCopy();
            Position += directionVector * Velocity;
        }

        public void DrawPath()
        {
            if (!IsMoving) { return; }
            TextureManager.GetInstance().DrawLine(Position, TargetPosition, Color.Gray, 2, 0);
        }
    }
}
