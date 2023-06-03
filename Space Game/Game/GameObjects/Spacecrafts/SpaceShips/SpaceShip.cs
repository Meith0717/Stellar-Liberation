using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class SpaceShip : Spacecraft
    {

        public float MaxVelocity { private get; set; }
        public WeaponManager WeaponManager { private get; set; }
        public CrossHair CrossHair { private get; set; }

        // Navigation
        private bool IsMoving = false;
        private bool mTrack = false;
        private float mVelocity = 0;
        private float mTravelTime;

        public SpaceShip(GameLayer gameLayer) : base(gameLayer) {}

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
            UpdateInputs(inputState);
            // if (inputState.mActionList.Contains(ActionType.Test) && IsSelect)
            // {
            //     var ships = ObjectLocator.Instance.GetObjectsInRadius(Position, 500).OfType<Spacecraft>().ToList();
            //     if (ships.Count > 0) 
            //     { 
            //         WeaponManager.Shoot(this, ships[0], Color.LightGreen, 5000);
            //     }
            // }
            WeaponManager.Update(gameTime);
            Vector2 mousePos = mGameLayer.mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);

            CrossHair.Update(IsMoving ? TargetPosition : mousePos, 0.05f / mGameLayer.mCamera.Zoom, Color.LightGreen, IsHover);
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
            GetTargetPosition(inputState);
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
        
            if (mGameLayer.mCamera.mIsMoving ||
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                return;
            }
        
            if (!mTrack) { return; }
            mGameLayer.mCamera.TargetPosition = Position;
        }
        private void GetTargetPosition(InputState inputState)
        {
            if (!IsSelect || mVelocity > 0) { return; }
            Vector2 MousePosition = mGameLayer.mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
            if (inputState.mMouseActionType == MouseActionType.RightClick)
            {
                TargetPosition = MousePosition;
                mVelocity = Globals.SubLightVelocity;
                mTrack = true;
                mGameLayer.mCamera.TargetPosition = Position;
            }
        }
        // Navigation Stuff
        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
            IsMoving = false;
            float targetRotation = MyUtility.GetAngle(Position, TargetPosition);
            float rotationRest = MathF.Abs(Rotation - targetRotation);
            float distanceToTarget = Vector2.Subtract(TargetPosition, Position).Length();
            Rotation += MovementController.Instance.GetRotation(Rotation, targetRotation, rotationRest);
            mVelocity += MovementController.Instance.GetVelocity(inputState, IsSelect, MaxVelocity, mVelocity, distanceToTarget);
            if (mVelocity == 0) { TargetPosition = Position; return; }
            IsMoving = true;
            Vector2 directionVector = MyUtility.GetDirection(Rotation);
            Position += directionVector * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            mTravelTime = (distanceToTarget /  mVelocity) / 1000;
        }
        // Draw Stuff
        public void DrawPath()
        {
            if (!IsMoving) { return; }
            mTextureManager.DrawAdaptiveLine(Position, TargetPosition, Color.LightGreen, 2, 0);
            mTextureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToTimeUnits((int)mTravelTime), Color.Red);
        }

        public void DrawTargetCrosshar()
        {
            if (IsSelect || IsMoving) 
            {
                CrossHair.Draw();
            }
        }
    }
}
