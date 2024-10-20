﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplorerRestarter.Utilities
{
    public class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
    {
        private const int DefaultHashSeed = 19;
        public static readonly HashSetComparer<T> Default = new HashSetComparer<T>();

        public bool Equals(HashSet<T> x, HashSet<T> y) => x.SetEquals(y);

        public int GetHashCode(HashSet<T> obj) => obj.Aggregate(
            DefaultHashSeed, 
            (hash, item) => hash * 31 + item.GetHashCode()
        );
    }
}