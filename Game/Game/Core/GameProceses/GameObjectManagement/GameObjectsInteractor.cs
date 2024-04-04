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
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Popups;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses
{
    public class GameObjectsInteractor
    {
        public readonly static int LookupRange = 1000;
        public GameObject2D HoveredGameObject { get; private set; }
        public HashSet<Spaceship> SelectedSpaceships => mSelectedSpaceShips;
        public readonly HashSet<Spaceship> mSelectedSpaceShips = new();
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, PlanetsystemState planetsystemState, Vector2 worldMousePosition, Layer hudLayer)
        {
            var spatialHashing = planetsystemState.SpatialHashing;
            // Select SpaceShips by Selection Box
            CheckForSelectionBox(inputState, spatialHashing, worldMousePosition);

            // Check for Hovered Object
            HoveredGameObject = null;
            var objectsByMouse = spatialHashing.GetObjectsInRadius<GameObject2D>(worldMousePosition, 5000);
            if (objectsByMouse.Count > 0 && objectsByMouse.First().BoundedBox.Contains(worldMousePosition))
                HoveredGameObject = objectsByMouse.First();

            // Interact
            if (!inputState.HasAction(ActionType.LeftClick)) return;
            switch (HoveredGameObject)
            {
                case Spaceship:
                    var spaceship = (Spaceship)HoveredGameObject;
                    SelectOrUnselect(spaceship);
                    break;
                case Planet:
                    var planet = (Planet)HoveredGameObject;
                    if (mSelectedSpaceShips.Count > 0)
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
                    if (SelectedSpaceships.Count > 0)
                        MoveSpaceShipsToPosition(worldMousePosition, planetsystemState);
                    break;
            }
        }

        #region Space Ships Interactions

        private void SelectOrUnselect(Spaceship spaceship)
        {
            if (spaceship.Fraction == Fractions.Enemys) return;
            if (mSelectedSpaceShips.Remove(spaceship)) return;
            mSelectedSpaceShips.Add(spaceship);
        }

        private void CheckForSelectionBox(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            if (!inputState.HasAction(ActionType.LeftClickHold))
            {
                if (mSelectionBox is null) return;
                var selectionBox = mSelectionBox;
                mSelectionBox = null;
                if (selectionBox.Length < 1000) return;
                mSelectedSpaceShips.Clear();
                var objInBox = spatialHashing.GetObjectsInRectangle<Spaceship>(selectionBox.ToRectangleF());
                foreach (var obj in objInBox)
                {
                    if (obj.Fraction == Fractions.Enemys) continue;
                    mSelectedSpaceShips.Add(obj);
                }
                return;
            }
            mSelectionBox ??= new(worldMousePosition);
            mSelectionBox.Update(worldMousePosition);
        }

        public void MoveSpaceShipsToPosition(Vector2 position, PlanetsystemState planetsystemState)
        {
            foreach (var obj in mSelectedSpaceShips)
                obj.ImpulseDrive.MoveToTarget(position);
            SelectedSpaceships.Clear();
        }

        public void MoveSpaceShipsToPlanet(Planet planet, PlanetsystemState planetsystemState)
        {
            foreach (var obj in mSelectedSpaceShips)
            {
                obj.ImpulseDrive.MoveToTarget(planet.GetPositionInOrbit(obj.Position));
                if (planetsystemState.GameObjects.Contains(obj)) continue;
                obj.HyperDrive.SetTarget(planetsystemState);
            }
            SelectedSpaceships.Clear();
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

