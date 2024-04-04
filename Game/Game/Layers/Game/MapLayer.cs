// MapLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapLayer : GameLayer
    {
        public MapLayer(GameState gameState, Vector2 camera2DPosition, Game1 game1)
            : base(gameState, new(MapFactory.MapScale * 3), game1)
        {
            AddUiElement(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });
            Camera2D.Position = camera2DPosition;
            foreach (var planetSystem in GameState.PlanetSystems)
                SpatialHashing.InsertObject(planetSystem, (int)planetSystem.Position.X, (int)planetSystem.Position.Y);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.01f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);

            inputState.DoAction(ActionType.ToggleHyperMap, GameState.PopLayer);

            foreach (var planetSystem in GameState.PlanetSystems)
                planetSystem.Update(gameTime, null, null);

            var objByMouse = SpatialHashing.GetObjectsInRadius<Planetsystem>(WorldMousePosition, 200);
            foreach (var system in objByMouse)
            base.Update(gameTime, inputState);
        }

        private void LeftPressAction(PlanetsystemState planetsystemState)
        {
            GameState.PopLayer();
            GameState.PopLayer();
            GameState.AddLayer(new PlanetsystemLayer(GameState, planetsystemState, Game1));
        }

        public override void ApplyResolution() { base.ApplyResolution(); }
    }
}
