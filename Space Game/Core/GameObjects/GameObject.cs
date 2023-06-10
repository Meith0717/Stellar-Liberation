using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class GameObject
    {
        // Managers
        protected TextureManager mTextureManager;
        protected SoundManager mSoundManager;
        protected FrustumCuller mFrustumCuller;
        protected SpatialHashing<GameObject> mSpatialHashing;
        protected GameLayer mGameLayer;

        // Location
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        // Rendering
        public Vector2 TextureOffset { get; set; }
        public string TextureId { get; set; }
        public float TextureScale { get; set; }
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }
        public int TextureDepth { get; set; }
        public Color TextureColor { get; set; }
        public CircleF BoundedBox { get; set; }

        // Constructor
        public GameObject(GameLayer gameLayer)                        
        {
            mGameLayer = gameLayer;
            mTextureManager = gameLayer.mTextureManager;
            mSoundManager = gameLayer.mSoundManager;
            mFrustumCuller = gameLayer.mFrustumCuller;
            mSpatialHashing = gameLayer.mSpatialHashing;
        }

        // Methods
        public abstract void UpdateLogik(GameTime gameTime, InputState inputState);
        public abstract void Draw();

    }
}
