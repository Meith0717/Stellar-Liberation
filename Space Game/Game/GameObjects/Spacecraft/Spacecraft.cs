using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.Maths;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {
        const float SubLightVelocity = 0.5f;
        public Vector2 TargetPosition { get; set; }
        private bool IsSelect { get; set; } = false;
        public float MaxVelocity {private get; set; }
        public string SelectTexture { get; set; }
        public string NormalTexture { get; set; }
        
        // Navigation
        private bool IsMoving = false;
        private bool mTrack = false;
        private float mVelocity = 0;
        private bool travel = false;

        public void Update(InputState inputState)
        {
            UpdateInputs(inputState);
            UpdateNavigation();
            Debug.WriteLine(mVelocity);
        }

        private new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
            IsMoving = false;
            GetTargetPosition(inputState);
            CheckForSelection(inputState);
            TextureId = IsSelect ? SelectTexture : NormalTexture;
        }
        private void CheckForSelection(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                IsSelect = mTrack = !IsSelect;
                Globals.mCamera2d.SetZoom(0.2f);
            }

            if (Globals.mCamera2d.mIsMoving ||
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                if (Globals.mCamera2d.mIsMoving) { return; }
                IsSelect = false;
            }

            if (!mTrack) { return; }
            Globals.mCamera2d.mTargetPosition = Position;

        }
        private void GetTargetPosition(InputState inputState)
        {
            if (!IsSelect) { return; }
            Vector2 MousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            List<InteractiveObject> GameObjects = Globals.mGameLayer.GetObjectsInRadius(MousePosition, 200).OfType<InteractiveObject>().ToList(); ;
            foreach (InteractiveObject gameObject in GameObjects)
            {
                if (inputState.mMouseActionType == MouseActionType.RightClick && gameObject.IsHover)
                {
                    TargetPosition = gameObject.Position;
                    IsSelect = false;
                    mVelocity = SubLightVelocity;
                    travel = true;
                }
            }
        }
        private void UpdateNavigation()
        {
            float targetRotation = MyMathF.GetInstance().GetRotation(Position, TargetPosition);
            float rotationRest = MathF.Abs(Rotation - targetRotation);
            float distanceToTarget = Vector2.Subtract(TargetPosition, Position).Length();

            DirectionController(targetRotation, rotationRest);
            VelocityController(rotationRest, distanceToTarget);
            MovementController(distanceToTarget);
        }
        private void DirectionController(float targetRotation, float rotationRest)
        {
            if (rotationRest <= 0.5) { Rotation = targetRotation; return;}
            if(targetRotation == Rotation || !travel) { return; }
            if (Rotation >= 2*MathF.PI) { Rotation = 0; }
            if (Rotation < 0) { Rotation = 2*MathF.PI;}
            if (Rotation < targetRotation && targetRotation - Rotation <= MathF.PI) { Rotation += 0.005f; }
            if (Rotation > targetRotation && Rotation - targetRotation > MathF.PI) { Rotation += 0.005f; }
            if (Rotation < targetRotation && targetRotation - Rotation > MathF.PI) { Rotation -= 0.005f; }
            if (Rotation > targetRotation && Rotation - targetRotation <= MathF.PI) { Rotation -= 0.005f; }
        }

        private void VelocityController(float rotationRest, float distanceToTarget)
        {
            float updateVelocity = MaxVelocity * .05f;
            if (distanceToTarget <= 40)
            {
                mVelocity = 0;
                travel = false;
                return;
            }
            if (distanceToTarget <= 2700 && mVelocity >= SubLightVelocity + updateVelocity)
            {
                mVelocity -= updateVelocity;
                return;
            }
            if ((rotationRest <= 0.01) && (mVelocity - updateVelocity <= MaxVelocity))
            {
                mVelocity += updateVelocity;
            }
            
        }

        private void MovementController(float distanceToTarget)
        { 
            if (mVelocity == 0) { return; }
            IsMoving = true;
            Vector2 directionVector = MyMathF.GetInstance().GetDirection(Rotation);
            Position += directionVector * mVelocity;
        }

        public void DrawPath()
        {
            if (!IsMoving) { return; }
            TextureManager.GetInstance().DrawLine(Position, TargetPosition, Color.Gray, 2, 0);
        }

        public void DrawSpaceCraft()
        {
            TextureManager.GetInstance().Draw(TextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            Crosshair.Draw(Color.White);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
        }
    }
}
