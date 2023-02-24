using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Space_Game.Core;
using Space_Game.Core.Animation;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using Space_Game.Core.TextureManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        // State Einumeration
        public enum StarState
        {
            Uncovered,
            Discovered,
            Explored
        }

        // Type Enumeration 
        public enum StarType
        {
            B,
            F,
            G,
            K,
            M
        }

        // Constant
        const int textureHeight = 1000;
        const int textureWidth = 1000;

        // Other Stuff 
        [JsonProperty] private List<Planet> mPlanetList = new();
        [JsonProperty] private Color mStarColor;

        private CrossHair mCrossHair;
        private bool mShowSystem = false;

        // GameObjecr Stuff _____________________________________
        public PlanetSystem(Vector2 position)
        {
            GetSystemType();

            Position = position;
            TextureOffset = new Vector2(textureWidth, textureHeight) / 2;

            TextureSclae = 1;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            TextureDepth = 0;
            TextureRotation = 0;
            TextureColor = Color.White;

            mCrossHair = new CrossHair(0.7f, 1f, position, mStarColor);
            GetPlanets();
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)position.X, (int)position.Y);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            this.ClickOnObject(inputState, 300, Clicked, null);
            mCrossHair.Update(Position, Hover, mStarColor);

            UpdatePlanets(gameTime, inputState);

            if (ViewIsSetTo(0.3f, 1500))
            {
                ShowPlanets();
                return;
            }
            HidePlanets();
        }

        public override void Draw()
        {
            this.DrawGameObject();

            if (!mShowSystem) { mCrossHair.Draw(); return; }
            foreach (Planet planet in mPlanetList)
            {
                planet.Draw();
            }
        }

        // Input Stuff _____________________________________
        private void Clicked()
        {
            Globals.mCamera2d.mTargetPosition = Position;
            Globals.mCamera2d.SetZoom(0.3f);
        }

        // Zoom Stuff _____________________________________
        private void ShowPlanets()
        {
            if (TextureSclae > 0.2)
            {
                TextureSclae -= 0.05f;
            }

            if (!mShowSystem) { mShowSystem = true; }
            foreach (Planet planet in mPlanetList)
            {
                if (planet.mAlpha < 1)
                {
                    planet.mAlpha += 0.05f;
                }
            }
        }

        private void HidePlanets()
        {
            if (TextureSclae < 1)
            {
                TextureSclae += 0.05f;
            }

            if (!mShowSystem) { return; }
            foreach (Planet planet in mPlanetList)
            {
                if (planet.mAlpha <= 0)
                {
                    mShowSystem = false;
                    return;
                }
                planet.mAlpha -= 0.05f;
            }
        }

        // Constructor Stuff _____________________________________
        private void GetSystemType()
        {
            Array starTypes = Enum.GetValues(typeof(StarType));
            StarType starType = (StarType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));
            switch (starType)
            {
                case StarType.B:
                    {
                        NormalTextureId = HoverTectureId = "sunTypeB";
                        mStarColor = new Color(240, 240, 240);
                        break;
                    }
                case StarType.F:
                    {
                        NormalTextureId = HoverTectureId = "sunTypeF";
                        mStarColor = new Color(176, 226, 255);
                        break;
                    }
                case StarType.G:
                    {
                        NormalTextureId = HoverTectureId = "sunTypeG";
                        mStarColor = new Color(255, 226, 0);
                        break;
                    }
                case StarType.K:
                    {
                        NormalTextureId = HoverTectureId = "sunTypeK";
                        mStarColor = new Color(255, 117, 0);
                        break;
                    }
                case StarType.M:
                    {
                        NormalTextureId = HoverTectureId = "sunTypeM";
                        mStarColor = new Color(255, 0, 0);
                        break;
                    }
            }
        }
        private void GetPlanets()
        {
            int amount = Globals.mRandom.Next(1, 5);
            for (int i = 0; i < amount; i++)
            {
                mPlanetList.Add(new Planet(500 + 200 * i, Position));
            }
        }

        // Update Stuff _____________________________________
        private void UpdatePlanets(GameTime gameTime, InputState inputState)
        {
            foreach (Planet planet in mPlanetList)
            {
                planet.Update(gameTime, inputState);
            }
        }

        /*private void GetSystemState()
        {
            switch (mStarState)
            {
                case StarState.Uncovered:
                    {
                        mStateColor = Color.White;
                        break;
                    }
                case StarState.Discovered:
                    {
                        mStateColor = Color.SkyBlue;
                        break;
                    }
                case StarState.Explored:
                    {
                        mStateColor = Color.Green;
                        break;
                    }
            }

        }*/

    }
}
