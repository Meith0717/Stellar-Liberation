using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
using Galaxy_Explovive.Core.MyMath;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class SpaceShip : Spacecraft
    {

        public float MaxVelocity { private get; set; }
        public WeaponManager mWeaponManager { private get; set; }

        // Navigation
        private bool IsMoving = false;
        private bool mTrack = false;
        private float mVelocity = 0;
        private float mTravelTime;

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(inputState);
            UpdateInputs(inputState);
            UpdateNavigation(gameTime, inputState);
            if (inputState.mActionList.Contains(ActionType.Test))
            {
                mWeaponManager.Shoot(this, this, Color.LightGreen, 5000);
            }
            mWeaponManager.Update(gameTime);
        }

        public override void Draw()
        {
            base.DrawSpaceCraft();
            DrawPath();
            mWeaponManager.Draw();
        }

        // Input Stuff
        private new void UpdateInputs(InputState inputState)
        {
            base.Update(inputState);
            IsMoving = false;
            CheckForSelection(inputState);
            GetTargetPosition(inputState);
            TextureId = IsSelect ? SelectTexture : NormalTexture;
        }
        private void CheckForSelection(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                IsSelect = mTrack = !IsSelect;
                Globals.mCamera2d.SetZoom(0.2f);
                return;
            }
        
            if (Globals.mCamera2d.mIsMoving ||
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                return;
            }
        
            if (!mTrack) { return; }
            Globals.mCamera2d.mTargetPosition = Position;
        
        }
        private void GetTargetPosition(InputState inputState)
        {
            if (!IsSelect) { return; }
            Vector2 MousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            List<InteractiveObject> GameObjects = ObjectLocator.Instance.GetObjectsInRadius(MousePosition, 200).OfType<InteractiveObject>().ToList(); ;
            foreach (InteractiveObject gameObject in GameObjects)
            {
                if (inputState.mMouseActionType == MouseActionType.RightClick && gameObject.IsHover)
                {
                    TargetPosition = gameObject.Position;
                    mVelocity = Globals.SubLightVelocity;
                    mTrack = true;
                    Globals.mCamera2d.mTargetPosition = Position;
                }
            }
        }
        // Navigation Stuff
        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
            float targetRotation = MyMath.Instance.GetRotation(Position, TargetPosition);
            float rotationRest = MathF.Abs(Rotation - targetRotation);
            float distanceToTarget = Vector2.Subtract(TargetPosition, Position).Length();
            Rotation += MovementController.Instance.GetRotation(Rotation, targetRotation, rotationRest);
            mVelocity += MovementController.Instance.StopByTarget(inputState, IsSelect, MaxVelocity, mVelocity, distanceToTarget);
            if (mVelocity == 0) { return; }
            IsMoving = true;
            Vector2 directionVector = MyMath.Instance.GetDirection(Rotation);
            Position += directionVector * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            mTravelTime = (distanceToTarget /  mVelocity) / 1000;
            Debug.WriteLine(new Vector2(distanceToTarget, mVelocity));
        }
        // Draw Stuff
        public void DrawPath()
        {
            if (!IsMoving) { return; }
            TextureManager.Instance.DrawAdaptiveLine(Position, TargetPosition, Color.Gray, 2, 0);
            TextureManager.Instance.DrawString("text", Position + TextureOffset ,
                MyMath.Instance.GetTimeFromSeconds((int)mTravelTime), Color.Red);
        }
    }
}
