using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Core.Effects
{
    internal class ParllaxManager
    {
        private Vector2 mLastPosition = Globals.mCamera2d.mPosition;
        private List<ParllaxBackground> mBackdrounds = new List<ParllaxBackground>();

        public void Update()
        {
            var movement = Globals.mCamera2d.mPosition - mLastPosition;
            foreach (ParllaxBackground backdround in mBackdrounds) 
            {
                backdround.Update(movement/0.9f);
            }
            mLastPosition = Globals.mCamera2d.mPosition;
        }

        public void Draw()
        {
            Globals.mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Draw();
            }
            Globals.mSpriteBatch.End();
        }

        public void Add(ParllaxBackground backdround)
        {
            mBackdrounds.Add(backdround);
        }

        public void OnResolutionChanged()
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.OnResolutionChanged();
            }
        }
    }
}
