using Microsoft.Xna.Framework;

namespace rache_der_reti.Core.Menu;

public class UiElementList : UiElement
{
    private bool OrientationIsVertical { get; }
    
    public UiElementList(bool orientationIsVertical)
    {
        OrientationIsVertical = orientationIsVertical;
    }

    public override void Update(Rectangle rectangle)
    {
        // apply margin
        CalculatedRectangle = new Rectangle(rectangle.X + MarginLeft, rectangle.Y + MarginTop,
            rectangle.Width - MarginLeft - MarginRight, rectangle.Height - MarginTop - MarginBottom);
        UpdateChilds();
    }

    private void UpdateChilds()
    {
        int totalDimensionParts = 0;
        foreach (UiElement uiElement in ChildElements)
        {
            totalDimensionParts += uiElement.DimensionParts;
        }
        
        if (!OrientationIsVertical)
        {
            int xOffset = 0;
            foreach (UiElement uiElement in ChildElements)
            {
                int width = (int)(((float)uiElement.DimensionParts / totalDimensionParts) * CalculatedRectangle.Width);

                int height = uiElement.GetHeight(CalculatedRectangle.Height);
                int yOffset = 0;

                switch (uiElement.MyVerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        yOffset = 0;
                        break;
                    case VerticalAlignment.Bottom:
                        yOffset = CalculatedRectangle.Height - height;
                        break;
                    case VerticalAlignment.Center:
                        yOffset = (CalculatedRectangle.Height - height) / 2;
                        break;
                }
                uiElement.Update(new Rectangle(CalculatedRectangle.X + xOffset, CalculatedRectangle.Y + yOffset, width, height));
                xOffset += width;
            }
        }
        else
        {
            int yOffset = 0;
            foreach (UiElement uiElement in ChildElements)
            {
                int height = (int)(((float)uiElement.DimensionParts / totalDimensionParts) * CalculatedRectangle.Height);

                int width = uiElement.GetWidth(CalculatedRectangle.Width);
                int xOffset = 0;

                switch (uiElement.MyHorizontalAlignt)
                {
                    case HorizontalAlignment.Left:
                        xOffset = 0;
                        break;
                    case HorizontalAlignment.Right:
                        xOffset = CalculatedRectangle.Width - width;
                        break;
                    case HorizontalAlignment.Center:
                        xOffset = (CalculatedRectangle.Width - width) / 2;
                        break;
                }
                uiElement.Update(new Rectangle(CalculatedRectangle.X + xOffset, CalculatedRectangle.Y + yOffset, width, height));
                yOffset += height;
            }
        }
    }
}