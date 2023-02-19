using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Game.Layers;
using System;
using System.Collections.Generic;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int textureHeight = 512;
        const int textureWidth = 512;
        const int scale = 1;

        private bool mSelectable;

        [JsonProperty] public List<Planet> mPlanetList = new();
        [JsonProperty] public StarType mStartype;
        [JsonProperty] private CrossHair mCrossHair;

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
            Array starTypes = Enum.GetValues(typeof(StarType));
            mStartype = (StarType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            Position = position;
            Offset = new Vector2(textureHeight, textureWidth) / scale / 2;
            TextureWidth = textureWidth / scale;
            TextureHeight = textureHeight / scale;
            HoverBox = new CircleF(Position, MathF.Max(TextureWidth/2f, TextureHeight/2f));
            mCrossHair = new CrossHair(0.4f, position);

            switch (mStartype)
            {
                case StarType.B:
                    {
                        TextureId = "sunTypeB";
                        break;
                    }
                case StarType.F:
                    {
                        TextureId = "sunTypeF";
                        break;
                    }
                case StarType.G:
                    {
                        TextureId = "sunTypeG";
                        break;
                    }
                case StarType.K:
                    {
                        TextureId = "sunTypeK";
                        break;
                    }
                case StarType.M: 
                    {
                        TextureId = "sunTypeM";
                        break;
                    }
            }

            int amount = Globals.mRandom.Next(1, 5);
            for (int i = 0; i < amount; i++)
            {
                mPlanetList.Add(new Planet(350 + 300 * i));
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            this.ManageHover(inputState, Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2()), Clicked);
            mCrossHair.Update(gameTime, inputState);
            if (Globals.mCamera2d.mZoom >= Globals.mCamera2d.mMimZoom - 0.5
                && HoverBox.Contains(Globals.mCamera2d.mPosition))
            {
                mSelectable = true;
                return;
            }
            if (mSelectable) { mSelectable = false; }
        }

        public override void Draw()
        {
            mCrossHair.DrawCrossHair1();
            TextureManager.GetInstance().Draw(TextureId, Position - Offset, TextureWidth, TextureHeight);
            if (Hover && mSelectable)
            {
                mCrossHair.DrawCrossHair2();
            }
        }

        public void Clicked()
        {
            Globals.mCamera2d.mTargetPosition = Position;
            Globals.mCamera2d.SetZoom(Globals.mCamera2d.mMimZoom);
            if (mSelectable)
            {
                Globals.mLayerManager.AddLayer(new PlanetSystemLayer(this));
                Globals.mCamera2d.mZoom = Globals.mCamera2d.mMimZoom;
            }
        }
    }
}
