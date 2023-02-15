using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core.GameObject;
using Space_Game.Game.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Game.GameObjects
{
    internal class PlanetSystem : GameObject
    {
        private LayerManager mLayerManager;
        private GraphicsDevice mGraphicsDevice;
        private SpriteBatch mSpriteBatch;
        private ContentManager mContentManager;
        private SoundManager mSoundManager;

        public PlanetSystem(LayerManager layerManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
            ContentManager contentManager, SoundManager soundManager) 
        {
            mLayerManager = layerManager;
            mGraphicsDevice = graphicsDevice;
            mSpriteBatch = spriteBatch;
            mContentManager = contentManager;
            mSoundManager = soundManager;

            Position = Vector2.Zero;
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
            mLayerManager.AddLayer(new PlanetSystemLayer(mLayerManager, mGraphicsDevice,
                mSpriteBatch, mContentManager, mSoundManager));
        }
    }
}
