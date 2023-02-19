using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;

namespace rache_der_reti.Core.Menu;

public class UiElement
{
    public Color BackgroundColor { get; set; } = Color.Transparent;
    public Color HoverBackgroundColor { get; set; } = Color.Transparent;

    public float BackgroundAlpha { get; set; } = 1f;
    
    // click handling
    protected bool mIsHovering;
    protected Action mOnClickAction;
    public void setOnClickPointer(Action action)
    {
        mOnClickAction = action;
    }

    // dimensions
    public int Width { get; set; } = -1;
    public int Height { get; set; } = -1;

    private int MinWidth { get; } = 0;
    public int MaxWidth { get; set; } = Int32.MaxValue;
    private int MinHeight { get; } = 0;
    public int MaxHeight { get; set; } = Int32.MaxValue;

    public int WidthPercent { get; set; } = -1;
    private int HeightPercent { get; } = -1;
    public int DimensionParts { get; set; } = 1;

    private int Bounds(int input, int min, int max)
    {
        if (input < min)
        {
            input = min;
        }
        if (input > max)
        {
            input = max;
        }
        return input;
    }

    internal int GetWidth(int parentWidth)
    {
        if (Width >= 0)
        {
            return Bounds(Width, MinWidth, MaxWidth);
        }

        if (WidthPercent >= 0)
        {
            return Bounds((int)(parentWidth * (WidthPercent / 100f)), MinWidth, MaxWidth);
        }
        return Bounds(parentWidth, MinWidth, MaxWidth);
    }
    public int GetHeight(int parentHeight)
    {
        if (Height >= 0)
        {
            return Bounds(Height, MinHeight, MaxHeight);
        }

        if (HeightPercent >= 0)
        {
            return Bounds((int)(parentHeight * (HeightPercent / 100f)), MinHeight, MaxHeight);
        }
        return Bounds(parentHeight, MinHeight, MaxHeight);
    }

    // margin stuff
    public int MarginLeft { get; set; }
    public int MarginRight { get; set; }
    public int MarginTop { get; set; }
    protected int MarginBottom { get; private set; }

    public void SetMargin(int margin)
    {
        MarginLeft = MarginRight = MarginTop = MarginBottom = margin;
    }
    
    // ReSharper disable once UnusedMember.Global
    public void SetMargin(int marginLeftRight, int marginTopBottom)
    {
        MarginLeft = MarginRight = marginLeftRight;
        MarginTop = MarginBottom = marginTopBottom;
    }

    // alignment
    public VerticalAlignment MyVerticalAlignment { get; set; } = VerticalAlignment.Center;
    public HorizontalAlignment MyHorizontalAlignt { get; set; } = HorizontalAlignment.Center;
    
    public enum HorizontalAlignment
    {
        Left, Center, Right
    }
    public enum VerticalAlignment
    {
        Top, Center, Bottom
    }

    // calculated position and dimensions
    protected Rectangle CalculatedRectangle { get; set; }
    
    // child elements
    public List<UiElement> ChildElements { get; } = new List<UiElement>();

    // update
    public virtual void Update(Rectangle rectangle)
    {
        // apply margin
        CalculatedRectangle = new Rectangle(rectangle.X + MarginLeft, rectangle.Y + MarginTop,
            rectangle.Width - MarginLeft - MarginRight, rectangle.Height - MarginTop - MarginBottom);
        
        // update childs
        foreach(UiElement uiElement in ChildElements)
        {
            UpdateChild(uiElement);
        }
    }

    private void UpdateChild(UiElement childElement)
    {
        int width = childElement.GetWidth(CalculatedRectangle.Width);
        int height = childElement.GetHeight(CalculatedRectangle.Height);

        // check bounds
        if (width < 0 || width > CalculatedRectangle.Width)
        {
            width = CalculatedRectangle.Width;
        }
        if (height < 0 || height > CalculatedRectangle.Height)
        {
            height = CalculatedRectangle.Height;
        }
        
        // calculate offsets
        var xOffset = CalculateXOffset(childElement, width);
        
        var yOffset = CalculateYOffset(childElement, height);
        
        // apply offset to child
        childElement.Update(new Rectangle(CalculatedRectangle.X + xOffset, CalculatedRectangle.Y + yOffset, width, height));
    }

    private int CalculateXOffset(UiElement childElement, int width)
    {
        var xOffset = childElement.MyHorizontalAlignt switch
        {
            HorizontalAlignment.Left => 0,
            HorizontalAlignment.Right => CalculatedRectangle.Width - width,
            HorizontalAlignment.Center => (CalculatedRectangle.Width - width) / 2,
            _ => 0
        };
        return xOffset;
    }

    private int CalculateYOffset(UiElement childElement, int height)
    {
        var yOffset = childElement.MyVerticalAlignment switch
        {
            VerticalAlignment.Top => 0,
            VerticalAlignment.Bottom => CalculatedRectangle.Height - height,
            VerticalAlignment.Center => (CalculatedRectangle.Height - height) / 2,
            _ => 0
        };
        return yOffset;
    }


    // render
    public virtual void Render()
    {
        if (BackgroundColor != Color.Transparent)
        {
            Color color = (mIsHovering && HoverBackgroundColor != Color.Transparent) ? HoverBackgroundColor : BackgroundColor;
            TextureManager.GetInstance().GetSpriteBatch().FillRectangle(CalculatedRectangle, new Color(color, BackgroundAlpha), 1);
        }
        foreach(UiElement uiElement in ChildElements)
        {
            uiElement.Render();
        }
    }
    
    // input handling
    public virtual void HandleInput(InputState inputState)
    {
        Point mousePosition = inputState.mMousePosition;
        if (IsPointInsideRectangle(mousePosition, GetClickBox()))
        {
            mIsHovering = true;
            if (inputState.mMouseActionType == MouseActionType.LeftClickReleased)
            {

                OnClick();
                if (mOnClickAction != null)
                {
                    mOnClickAction();
                }
            }
        }
        else
        {
            mIsHovering = false;
        }
        
        foreach(UiElement uiElement in ChildElements)
        {
            uiElement.HandleInput(inputState);
        } 
    }


    // protected methods
    private static bool IsPointInsideRectangle(Point point, Rectangle rectangle)
    {
        return point.X >= rectangle.Left 
               && point.X <= rectangle.Right
               && point.Y > rectangle.Top 
               && point.Y < rectangle.Bottom;
    }
    
    protected virtual Rectangle GetClickBox()
    {
        return CalculatedRectangle;
    }

    protected virtual void OnClick()
    {
    }
}