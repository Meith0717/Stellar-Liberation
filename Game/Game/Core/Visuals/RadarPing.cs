// RadarPing.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;

namespace StellarLiberation.Game.Core.Visuals
{
    public class RadarPing
    {
        private Vector2 mPosition;

        private float mRadius;
        private float mAlpha;
        private int CoolDown;

        public void Update(Vector2 position, GameTime gameTime)
        {
            mPosition = position;
            CoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            mRadius = MathHelper.Clamp(mRadius + .01f, 0f, 1f);
            mAlpha = MathHelper.Clamp(mAlpha - .01f, 0f, 1f);
            if (CoolDown > 0) return;
            CoolDown = 2000;
            mRadius = 0;
            mAlpha = 1;
        }

        public void Draw(Scene scene)
        {
            var intensity = (int)(255 * mAlpha);
            var color = new Color(intensity, intensity, intensity, intensity);
            TextureManager.Instance.DrawAdaptiveCircle(mPosition, mRadius * 10000, color, 200, 1, 1);
        }
    }
}
