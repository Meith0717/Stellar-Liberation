// GameObjectsInteractor.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Popups;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses
{
    public class GameObjectsInteractor
    {
        public readonly static int LookupRange = 1000;
        public GameObject HoveredGameObject { get; private set; }
        public readonly HashSet<Flagship> SelectedFlagships = new();
        public readonly HashSet<Battleship> SelectedBattleships = new();
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, PlanetsystemState planetsystemState, Vector2 worldMousePosition, Layer hudLayer)
        {
            var spatialHashing = planetsystemState.SpatialHashing;

            // Select SpaceShips by Selection Box
            CheckForSelectionBox(inputState, spatialHashing, worldMousePosition);

            var leftClicked = inputState.HasAction(ActionType.LeftClick);
            var rightClicked = inputState.HasAction(ActionType.RightClick);

            // Check for Hovered Object
            HoveredGameObject = null;
            var nearestObjectsByMouse = spatialHashing.GetObjectsInRadius<GameObject>(worldMousePosition, 5000).FirstOrDefault(defaultValue: null);
            if (nearestObjectsByMouse != null)
            {
                if (nearestObjectsByMouse.BoundedBox.Contains(worldMousePosition))
                    HoveredGameObject = nearestObjectsByMouse;
            }

            // Left Click Interactions
            if (leftClicked)
            {
                switch (HoveredGameObject)
                {
                    case Battleship:
                        var battleship = (Battleship)HoveredGameObject;
                        SelectOrUnselect(battleship);
                        break;
                    case Flagship:
                        var spaceship = (Flagship)HoveredGameObject;
                        SelectOrUnselect(spaceship);
                        break;
                    case Planet:
                        var planet = (Planet)HoveredGameObject;
                        if (SelectedFlagships.Count > 0)
                        {
                            MoveSpaceShipsToPlanet(planet, planetsystemState);
                            break;
                        }
                        break;
                    case Star:
                        var star = (Star)HoveredGameObject;
                        hudLayer.ClearUiElements();
                        hudLayer.AddUiElement(new StarInfoPopup(star));
                        break;
                    case null:
                        SelectedBattleships.Clear();
                        if (SelectedFlagships.Count > 0)
                            MoveSpaceShipsToPosition(worldMousePosition, planetsystemState);
                        break;
                }
            }

            // Right Click Interactions
            if (rightClicked)
            {
                switch (HoveredGameObject)
                {
                    case Battleship:
                        break;
                    case Flagship:
                        var flagship = (Flagship)HoveredGameObject;
                        hudLayer.ClearUiElements();
                        hudLayer.AddUiElement(new FlagshipPopup(flagship));
                        break;
                    case Planet:
                        var planet = (Planet)HoveredGameObject;
                        break;
                    case Star:
                        var star = (Star)HoveredGameObject;
                        break;
                    case null:
                        break;
                }
            }

        }

        #region Space Ships Interactions

        private void SelectOrUnselect(Spacecraft spaceship)
        {
            if (spaceship.Fraction == Fractions.Enemys) return;
            switch (spaceship)
            {
                case Flagship:
                    if (SelectedFlagships.Remove((Flagship)spaceship)) return;
                    SelectedFlagships.Add((Flagship)spaceship);
                    break;
                case Battleship:
                    if (SelectedBattleships.Remove((Battleship)spaceship)) return;
                    SelectedBattleships.Add((Battleship)spaceship);
                    break;
            }
        }

        private void CheckForSelectionBox(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            if (!inputState.HasAction(ActionType.LeftClickHold))
            {
                if (mSelectionBox is null) return;
                var selectionBox = mSelectionBox;
                mSelectionBox = null;
                if (selectionBox.Length < 1000) return;
                SelectedFlagships.Clear();
                SelectedBattleships.Clear();
                var objInBox = spatialHashing.GetObjectsInRectangle<Spacecraft>(selectionBox.ToRectangleF());
                foreach (var obj in objInBox.OfType<Flagship>())
                {
                    if (obj.Fraction == Fractions.Enemys) continue;
                    SelectedFlagships.Add(obj);
                }
                foreach (var obj in objInBox.OfType<Battleship>())
                {
                    if (obj.Fraction == Fractions.Enemys) continue;
                    SelectedBattleships.Add(obj);
                }
                return;
            }
            mSelectionBox ??= new(worldMousePosition);
            mSelectionBox.Update(worldMousePosition);
        }

        public void MoveSpaceShipsToPosition(Vector2 position, PlanetsystemState planetsystemState)
        {
            foreach (var obj in SelectedFlagships)
                obj.ImpulseDrive.MoveToTarget(position);
            SelectedFlagships.Clear();
        }

        public void MoveSpaceShipsToPlanet(Planet planet, PlanetsystemState planetsystemState)
        {
            foreach (var obj in SelectedFlagships)
            {
                obj.ImpulseDrive.MoveToTarget(planet.GetPositionInOrbit(obj.Position));
                if (planetsystemState.GameObjects.Contains(obj)) continue;
                obj.HyperDrive.SetTarget(planetsystemState);
            }
            SelectedFlagships.Clear();
        }

        #endregion

        public void Draw(Camera2D camera2D)
        {
            if (mSelectionBox is null) return;
            TextureManager.Instance.SpriteBatch.FillRectangle(mSelectionBox.ToRectangleF(), Color.Gray * 0.2f, 1);
            TextureManager.Instance.DrawAdaptiveRectangleF(mSelectionBox.ToRectangleF(), Color.Gray, 2, 1, camera2D.Zoom);
        }
    }
}

