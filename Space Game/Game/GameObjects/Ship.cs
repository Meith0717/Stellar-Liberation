using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Core.Maths;
using System;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class Ship : MovableObject
    {
        // Constant
        const int textureWidth = 352;
        const int textureHeight = 256;

        // Cross Hair
        private CrossHair mCrossHair;
        private CrossHair mTargetCrossHair;

        // bools
        private bool mSelect = false;
        private bool mTrack = false;

        public Ship(Vector2 position)
        {
            // Textures Stuff
            TextureHeight = textureHeight;
            TextureWidth = textureWidth;
            NormalTextureId = "ship";
            HoverTectureId = "shipHover";

            // Position Stuff
            Position = TargetPoint = position;
            Velocity = 0.01f;
            Offset = new Vector2(TextureWidth, TextureHeight) / 2;

            // Cross Hair Stuff
            mCrossHair = new CrossHair(0.062f, 0.08f, Position, Color.Red);
            mTargetCrossHair = new CrossHair(0.062f, 0.08f, TargetPoint, Color.Red);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Update Other Stuff
            mCrossHair.Update(Position, Hover, Color.Red);
            mTargetCrossHair.Update(TargetPoint, Hover, Color.Red);
            HoverBox = new CircleF(Position, MathF.Max(TextureHeight / 10f, TextureWidth / 10f));

            // Check Left Klick Outside Hover
            SetTargetOnRightClick(inputState);

            // Hover and Movement Updates            
            this.ManageHover(inputState, SelectOnCkick, TrackOnDoubleClick);
            this.Move(gameTime);

            // Textures Updates
            TextureId = NormalTextureId;
            if (mSelect) {
                TextureId = HoverTectureId;
            }
            if (mTrack) 
            {
                if (!IsMoving) { mTrack = !mTrack; }
                Globals.mCamera2d.mTargetPosition = Position;
            }
        }

        public override void Draw()
        {
            // Draw Other Stuff
            base.Draw();
            mCrossHair.Draw();

            // Draw Ship
            var ship = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(ship, Position, null,
                Color.White, Rotation, Offset, 0.15f, SpriteEffects.None, 0.0f);

            // Draw Position and Target Cross Hair
            if (!IsMoving) { return; }
            mTargetCrossHair.mDrawInnerRing = true;
            mTargetCrossHair.Draw();

            TextureManager.GetInstance().DrawString("text", Position + new Vector2(15, 25),
                MyMathF.GetInstance().GetTimeFromSekonds(TravelTime), Color.Red);
        }

        private void SetTargetOnRightClick(InputState inputState)
        {
            if (!mSelect || Hover) { return; }
            if (inputState.mMouseActionType == MouseActionType.RightClick) 
            {
                this.SetTarget(Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2()));            
                mSelect = false;
            }
        }

        private void SelectOnCkick()
        {
            mSelect = !mSelect;
        }

        private void TrackOnDoubleClick()
        {
            mTrack = !mTrack;
            Globals.mCamera2d.SetZoom(Globals.mCamera2d.mMimZoom);
        }
    }
}
