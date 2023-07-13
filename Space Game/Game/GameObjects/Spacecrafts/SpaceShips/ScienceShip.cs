using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;

namespace GalaxyExplovive.Game.GameObjects.Spacecraft.ScienceShip
{
    public class ScienceShip : SpaceShip
    {
        public ScienceShip(Vector2 position) : base(position)
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
