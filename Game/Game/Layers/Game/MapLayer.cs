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
        private readonly MapHud mMapHud;

        public MapLayer(GameState gameState, MapState mapState, Vector2 camera2DPosition, Game1 game1)
            : base(gameState, mapState.SpatialHasing, game1)
        {
            mMapHud = new(game1, gameState);
            AddUiElement(new UiSprite("gameBackground") { Anchor = Anchor.Center, FillScale = FillScale.FillIn });
            Camera2D.Position = camera2DPosition;
            mMapState = mapState;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.01f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);

            mMapHud.Update(gameTime, inputState);
            inputState.DoAction(ActionType.ToggleHyperMap, GameState.PopLayer);
            ManageInput(inputState);
            base.Update(gameTime, inputState);
        }

        private void ManageInput(InputState inputState)
        {
            if (!inputState.HasAction(ActionType.LeftReleased)) return;
            var objsByMouse = SpatialHashing.GetObjectsInRadius<Planetsystem>(WorldMousePosition, 200);
            if (objsByMouse.Count <= 0) return;
            var planetSystem = objsByMouse.First();
            if (!planetSystem.BoundedBox.Contains(WorldMousePosition)) return;
            GameState.PopLayer();
            GameState.PopLayer();
            GameState.AddLayer(new PlanetsystemLayer(GameState, planetSystem.PlanetsystemState, Game1));
        }

        public override void ApplyResolution()
        {
            mMapHud.ApplyResolution();
            base.ApplyResolution();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            mMapHud.Draw(spriteBatch);

            spriteBatch.Begin(transformMatrix: ViewTransformationMatrix);
            mMapState.Draw(Camera2D.Zoom, GameState);
            spriteBatch.End();

        }
    }
}
