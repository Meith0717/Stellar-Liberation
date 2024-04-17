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
        public readonly List<Battleship> SelectedBattleship = new();
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, PlanetsystemState planetsystemState, Vector2 worldMousePosition, Layer hudLayer)
        {
            var spatialHashing = planetsystemState.SpatialHashing;

            // Select SpaceShips by Selection Box
            // TryCatchInSelectionbox(inputState, spatialHashing, worldMousePosition);

            var leftClicked = inputState.HasAction(ActionType.LeftClickReleased);
            var rightClicked = inputState.HasAction(ActionType.RightClickReleased);

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
                        SelectOrUnselect(battleship, hudLayer);
                        break;
                    case Flagship:
                        var flagship = (Flagship)HoveredGameObject;
                        if (SelectedBattleship.Count > 0)
                        {
                            for (int i = 0; i < flagship.Hangar.FreeSpace; i++)
                            {
                                if (i > SelectedBattleship.Count) break;
                                var obj = SelectedBattleship[i];
                                if (obj.Flagship is not null) continue;
                                obj.Flagship = flagship;
                            }
                        }
                        SelectOrUnselect(flagship, hudLayer);
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
                        //hudLayer.AddUiElement(new StarInfoPopup(star));
                        break;
                    case null:
                        SelectedBattleship.Clear();
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
                    case Star:
                        var star = (Star)HoveredGameObject;
                        break;
                    case null:
                        break;
                }
            }

        }

        #region Space Ships Interactions

        private void SelectOrUnselect(Spacecraft spaceship, Layer hudLayer)
        {
            if (spaceship.Fraction == Fractions.Enemys) return;
            switch (spaceship)
            {
                case Flagship:
                    if (SelectedFlagships.Remove((Flagship)spaceship))
                    {
                        hudLayer.ClearUiElements();
                        return;
                    }
                    SelectedFlagships.Clear();
                    hudLayer.ClearUiElements();
                    hudLayer.AddUiElement(new FlagshipPopup((Flagship)spaceship, hudLayer.ClearUiElements));
                    SelectedFlagships.Add((Flagship)spaceship);
                    break;
                case Battleship:
                    if (SelectedBattleship.Remove((Battleship)spaceship)) return;
                    SelectedBattleship.Clear();
                    SelectedBattleship.Add((Battleship)spaceship);
                    break;
            }
        }

        private bool TryCatchInSelectionbox(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            if (!inputState.HasAction(ActionType.LeftClickHold))
            {
                if (mSelectionBox is null) return false;
                var selectionbox = mSelectionBox;
                mSelectionBox = null;
                SelectedBattleship.Clear();
                var objInBox = spatialHashing.GetObjectsInRectangle<Spacecraft>(selectionbox.ToRectangleF());
                foreach (var obj in objInBox)
                {
                    if (obj.Fraction == Fractions.Enemys 
                        || obj is not Battleship) 
                        continue;
                    SelectedBattleship.Add((Battleship)obj);
                }
                return true;
            }
            mSelectionBox ??= new(worldMousePosition);
            mSelectionBox.Update(worldMousePosition);
            return false;
        }

        public void MoveSpaceShipsToPosition(Vector2 position)
        {
            foreach (var obj in SelectedFlagships)
                obj.ImpulseDrive.MoveToTarget(position);
        }

        public void MoveSpaceShipsToPlanet(Planet planet, PlanetsystemState planetsystemState)
        {
            foreach (var obj in SelectedFlagships)
            {
                obj.ImpulseDrive.MoveToTarget(planet.GetPositionInOrbit(obj.Position));
                if (planetsystemState.GameObjects.Contains(obj)) continue;
                obj.HyperDrive.SetTarget(planetsystemState);
            }
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

