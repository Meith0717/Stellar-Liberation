﻿// Sector.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

namespace StellarLiberation.Game.Core.GameProceses
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

        public void Update(Fraction occupier)
        {
            mColor = occupier switch
            {
                Fraction.Neutral => mNeuteralColor,
                Fraction.Allied => mAlliedColor,
                Fraction.Enemys => mEnemyCollor,
                _ => throw new System.NotImplementedException()
            };
        }

        public void Draw() { TextureManager.Instance.Draw("particle", mBounds.Location.ToVector2(), mBounds.Width, mBounds.Height, mColor); }
    }
}
