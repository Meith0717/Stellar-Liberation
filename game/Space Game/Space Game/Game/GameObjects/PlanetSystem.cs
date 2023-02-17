using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        const int scale = 16;

        [JsonProperty] public List<Planet> mPlanetList = new();
        [JsonProperty] public StarType mStartype;

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
            HoverBox = new CircleF(Position, Math.Max(TextureWidth, TextureHeight));

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
  
            mPlanetList.Add(new Planet(150, 50));
            mPlanetList.Add(new Planet(200, 90));
            mPlanetList.Add(new Planet(250, 180));

        }

        public override void Update(GameTime gameTime, InputState inputState, Camera2d camera2d)
        {
            this.ManageHover(inputState, camera2d.ViewToWorld(inputState.mMousePosition.ToVector2()), Clicked);
        }

        public override void Draw()
        {
            TextureManager.GetInstance().Draw(TextureId, Position - Offset, TextureWidth, TextureHeight);
        }

        public void Clicked()
        {
            Globals.mCamera2d.mTargetPosition = Position;
            Globals.mCamera2d.SetZoom(5);
            Globals.mLayerManager.AddLayer(new PlanetSystemLayer(this));
        }
    }
}
