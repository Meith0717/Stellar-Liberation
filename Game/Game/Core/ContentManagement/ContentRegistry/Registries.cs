// Registries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System.Collections.Generic;
using System.Reflection;

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public abstract class Registries
    {
        public static List<Registry> GetRegistryList<RegistriesType>()
        {
            FieldInfo[] fields = typeof(RegistriesType).GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Registry> registrys = new();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != typeof(Registry)) continue;
                Registry registry = (Registry)field.GetValue(null);
                registrys.Add(registry);
            }
            return registrys;
        }
    }
}
