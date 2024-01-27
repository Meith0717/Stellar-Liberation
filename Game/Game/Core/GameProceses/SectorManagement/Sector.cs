// Sector.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.Core.GameProceses.SectorManagement
{
    public class Sector
    {
        private readonly Color mAlliedColor;
        private readonly Color mEnemyCollor;
        private readonly Color mNeuteralColor;
        private readonly Rectangle mBounds;
        private Color mColor;

        public Sector(Vector2 position, int width, int height)
        {
            mAlliedColor = new(0, 45, 50, 10);
            mEnemyCollor = new(50, 0, 0, 10);
            mNeuteralColor = Color.Transparent;

            mBounds = new(position.ToPoint(), new(width, height));
        }

        public void Update(Fractions occupier)
        {
            mColor = occupier switch
            {
                Fractions.Neutral => mNeuteralColor,
                Fractions.Allied => mAlliedColor,
                Fractions.Enemys => mEnemyCollor,
                _ => throw new System.NotImplementedException()
            };
        }

        public void Draw() { TextureManager.Instance.Draw(GameSpriteRegistries.particle, mBounds.Location.ToVector2(), mBounds.Width, mBounds.Height, mColor); }
    }
}
