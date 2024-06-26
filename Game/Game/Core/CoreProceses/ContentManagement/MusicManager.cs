// MusicManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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

        public void LoadContent(ContentManager content, string iD, string path)
        {
            var soundEffect = content.Load<SoundEffect>(path);
            soundEffect.Name = iD;
            mMusics.Add(soundEffect);
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

