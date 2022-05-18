using System;
using UnityEngine;

namespace VS.Parser
{
    [Serializable]
    public class HighlightModule
    {
        [SerializeField] private string[] m_ModuleNames;
        [SerializeField] private string m_ColorInHex;

        public string[] ModuleNames
        {
            get
            {
                return m_ModuleNames;
            }
        }

        public string ColorInHex
        {
            get
            {
                return m_ColorInHex;
            }
        }

        public Color32 Color
        {
            get
            {
                int _bigint = int.Parse(ColorInHex, System.Globalization.NumberStyles.HexNumber);
                var _r = (_bigint >> 16) & 255;
                var _g = (_bigint >> 8) & 255;
                var _b = _bigint & 255;

                Color32 _col = new Color32((byte)_r,(byte)_g,(byte)_b,255);
                return _col;
            }
        }

        public HighlightModule(string[] _Name, string _ColorInHex)
        {
            this.m_ModuleNames = _Name;
            this.m_ColorInHex = _ColorInHex;
        }
    }
}