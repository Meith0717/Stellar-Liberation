﻿using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class Cargo : SpaceShip
    {
        public Cargo(GameLayer gameLayer, Vector2 position) : base(gameLayer)
        {
            Position = TargetPosition = position;
            Rotation = 0;

            // Rendering
            NormalTexture = "ship";
            SelectTexture = "shipSekect";
            TextureScale = 0.5f;
            TextureWidth = 209;
            TextureHeight = 128;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureDepth = 2;
            TextureColor = Color.White;
            MaxVelocity = 10;
            WeaponManager = new(mSoundManager, mGameLayer.mSpatialHashing);
            CrossHair = new(gameLayer, Vector2.Zero, TextureScale);
            
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
        }

        public override void Draw()
        {
            base.Draw();
            DrawSpaceCraft();
        }
    }
}
