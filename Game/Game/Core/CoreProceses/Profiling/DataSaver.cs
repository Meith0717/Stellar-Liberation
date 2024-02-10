// DataSaver.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.Persistance;
using System;
using System.IO;

namespace StellarLiberation.Game.Core.CoreProceses.Profiling
{
    internal class DataSaver
    {
        private static readonly string BenchmatksSaveDirectory = "benchmarks";

        public static void SaveToCsv(Serializer serializer, DataCollector dataCollector)
        {
            serializer.CreateFolder(BenchmatksSaveDirectory);
            DateTime currentDateTime = DateTime.Now;
            var fileName = $"{currentDateTime.ToString("yyyyMMddHHmmss")}.csv";
            var path = Path.Combine(BenchmatksSaveDirectory, fileName);
            using StreamWriter fileWriter = serializer.GetStreamWriter(path);
            // Write header row with data descriptions
            fileWriter.WriteLine(dataCollector.Lables);

            // Write data row with corresponding values
            foreach (var entry in dataCollector.Data)
                fileWriter.WriteLine(string.Join(",", entry));
        }
    }
}
