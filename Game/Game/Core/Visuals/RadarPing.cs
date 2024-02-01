// RadarPing.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.Visuals
{
    public class RadarPing
    {
        private Vector2 mPosition;
        private Vector2 mVariance = Vector2.Zero;

        private float mRadius;
        private float mAlpha;
        private int CoolDown;

        public void Update(Vector2 position, GameTime gameTime)
        {
            mPosition = position + mVariance;
            CoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            mRadius = MathHelper.Clamp(mRadius + .01f, 0f, 1f);
            mAlpha = MathHelper.Clamp(mAlpha - .01f, 0f, 1f);
            if (CoolDown > 0) return;
            CoolDown = ExtendetRandom.Random.Next(2000, 10000);
            mRadius = 0;
            mAlpha = 1;
            mVariance = ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 5000));
        }

        public void Draw(GameLayer scene)
        {
            int getColor(int a) => (int)(a * mAlpha);
            var color = new Color(getColor(200), getColor(200), getColor(255), getColor(255));
            TextureManager.Instance.DrawAdaptiveCircle(mPosition, mRadius * 10000, color, 200, 1, 1);
        }
    }
}
