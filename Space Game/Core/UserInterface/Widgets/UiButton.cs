using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiButton : UiElement
    {

        private Texture2D mTexture;
        private float mScale;

        public UiButton(UiLayer root, float relX, float relY, string buttonTexture) : base(root)
        {
            mTexture = TextureManager.Instance.GetTexture("buttonTexture");
            mTexture = TextureManager.Instance.GetTexture("buttonTexture");
            Canvas = new(root, relX, relY, 10, 10);
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void OnResolutionChanged()
        {
            throw new NotImplementedException();
        }

        public override void Update(InputState inputState)
        {
            throw new NotImplementedException();
        }
    }
}
