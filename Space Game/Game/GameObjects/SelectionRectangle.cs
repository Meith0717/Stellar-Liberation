using Microsoft.Xna.Framework;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using Space_Game.Core.TextureManagement;

namespace Space_Game.Game.GameObjects
{
    public class SelectionRectangle : GameObject
    {
        private Camera2d mCamera;
        internal Rectangle mSelectionRectangle;
        internal Rectangle mMouseRectangle;

        internal Point mMousePosition;

        public SelectionRectangle(Camera2d camera)
        {
            mCamera = camera;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mMouseActionType != MouseActionType.LeftClickHold)
            {
                mSelectionRectangle.Width = mSelectionRectangle.Height = 0;
                return;
            }

            mMousePosition = inputState.mMousePosition;
            mMouseRectangle = inputState.mMouseRectangle;

            // Transform location and position.
            Point topLeft = mCamera.ViewToWorld(mMouseRectangle.Location.ToVector2()).ToPoint();
            Point bottomRight = new Point(
                mMouseRectangle.Location.X + mMouseRectangle.Width,
                mMouseRectangle.Location.Y + mMouseRectangle.Height
            );

            // Transform bottom right.
            bottomRight = mCamera.ViewToWorld(bottomRight.ToVector2()).ToPoint();

            // Get location, width and height of rectangle.
            mSelectionRectangle.Location = topLeft;
            mSelectionRectangle.Width = bottomRight.X - topLeft.X;
            mSelectionRectangle.Height = bottomRight.Y - topLeft.Y;
        }

        public override void Draw()
        {
            TextureManager.GetInstance().DrawRectangle("transparent", mSelectionRectangle);
        }
    }
}
