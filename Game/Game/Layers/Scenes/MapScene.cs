// MapScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class MapScene : Scene
    {
        private readonly UiFrame mBackgroundLayer;

        private readonly List<PlanetSystem> mPlanetSystems;
        private readonly PlanetSystem mCurrentSystem;

        public MapScene(GameLayer gameLayer, List<PlanetSystem> planetSystems, PlanetSystem currentSystem)
            : base(gameLayer, 100)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };

            gameLayer.HudLayer.Hide = true;
            mCurrentSystem = currentSystem;
            Camera2D.Position = mCurrentSystem.Position;
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems)
            {
                SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            }
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.1f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(inputState, Camera2D);

            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, 1);
            inputState.DoAction(ActionType.ToggleHyperMap, () => { GameLayer.HudLayer.Hide = false; GameLayer.PopScene(); });
            foreach (var system in mPlanetSystems)
            {
                system.Update(gameTime, inputState, this);
            }
        }

        public override void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            base.DrawOnScreenView(sceneManagerLayer, spriteBatch);
            mBackgroundLayer.Draw();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
