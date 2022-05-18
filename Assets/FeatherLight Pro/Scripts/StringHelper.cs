using UnityEngine;
using System;

namespace FeatherLight.Pro
{
    public static class StringHelper
    {
        public static bool ContainsArray(string text, string[] _values)
        {
            bool _output = false;

            foreach(string _v in _values)
            {
                if (text.Contains(_v))
                    _output = true;
            }

            return _output;
        }
    }
}