﻿using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public class Cargo : Spacecraft
    {
        public Cargo(Vector2 position)
        {
            Position = position;
            Rotation = 0;

            // Rendering
            NormalTextureId = "ship";
            TextureSclae = 1;
            TextureWidth = 209;
            TextureHeight = 128;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureDepth = 1;
            TextureColor = Color.White;
            TextureRadius = 100;
            Velocity = 5;
            BoundedBox = new CircleF(position, TextureRadius);
            Crosshair = new CrossHair(0.2f, 0.22f, position);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.UpdateInputs(inputState);
        }

        public override void Draw()
        {
            TextureManager.GetInstance().Draw(NormalTextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            Crosshair.Draw(Color.White);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox); 
        }
    }
}
