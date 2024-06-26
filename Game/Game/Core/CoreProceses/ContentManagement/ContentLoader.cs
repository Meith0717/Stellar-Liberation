// ContentLoader.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        internal ContentLoader(ContentManager content)
        {
            mContent = content;
            mMainDirrectories = GetDirectorys(content.RootDirectory);
        }

        private string[] GetFiles(string directoryPath) => Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        private string[] GetDirectorys(string contentPath) => Directory.GetDirectories(contentPath, "*.*", SearchOption.TopDirectoryOnly);

        private string[] GetFilePathsWithoutRoot(string[] files)
        {
            List<string> newFiles = new();
            foreach (var filePath in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var split = filePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
                if (split.Count > 1) split.RemoveAt(0);
                var directory = Path.GetDirectoryName(Path.Combine(split.ToArray()));
                var pathWithoutExtension = Path.Combine(directory, fileName);
                newFiles.Add(pathWithoutExtension);
            }
            return newFiles.ToArray();
        }

        private void LoadContent(string contentDirectory, Action<ContentManager, string, string> managerLoader)
        {
            var directory = Path.Combine(mContent.RootDirectory, contentDirectory);
            var files = GetFiles(directory);
            files = GetFilePathsWithoutRoot(files);

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                ProcessMessage = $"Loading: {file}";
                var fileID = Path.GetFileName(file);
                managerLoader.Invoke(mContent, fileID, file);
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
