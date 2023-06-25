using Galaxy_Explovive.Core;
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

        public override void SelectActions(InputState inputState, GameEngine engine)
        {
            GetTarget(inputState, engine);
            if (IsPressed && TargetObj is not null) { Track = true; }
            if (engine.Camera.MovedByUser || TargetObj == null)
            {
                Track = false;
            }
            if (IsPressed && mSelect) 
            { 
                mSelect = Track = false;
                engine.SelectObject = null;
                return;
            }
            mSelect = true;
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogik(gameTime, inputState, engine);
            Track = Track && (engine.SelectObject == this);
            if (Track) { engine.Camera.TargetPosition = Position; }
            mSelect = mSelect && (engine.SelectObject == this);
            CrossHair.Update(null, 0, Color.Wheat, false);
            engine.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            UpdateNavigation(gameTime, inputState);
            engine.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            base.DrawLife(textureManager);
            DrawPath(textureManager, engine);
            DrawTargetCrosshar(textureManager, engine);
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

        private void GetTarget(InputState inputState, GameEngine engine)
        {
            if (TargetObj != null)
            {
                CrossHair.Update(TargetObj.Position, 1f / engine.Camera.Zoom, Color.Green, false); 
                return; 
            }
            var target = MovementController.SelectTargetObject(this, engine.SpatialHashing, engine.WorldMousePosition);
            switch (target)
            {
                case null:
                    CrossHair.Update(engine.WorldMousePosition, 1f / engine.Camera.Zoom, Color.IndianRed, false);
                    return;
                case not null:
                    CrossHair.Update(target.Position, 1f / engine.Camera.Zoom, Color.LightGreen, false);
                    break;
            }

            bool b = inputState.mMouseActionType == MouseActionType.LeftClick;
            if (engine.Camera.Zoom < 0.2f && b)
            {
                engine.Camera.TargetPosition = target.Position;
                engine.Camera.SetZoom(0.25f);
                return;
            }
            TargetObj = b ? target : null;
            StartPosition = b ? Position : null;
            if (!b) return;
            mVelocity = 0.1f;
            Track = true;
        }

        // Draw Stuff
        public void DrawPath(TextureManager textureManager, GameEngine engine)
        {
            if (TargetObj == null) { return; }
            textureManager.DrawString("text", Position + TextureOffset ,
                MyUtility.ConvertSecondsToGameTimeUnits((int)(mTravelTime + engine.GameTime)), 1, Color.LightBlue);
        }

        public void DrawTargetCrosshar(TextureManager textureManager, GameEngine engine)
        {
            CrossHair.Draw(textureManager, engine);
        }
    }
}
