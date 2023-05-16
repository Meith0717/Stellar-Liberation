using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiSprite : UiElement
    {

        private Texture2D mTexture;
        public Color Color { get; set; } = Color.White;

        public UiSprite(UiLayer root, double relX, double relY, string spriteTexture, float scale = 1,
            UiCanvas.RootFill fill = UiCanvas.RootFill.Fix) : base(root)
        {
            mTexture = TextureManager.Instance.GetTexture(spriteTexture);
            Canvas = new(root, (float)relX, (float)relY, (int)(mTexture.Width * scale), (int)(mTexture.Height * scale))
            {
                Fill = fill
            };
        }

        public override void Draw()
        {
            SpriteBatch sb = TextureManager.Instance.GetSpriteBatch();
            sb.Draw(mTexture, Canvas.ToRectangle(), Color);
        }

        public override void OnResolutionChanged()
        {
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState) { }
    }
}
