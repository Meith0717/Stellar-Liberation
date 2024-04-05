// DebugSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.Debugging
{
    public class DebugSystem
    {
        public float DrawnObjectCount;
        public float UpdateObjectCount;

        private List<DebugAction> debugActions;

        public bool IsActive => IsDebug;
        private bool IsDebug;
        private bool DrawBuckets;
        private bool ShowObjectsInBucket;
        private bool ShowHitBoxes;
        private bool ShowSensorRadius;
        private bool ShowPaths;
        private bool ShowAi;

        public DebugSystem(bool isActive = false)
        {
            IsDebug = isActive;
            debugActions = new()
            {
                new("Spatial Hashing Grid", ActionType.F1, () => DrawBuckets = !DrawBuckets),
                new("Objects in Bucket", ActionType.F2, () => ShowObjectsInBucket = !ShowObjectsInBucket),
                new("Hit Box", ActionType.F3, () => ShowHitBoxes = !ShowHitBoxes),
                new("Sensor Radius", ActionType.F4, () => ShowSensorRadius = !ShowSensorRadius),
                new("Moving Direction", ActionType.F5, () => ShowPaths = !ShowPaths),
                new("AI", ActionType.F6, () => ShowAi = !ShowAi),
                new("N.A", ActionType.F7, null),
                new("N.A", ActionType.F8, null),
                new("N.A", ActionType.F9, null),
                new("N.A", ActionType.F10, null),
            };
        }

        public void Update(InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleDebug, () => IsDebug = !IsDebug);
            DrawnObjectCount = 0;
            UpdateObjectCount = 0;
            if (!IsDebug)
            {
                DrawBuckets = ShowObjectsInBucket = ShowHitBoxes = ShowSensorRadius = ShowPaths = false;
                return;
            }
            foreach (var action in debugActions) action.Update(inputState);
        }

        public void DrawOnScene(GameLayer scene)
        {
            if (!IsDebug) return;
            if (DrawBuckets) DebugSpatialHashing.Buckets(scene, scene.WorldMousePosition);
            if (ShowObjectsInBucket) DebugSpatialHashing.ObjectsInBucket(scene);
        }

        public void ShowInfo(Vector2 position)
        {
            if (!IsDebug) return;
            foreach (var action in debugActions)
            {
                action.DrawInfo(position);
                position.Y += 15;
            }

        }

        public void DrawHitbox(CircleF box, GameLayer scene)
        {
            if (!ShowHitBoxes) { return; }
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.Red, 2, (int)TextureManager.MaxLayerDepth,
                scene.Camera2D.Zoom);
        }

        public void DrawMovingDir(Vector2? target, Flagship spaceShip, GameLayer scene)
        {
            if (target is null | !ShowPaths) return;
            TextureManager.Instance.DrawAdaptiveLine(spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, spaceShip.MovingDirection, spaceShip.Velocity * 500), Color.LightBlue, 2, spaceShip.TextureDepth - 1, scene.Camera2D.Zoom);
        }

        public void DrawSensorRadius(Vector2 center, float radius, GameLayer scene)
        {
            if (!ShowSensorRadius) { return; }
            CircleF box = new(center, radius);
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.LightBlue, 2, (int)TextureManager.MaxLayerDepth,
                scene.Camera2D.Zoom);
        }

        public void DrawAiDebug(CircleF circle, string message, float camZoom)
        {
            if (!ShowAi) return;
            TextureManager.Instance.DrawString(FontRegistries.subTitleFont, circle.ToRectangleF().TopRight, message, 0.3f / camZoom, Color.White);
        }
    }
}
