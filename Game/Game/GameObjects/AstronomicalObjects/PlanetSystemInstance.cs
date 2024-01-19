// PlanetSystemInstance.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    public class PlanetSystemInstance
    {
        public readonly Danger Danger;
        public CircleF SystemBounding;
        public readonly GameObjectManager GameObjects;
        private readonly GameObjectManager AstronomicalObjects;
        private readonly Star mStar;

        public PlanetSystemInstance(GameObjectManager gameObjectManager, GameObjectManager astronomicalObjs, Star star)
        {
            GameObjects = gameObjectManager;
            AstronomicalObjects = astronomicalObjs;
            mStar = star;
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            inputState.DoAction(ActionType.LeftClick, () => System.Diagnostics.Debug.WriteLine(PlanetGenerator.GetTemperature(mStar.Temperature, (int)Vector2.Distance(scene.WorldMousePosition, mStar.Position)) - 273));

            GameObjects.Update(gameTime, inputState, scene);
            AstronomicalObjects.Update(gameTime, inputState, scene);
        }
    }
}
