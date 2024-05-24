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
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, PlanetsystemState planetsystemState, Vector2 worldMousePosition, Layer hudLayer)
        {
            var spatialHashing = planetsystemState.SpatialHashing;
            var leftClicked = inputState.HasAction(ActionType.LeftReleased);
            var rightClicked = inputState.HasAction(ActionType.RightReleased);

            // Check for Hovered Object
            HoveredGameObject = null;
            var nearestObjectsByMouse = spatialHashing.GetObjectsInRadius<GameObject>(worldMousePosition, 5000).Where(obj => obj.BoundedBox.Contains(worldMousePosition)).ToList();
            if (nearestObjectsByMouse.Count > 0)
            {
                nearestObjectsByMouse.Sort((obj1, obj2) =>
                {
                    var distance1 = Vector2.DistanceSquared(worldMousePosition, obj1.Position);
                    var distance2 = Vector2.DistanceSquared(worldMousePosition, obj2.Position);
                    return distance1.CompareTo(distance2);
                });
                HoveredGameObject = nearestObjectsByMouse.First();
            }

            // Left Click Interactions
            if (leftClicked)
            {
                switch (HoveredGameObject)
                {
                    case Flagship:
                        var flagship = (Flagship)HoveredGameObject;
                        SelectOrUnselect(inputState, flagship, hudLayer);
                        break;
                    case Planet:
                        var planet = (Planet)HoveredGameObject;
                        if (SelectedFlagships.Count > 0)
                        {
                            MoveSpaceShipsToPlanet(planet, planetsystemState);
                            break;
                        }
                        break;
                    case null:
                        if (SelectedFlagships.Count > 0)
                            MoveSpaceShipsToPosition(worldMousePosition);
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
                        break;
                    case Planet:
                        var planet = (Planet)HoveredGameObject;
                        break;
                    case null:
                        break;
                }
            }
        }

        #region Space Ships Interactions

        private void SelectOrUnselect(InputState inputState, Spacecraft spaceship, Layer hudLayer)
        {
            if (spaceship.Fraction == Fractions.Enemys) return;
            hudLayer.ClearUiElements();
            if (SelectedFlagships.Contains((Flagship)spaceship) 
                && SelectedFlagships.Count <= 1)
            {
                SelectedFlagships.Clear();
                return;
            }
            if (!inputState.HasAction(ActionType.CtrlLeft) || SelectedFlagships.Count < 1)
            {
                SelectedFlagships.Clear();
                hudLayer.AddUiElement(new FlagshipPopup((Flagship)spaceship, hudLayer.ClearUiElements));
            }
            SelectedFlagships.Add((Flagship)spaceship);
        }

        private void CatchInSelectionbox(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            if (inputState.HasAction(ActionType.LeftClickHold))
            {
                mSelectionBox ??= new(worldMousePosition);
                mSelectionBox.Update(worldMousePosition);
                return;
            }

            if (mSelectionBox is null) return;
            var selectionbox = mSelectionBox;
            mSelectionBox = null;
            if (selectionbox.Length < 1000) return;
            var objInBox = spatialHashing.GetObjectsInRectangle<Spacecraft>(selectionbox.ToRectangleF());
            inputState.Actions.Remove(ActionType.LeftReleased);
        }

        public void MoveSpaceShipsToPosition(Vector2 position)
        {
            foreach (var obj in SelectedFlagships)
                obj.ImpulseDrive.MoveToTarget(position);
            if (SelectedFlagships.Count > 1) 
                SelectedFlagships.Clear();
        }

        public void MoveSpaceShipsToPlanet(Planet planet, PlanetsystemState planetsystemState)
        {
            foreach (var obj in SelectedFlagships)
            {
                obj.ImpulseDrive.MoveToTarget(planet.GetPositionInOrbit(obj.Position));
                if (planetsystemState.GameObjects.Contains(obj))
                    continue;
                obj.HyperDrive.SetTarget(planetsystemState);
            }
            if (SelectedFlagships.Count > 1)
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

