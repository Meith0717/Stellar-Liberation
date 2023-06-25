﻿using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public abstract class AstronomicalBody : InteractiveObject
    {
        [JsonIgnore] private bool mTrack = false;
        protected AstronomicalBody() : base() {}

        public override void SelectActions(InputState inputState, GameEngine engine)
        {
            if (IsPressed)
            {
                if (GetType() == typeof(Planet))
                {
                    engine.Camera.SetZoom(1.2f);
                }
                if (GetType() == typeof(Star))
                {
                    engine.Camera.SetZoom(0.25f);
                }
            }
            engine.Camera.TargetPosition = Position;

            if (engine.Camera.MovedByUser || (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                engine.SelectObject = null;
            }
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogik(gameTime, inputState, engine);
        }
    }
}
