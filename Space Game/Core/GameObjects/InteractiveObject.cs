using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        [JsonIgnore] private readonly CrossHair mCrossHair;
        [JsonIgnore] public bool IsHover { get; private set; }
        [JsonIgnore] public bool IsPressed { get; private set; }

        public InteractiveObject() 
        {
            mCrossHair = new(Position, TextureScale * 20, CrossHair.CrossHairType.Select);
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine gameEngine)
        {
            void CheckForSelect(InputState inputState)
            {
                IsPressed = false;
                BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
                IsHover = BoundedBox.Contains(gameEngine.WorldMousePosition);
                if (!IsHover || inputState.mMouseActionType != MouseActionType.LeftClick) { return; }
                IsPressed = true;
                if (gameEngine.SelectObject != null) { return; }
                gameEngine.SelectObject = this;
                gameEngine.Camera.TargetPosition = Position;
            }
            mCrossHair.Update(Position, TextureScale * 20, Color.OrangeRed, false);
            CheckForSelect(inputState);
        }

        public override void Draw(TextureManager textureManager, GameEngine gameEngine)
        {
            gameEngine.DebugSystem.DrawnObjectCount += 1;
            textureManager.DrawGameObject(this, IsHover);
            gameEngine.DebugSystem.DrawBoundBox(textureManager, BoundedBox);
            if (gameEngine.SelectObject != this) return;
            mCrossHair.Draw(textureManager, gameEngine);
        }

        public abstract void SelectActions(InputState inputState, GameEngine gameEngine);
    }
}
