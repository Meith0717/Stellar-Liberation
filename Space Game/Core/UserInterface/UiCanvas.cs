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
        public double X = 0.5;
        public double Y = 0.5;
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


        // Constructor ########################################################################## 
        public UiCanvas(UiElement root)
        {
            mRoot = root;
        }

        // #########################################################################################


        // Update Resolution
        public void OnResolutionChanged()
        {
            // Get new position depending on the Root
            mRootRectangle = GetRootRectangle(mRoot);
            X = (mRelCenterX * mRootRectangle.Width) + mRootRectangle.X;
            Y = (mRelCenterY * mRootRectangle.Height) + mRootRectangle.Y;
            Width = (mRelWidth * mRootRectangle.Width);
            Height = (mRelHeight * mRootRectangle.Height);

            if (Width < MinWidth) { Width = MinWidth; }
            if (Width > MaxWidth) { Width = MaxWidth; }
            if (Height < MinHeight) { Height = MinHeight; }
            if (Height > MaxHeight) { Height = MinHeight; }
            GetSticky(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);
            GetSide(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);
        }
        
        public Rectangle ToRectangle()
        {
            return new RectangleF((float)X - (Width / 2), (float)Y - (Height / 2), Width, Height).ToRectangle();
        }

        // Resolution Stuff ##################################################################
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
                    X = rootX + (rootWidth / 2);
                    Y = rootY + (rootHeight / 2);
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
                    X = rootX + (rootWidth / 2);
                    Y = rootY + (rootHeight / 2);
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
                    X = rootX + (rootWidth / 2);
                    Y = rootY + (rootHeight / 2);
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
                    X -= X - rootX - (Width / 2) ; 
                    break;
                case RootSide.Right:
                    X += rootWidth + rootX - (X + (Width / 2));
                    break;
                case RootSide.Top:
                    Y -= Y - rootY - (Height / 2);
                    break;
                case RootSide.Bottom:
                    Y += rootHeight + rootY - (Y + (Height / 2));  
                    break;
            }
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
                rootX = root.Canvas.X - (rootWidth / 2);
                rootY = root.Canvas.Y - (rootHeight / 2);
            }
            return new RectangleF(rootX, rootY, rootWidth, rootHeight);
        }
        // ####################################################################################

    }
}
