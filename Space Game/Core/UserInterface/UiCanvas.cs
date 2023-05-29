using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.UserInterface
{
    public class UiCanvas
    {
        public enum RootFill { Fill, Fit, Cover, Fix }
        public enum RootSide { W, E, N, S, SE, NE, NW, SW, None }

        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;

        public float RelativeX = .5f;
        public float RelativeY = .5f;
        public float RelativeH = .5f;
        public float RelativeW = .5f;
        public float? Height = null;
        public float? Width = null;
        public int MarginX = 0;
        public int MarginY = 0;

        private float mCenterX = 0;
        private float mCenterY = 0;
        private float mWidth = 100;
        private float mHeight = 100;

        private readonly UiElement mRoot;
        private RectangleF mRootRectangle;

        // Constructor ########################################################################## 
        public UiCanvas(UiElement root) { mRoot = root; }

        // #########################################################################################


        // Update Resolution
        public void OnResolutionChanged(GraphicsDevice graphicsDevice)
        {
            // Get new position depending on the Root
            mRootRectangle = GetRootRectangle(graphicsDevice);
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

        public RectangleF GetRootRectangle(GraphicsDevice graphicsDevice)
        {
            float rootX, rootY, rootWidth, rootHeight;

            rootX = graphicsDevice.Viewport.X;
            rootY = graphicsDevice.Viewport.Y;
            rootWidth = graphicsDevice.Viewport.Width;
            rootHeight = graphicsDevice.Viewport.Height;

            if (mRoot != null)
            {
                rootWidth = mRoot.mCanvas.mWidth;
                rootHeight = mRoot.mCanvas.mHeight;
                rootX = mRoot.mCanvas.mCenterX - rootWidth / 2;
                rootY = mRoot.mCanvas.mCenterY - rootHeight / 2;
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
            void SideN() { mCenterY -= mCenterY - rootY - mHeight / 2 - MarginY; }
            void SideE() { mCenterX += rootWidth + rootX - (mCenterX + mWidth / 2) + MarginX; }
            void SideS() { mCenterY += rootHeight + rootY - (mCenterY + mHeight / 2) + MarginY; }
            void SideW() { mCenterX -= mCenterX - rootX - mWidth / 2 - MarginX; }

            switch (Side)
            {
                case RootSide.None:
                    break;
                case RootSide.N:
                    SideN();
                    break;
                case RootSide.NE:
                    SideN();
                    SideE();
                    break;
                case RootSide.E:
                    SideE();
                    break;
                case RootSide.SE: 
                    SideS();
                    SideE();
                    break;
                case RootSide.S:
                    SideS();
                    break;
                case RootSide.SW:
                    SideS();
                    SideW();
                    break;
                case RootSide.W:
                    SideW();
                    break;
                case RootSide.NW:
                    SideN();
                    SideW();
                    break;
            }
        }
    }
}
