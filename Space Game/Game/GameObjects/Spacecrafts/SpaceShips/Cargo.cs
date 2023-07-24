using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.Spacecraft.SpaceShips
{
    public class Cargo : SpaceShip
    {
        public Cargo(Vector2 position) : base(position)
        {
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
            MaxVelocity = 20f;
            WeaponManager = null;
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            textureManager.DrawGameObject(this, IsHover);
        }
    }
}
