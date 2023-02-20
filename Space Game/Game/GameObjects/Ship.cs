using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class Ship : MovableObject
    {
        const int textureWidth = 352;
        const int textureHeight = 256;
        private bool mSelect;
        private CrossHair mCrossHair;
        private CrossHair mTargetCrossHair;


        public Ship(Vector2 position)
        {
            Position = TargetPoint = position;
            TextureHeight= textureHeight;
            TextureWidth= textureWidth;
            Velocity = 0.01f;
            Offset = new Vector2(TextureWidth, TextureHeight) / 2;
            mCrossHair = new CrossHair(0.062f, 0.08f, Position, Color.Red);
            mTargetCrossHair = new CrossHair(0.062f, 0.08f, TargetPoint, Color.Red);
            NormalTextureId = "ship";
            HoverTectureId = "shipHover";
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            SetTarget(inputState);
            HoverBox = new CircleF(Position, MathF.Max(TextureHeight / 10f, TextureWidth / 10f));
            this.ManageHover(inputState, Select, this.StopMoving);
            this.Move(gameTime);
            mCrossHair.Update(Position, Hover);
            mTargetCrossHair.Update(TargetPoint, Hover);
            TextureId = NormalTextureId;
            if (mSelect) {
                TextureId = HoverTectureId;
            }
        }

        public override void Draw()
        {
            base.Draw();
            mCrossHair.Draw();
            var ship = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(ship, Position, null,
                Color.White, Rotation, Offset, 0.15f, SpriteEffects.None, 0.0f);
            if (!IsMoving) { return; }
            mTargetCrossHair.mDrawInnerRing = true;
            mTargetCrossHair.Draw();
        }

        private void SetTarget(InputState inputState)
        {
            if (!mSelect || Hover) { return; }
            if (inputState.mMouseActionType == MouseActionType.LeftClickDouble) 
            {
                this.SetTarget(Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2()));            
                mSelect = false;
            }
        }

        private void Select()
        {
            mSelect = !mSelect;
            Globals.mCamera2d.mTargetPosition = Position;
        }

    }
}
