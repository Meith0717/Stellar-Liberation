// MusicManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Utilitys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{

    public class MusicManager
    {
        private static MusicManager mInstance;
        public static MusicManager Instance { get { return mInstance ??= new(); } }

        private readonly List<SoundEffect> mMusics = new();
        private SoundEffectInstance mMusicInstance;
        private int mMusicIndex;
        public float OverallVolume = 1f;

        public void LoadRegistries(ContentManager content, List<Registry> registries)
        {
            foreach (Registry reg in registries)
            {
                var soundEffect = content.Load<SoundEffect>(reg.FilePath);
                mMusics.Add(soundEffect);
            };
            mMusicInstance = ExtendetRandom.GetRandomElement(mMusics).CreateInstance();
            mMusicInstance.Volume = OverallVolume;
            mMusicInstance.Play();
        }

        public void Pause() => mMusicInstance.Pause();
        public void Resume() => mMusicInstance.Resume();

        public void Update()
        {
            if (mMusicInstance.State == SoundState.Playing) return;
            if (mMusicInstance.State == SoundState.Paused) return;
            mMusicIndex = (mMusicIndex + 1) % mMusics.Count;
            var music = mMusics[mMusicIndex];
            mMusicInstance = music.CreateInstance();
            mMusicInstance.Volume = OverallVolume;
            mMusicInstance.Play();
        }

        public void ChangeOverallVolume(float sliderValue)
        {
            if (sliderValue >= 0 && sliderValue <= 1) mMusicInstance.Volume = sliderValue;
            OverallVolume = sliderValue;
        }
    }
}

