using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Rendering;
using GalaxyExplovive.Core.GameObject;
using Microsoft.Xna.Framework;

namespace GalaxyExplovive.Game.GameObjects.Spacecraft.SpaceShips
{
    public class Cargo : SpaceShip
    {
        public Cargo(Vector2 position) : base()
        {
            Position = position;
            Rotation = 0;
            SelectZoom = 1;

            // Rendering
            TextureId = "ship";
            TextureScale = 0.5f;
            Width = 209;
            Height = 128;
            TextureOffset = new Vector2(Width, Height) / 2;
            TextureDepth = 2;
            TextureColor = Color.White;
            MaxVelocity = 5f;
            WeaponManager = null;
            CrossHair = new(CrossHair.CrossHairType.Target);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            textureManager.DrawGameObject(this, IsHover);
        }
    }
}
