using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Game.Layers;
using System;

namespace Space_Game.Game.GameObjects
{
    public class PlanetSystem : GameObject
    {

        public PlanetSystem(Vector2 position) 
        {
            Position = position;
            Offset = new Vector2(16, 16);
            TextureId = "star";
            TextureWidth = TextureHeight = 32;
            HoverBox = new CircleF(Position, Math.Max(TextureWidth/2, TextureHeight/2));
        }

        public override void Update(GameTime gameTime, InputState inputState, Camera2d camera2d)
        {
            this.ManageHover(inputState, camera2d.ViewToWorld(inputState.mMousePosition.ToVector2()), Clicked);
        }

        public override void Draw()
        {
            TextureManager.GetInstance().Draw("star", Position - Offset, TextureWidth, TextureHeight);
            if (Hover)
            {
                TextureManager.GetInstance().GetSpriteBatch().DrawCircle(HoverBox, 30, Color.LightSkyBlue);
            }
        }

        public void Clicked()
        {
            Globals.mLayerManager.AddLayer(new PlanetSystemLayer());
        }
    }
}
