using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Galaxy_Explovive.Game.Layers;
using Galaxy_Explovive.Game;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        [JsonIgnore] public bool IsHover { get; private set; }
        [JsonIgnore] public bool IsPressed { get; private set; }
        [JsonIgnore] private CrossHair mCrossHair;

        public InteractiveObject() 
        {
            mCrossHair = new(Position, TextureScale, CrossHair.CrossHairType.Select);
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            void CheckForSelect(InputState inputState)
            {
                IsPressed = false;
                BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
                var mousePosition = GameGlobals.Camera.ViewToWorld(inputState.mMousePosition.ToVector2());
                IsHover = BoundedBox.Contains(mousePosition);
                if (!IsHover) { return; }
                if (inputState.mMouseActionType != MouseActionType.LeftClick) { return; }
                IsPressed = true;
                if (GameGlobals.SelectObject != null) { return; }
                GameGlobals.SelectObject = this;
                GameGlobals.Camera.TargetPosition = Position;
            }
            mCrossHair.Update(Position, TextureScale*20, Color.OrangeRed, false);
            CheckForSelect(inputState);
        }

        public override void Draw(TextureManager textureManager)
        {
            textureManager.DrawGameObject(this, IsHover);
            GameGlobals.DebugSystem.DrawBoundBox(textureManager, BoundedBox);
            if (GameGlobals.SelectObject != this) return;
            mCrossHair.Draw(textureManager);
        }

        public abstract void SelectActions(InputState inputState);
    }
}
