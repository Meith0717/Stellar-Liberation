﻿// DataCollector.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.Extensions;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StellarLiberation.Game.Core.CoreProceses.Profiling
{
    internal class DataCollector
    {
        public readonly List<string> Lables;
        public readonly List<List<float>> Data;
        private readonly int mRecordCount;

        public DataCollector(int recordCount, List<string> lables) 
        {
            if (lables.Count != recordCount) throw new System.ArgumentException();
            mRecordCount = recordCount;
            Lables = lables;
            Data = new();
        }

        public void AddData(List<float> frameData)
        {
            if (frameData.Count != mRecordCount) throw new System.ArgumentException();
            Data.Add(frameData);
            System.Diagnostics.Debug.Write("Data Collect");
        }
    }
}
