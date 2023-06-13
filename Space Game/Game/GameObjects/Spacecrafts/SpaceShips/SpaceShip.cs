using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TargetMovementController;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Weapons;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class SpaceShip : Spacecraft
    {
        public GameObject TargetObj = null;
        public Vector2? StartPosition { get; set; } = null;
        public float MaxVelocity { private get; set; }
        public WeaponManager WeaponManager { private get; set; }
        public CrossHair CrossHair { private get; set; }

        // Navigation
        public bool Track = false;
        public bool Stop = false;
        private bool mSelect = false;
        private float mVelocity = 0;
        private float mTravelTime;
        private MovementController mMovementController;

        public SpaceShip(Game game) : base(game) { mMovementController = new(this); }

        public override void SelectActions(InputState inputState)
        {
            GetTarget(inputState);
            if (IsPressed && TargetObj is not null) { Track = true; }
            if (mGame.mCamera.MovedByUser || TargetObj == null)
            {
                Track = false;
            }
            if (IsPressed && mSelect) 
            { 
                mSelect = Track = false; 
                mGame.SelectObject = null;
                return;
            }
            mSelect = true;
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            Track = Track && (mGame.SelectObject == this);
            if (Track) { mGame.mCamera.TargetPosition = Position; }
            mSelect = mSelect && (mGame.SelectObject == this);
            CrossHair.Update(null, 0, Color.Wheat, false);
            mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
            WeaponManager.Update(gameTime);
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void Draw()
        {
            base.Draw();
            base.DrawLife();
            DrawPath();
            WeaponManager.Draw(mTextureManager);
            DrawTargetCrosshar();
        }

        // Input Stuff
        
        private void UpdateNavigation(GameTime gameTime, InputState inputState)
        {
            if (TargetObj == null) { return; }

            var hasReachedTarget = !mMovementController.MoveToTarget(TargetObj, 
                (Vector2)StartPosition, Rotation, mVelocity, MaxVelocity, Stop);
            if (hasReachedTarget) 
            {
                Stop = false;
                TargetObj = null;
                return;
            }
            Rotation = mMovementController.GetMovement().Angle;
            mVelocity = mMovementController.GetMovement().Velocity;
            Position += MyUtility.GetDirection(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;

            mTravelTime = (Vector2.Distance(Position, TargetObj.Position) /  mVelocity) / 1000;
        }

        private void GetTarget(InputState inputState)
        {
            if (TargetObj != null)
            {
                CrossHair.Update(TargetObj.Position, 1f / mGame.mCamera.Zoom, Color.Green, false); 
                return; 
            }
            var target = MovementController.SelectTargetObject(this, mSpatialHashing, mGame.mWorldMousePosition);
            switch (target)
            {
                case null:
                    CrossHair.Update(mGame.mWorldMousePosition, 1f / mGame.mCamera.Zoom, Color.IndianRed, false);
                    return;
                case not null:
                    CrossHair.Update(target.Position, 1f / mGame.mCamera.Zoom, Color.LightGreen, false);
                    break;
            }

            bool b = inputState.mMouseActionType == MouseActionType.LeftClick;
            if (mGame.mCamera.Zoom < 0.2f && b)
            {
                mGame.mCamera.TargetPosition = target.Position;
                mGame.mCamera.SetZoom(0.25f);
                return;
            }
            TargetObj = b ? target : null;
            StartPosition = b ? Position : null;
            if (!b) return;
            mVelocity = 0.1f;
            Track = true;
        }

        // Draw Stuff
        public void DrawPath()
        {
            if (TargetObj == null) { return; }
            mTextureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToGameTimeUnits((int)(mTravelTime + mGame.GameTime)), 1, Color.LightBlue);
        }

        public void DrawTargetCrosshar()
        {
            CrossHair.Draw();
        }
    }
}
