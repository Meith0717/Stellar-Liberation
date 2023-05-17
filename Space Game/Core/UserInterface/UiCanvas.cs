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
        public float CenterX = 0;
        public float CenterY = 0;
        public float Width = 100;
        public float Height = 100;

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
            GetSticky(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);
            GetSide(mRootRectangle.Width, mRootRectangle.Height, mRootRectangle.X, mRootRectangle.Y);
        }
        
        public Rectangle ToRectangle()
        {
            float X = CenterX - (Width / 2);
            float Y = CenterY - (Height / 2);
            return new RectangleF(X, Y , Width, Height).ToRectangle();
        }

        public  RectangleF GetRootRectangle()
        {
            float rootX, rootY, rootWidth, rootHeight;

            rootX = Globals.mGraphicsDevice.Viewport.X;
            rootY = Globals.mGraphicsDevice.Viewport.Y;
            rootWidth = Globals.mGraphicsDevice.Viewport.Width;
            rootHeight = Globals.mGraphicsDevice.Viewport.Height;

            if (mRoot != null)
            {
                rootWidth = mRoot.Canvas.Width;
                rootHeight = mRoot.Canvas.Height;
                rootX = mRoot.Canvas.CenterX - (rootWidth / 2);
                rootY = mRoot.Canvas.CenterY - (rootHeight / 2);
            }
            return new RectangleF(rootX, rootY, rootWidth, rootHeight);
        }


        // Resolution Stuff ##################################################################
        private void GetSticky(float rootWidth, float rootHeight, float rootX, float rootY)
        {
            float rootAspectRatio = rootWidth / rootHeight;
            float aspectRatio = Width / Height;

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
                    if (rootAspectRatio < aspectRatio)
                    {
                        Height = rootHeight;
                        Width = Height * aspectRatio;
                    }
                    else
                    {
                        Width = rootWidth;
                        Height = Width / aspectRatio;
                    }
                    CenterX = rootX + (rootWidth / 2);
                    CenterY = rootY + (rootHeight / 2);
                    break;
                case RootFill.Fit:
                    if (rootAspectRatio > aspectRatio)
                    {
                        Height = rootHeight;
                        Width = Height * aspectRatio;
                    }
                    else
                    {
                        Width = rootWidth;
                        Height = Width / aspectRatio;
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
        // ####################################################################################

    }
}
