// GameLayerFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Layers;
using System;
using System.IO;
using System.Threading;

namespace StellarLiberation.Game.Core.CoreProceses.Persistance
{
    public class PersistanceManager
    {
        private readonly Serializer mSerializer;
        public readonly string GameSaveDirectory = "gameSave";

        public string GameSaveFilePath => Path.Combine(GameSaveDirectory, "save");

        public PersistanceManager()
        {
            mSerializer = new(".stellarLieberation");
            mSerializer.CreateFolder(GameSaveDirectory);
        }

        public GameLayer LoadGameLayer()
        {
            if (!mSerializer.FileExist(GameSaveFilePath)) throw new FileNotFoundException();
            var gameLayer = new GameLayer();
            return (GameLayer)mSerializer.PopulateObject(gameLayer, GameSaveFilePath);
        }

        public void SaveGameLayer(GameLayer gameLayer)
        {
            if (gameLayer is null) throw new System.Exception();
            mSerializer.SerializeObject(gameLayer, GameSaveFilePath);
        }

        public void LoadGameLayerAsync(Action<GameLayer> onLoadComplete, Action<Exception> onError)
        {
            Thread loadThread = new Thread(() =>
            {
                try
                {
                    if (!mSerializer.FileExist(GameSaveFilePath)) throw new FileNotFoundException();
                    var gameLayer = new GameLayer();
                    gameLayer = (GameLayer)mSerializer.PopulateObject(gameLayer, GameSaveFilePath);

                    onLoadComplete?.Invoke(gameLayer);
                }
                catch (Exception ex)
                {
                    throw ex;
                    onError?.Invoke(ex);
                }
            });

            loadThread.Start();
        }

        public void SaveGameLayerAsync(GameLayer gameLayer, Action onSaveComplete, Action<Exception> onError)
        {
            Thread saveThread = new Thread(() =>
            {
                try
                {
                    if (gameLayer is null) throw new Exception();
                    mSerializer.SerializeObject(gameLayer, GameSaveFilePath);

                    onSaveComplete?.Invoke();
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }
            });

            saveThread.Start();
        }
    }
}
