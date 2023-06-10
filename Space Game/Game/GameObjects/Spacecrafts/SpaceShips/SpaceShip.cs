﻿using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Weapons;
using Microsoft.Xna.Framework;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class SpaceShip : Spacecraft
    {
        public Vector2? TargetPosition { get; set; } = null;
        public Vector2? StartPosition { get; set; } = null;
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
            UpdateInputs(inputState);
            UpdateNavigation(gameTime, inputState);
            WeaponManager.Update(gameTime);
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            System.Diagnostics.Debug.WriteLine(mVelocity);
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
            TextureId = IsSelect ? SelectTexture : NormalTexture;
        }                                                  
        
        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
            GetTarget(inputState);
            if (TargetPosition == null) { return; }

            var targetPosition = (Vector2)TargetPosition;
            var startPosition = (Vector2)StartPosition;

            float targetRotation = MyUtility.GetAngle(Position, targetPosition);
            float rotationRest = MathF.Abs(Rotation - targetRotation);
            float targetDistance = Vector2.Subtract(targetPosition, Position).Length();
            float totalDistance = Vector2.Subtract(targetPosition, startPosition).Length();

            if ( targetDistance < 50) 
            { 
                TargetPosition = null; 
                StartPosition = null;  
                return; 
            }

            Rotation += MovementController.GetRotation(Rotation, targetRotation, rotationRest);
           MovementController.GetVelocity(ref mVelocity, MaxVelocity, targetDistance, totalDistance, rotationRest);
            Position += MyUtility.GetDirection(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;

            mTravelTime = (targetDistance /  mVelocity) / 1000;
        }

        private void GetTarget(InputState inputState)
        {
            if (!IsSelect) { return; }
            if (TargetPosition != null)
            {
                CrossHair.Update(TargetPosition, 0.1f / mGameLayer.mCamera.Zoom, Color.Green, IsHover);; return; }
            var target = MovementController.GetTargetPosition(mSpatialHashing, mGameLayer.mWorldMousePosition);
            switch (target)
            {
                case null:
                    CrossHair.Update(mGameLayer.mWorldMousePosition, 0.05f / mGameLayer.mCamera.Zoom, Color.White, IsHover);
                    return;
                case not null:
                    CrossHair.Update(target, 0.1f / mGameLayer.mCamera.Zoom, Color.LightGreen, IsHover);
                    break;
            }
            bool b = inputState.mMouseActionType == MouseActionType.LeftClick;
            TargetPosition = b ? target : null;
            mTrack = b;
            StartPosition = b ? Position : null;
        }


        // Draw Stuff
        public void DrawPath()
        {
            if (TargetPosition == null) { return; }
            mTextureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToTimeUnits((int)mTravelTime), Color.LightBlue);
        }

        public void DrawTargetCrosshar()
        {
            if (!IsSelect) { return; }
            CrossHair.Draw();
        }
    }
}