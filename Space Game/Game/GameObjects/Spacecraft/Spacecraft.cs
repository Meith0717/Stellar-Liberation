using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.Maths;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public Vector2 TargetPosition { get; set; }
        private bool IsSelect { get; set; } = false;
        public float Velocity {private get; set; }
        public string SelectTexture { get; set; }
        public string NormalTexture { get; set; }
        public bool IsMoving { get; private set; }
        private bool mTrack = false;
        private float mTargetRotation = 0;
        private float mVelocity = 0;

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
            IsMoving = false;
            GetTargetPosition(inputState);
            Redirect();
            Move();
            CheckForSelection(inputState);
            TextureId = IsSelect ? SelectTexture : NormalTexture;
        }

        private void CheckForSelection(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                IsSelect = mTrack = !IsSelect;
                Globals.mCamera2d.SetZoom(0.2f);
            }

            if (Globals.mCamera2d.mIsMoving ||
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                if (Globals.mCamera2d.mIsMoving) { return; }
                IsSelect = false;
            }

            if (!mTrack) { return; }
            Globals.mCamera2d.mTargetPosition = Position;

        }

        private void GetTargetPosition(InputState inputState)
        {
            if (!IsSelect) { return; }

            Vector2 MousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            List<InteractiveObject> GameObjects = Globals.mGameLayer.GetObjectsInRadius(MousePosition, 200).OfType<InteractiveObject>().ToList(); ;
            foreach (InteractiveObject gameObject in GameObjects)
            {
                if (inputState.mMouseActionType == MouseActionType.RightClick && gameObject.IsHover) 
                { 
                    TargetPosition = gameObject.Position;
                    Debug.WriteLine(new Vector2(Rotation, mTargetRotation));
                    IsSelect = false;
                }
            } 
        }
        private void Redirect()
        {
            mTargetRotation = MyMathF.GetInstance().GetRotation(Position, TargetPosition);
            if (mTargetRotation == Rotation) { return; }
            float update = MathF.Abs(Rotation - mTargetRotation) * 0.01f;
            if (Rotation < mTargetRotation) { Rotation += update; }
            if (Rotation > mTargetRotation) { Rotation -= update; }
            if (MathF.Abs(Rotation - mTargetRotation) < 0.01) 
            {
                Rotation = mTargetRotation;
                if (Velocity < mVelocity) { return; }
                mVelocity += Velocity * 0.05f;
                return; 
            }
            mVelocity = 0.5f;
        }
        private void Move()
        { 
            Vector2 DirectionToTarget = Vector2.Subtract(TargetPosition, Position);
            if (DirectionToTarget.Length() < 40) { return; }
            IsMoving = true;
            Vector2 directionVector = MyMathF.GetInstance().GetDirection(Rotation);
            Position += directionVector * mVelocity;
        }

        public void DrawPath()
        {
            if (!IsMoving) { return; }
            TextureManager.GetInstance().DrawLine(Position, TargetPosition, Color.Gray, 2, 0);
        }

        public void DrawSpaceCraft()
        {
            TextureManager.GetInstance().Draw(TextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            Crosshair.Draw(Color.White);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
        }
    }
}
