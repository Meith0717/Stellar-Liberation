using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Effects
{
    internal class ParllaxManager
    {
        private Vector2 mLastPosition = Globals.Camera2d.Position;
        private List<ParllaxBackground> mBackdrounds = new List<ParllaxBackground>();

        public void Update()
        {
            var movement = Globals.Camera2d.Position - mLastPosition;
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Update(movement * Globals.Camera2d.mZoom);
            }
            mLastPosition = Globals.Camera2d.Position;
        }

        public void Draw()
        {
            foreach (ParllaxBackground backdround in mBackdrounds)
            {
                backdround.Draw();
            }
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
