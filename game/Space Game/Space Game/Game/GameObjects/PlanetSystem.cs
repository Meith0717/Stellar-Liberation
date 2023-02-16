using Microsoft.Xna.Framework;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Game.Layers;
using System;

namespace Space_Game.Game.GameObjects
{
    public class PlanetSystem : GameObject
    {
        public StarType mStartype;

        public enum StarType
        {
            B,
            F,
            G,
            K,
            M
        }

        public PlanetSystem(Vector2 position) 
        {
            var textureHeight = 256;
            var textureWidth = 256;
            Scale = 16;

            Array starTypes = Enum.GetValues(typeof(StarType));
            mStartype = (StarType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            Position = position;
            Offset = new Vector2(textureHeight / 2, textureWidth / 2);
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            HoverBox = new CircleF(Position, Math.Max(TextureWidth / Scale, TextureHeight / Scale));
            switch (mStartype)
            {
                case StarType.B:
                    {
                        NormalTextureId = "sunTypeB";
                        HoverTectureId = "sunTypeBHover";
                        break;
                    }
                case StarType.F:
                    {
                        NormalTextureId = "sunTypeF";
                        HoverTectureId = "sunTypeFHover";
                        break;
                    }
                case StarType.G:
                    {
                        NormalTextureId = "sunTypeG";
                        HoverTectureId = "sunTypeGHover";
                        break;
                    }
                case StarType.K:
                    {
                        NormalTextureId = "sunTypeK";
                        HoverTectureId = "sunTypeKHover";
                        break;
                    }
                case StarType.M: 
                    {
                        NormalTextureId = "sunTypeM";
                        HoverTectureId = "sunTypeMHover";
                        break;
                    }
            }
            TextureId = NormalTextureId;
        }

        public override void Update(GameTime gameTime, InputState inputState, Camera2d camera2d)
        {
            this.ManageHover(inputState, camera2d.ViewToWorld(inputState.mMousePosition.ToVector2()), Clicked);
            if (Hover)
            {
                TextureId = HoverTectureId;
            } else
            {
                TextureId = NormalTextureId;
            }
        }

        public override void Draw()
        {
            TextureManager.GetInstance().Draw(TextureId, Position - Offset / Scale, TextureWidth / Scale, TextureHeight / Scale);
        }

        public void Clicked()
        {
            Globals.mLayerManager.AddLayer(new PlanetSystemLayer(this));
        }
    }
}
