using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class Cargo : SpaceShip
    {
        public Cargo(Vector2 position) : base()
        {
            Position = position;
            Rotation = 0;

            // Rendering
            NormalTexture = TextureId = "ship";
            SelectTexture = "shipSekect";
            TextureScale = 0.5f;
            TextureWidth = 209;
            TextureHeight = 128;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureDepth = 2;
            TextureColor = Color.White;
            MaxVelocity = 5f;
            WeaponManager = null;
            CrossHair = new(Vector2.Zero, TextureScale, CrossHair.CrossHairType.Target);
            
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogik(gameTime, inputState, engine);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
        }
    }
}
