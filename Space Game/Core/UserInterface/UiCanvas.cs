using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Galaxy_Explovive.Core.UserInterface
{
    public class UiCanvas
    {
        public enum RootFill { Fill, Fit, Cover, Fix }
        public enum RootSide { Left, Right, Top, Bottom, None }

        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;
        public float CenterX;
        public float CenterY;
        public float Width;
        public float Height;
        public float MinWidth = 0;
        public float MinHeight = 0;
        public float MaxWidth = Int32.MaxValue;
        public float MaxHeight = Int32.MaxValue;


        private UiElement mRoot;
        private RectangleF mRootRectangle;
        private float mAspectRatio;
        private float mRelCenterX;
        private float mRelCenterY;
        private float mRelHeight;
        private float mRelWidth;

        public UiCanvas(UiElement root, float relCenterX, float relCenterY, int width, int height)
        {
            mRoot = root;
            mRelCenterX = relCenterX;
            mRelCenterY = relCenterY;
            MinWidth = MaxWidth = width;
            MinHeight = MaxHeight = height;
        }
        public UiCanvas(UiElement root, float relCenterX, float relCenterY, float relWidth, float relHeight)
        {
            mRoot = root;
            mRelCenterX = relCenterX;
            mRelCenterY = relCenterY;
            mRelWidth = relWidth;
            mRelHeight = relHeight;
        }
        private static RectangleF GetRootRectangle(UiElement root)
        {
            float rootX, rootY, rootWidth, rootHeight;

            rootX = Globals.mGraphicsDevice.Viewport.X;
            rootY = Globals.mGraphicsDevice.Viewport.Y;
            rootWidth = Globals.mGraphicsDevice.Viewport.Width;
            rootHeight = Globals.mGraphicsDevice.Viewport.Height;

            if (root != null)
            {
                rootWidth = root.Canvas.Width;
                rootHeight = root.Canvas.Height;
                rootX = root.Canvas.CenterX - (rootWidth / 2);
                rootY = root.Canvas.CenterY - (rootHeight / 2);
            }
            return new RectangleF(rootX, rootY, rootWidth, rootHeight);
        }
        public void Update()
        {
            mRootRectangle = GetRootRectangle(mRoot);

            CenterX = (mRelCenterX * mRootRectangle.Width) + mRootRectangle.X;
            CenterY = (mRelCenterY * mRootRectangle.Height) + mRootRectangle.Y;
            Width = (mRelWidth * mRootRectangle.Width);
            Height = (mRelHeight * mRootRectangle.Height);

            float rootX, rootY, rootWidth, rootHeight;

            rootX = mRootRectangle.X;
            rootY = mRootRectangle.Y;
            rootWidth = mRootRectangle.Width;
            rootHeight = mRootRectangle.Height;

            if (Width < MinWidth) { Width = MinWidth; }
            if (Width > MaxWidth) { Width = MaxWidth; }
            if (Height < MinHeight) { Height = MinHeight; }
            if (Height > MaxHeight) { Height = MinHeight; }
            GetSticky(rootWidth, rootHeight, rootX, rootY);
            GetSide(rootWidth, rootHeight, rootX, rootY);
        }
        

        public Rectangle ToRectangle()
        {
            return new RectangleF(CenterX - (Width / 2), CenterY - (Height / 2), Width, Height).ToRectangle();
        }
        private void GetSticky(float rootWidth, float rootHeight, float rootX, float rootY)
        {
            float rootAspectRatio = rootWidth / rootHeight;
            mAspectRatio = Width / Height;

            switch (Fill)
            {
                case RootFill.Fix:
                    break;
                case RootFill.Cover:
                    Width = rootWidth;
                    Height = rootHeight;
                    CenterX = rootX + (rootWidth / 2);
                    CenterY = rootY + (rootHeight / 2);
                    break;
                case RootFill.Fill:
                    if (rootAspectRatio < mAspectRatio)
                    {
                        Height = rootHeight;
                        Width = Height * mAspectRatio;
                    }
                    else
                    {
                        Width = rootWidth;
                        Height = Width / mAspectRatio;
                    }
                    CenterX = rootX + (rootWidth / 2);
                    CenterY = rootY + (rootHeight / 2);
                    break;
                case RootFill.Fit:
                    if (rootAspectRatio > mAspectRatio)
                    {
                        Height = rootHeight;
                        Width = Height * mAspectRatio;
                    }
                    else
                    {
                        Width = rootWidth;
                        Height = Width / mAspectRatio;
                    }
                    CenterX = rootX + (rootWidth / 2);
                    CenterY = rootY + (rootHeight / 2);
                    break;
            }
        }
        private void GetSide(float rootWidth, float rootHeight, float rootX, float rootY)
        {
            switch (Side)
            {
                case RootSide.None:
                    break;
                case RootSide.Left:
                    CenterX -= CenterX - rootX - (Width / 2) ; 
                    break;
                case RootSide.Right:
                    CenterX += rootWidth + rootX - (CenterX + (Width / 2));
                    break;
                case RootSide.Top:
                    CenterY -= CenterY - rootY - (Height / 2);
                    break;
                case RootSide.Bottom:
                    CenterY += rootHeight + rootY - (CenterY + (Height / 2));  
                    break;
            }
        }
        
    }
}
