using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Game.Core.Effects;
using Space_Game.Core.LayerManagement;
using Space_Game.Core.SoundManagement;
using Space_Game.Core.TextureManagement;
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
        public static float mTimeWarp;
    }
}
