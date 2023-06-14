using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TargetMovementController;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Weapons;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    [Serializable]
    public class SpaceShip : Spacecraft
    {
        [JsonProperty] public GameObject TargetObj = null;
        [JsonProperty] public Vector2? StartPosition { get; set; } = null;
        [JsonProperty] public float MaxVelocity { private get; set; }
        [JsonIgnore] public WeaponManager WeaponManager { private get; set; }
        [JsonIgnore] public CrossHair CrossHair { private get; set; }

        // Navigation
        [JsonIgnore] public bool Track = false;
        [JsonIgnore] public bool Stop = false;
        [JsonIgnore] private bool mSelect = false;
        [JsonProperty] private float mVelocity = 0;
        [JsonIgnore] private float mTravelTime;
        [JsonIgnore] private MovementController mMovementController;

        public SpaceShip() : base() { mMovementController = new(this); }

        public override void SelectActions(InputState inputState)
        {
            GetTarget(inputState);
            if (IsPressed && TargetObj is not null) { Track = true; }
            if (GameGlobals.Camera.MovedByUser || TargetObj == null)
            {
                Track = false;
            }
            if (IsPressed && mSelect) 
            { 
                mSelect = Track = false;
                GameGlobals.SelectObject = null;
                return;
            }
            mSelect = true;
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            Track = Track && (GameGlobals.SelectObject == this);
            if (Track) { GameGlobals.Camera.TargetPosition = Position; }
            mSelect = mSelect && (GameGlobals.SelectObject == this);
            CrossHair.Update(null, 0, Color.Wheat, false);
            GameGlobals.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
            WeaponManager.Update(gameTime);
            GameGlobals.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void Draw(TextureManager textureManager)
        {
            base.Draw(textureManager);
            base.DrawLife(textureManager);
            DrawPath(textureManager);
            WeaponManager.Draw(textureManager);
            DrawTargetCrosshar(textureManager);
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
                CrossHair.Update(TargetObj.Position, 1f / GameGlobals.Camera.Zoom, Color.Green, false); 
                return; 
            }
            var target = MovementController.SelectTargetObject(this, GameGlobals.SpatialHashing, GameGlobals.WorldMousePosition);
            switch (target)
            {
                case null:
                    CrossHair.Update(GameGlobals.WorldMousePosition, 1f / GameGlobals.Camera.Zoom, Color.IndianRed, false);
                    return;
                case not null:
                    CrossHair.Update(target.Position, 1f / GameGlobals.Camera.Zoom, Color.LightGreen, false);
                    break;
            }

            bool b = inputState.mMouseActionType == MouseActionType.LeftClick;
            if (GameGlobals.Camera.Zoom < 0.2f && b)
            {
                GameGlobals.Camera.TargetPosition = target.Position;
                GameGlobals.Camera.SetZoom(0.25f);
                return;
            }
            TargetObj = b ? target : null;
            StartPosition = b ? Position : null;
            if (!b) return;
            mVelocity = 0.1f;
            Track = true;
        }

        // Draw Stuff
        public void DrawPath(TextureManager textureManager)
        {
            if (TargetObj == null) { return; }
            textureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToGameTimeUnits((int)(mTravelTime + GameGlobals.GameTime)), 1, Color.LightBlue);
        }

        public void DrawTargetCrosshar(TextureManager textureManager)
        {
            CrossHair.Draw(textureManager);
        }
    }
}
