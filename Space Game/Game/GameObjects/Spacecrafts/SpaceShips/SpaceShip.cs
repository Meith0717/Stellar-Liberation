using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;

using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Core.GameObject;
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
        [JsonIgnore] public CrossHair CrossHair { private get; set; }

        // Navigation
        [JsonIgnore] public bool Stop = false;
        [JsonProperty] private float mVelocity = 0;
        [JsonIgnore] private float mTravelTime;
        [JsonIgnore] private MovementController mMovementController;

        public SpaceShip() : base() { mMovementController = new(this); }

        public override void SelectActions(InputState inputState, GameEngine engine)
        {
            base.SelectActions(inputState, engine);
            GetTarget(inputState, engine);
            if (engine.Camera.MovedByUser) { IsTracked = false; }
        }

        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            base.UpdateLogic(gameTime, inputState, engine);
            UpdateNavigation(gameTime, inputState);
            AddToSpatialHashing(engine);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
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
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;

            mTravelTime = (Vector2.Distance(Position, TargetObj.Position) / mVelocity) / 1000;
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
                engine.Camera.SetTarget(target.Position);
                engine.Camera.SetZoom(0.25f);
                return;
            }
            TargetObj = b ? target : null;
            StartPosition = b ? Position : null;
            if (!b) return;
            mVelocity = 0.1f;
            IsTracked = true;
        }

        // Draw Stuff
        public void DrawPath(TextureManager textureManager, GameEngine engine)
        {
            if (TargetObj == null) { return; }
            textureManager.DrawString("text", Position + TextureOffset,
                Utility.ConvertSecondsToGameTimeUnits((int)(mTravelTime + engine.GameTime)), 1, Color.LightBlue);
        }

        public void DrawTargetCrosshar(TextureManager textureManager, GameEngine engine)
        {
            CrossHair.Draw(textureManager, engine);
        }
    }
}
