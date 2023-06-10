using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
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
        public bool mTrack = false;
        private float mVelocity = 0;
        private float mTravelTime;

        public SpaceShip(GameLayer gameLayer) : base(gameLayer) {}

        public override void UpdateInputs(InputState inputState)
        {
            GetTarget(inputState);
            if (IsPressed) { mTrack = true; }
            if (mGameLayer.mCamera.mIsMoving) { mTrack = false; }
            if (mTrack)
            {
                mGameLayer.mCamera.TargetPosition = Position;
            }
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
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
        
        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
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
            CrossHair.Draw();
        }
    }
}
