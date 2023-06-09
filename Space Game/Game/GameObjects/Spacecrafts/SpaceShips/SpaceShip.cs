using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Weapons;
using Galaxy_Explovive.Game.GameLogik;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class SpaceShip : Spacecraft
    {

        public float MaxVelocity { private get; set; }
        public WeaponManager WeaponManager { private get; set; }
        public CrossHair CrossHair { private get; set; }

        // Navigation
        private bool mTrack = false;
        private float mVelocity = 0;
        private float mTravelTime;

        public SpaceShip(GameLayer gameLayer) : base(gameLayer) {}

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
            UpdateInputs(inputState);
            WeaponManager.Update(gameTime);
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void Draw()
        {
            base.DrawSpaceCraft();
            DrawPath();
            WeaponManager.Draw(mTextureManager);
            DrawTargetCrosshar();
        }

        // Input Stuff
        private new void UpdateInputs(InputState inputState)
        {
            base.Update(inputState);
            GetTarget(inputState);
            CheckForSelection(inputState);
            TextureId = IsSelect ? SelectTexture : NormalTexture;
        }
        private void CheckForSelection(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                IsSelect = mTrack = !IsSelect;
                mGameLayer.mGameMessages.AddMessage(IsSelect ? Messages.ShipSelected : Messages.ShipDeselected, mGameLayer.GameTime);
                mGameLayer.SelectObject = IsSelect ? this : null;
                if (!IsSelect) { return; }
                mGameLayer.mCamera.SetZoom(1f);
                return;
            }
        
            if (mGameLayer.mCamera.mIsMoving || (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                return;
            }
        
            if (!mTrack) { return; }
            mGameLayer.mCamera.TargetPosition = Position;
        }
        
        private void GetTarget(InputState inputState)
        {
            if (!IsSelect) { return; }
            if (TargetPosition != null) { CrossHair.Update(null, 1, Color.Transparent, false); return; }
            var target = MovementController.GetTargetPosition(mSpatialHashing, inputState, mGameLayer.mWorldMousePosition);
            switch (target)
            {
                case null:
                    CrossHair.Update(mGameLayer.mWorldMousePosition, 0.1f / mGameLayer.mCamera.Zoom, Color.White, IsHover);
                    return;
                case not null:
                    CrossHair.Update(target, 0.1f / mGameLayer.mCamera.Zoom, Color.Green, IsHover);
                    break;
            }
            TargetPosition = (inputState.mMouseActionType == MouseActionType.LeftClick) ? target : null;
        }

        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
            if (TargetPosition == null) { return; }

            var targetPosition = (Vector2)TargetPosition;

            float targetRotation = MyUtility.GetAngle(Position, targetPosition);
            float rotationRest = MathF.Abs(Rotation - targetRotation);
            float distanceToTarget = Vector2.Subtract(targetPosition, Position).Length();

            Rotation += MovementController.GetRotation(Rotation, targetRotation, rotationRest);
            mVelocity += MovementController.GetVelocity(inputState, IsSelect, MaxVelocity, mVelocity, distanceToTarget);
            Position += MyUtility.GetDirection(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;

            mTravelTime = (distanceToTarget /  mVelocity) / 1000;
        }

        // Draw Stuff
        public void DrawPath()
        {
            if (TargetPosition is null) { return; }
            mTextureManager.DrawAdaptiveLine(Position, (Vector2)TargetPosition, Color.LightGreen, 2, 0);
            mTextureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToTimeUnits((int)mTravelTime), Color.Red);
        }

        public void DrawTargetCrosshar()
        {
            if (!IsSelect) { return; }
            CrossHair.Draw();
        }
    }
}
