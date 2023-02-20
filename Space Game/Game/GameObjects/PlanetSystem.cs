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
using System.Diagnostics;

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

        [JsonProperty] public StarState mStarState = StarState.Explored;

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
        const int textureHeight = 512;
        const int textureWidth = 512;

        // Other Stuff 
        [JsonProperty] private List<Planet> mPlanetList = new();
        [JsonProperty] private Color mStateColor;
        [JsonProperty] private Color mStarColor;

        // Crosshair Stuff
        private CrossHair mCrossHair;

        // Bool Stuff
        private bool mShowSystem = false;

        // GameObjecr Stuff _____________________________________
        public PlanetSystem(Vector2 position) 
        {
            GetSystemType();
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            Offset = new Vector2(textureHeight, textureWidth) / 2;
            Position = position;
            HoverBox = new CircleF(Position, MathF.Max(TextureWidth/2.5f, TextureHeight/2.5f));
            mCrossHair = new CrossHair(0.3f, 0.4f, position, mStarColor);
            GetPlanets();
            
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            this.ManageHover(inputState, Clicked, DoubleClicked);
            mCrossHair.Update(Position, Hover);
            UpdatePlanets(gameTime, inputState);
            GetSystemState();

            if (IsInZoom(0.1f) && IsInDistance(800))
            {
                ShowPlanets();
                return;
            }
            HidePlanets();
        }

        public override void Draw()
        {
            var stateTexture = TextureManager.GetInstance().GetTexture("systemState");
            TextureManager.GetInstance().GetSpriteBatch().Draw(stateTexture, Position, null,
                mStateColor, 0, new Vector2(512, 512), 3f, SpriteEffects.None, 0);

            TextureManager.GetInstance().Draw(TextureId, Position - Offset, TextureWidth, TextureHeight);
            if (!mShowSystem) { return; }
            foreach (Planet planet in mPlanetList)
            {
                planet.Draw();
            }
        }

        // Input Stuff _____________________________________
        private void Clicked()
        {
            Globals.mCamera2d.mTargetPosition = Position;
        }

        private void DoubleClicked()
        {
            Globals.mCamera2d.SetZoom(Globals.mCamera2d.mMimZoom);
        }

        // Zoom Stuff _____________________________________
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

        // Constructor Stuff _____________________________________
        private void GetSystemType()
        {
            Array starTypes = Enum.GetValues(typeof(StarType));
            StarType starType = (StarType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));
            switch (starType)
            {
                case StarType.B:
                    {
                        TextureId = "sunTypeB";
                        mStarColor = new Color(240, 240, 240);
                        break;
                    }
                case StarType.F:
                    {
                        TextureId = "sunTypeF";
                        mStarColor = new Color(176, 226, 255);
                        break;
                    }
                case StarType.G:
                    {
                        TextureId = "sunTypeG";
                        mStarColor = new Color(255, 226, 0);
                        break;
                    }
                case StarType.K:
                    {
                        TextureId = "sunTypeK";
                        mStarColor = new Color(255, 117, 0);
                        break;
                    }
                case StarType.M:
                    {
                        TextureId = "sunTypeM";
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
                mPlanetList.Add(new Planet(250 + 160 * i, Position));
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
    
        private void GetSystemState()
        {
            switch (mStarState)
            {
                case StarState.Uncovered:
                    {
                        mStateColor = Color.Transparent;
                        break;
                    }
                case StarState.Discovered:
                    {
                        mStateColor = Color.SkyBlue;
                        break;
                    }
                case StarState.Explored:
                    {
                        mStateColor = Color.LightGreen;
                        break;
                    }
            }

        }

        private bool IsInZoom(float zoom)
        {
            return Globals.mCamera2d.mZoom >= zoom;
        }

        private bool IsInDistance(int distance)
        {
            return Vector2.Distance(Position, Globals.mCamera2d.mPosition) < distance;
        }
    }
}
