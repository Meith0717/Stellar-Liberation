// PersistanceManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System;
using System.IO;
using System.Threading;

namespace StellarLiberation.Game.Core.CoreProceses.Persistance
{
    public class PersistanceManager
    {
        private readonly Serializer mSerializer;
        private static readonly string GameSaveDirectory = "gameSave";

        public static string GameSaveFilePath => Path.Combine(GameSaveDirectory, "save");
        public static string SettingsSaveFilePath => Path.Combine(GameSaveDirectory, "settings");

        public PersistanceManager()
        {
            mSerializer = new(".stellarLieberation");
            mSerializer.CreateFolder(GameSaveDirectory);
        }

        public void LoadAsync<Object>(string path, Action<Object> onLoadComplete, Action<Exception> onError) where Object : new()
        {
                Thread loadThread = new(() =>
                {
                    try
                    {
                        Object @object = new();
                        @object = (Object)mSerializer.PopulateObject(@object, path);
                        onLoadComplete?.Invoke(@object);
                    }
                    catch (Exception e)
                    {
                        onError?.Invoke(e);
                    }
                });

            loadThread.Start();
        }

        public void SaveAsync(string path, Object @object, Action onSaveComplete, Action<Exception> onError)
        {
            Thread saveThread = new(() =>
            {
                try
                {
                    if (@object is null) throw new Exception();
                    mSerializer.SerializeObject(@object, path);
                    onSaveComplete?.Invoke();
                }
                catch (Exception e)
                {
                    onError?.Invoke(e);
                }
            });

            saveThread.Start();
        }
    }

}
