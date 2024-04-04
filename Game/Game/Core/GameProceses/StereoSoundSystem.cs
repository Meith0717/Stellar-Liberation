// StereoSoundSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses
{
    public readonly struct StereoSound(Vector2 SoundPosition, string SoundId)
    {
        public readonly Vector2 SoundPosition = SoundPosition;
        public readonly string SoundId = SoundId;
    }

    public class StereoSoundSystem
    {
        private const int ListenDistance = 30000;

        public void Update(Vector2 camera2DPosition, float camera2DZoom, Queue<StereoSound> soundQueue)
        {
            while (soundQueue.Count > 0)
            {
                var stereoSound = soundQueue.Dequeue();
                PlaySound(stereoSound, camera2DPosition, camera2DZoom);
            }
        }

        private static void PlaySound(StereoSound stereoSound, Vector2 camera2DPosition, float camera2DZoom)
        {
            var distance = Vector2.Distance(stereoSound.SoundPosition, camera2DPosition);
            var volume = (1 + MathF.Cos(MathHelper.Clamp(distance, 0, ListenDistance) * MathF.PI / ListenDistance)) / 2;
            volume *= MathHelper.Clamp(camera2DZoom * 2f, 0, 1);
            if (volume <= 0) return;

            var pan = (stereoSound.SoundPosition - camera2DPosition).X;
            if (pan > -500 && pan < 500) { pan = 0; }

            SoundEffectManager.Instance.PlaySound(stereoSound.SoundId, false, volume, pan);
        }
    }
}
