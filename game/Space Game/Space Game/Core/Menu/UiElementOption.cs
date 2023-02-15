using Microsoft.Xna.Framework;
using rache_der_reti.Core.InputManagement;

namespace rache_der_reti.Core.Menu;

public class UiElementOption : UiElementList
{
    private readonly UiElementText mOptionsDescriptorText;
    private readonly UiElementText mOptionsText;
    private readonly string[] mOptions;
    private int mOptionsIndex;


    public UiElementOption(string optionsDescriptor, string[] options) : base(false)
    {
        mOptions = options;
        mOptionsDescriptorText = new UiElementText(optionsDescriptor);
        mOptionsText = new UiElementText(mOptions[mOptionsIndex]);

        ChildElements.Add(mOptionsDescriptorText);
        ChildElements.Add(mOptionsText);

        SetMargin(5);
    }

/*
    public void ChangeOptionsIndex(int index)
    {
        mOptionsIndex = index;
        mOptionsText.UpdateText(mOptions[mOptionsIndex]);
    }
*/
/*
    public void ChangeOptionsIndex(string option)
    {
        int i = 0;
        while (i < mOptions.Length)
        {
            if (mOptions[i] == option)
            {
                break;
            }
            i++;
        }
        i %= mOptions.Length;
        mOptionsText.UpdateText(mOptions[i]);
    }
*/

/*
    public string GetSelectedOption()
    {
        return mOptions[mOptionsIndex];
    }
*/

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