using rache_der_reti.Core.InputManagement;

namespace rache_der_reti.Core.Menu;

public class UiElementSlider : UiElementSprite
{
    private readonly UiElementSprite mKnob;
    public float mSliderValue;
    private readonly float mStartX = 0.5f;
    private readonly float mEndX = 0.92f;
    public UiElementSlider(string spriteId) : base(spriteId)
    {
        mKnob = new UiElementSprite("settings_sliderknob");
        mKnob.MyHorizontalAlignt = HorizontalAlignment.Left;
        ChildElements.Add(mKnob);
    }

    public void SetSliderValue(float startValue)
    {
        mSliderValue = startValue;
        ApplySliderValue();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.mMouseActionType == MouseActionType.LeftClickHold && 
            CalculatedRectangle.Contains(inputState.mMousePosition))
        {
            int xOffset = inputState.mMousePosition.X - CalculatedRectangle.Left;

            int width = (int)(CalculatedRectangle.Width * (mEndX - mStartX));
            int minX = (int)(CalculatedRectangle.Width * mStartX);
            int maxX = (int)(CalculatedRectangle.Width * mEndX);

            if (xOffset < minX)
            {
                xOffset = minX;
            } else if (xOffset > maxX)
            {
                xOffset = maxX;
            }

            mSliderValue = (xOffset - CalculatedRectangle.Width * mStartX) / width;
            ApplySliderValue();
            mOnClickAction();
        }
    }

    private void ApplySliderValue()
    {
        int width = (int)(CalculatedRectangle.Width * (mEndX - mStartX));
        int minX = (int)(CalculatedRectangle.Width * mStartX);
        int xOffset = (int)(mSliderValue * width + minX);
        mKnob.MarginLeft = xOffset;
        mKnob.MarginRight = CalculatedRectangle.Width - xOffset;
    }
}