// MusicManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{

    public class MusicManager
    {
        private static MusicManager mInstance;
        public static MusicManager Instance { get { return mInstance ??= new(); } }
        private readonly Dictionary<string, SoundEffectInstance> mMusicInstances = new();
        public float OverallVolume = 1.0f;

        public void LoadRegistries(ContentManager content, List<Registry> registries)
        {
            foreach (Registry reg in registries)
            {
                var soundEffect = content.Load<SoundEffect>(reg.FilePath);
                mMusicInstances[reg.Name] = soundEffect.CreateInstance();
            };
        }

        public void PlayMusic(string soundId)
        {
            if (!mMusicInstances.ContainsKey(soundId)) throw new Exception("SoundId not found!");

            SoundEffectInstance instance = mMusicInstances[soundId];
            instance.Volume = OverallVolume;
            instance.IsLooped = true;
            instance.Stop();
            instance.Play();
        }

        public void StopAllMusics()
        {
            foreach (var instance in mMusicInstances.Values) instance.Stop();
        }


        internal void ChangeOverallVolume(float sliderValue)
        {
            foreach (var instance in mMusicInstances.Values)
            {
                if (sliderValue >= 0 && sliderValue <= 1) instance.Volume = sliderValue;
            }
            OverallVolume = sliderValue;
        }
    }
}

