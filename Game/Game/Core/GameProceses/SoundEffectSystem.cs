// SoundEffectSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.Visuals.Rendering;
using System;

namespace StellarLiberation.Game.Core.GameProceses
{
    public static class SoundEffectSystem
    {
        private const int ListenDistance = 30000;

        public static void PlaySound(string soundId, Camera2D camera2D, Vector2 position, bool allowInterrupt = false)
        {
            var distance = Vector2.Distance(position, camera2D.Position);
            var volume = (1 + MathF.Cos(MathHelper.Clamp(distance, 0, ListenDistance) * MathF.PI / ListenDistance)) / 2;
            volume *= MathHelper.Clamp(camera2D.Zoom * 2f, 0, 1);
            if (volume <= 0) return;

            var pan = (position - camera2D.Position).X;
            if (pan > -500 && pan < 500) { pan = 0; }

            SoundEffectManager.Instance.PlaySound(soundId, allowInterrupt, volume, pan);
        }
    }
}
