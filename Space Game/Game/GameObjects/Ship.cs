using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.Effects;
using Space_Game.Core.GameObject;
using Space_Game.Core.Maths;
using System;
using System.Diagnostics;
using System.Linq;

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
        private bool mAttack = false;
        private bool mEnemy;

        // Weapons
        private double mShootTimer;

        // Live
        private int mShield = 100;
        private int mHull = 100;

        public Ship(Vector2 position, bool enemy = false)
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

            // Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)position.X, (int)position.Y);
            mEnemy = enemy;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mShootTimer += gameTime.ElapsedGameTime.Milliseconds / 1000d * Globals.mTimeWarp;

            // Update Other Stuff
            mCrossHair.Update(Position, Hover, mEnemy ? Color.Red : Color.LightGreen);
            mTargetCrossHair.Update(TargetPoint, Hover, mEnemy ? Color.Red : Color.LightGreen);
            HoverBox = new CircleF(Position, MathF.Max(TextureHeight / 10f, TextureWidth / 10f));
            ShootOnOtherShip();
            ToggleAttak(inputState);

            // Hover and Movement Updates
            CheckForSelectionRectangle();
            this.ManageHover(inputState, TrackOnCkick, OnDoubleClick);
            this.Move(gameTime);

            // Check Left Klick Outside Hover
            SetTargetOnRightClick(inputState);

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
        {    if (mHull <= 0) { return; }
            // Draw Other Stuff
            this.DrawPath(mEnemy ? Color.Red : Color.Green);
            mCrossHair.Draw();

            // Draw Ship
            var ship = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(ship, Position, null,
                Color.White, TextureRotation, Offset, 0.15f, SpriteEffects.None, 0.0f);

            // Draw Position and Target Cross Hair
            if (!IsMoving) { return; }
            mTargetCrossHair.mDrawInnerRing = true;
            mTargetCrossHair.Draw();

            TextureManager.GetInstance().DrawString("text", Position + new Vector2(15, 25),
                MyMathF.GetInstance().GetTimeFromSekonds(TravelTime), mEnemy ? Color.Red : Color.LightGreen);
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

        private void TrackOnCkick()
        {
            if (mEnemy) { return; }
            mTrack = !mTrack;
            if (!mTrack) { return; }
            Globals.mCamera2d.SetZoom(Globals.mCamera2d.mMimZoom);
        }

        private void OnDoubleClick()
        {
        }
    
        private void CheckForSelectionRectangle()
        {
            if (mEnemy) { return; }
            if (Globals.mGameLayer.mSelectionRectangle.mSelectionRectangle.Contains(Position))
            {
                mSelect= true;
            }
        }
    
        private void ToggleAttak(InputState inputState)
        {
            if (mEnemy) { return; }
            if (inputState.mActionList.Contains(ActionType.Shoot)) 
            {
                mAttack = !mAttack;
            }
        }

        private void ShootOnOtherShip()
        {
            if (!mAttack) { return; }
            if (mShootTimer < 1.5 - Globals.mRandom.NextDouble()) { return; }
            mShootTimer = 0;
            var objects = Globals.mGameLayer.GetObjectsInRadius(Position, 1000).OfType<Ship>().ToList();
            objects.Remove(this);
            if (objects.Count == 0) { return; }
            Ship obj = objects[Globals.mRandom.Next(objects.Count)];
            if ((mEnemy && !obj.mEnemy) || (!mEnemy && obj.mEnemy))
            { 
                Globals.mWeaponManager.Shoot(this, obj, Color.OrangeRed , 1000);
            }
        }
    
        public void TakeDamage()
        {
            if (mShield > 0)
            {
                mShield -= 5;
                return;
            }
            mHull -= 5;
        }
    }
}
