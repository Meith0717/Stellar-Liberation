// SpaceShipInteractor.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses
{
    internal class SpaceShipInteractor
    {
        private readonly HashSet<Spaceship> mSelectedSpaceShips = new();
        private SelectionBox mSelectionBox;

        public void Update(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            CheckForMouseSelection(inputState, spatialHashing, worldMousePosition);
            CheckForSelectionBox(inputState, spatialHashing, worldMousePosition);

            if (mSelectedSpaceShips.Count <= 0) return;
            if (!inputState.HasAction(ActionType.LeftClick)) return;
            foreach (var spaceship in mSelectedSpaceShips)
            {
                spaceship.SublightDrive.MoveToTarget(worldMousePosition);
                spaceship.SublightDrive.SetVelocity(1);
            }
            mSelectedSpaceShips.Clear();
        }

        #region Selecion Logik

        private void CheckForMouseSelection(InputState inputState, SpatialHashing spatialHashing, Vector2 worldMousePosition)
        {
            if (!inputState.Actions.Contains(ActionType.LeftClick)) return;
            var shipsByMouse = spatialHashing.GetObjectsInRadius<Spaceship>(worldMousePosition, 5000);
            if (shipsByMouse.Count <= 0) return;
            inputState.Actions.Remove(ActionType.LeftClick);
            var nearestSpaceShip = shipsByMouse.First();
            if (!nearestSpaceShip.BoundedBox.Contains(worldMousePosition))
                return;
            if (mSelectedSpaceShips.Remove(nearestSpaceShip)) return;
            mSelectedSpaceShips.Add(nearestSpaceShip);
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
            foreach (var spaceship in mSelectedSpaceShips)
                TextureManager.Instance.DrawCircle(spaceship.Position, spaceship.BoundedBox.Radius, Color.Purple, 10, 1);

            if (mSelectionBox is null) return;
            TextureManager.Instance.SpriteBatch.FillRectangle(mSelectionBox.ToRectangleF(), Color.Gray * 0.2f, 1);
            TextureManager.Instance.DrawAdaptiveRectangleF(mSelectionBox.ToRectangleF(), Color.Gray, 2, 1, camera2D.Zoom);
        }
    }
}
