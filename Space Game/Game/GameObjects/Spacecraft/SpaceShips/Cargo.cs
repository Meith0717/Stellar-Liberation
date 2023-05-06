using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class Cargo : SpaceShip
    {
        public Cargo(Vector2 position)
        {
            Position = TargetPosition = position;
            Rotation = 0;

            // Rendering
            NormalTexture = "ship";
            SelectTexture = "shipSekect";
            TextureSclae = 1;
            TextureWidth = 209;
            TextureHeight = 128;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureDepth = 2f;
            TextureColor = Color.White;
            MaxVelocity = 10;
            mWeaponManager = new();
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
