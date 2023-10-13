
using CelestialOdyssey.Core.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.Animation;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Animations
{
    public class SpriteSheet
    {
        public Texture2D Texture { get; private set; }
        public float Scale { get; private set; }
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }
        public int CountWidth { get; private set; }
        public int CountHeight { get; private set; }

        private List<string> mActiveAnimation = new();
        private Dictionary<string, Animation> mAnimations = new();

        public SpriteSheet(string textureId, int spriteCountWidth, int spriteCountHeight, float scale) 
        {
            Texture = TextureManager.Instance.GetTexture(textureId);
            CountHeight = spriteCountHeight;
            CountWidth = spriteCountWidth; 
            SpriteWidth = Texture.Width / spriteCountWidth;
            SpriteHeight = Texture.Height / spriteCountHeight;
            Scale = scale;
        }

        public void Animate(string id, Animation animation) => mAnimations.Add(id, animation);

        public bool Play(string id)
        {
            if (!mAnimations.ContainsKey(id)) return false;
            mActiveAnimation.Add(id);
            mAnimations[id].Reset();
            return true;
        }

        public bool Stop(string id)
        {
            if (!mAnimations.ContainsKey(id)) return false;
            return mActiveAnimation.Remove(id);
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            List<string> deleteLst = new();
            foreach (var animation in mActiveAnimation)
            {
                var anim = mAnimations[animation];
                mAnimations[animation].Update(position, gameTime);
                if (anim.IsRunning) continue;
                deleteLst.Add(animation);
            }

            foreach (var animation in deleteLst) mActiveAnimation.Remove(animation);
        }

        public void Draw(int depth)
        {
            foreach (var animation in mActiveAnimation) mAnimations[animation].Draw(this, depth);
        }
    }
}
