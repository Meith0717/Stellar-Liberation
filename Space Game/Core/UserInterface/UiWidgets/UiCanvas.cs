using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.ComponentModel;

namespace Galaxy_Explovive.Core.UserInterface.UiWidgets
{
    public class UiCanvas
    {
        public enum RootFill { Fill, Fit, Cover, Fix }
        public enum RootSide { Left, Right, Top, Bottom, None }

        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;

        public float RelativeX = .5f;
        public float RelativeY = .5f;
        public float RelativeH = .5f;
        public float RelativeW = .5f;
        public float? Height = null;
        public float? Width = null;

        private float mCenterX = 0;
        private float mCenterY = 0;
        private float mWidth = 100;
        private float mHeight = 100;

        private readonly UiElement mRoot;
        private RectangleF mRootRectangle;

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
            mRootRectangle = GetRootRectangle();
            mCenterX = mRootRectangle.Width * RelativeX + mRootRectangle.X;
            mCenterY = mRootRectangle.Height * RelativeY + mRootRectangle.Y;
            mWidth = Width == null ? mRootRectangle.Width * RelativeW : (float)Width;
            mHeight = Height == null ? mRootRectangle.Height * RelativeW : (float)Height;
            GetSticky(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);
            GetSide(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);

        }

        public Rectangle ToRectangle()
        {
            float X = mCenterX - mWidth / 2;
            float Y = mCenterY - mHeight / 2;
            return new RectangleF(X, Y, mWidth, mHeight).ToRectangle();
        }

        public RectangleF GetRootRectangle()
        {
            float rootX, rootY, rootWidth, rootHeight;

            rootX = Globals.mGraphicsDevice.Viewport.X;
            rootY = Globals.mGraphicsDevice.Viewport.Y;
            rootWidth = Globals.mGraphicsDevice.Viewport.Width;
            rootHeight = Globals.mGraphicsDevice.Viewport.Height;

            if (mRoot != null)
            {
                rootWidth = mRoot.Canvas.mWidth;
                rootHeight = mRoot.Canvas.mHeight;
                rootX = mRoot.Canvas.mCenterX - rootWidth / 2;
                rootY = mRoot.Canvas.mCenterY - rootHeight / 2;
            }
            return new RectangleF(rootX, rootY, rootWidth, rootHeight);
        }


        // Resolution Stuff ##################################################################
        private void GetSticky(float rootWidth, float rootHeight, float rootX, float rootY)
        {
            float rootAspectRatio = rootWidth / rootHeight;
            float aspectRatio = mWidth / mHeight;

            switch (Fill)
            {
                case RootFill.Fix:
                    break;

                case RootFill.Cover:
                    mWidth = rootWidth;
                    mHeight = rootHeight;
                    mCenterX = rootX + rootWidth / 2;
                    mCenterY = rootY + rootHeight / 2;
                    break;

                case RootFill.Fill:
                    if (rootAspectRatio < aspectRatio)
                    {
                        mHeight = rootHeight;
                        mWidth = mHeight * aspectRatio;
                    }
                    else
                    {
                        mWidth = rootWidth;
                        mHeight = mWidth / aspectRatio;
                    }
                    mCenterX = rootX + rootWidth / 2;
                    mCenterY = rootY + rootHeight / 2;
                    break;

                case RootFill.Fit:
                    if (rootAspectRatio > aspectRatio)
                    {
                        mHeight = rootHeight;
                        mWidth = mHeight * aspectRatio;
                    }
                    else
                    {
                        mWidth = rootWidth;
                        mHeight = mWidth / aspectRatio;
                    }
                    mCenterX = rootX + rootWidth / 2;
                    mCenterY = rootY + rootHeight / 2;
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
                    mCenterX -= mCenterX - rootX - mWidth / 2;
                    break;
                case RootSide.Right:
                    mCenterX += rootWidth + rootX - (mCenterX + mWidth / 2);
                    break;
                case RootSide.Top:
                    mCenterY -= mCenterY - rootY - mHeight / 2;
                    break;
                case RootSide.Bottom:
                    mCenterY += rootHeight + rootY - (mCenterY + mHeight / 2);
                    break;
            }
        }
        // ####################################################################################

    }
}
