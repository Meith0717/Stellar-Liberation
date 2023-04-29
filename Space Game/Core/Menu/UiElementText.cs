using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.Menu;

public class UiElementText : UiElement
{
    public string Font;
    internal Color FontColor { get; set; } = Color.Black;
    internal string mText;

    public UiElementText(string text, string font)
    {
        Font = font;
        UpdateText(text);
    }

    public void UpdateText(string text)
    {
        mText = text;
        Vector2 stringDimensions = TextureManager.Instance.GetSpriteFont(Font).MeasureString(text);
        Width = (int)stringDimensions.X;
        Height = (int)stringDimensions.Y;
    }

    public override void Render()
    {
        base.Render();
        TextureManager.Instance.DrawString(Font, new Vector2(CalculatedRectangle.X + (CalculatedRectangle.Width - Width) / 2,
            CalculatedRectangle.Y + (CalculatedRectangle.Height - Height) / 2), mText, FontColor);
    }
}