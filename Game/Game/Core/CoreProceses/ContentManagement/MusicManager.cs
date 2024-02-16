// MusicManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{

    public class MusicManager
    {
        private static MusicManager mInstance;
        public static MusicManager Instance { get { return mInstance ??= new(); } }

        private readonly List<SoundEffect> mMusics = new();
        private SoundEffectInstance mMusicInstance;
        private bool mLoadet;
        public float OverallVolume;

        public void LoadRegistries(ContentManager content, List<Registry> registries)
        {
            foreach (Registry reg in registries)
            {
                var soundEffect = content.Load<SoundEffect>(reg.FilePath);
                mMusics.Add(soundEffect);
            };
            mLoadet = true;
        }

        public void SetVolume(float master, float volume) => OverallVolume = MathHelper.Clamp(volume, 0f, 1f) * MathHelper.Clamp(master, 0f, 1f);

        public void Pause() => mMusicInstance.Pause();
        public void Resume() => mMusicInstance.Resume();

        public void Update()
        {
            if (!mLoadet) return;
            if (mMusicInstance is null)
            {
                var music = ExtendetRandom.GetRandomElement(mMusics);
                System.Diagnostics.Debug.WriteLine(music.Name);
                mMusicInstance = music.CreateInstance();
                mMusicInstance.Volume = OverallVolume;
                mMusicInstance.Play();
                return;
            }

            if (mMusicInstance.State == SoundState.Playing) return;
            if (mMusicInstance.State == SoundState.Paused) return;
            mMusicInstance = null;
        }

        public void ChangeOverallVolume(float master, float volume)
        {
            SetVolume(master, volume);
            if (mMusicInstance is null) return;
            mMusicInstance.Volume = OverallVolume;
        }
    }
}

