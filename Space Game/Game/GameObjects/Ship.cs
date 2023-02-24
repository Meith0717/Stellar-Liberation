using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using Space_Game.Core.Maths;
using Space_Game.Core.TextureManagement;
using System;
using System.Linq;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class Ship : MovableObject
    {
        // Constant
        const int textureWidth = 352;
        const int textureHeight = 256;
        const float sclae = 0.15f;

        // Cross Hair
        private CrossHair mCrossHair;
        private CrossHair mTargetCrossHair;

        // bools
        private bool mSelect = false;
        private bool mTrack = false;
        private bool mAttack = false;
        private bool mDestroyed = false;
        private bool mEnemy;

        // Weapons
        private double mShootTimer;

        // Live
        private int mShield = 100;
        private int mHull = 100;

        // Methodes _____________________________________________________________
        public Ship(Vector2 position, bool enemy = false)
        {
            // Textures Stuff
            TextureHeight = textureHeight;
            TextureWidth = textureWidth;
            NormalTextureId = "ship";
            HoverTectureId = "shipHover";
            TextureColor = Color.White;
            TextureRotation = 0;
            TextureDepth = 1;
            TextureSclae = 0.15f;

            // Position Stuff
            Position = TargetPoint = position;
            Velocity = 0.05f;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;

            // Cross Hair Stuff
            mCrossHair = new CrossHair(0.062f, 0.08f, Position, Color.Red);
            mTargetCrossHair = new CrossHair(0.062f, 0.08f, TargetPoint, Color.Red);

            // Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)position.X, (int)position.Y);
            mEnemy = enemy;
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (mDestroyed) { return; }
            mShootTimer += gameTime.ElapsedGameTime.Milliseconds / 1000d * Globals.mTimeWarp;
            mCrossHair.Update(Position, Hover, mEnemy ? Color.Red : Color.LightGreen);
            mTargetCrossHair.Update(TargetPoint, Hover, mEnemy ? Color.Red : Color.LightGreen);
            ShootOnOtherShip();
            ToggleAttak(inputState);
            CheckForSelectionRectangle();
            this.ClickOnObject(inputState, 50, TrackOnCkick, null);
            this.Move(gameTime);
            SetTargetOnRightClick(inputState);
            UpdateTrackAnimation();
            CheckIfDestroied();
        }
        public override void Draw()
        {
            if (mDestroyed) { return; }
            this.DrawPath(mEnemy ? Color.Red : Color.Green);
            this.DrawGameObject();
            DrawLife();
            DrawMoving();
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

        // Update Stuff _________________________________________________
        private void UpdateTrackAnimation()
        {
            if (!mTrack) { return; }
            if (!IsMoving) { mTrack = !mTrack; }
            Globals.mCamera2d.mTargetPosition = Position;
        }
        private void CheckForSelectionRectangle()
        {
            if (mEnemy) { return; }
            if (Globals.mGameLayer.mSelectionRectangle.mSelectionRectangle.Contains(Position))
            {
                mSelect = true;
            }
        }
        private void SetTargetOnRightClick(InputState inputState)
        {
            if (!mSelect || Hover) { return; }
            if (inputState.mMouseActionType == MouseActionType.RightClick)
            {
                SetTarget(Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2()));
                mSelect = false;
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
                Globals.mWeaponManager.Shoot(this, obj, Color.OrangeRed, 1000);
            }
        }
        private void CheckIfDestroied()
        {
            if (mHull <= 0)
            {
                RemoveFromSpatialHashing();
                mDestroyed = true;
            }
        }
        private void TrackOnCkick()
        {
            if (mEnemy) { return; }
            mTrack = !mTrack;
            if (!mTrack) { return; }
        }
        // Draw Stuff _________________________________________________
        private void DrawLife()
        {
            var shieldPosition = Position - (TextureOffset * sclae);
            shieldPosition.Y -= 20;
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(shieldPosition, TextureWidth * sclae, 0, Color.LightGray, 2, 0);
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(shieldPosition, TextureWidth * sclae * mHull / 100, 0, Color.Green, 2, 1);
            shieldPosition.Y -= 10;
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(shieldPosition, TextureWidth * sclae, 0, Color.LightGray, 2, 0);
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(shieldPosition, TextureWidth * sclae * mShield / 100, 0, Color.Blue, 2, 1);
        }
        private void DrawMoving()
        {
            if (!IsMoving) { return; }
            mTargetCrossHair.mDrawInnerRing = true;
            mCrossHair.Draw();
            mTargetCrossHair.Draw();
            TextureManager.GetInstance().DrawString("text", Position + new Vector2(15, 25),
                MyMathF.GetInstance().GetTimeFromSekonds(TravelTime), mEnemy ? Color.Red : Color.LightGreen);
        }
    }
}
