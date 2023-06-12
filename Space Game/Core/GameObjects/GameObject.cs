using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.Layers;
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
        protected Game.Game mGame;

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
        public GameObject(Game.Game game)                        
        {
            mGame = game;
            mTextureManager = game.mTextureManager;
            mSoundManager = game.mSoundManager;
            mFrustumCuller = game.mFrustumCuller;
            mSpatialHashing = game.mSpatialHashing;
        }

        // Methods
        public abstract void UpdateLogik(GameTime gameTime, InputState inputState);
        public abstract void Draw();

    }
}
