using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Core.TargetMovementController;
using GalaxyExplovive.Core.Weapons;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace GalaxyExplovive.Game.GameObjects.Spacecraft.SpaceShips
{
    [Serializable]
    public class SpaceShip : Spacecraft
    {
        [JsonProperty] public GameObject TargetObj = null;
        [JsonProperty] public Vector2? StartPosition { get; set; } = null;
        [JsonProperty] public float MaxVelocity { private get; set; }
        [JsonIgnore] public WeaponManager WeaponManager { private get; set; }

        // Navigation
        [JsonIgnore] public bool Stop = false;
        [JsonProperty] private float mVelocity = 0;
        [JsonIgnore] private float mTravelTime;
        [JsonIgnore] private MovementController mMovementController;

        public SpaceShip(Vector2 position) : base(position) { mMovementController = new(this); }

        internal override void SelectActions(InputState inputState, GameEngine engine)
        {
            if (engine.Camera.MovedByUser) 
            { 
                IsTracked = false; 
            } 
            GetTarget(inputState, engine);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            UpdateNavigation(gameTime, inputState);
            base.Update(gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            DrawPath(textureManager, engine);
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
                IsTracked = false;
                TargetObj = null;
                return;
            }
            Rotation = mMovementController.GetMovement().Angle;
            mVelocity = mMovementController.GetMovement().Velocity;
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;

            mTravelTime = Vector2.Distance(Position, TargetObj.Position) / mVelocity / 1000;
        }

        private void GetTarget(InputState inputState, GameEngine engine)
        {
            if (TargetObj != null) return;
            var target = MovementController.SelectTargetObject(this, engine.SpatialHashing, engine.WorldMousePosition);
            if (target == null) return;
            bool IsLeftClick = inputState.mMouseActionType == MouseActionType.LeftClickReleased;
            if (engine.Camera.Zoom < 0.2f && IsLeftClick)
            {
                engine.Camera.MoveToTarget(target.Position);
                engine.Camera.MoveToZoom(0.25f);
                return;
            }
            TargetObj = IsLeftClick ? target : null;
            StartPosition = IsLeftClick ? Position : null;
            if (!IsLeftClick) return;
            mVelocity = 0.1f;
            IsTracked = true;
        }

        // Draw Stuff
        public void DrawPath(TextureManager textureManager, GameEngine engine)
        {
            if (TargetObj == null) { return; }
            textureManager.DrawString("text", Position + TextureOffset,
                Utility.ConvertSecondsToTimeUnits((int)mTravelTime), 1, Color.LightBlue);
        }
    }
}
