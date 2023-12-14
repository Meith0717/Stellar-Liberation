// ContentLoader.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using System;
using System.Threading;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{
    public static class ContentLoader
    {
        public static void LoadAsync(ContentManager Content, SpriteBatch spriteBatch, Action onLoadComplete, Action<Exception> onError)
        {
            Thread loadThread = new(() =>
            {
                try
                {
                    TextureManager.Instance.SetSpriteBatch(spriteBatch);
                    TextureManager.Instance.LoadTextureRegistries(Content, Registries.GetRegistryList<TextureRegistries>());
                    TextureManager.Instance.LoadFontRegistries(Content, Registries.GetRegistryList<FontRegistries>());
                    SoundEffectManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<SoundEffectRegistries>());
                    MusicManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<MusicRegistries>());

                    onLoadComplete?.Invoke();
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }
            });

            loadThread.Start();
        }
    }
}
