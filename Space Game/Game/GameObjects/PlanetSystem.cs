using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] public Star Star;
        [JsonProperty] public List<Planet> Planets;
        [JsonProperty] private CircleF BoundBox;

        public PlanetSystem(Vector2 position) : base()
        {
            Star = new(position);
            Planets = new();
            int radiusLimit = (int)(Star.Width / 2 * Star.TextureScale);
            int orbitNr;
            for (orbitNr = 1; orbitNr <= MyUtility.Random.Next(2, 6); orbitNr++)
            {
                Planets.Add(new Planet(orbitNr, position, Star.mLightColor, radiusLimit));    
            }
            BoundBox = new CircleF(position, 5000);
        }

        public void Update(GameTime time, InputState inputState, GameEngine engine)
        {
            if (engine.FrustumCuller.CircleOnWorldView(Star.BoundedBox))
            {                
                engine.UpdateGameObject(time, inputState, Star);
                engine.UpdateGameObjects(time, inputState, Planets);
            }
        }

        public void Draw(TextureManager textureManager, GameEngine engine)
        {
            engine.DrawGameObject(textureManager, Star);
            engine.DrawGameObjects(textureManager, Planets);
        }
    }
}
