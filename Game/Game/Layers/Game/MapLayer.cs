// MapLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System.Linq;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapLayer : GameLayer
    {
        private readonly MapState mMapState;
        private readonly MapHudLayer mHud;

        public MapLayer(GameState gameState, MapState mapState, Vector2 camera2DPosition, Game1 game1)
            : base(gameState, mapState.SpatialHasing, game1)
        {
            mHud = new(gameState, this, game1);
            AddUiElement(new UiSprite("gameBackground") { Anchor = Anchor.Center, FillScale = FillScale.FillIn });
            Camera2D.Position = camera2DPosition;
            mMapState = mapState;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mHud.Update(gameTime, inputState);

            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.01f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);

            inputState.DoAction(ActionType.ToggleHyperMap, GameState.CloseMap);
            ManageInput(inputState);
            base.Update(gameTime, inputState);
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            mHud.ApplyResolution();
        }

        private void ManageInput(InputState inputState)
        {
            if (!inputState.HasAction(ActionType.LeftReleased)) return;
            var objsByMouse = SpatialHashing.GetObjectsInRadius<Planetsystem>(WorldMousePosition, 200);
            if (objsByMouse.Count <= 0) return;
            var planetSystem = objsByMouse.First();
            if (!planetSystem.BoundedBox.Contains(WorldMousePosition)) return;
            GameState.OpenPlanetSystem(planetSystem.PlanetsystemState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin(transformMatrix: ViewTransformationMatrix);
            mMapState.Draw(Camera2D.Zoom, GameState);
            spriteBatch.End();
            mHud.Draw(spriteBatch);
        }
    }
}
