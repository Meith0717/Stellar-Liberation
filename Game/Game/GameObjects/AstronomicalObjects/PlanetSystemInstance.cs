// PlanetSystemInstance.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    public class PlanetSystemInstance
    {
        public readonly Danger Danger;
        public CircleF SystemBounding;
        public readonly GameObjectManager GameObjects;
        private readonly GameObjectManager AstronomicalObjects;

        public PlanetSystemInstance(GameObjectManager gameObjectManager, GameObjectManager astronomicalObjs)
        {
            GameObjects = gameObjectManager;
            AstronomicalObjects = astronomicalObjs;
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            GameObjects.Update(gameTime, inputState, scene);
            AstronomicalObjects.Update(gameTime, inputState, scene);
        }
    }
}
