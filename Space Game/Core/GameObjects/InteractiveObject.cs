using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Galaxy_Explovive.Game.Layers;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class InteractiveObject : GameObject
    {
        public InteractiveObject(GameLayer gameLayer) : base(gameLayer) 
        {
            mCrossHair = new(gameLayer, Position, TextureScale, CrossHair.CrossHairType.Select);
        }
        public bool IsHover { get; private set; }
        public bool IsPressed { get; private set; }
        private CrossHair mCrossHair;

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            void CheckForSelect(InputState inputState)
            {
                IsPressed = false;
                BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
                var mousePosition = mGameLayer.mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
                IsHover = BoundedBox.Contains(mousePosition);
                if (!IsHover) { return; }
                if (inputState.mMouseActionType != MouseActionType.LeftClick) { return; }
                IsPressed = true;
                if (mGameLayer.SelectObject != null) { return; }
                mGameLayer.SelectObject = this;
                mGameLayer.mCamera.TargetPosition = Position;
            }
            mCrossHair.Update(Position, TextureScale*20, Color.OrangeRed, false);
            CheckForSelect(inputState);
        }

        public override void Draw()
        {
            mTextureManager.DrawGameObject(this, IsHover);
            mGameLayer.mDebugSystem.DrawBoundBox(mTextureManager, BoundedBox);
            if (mGameLayer.SelectObject != this) return;
            mCrossHair.Draw();
        }

        public abstract void SelectActions(InputState inputState);
    }
}
