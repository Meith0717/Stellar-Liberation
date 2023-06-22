using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class SelectableObject : GameObject
    {
        [JsonIgnore] public bool IsHover { get; private set; }
        [JsonIgnore] public bool IsPressed { get; private set; }
        [JsonIgnore] private CrossHair mCrossHair;

        public SelectableObject() 
        {
            mCrossHair = new(Position, TextureScale, CrossHair.CrossHairType.Select);
        }

        public override void UpdateLogik(GameTime time, Engine engine, InputState input)
        {
            void CheckForSelect(InputState inputState)
            {
                IsPressed = false;
                mCrossHair.Update(Position, TextureScale * 20, Color.Transparent, false);
                BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
                var mousePosition = engine.MouseWorldPosition;
                IsHover = BoundedBox.Contains(mousePosition);
                if (!IsHover) { return; }
                if (inputState.mMouseActionType != MouseActionType.LeftClick) { return; }
                IsPressed = true;
                if (engine.SelectedObject != null) { return; }
                engine.SelectedObject = this;
                mCrossHair.Update(Position, TextureScale*20, Color.OrangeRed, false);
                engine.mCamera2d.SetPosition(Position);
            }  
            CheckForSelect(input);
        }
        public abstract void SelectActions(Engine engine, InputState input);

        public override void Draw(TextureManager textureManager)
        {
            textureManager.DrawGameObject(this, IsHover);
            mCrossHair.Draw(textureManager);
        }

    }
}
