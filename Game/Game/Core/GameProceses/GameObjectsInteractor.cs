// SpaceShipInteractor.cs 
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
using StellarLiberation.Game.Layers.GameLayers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses
{
    internal class GameObjectsInteractor
    {
        public GameObject2D HoveredGameObject { get; private set; }
        public readonly HashSet<Spaceship> mSelectedSpaceShips = new();
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, GameLayer gameLayer)
        {
            var spatialHashing = gameLayer.SpatialHashing;
            var worldMousePosition = gameLayer.WorldMousePosition;

            CheckForSelectionBox(inputState, spatialHashing, worldMousePosition);
            HoveredGameObject = null;
            var objectsByMouse = spatialHashing.GetObjectsInRadius<GameObject2D>(worldMousePosition, 5000);
            if (objectsByMouse.Count <= 0) return;
            var nearestObjectsByMouse = objectsByMouse.First();
            if (!nearestObjectsByMouse.BoundedBox.Contains(worldMousePosition)) return;
            HoveredGameObject = nearestObjectsByMouse;
            if (!(inputState.Actions.Contains(ActionType.LeftClick)
                || inputState.Actions.Contains(ActionType.RightClick))) 
                return;

            switch (nearestObjectsByMouse)
            {
                case Spaceship:
                    var spaceship = (Spaceship)nearestObjectsByMouse;
                    Debug.WriteLine($"Spaceship clicked: {spaceship}");
                    if (spaceship.Fraction == Fractions.Enemys) break;
                    SelectOrUnselect(spaceship);
                    return;
                case Planet:
                    var planet = (Planet)nearestObjectsByMouse;
                    Debug.WriteLine($"Planet clicked: {planet}");
                    switch (mSelectedSpaceShips.Count)
                    {
                        case <= 0:
                            break;
                        case > 0:
                            foreach (var obj in mSelectedSpaceShips)
                            {
                                obj.SublightDrive.SetVelocity(1f);
                                obj.SublightDrive.MoveToTarget(planet.GetPositionInOrbit(obj.Position));
                            }
                            mSelectedSpaceShips.Clear();
                            break;
                    }
                    return;
            }
        }

        #region Space Ships Interactions
        public HashSet<Spaceship> SelectedSpaceships => mSelectedSpaceShips;

        private void SelectOrUnselect(Spaceship spaceship)
        {
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
                var objInBox = spatialHashing.GetObjectsInRectangle<Spaceship>(selectionBox.ToRectangleF());
                foreach (var obj in objInBox)
                    mSelectedSpaceShips.Add(obj);
                return;
            }
            mSelectionBox ??= new(worldMousePosition);
            mSelectionBox.Update(worldMousePosition);
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

