using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;

namespace Galaxy_Explovive.Core.Menu;

public class UiElementOption : UiElementList
{
    private readonly UiElementText mOptionsDescriptorText;
    private readonly UiElementText mOptionsText;
    private readonly string[] mOptions;
    private int mOptionsIndex;


    public UiElementOption(string optionsDescriptor, string[] options) : base(false)
    {
        mOptions = options;
        mOptionsDescriptorText = new UiElementText(optionsDescriptor, "text");
        mOptionsText = new UiElementText(mOptions[mOptionsIndex], "text");

        ChildElements.Add(mOptionsDescriptorText);
        ChildElements.Add(mOptionsText);

        SetMargin(5);
    }

    public override void HandleInput(InputState inputState)
    {
        base.HandleInput(inputState);
        if (mIsHovering)
        {
            BackgroundAlpha = 0.2f;
            BackgroundColor = Color.Gray;
            mOptionsText.FontColor = Color.Black;
            mOptionsDescriptorText.FontColor = Color.Black;
        }
        else
        {
            BackgroundAlpha = 1;
            BackgroundColor = Color.Black;
            mOptionsText.FontColor = Color.White;
            mOptionsDescriptorText.FontColor = Color.White;
        }
    }

    protected override void OnClick()
    {
        mOptionsIndex = (mOptionsIndex + 1) % mOptions.Length;
        mOptionsText.UpdateText(mOptions[mOptionsIndex]);
    }
}