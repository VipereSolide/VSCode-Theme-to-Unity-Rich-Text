using System;
using UnityEngine;

namespace VS.Parser
{
    [System.Serializable]
    public class DetectedModule
    {
        [SerializeField] private string m_Syntax = string.Empty;
        [SerializeField] private string m_Color = string.Empty;

        public string Syntax
        {
            get
            {
                return m_Syntax;
            }
        }

        public string ColorInHex
        {
            get
            {
                    return m_Color;
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

        public DetectedModule(string _syntax, string _color)
        {
            this.m_Color = _color;
            this.m_Syntax = _syntax;
        }
    }
}