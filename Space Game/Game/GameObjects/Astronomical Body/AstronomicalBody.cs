using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public abstract class AstronomicalBody : SelectableObject
    {
        [JsonIgnore] private bool mTrack = false;
        protected AstronomicalBody() : base() {}

        public override void SelectActions(InputState inputState, Engine engine)
        {
            if (IsPressed)
            {
                if (GetType() == typeof(Planet))
                {
                    GameGlobals.Camera.SetZoom(1.2f);
                }
                if (GetType() == typeof(Star))
                {
                    GameGlobals.Camera.SetZoom(0.25f);
                }
            }
            engine.mCamera2d.SetPosition(Position);

            if (GameGlobals.Camera.MovedByMouse || (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                GameGlobals.SelectObject = null;
            }
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, Engine engine)
        {
            base.UpdateLogik(gameTime, inputState, engine);
        }
    }
}
