using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int textureHeight = 512;
        const int textureWidth = 512;
        const int scale = 1;

        [JsonProperty] public List<Planet> mPlanetList = new();
        [JsonProperty] public StarType mStartype;
        private CrossHair mCrossHair;
        private bool mShowSystem;

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
            HoverBox = new CircleF(Position, MathF.Max(TextureWidth/2.5f, TextureHeight/2.5f));
            mCrossHair = new CrossHair(0.3f, 0.4f, position, Color.SkyBlue);

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
                mPlanetList.Add(new Planet(250 + 160 * i, position));
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            this.ManageHover(inputState, Clicked, DoubleClicked);
            mCrossHair.Update(Position, Hover);
            foreach (Planet planet in mPlanetList)
            {
                planet.Update(gameTime, inputState);
            }
            if (Globals.mCamera2d.mZoom >= Globals.mCamera2d.mMimZoom - 0.5
                && Vector2.Distance(Position, Globals.mCamera2d.mPosition) < 800)
            {
                ShowPlanets();
                return;
            }
            HidePlanets();
        }

        public override void Draw()
        {
            mCrossHair.Draw();
            TextureManager.GetInstance().Draw(TextureId, Position - Offset, TextureWidth, TextureHeight);
            if (!mShowSystem) { return; }
            foreach (Planet planet in mPlanetList)
            {
                planet.Draw();
            }

        }

        private void Clicked()
        {
            Globals.mCamera2d.mTargetPosition = Position;
        }

        private void DoubleClicked()
        {
            Globals.mCamera2d.SetZoom(Globals.mCamera2d.mMimZoom);
        }

        private void ShowPlanets()
        {
            if (!mShowSystem) { mShowSystem = true; }
            foreach(Planet planet in mPlanetList)
            {
                if (planet.mAlpha >= 255) 
                { 
                    return ; 
                }
                planet.mAlpha += 5;
                Debug.WriteLine(planet.mAlpha);
            }
        }

        private void HidePlanets()
        {
            if (!mShowSystem) { return; }
            foreach (Planet planet in mPlanetList)
            {
                if (planet.mAlpha <= 0)
                {
                    mShowSystem = false;
                    return;
                }
                planet.mAlpha -= 5;
                Debug.WriteLine(planet.mAlpha);
            }
        }
    }
}
