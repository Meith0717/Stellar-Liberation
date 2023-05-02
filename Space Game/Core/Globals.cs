﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Game.Layers;
using System;
using Galaxy_Explovive.Core.PositionManagement;

namespace Galaxy_Explovive.Core
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
        public static DebugSystem mDebugSystem;
        public static SpatialHashing<GameObject.GameObject> mSpatialHashing;
        public static float SubLightVelocity = 0.01f;
    }
}
