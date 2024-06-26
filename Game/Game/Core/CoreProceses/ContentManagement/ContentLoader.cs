// ContentLoader.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{
    public class ContentLoader
    {
        private readonly ContentManager mContent;
        private readonly string[] mMainDirrectories;

        public string ProcessMessage { get; private set; } = "";
        public double Process { get; private set; } = 0;

        internal ContentLoader(ContentManager content)
        {
            mContent = content;
            mMainDirrectories = GetDirectorys(content.RootDirectory);
        }

        private string[] GetFiles(string directoryPath) => Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        private string[] GetDirectorys(string contentPath) => Directory.GetDirectories(contentPath, "*.*", SearchOption.TopDirectoryOnly);

        private void LoadContent(string contentDirectory, Action<ContentManager, string, string> managerLoader)
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, contentDirectory);
            var files = GetFiles(rootDirectory);
            Process = 0;

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var split = filePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
                if (split.Count > 1) split.RemoveAt(0);
                var directory = Path.GetDirectoryName(Path.Combine(split.ToArray()));
                var pathWithoutExtension = Path.Combine(directory, fileName);

                ProcessMessage = $"Loading: {filePath}";
                Process += 1d / files.Length;

                managerLoader.Invoke(mContent, fileName, pathWithoutExtension);
            }
        }

        public void LoadEssenzialContent()
        {
            LoadContent("gui", TextureManager.Instance.LoadTextureContent);
            LoadContent("fonts", TextureManager.Instance.LoadFontContent);
        }

        private void LoadContent()
        {
            LoadContent("music", MusicManager.Instance.LoadContent);
            LoadContent("sfx", SoundEffectManager.Instance.LoadContent);
            LoadContent("textures", TextureManager.Instance.LoadTextureContent);
            ProcessMessage = "Ready";
            Thread.Sleep(2000);
        }

        public void LoadContentAsync(Action onLoadComplete, Action<Exception> onError)
        {
            Thread thread = new(() => {
                try
                {
                    LoadContent();
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }
                onLoadComplete.Invoke();
            });

            thread.Start();
        }
    }
}
