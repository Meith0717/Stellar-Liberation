using System;
using System.Net.Http.Headers;

namespace Galaxy_Explovive.Core.Utility
{
    internal class RandomSingleton
    {
        private static Random mInstance;

        public static Random Instance { get { return mInstance ??= new(); } }
    }
}
