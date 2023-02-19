using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Game.Layers;
using System;

namespace Space_Game.Core
{
    public class Globals
    {
        public static LayerManager mLayerManager; 
        public static GraphicsDevice mGraphicsDevice;   // Will not be saved
        public static SpriteBatch mSpriteBatch;         // Will not be saved
        public static ContentManager mContentManager;   // Will not be saved
        public static SoundManager mSoundManager;       // Will not be saved
        public static Random mRandom;
        public static Camera2d mCamera2d;
        public static GameLayer mGameLayer;
    }
}
