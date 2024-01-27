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
        private static readonly string DataSaveDirectory = "data";

        public static string GameSaveFilePath => Path.Combine(GameSaveDirectory, "save");
        public static string SettingsSaveFilePath => Path.Combine(DataSaveDirectory, "settings");

        public PersistanceManager()
        {
            mSerializer = new("Stellar Lieberation");
            mSerializer.CreateFolder(GameSaveDirectory);
            mSerializer.CreateFolder(DataSaveDirectory);
        }

        public void Load<Object>(string path, Action<Object> onLoadComplete, Action<Exception> onError) where Object : new()
        {
            try
            {
                Object @object = new();
                @object = (Object)mSerializer.PopulateObject(@object, path);
                onLoadComplete?.Invoke(@object);
            }
            catch (Exception e) { onError?.Invoke(e); }
        }

        public void LoadAsync<Object>(string path, Action<Object> onLoadComplete, Action<Exception> onError, Action onAllways = null) where Object : new()
        {
            Thread loadThread = new(() =>
            {
                try
                {
                    Object @object = new();
                    @object = (Object)mSerializer.PopulateObject(@object, path);
                    onLoadComplete?.Invoke(@object);
                }
                catch (Exception e) { onError?.Invoke(e); }
                onAllways?.Invoke();
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
                catch (Exception e) { onError?.Invoke(e); }
            });

            saveThread.Start();
        }
    }

}
