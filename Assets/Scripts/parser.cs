using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VS.Parser
{
    public class parser : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text m_Text;
        [SerializeField] private List<CodeCell> m_Cells = new List<CodeCell>();

        void Start()
        {
            m_Cells = GetLineType(m_Text.text);
            m_Text.text = Parse(m_Text.text); 
        }

        public string m_ClassColor = "56B7C3";
        public string m_NameSpaceNameUsingColor = "E06C75";
        public string m_ClassNameColor = "838FA7CC";
        public string m_CommentColor = "676E95";
        public string m_MethodInitilializerColor = "6495EE";
        public string m_MethodParameterColor = "E4BF7F";
    
        public string[] m_CurlyBracketsColors = {"E4BF7F","CF68E1","6495EE"};
        public List<HighlightModule> m_Modules = new List<HighlightModule>()
        {
            new HighlightModule(new string[] {"int","bool","float","object","string","double","long","void"}, "E06C75"),
            new HighlightModule(new string[] {"class","enum","namespace","interface","public","private","protected"}, "676E95"),
            new HighlightModule(new string[] {";",".","-","*","+","/"}, "676E95"),
            new HighlightModule(new string[] {"using"}, "CF68E1")
        };

        public List<DetectedModule> m_DetectedModules = new List<DetectedModule>();
        public List<Method> m_Methods = new List<Method>();

        private string ColorToHex(Color32 _Color)
        {
            int _red = (int)_Color.r;
            int _green = (int)_Color.g;
            int _blue = (int)_Color.b;
            int _alpha = (int)_Color.a;

            string _result = _red.ToString("X2") + _green.ToString("X2") + _blue.ToString("X2") + _alpha.ToString("X2");
            return _result;
        }

        public string Parse(string _Text)
        {
            string _output = _Text;

            // LOGIC
            string[] _textInWords = _Text.Split(new char[]{' ', '\n'});
            string[] _textInLines = new string[]{} /* _Text.Split(new char[]{'\n'}) */ ;
            string[] _openedCurlyBracketsInText = _Text.Split(new char[]{'{'});
            string[] _closedCurlyBracketsInText = _Text.Split(new char[]{'}'});

            bool _openMethod = false;
            int _methodStartIndex = 0;
            int _methodEndIndex = 0;

            for(int i = 0; i < _textInWords.Length; i++)
            {
                string _currentWord = _textInWords[i];
                string _lastWord = _textInWords[(i > 0) ? i - 1 : 0];

                // Detect Class Names
                if (_lastWord == "class" || _lastWord == "namespace" && i > 0)
                {
                    m_DetectedModules.Add(new DetectedModule(_currentWord, m_ClassColor));
                }
            
                // Detect Using
                if (_lastWord == "using" && i > 0)
                {
                    m_DetectedModules.Add(new DetectedModule(_currentWord, m_NameSpaceNameUsingColor));
                }

                // Detect Class Types
                if (i > 2)
                {
                    if (_textInWords[i - 3] == "class")
                    {
                        m_DetectedModules.Add(new DetectedModule(_currentWord, m_ClassNameColor));
                    }
                }

                // Detects Methods
                if (i > 2 && i < _textInWords.Length)
                {
                    bool _isMethod = false;

                    foreach(string _type in m_Modules[0].ModuleNames)
                    {
                        if (_lastWord == _type)
                        {
                            _isMethod = true;
                            break;
                        }
                    }

                    if (_isMethod)
                    {
                        _isMethod = _currentWord.Contains("(");

                        if (_isMethod)
                        {
                            m_DetectedModules.Add(new DetectedModule(_currentWord, m_MethodInitilializerColor));
                            _methodStartIndex = _Text.IndexOf(_currentWord);
                            _openMethod = true;
                        }

                        if (_openMethod && _currentWord.Contains(")"))
                        {
                            _openMethod = false;
                        }
                    }
                }

                if (_openMethod && _currentWord.Contains(")"))
                {
                    _methodEndIndex = _Text.IndexOf(_currentWord) + _currentWord.Length;
                    int _methodLength = _methodEndIndex - _methodStartIndex;
                    string _syntax = _Text.Substring(_methodStartIndex, _methodLength);
                
                    m_Methods.Add(new Method(_syntax));
                    _openMethod = false;
                }
            }

            foreach(Method _method in m_Methods)
            {
                _output = _output.Replace(_method.MethodSyntax, ParseMethod(_method));
            }

            int _openedCurlyBracketsCount = 0;
            string _lineEndString = "";
            _textInLines = _output.Split(new char[]{'\n'});

            for(int _lineIndex = 0; _lineIndex < _textInLines.Length; _lineIndex++)
            {            
                // Detect curly brackets
                if (_textInLines[_lineIndex].Contains("{"))
                {
                    _openedCurlyBracketsCount++;
                    if(_openedCurlyBracketsCount >= m_CurlyBracketsColors.Length) _openedCurlyBracketsCount = 0;

                    string _newLine = _textInLines[_lineIndex].Replace("{","<#" + m_CurlyBracketsColors[_openedCurlyBracketsCount] + ">{</color>");
                    _textInLines[_lineIndex] = _newLine;
                }

                if (_textInLines[_lineIndex].Contains("}"))
                {
                    string _newLine = _textInLines[_lineIndex].Replace("}","<#" + m_CurlyBracketsColors[_openedCurlyBracketsCount] + ">}</color>");
                    _textInLines[_lineIndex] = _newLine;

                    _openedCurlyBracketsCount--;
                    if(_openedCurlyBracketsCount <= -1) _openedCurlyBracketsCount = m_CurlyBracketsColors.Length - 1;
                }

                // Detect Comments
                if (_textInLines[_lineIndex].Length < 2)
                    continue;

                if (_textInLines[_lineIndex].Contains("/"))
                {
                    string _noSpace = _textInLines[_lineIndex].Replace(" ","");

                    if (_noSpace[0] == '/' && _noSpace[1] == '/')
                    {
                        _textInLines[_lineIndex] = "<#" + m_CommentColor + ">" + _textInLines[_lineIndex] + "</color>";
                    }
                }

                _lineEndString += _textInLines[_lineIndex] + "\n";
            }

            _output = _lineEndString;

            foreach(HighlightModule _module in m_Modules)
            {
                for (int i = 0; i < _module.ModuleNames.Length; i++)
                {
                    _output = _output.Replace(_module.ModuleNames[i], "<#" + _module.ColorInHex + ">" + _module.ModuleNames[i] + "</color>");
                }
            }
        
            foreach(DetectedModule _detected in m_DetectedModules)
            {
                _output = _output.Replace(_detected.Syntax, "<#" + _detected.ColorInHex + ">" + _detected.Syntax + "</color>");
            }

            return _output;
        }

        private string ParseMethod(Method _Method)
        {
            string _output = "";
            string _name = _Method.MethodSyntax.Split('(')[0];
            string _parametersText = _Method.MethodSyntax.Split('(')[1].Split(')')[0];

            string[] _params = _parametersText.Split(',');

            _parametersText = "";

            for(int i = 0; i < _params.Length; i++)
            {
                string _newParam = _params[i].Replace(",","");
                string _newParamType = "";

                foreach(string _type in m_Modules[0].ModuleNames)
                {
                    if (_newParam.Contains(_type))
                    {
                        _newParamType = _type;
                    }

                    _newParam = _newParam.Replace(_type,"");
                }

                _newParam = "<#" + m_MethodParameterColor + ">" + _newParam + "</color>";

                if (i < _params.Length - 1)
                    _newParam += ", ";

                _parametersText += ((i > 0) ? " " : "") + (_newParamType + " " + _newParam.Replace(" ",""));
            }

            return "<#" + m_MethodInitilializerColor + ">" + _name + "</color>" + "(" + _parametersText + ")";
        }
    
        public List<CodeCell> GetLineType(string _Text)
        {
            List<CodeCell> _output = new List<CodeCell>();
            string[] _textInCells = _Text.Split('\n');

            foreach(string _cell in _textInCells)
            {
                string _name = _cell;
                CellType _type = CellType.Condition;
                string _cellNoSpace = _cell.Replace(" ","");

                // Condition
                if (_cellNoSpace.Contains("if("))
                {
                    _type = CellType.Condition;
                }
                
                // Variable Declaration
                if (ContainsArray(_cell, m_Modules[0].ModuleNames) && _cell.Contains("="))
                {
                    _type = CellType.VariableDeclaration;
                }
                else if (!ContainsArray(_cell, m_Modules[0].ModuleNames) && _cell.Contains("=")) // Variable Set
                {
                    _type = CellType.VariableSet;
                }

                _output.Add(new CodeCell(_name, _type));
            }

            return _output;
        }

        private bool ContainsArray(string text, string[] _values)
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

    [System.Serializable]
    public class CodeCell
    {
        [SerializeField] private string m_cellName;
        [SerializeField] private CellType m_cellType;

        public string CellName
        {
            get { return m_cellName; }
        }

        public CellType CellType
        {
            get { return m_cellType; }
        }

        public CodeCell(string _CellName, CellType _CellType)
        {
            this.m_cellName = _CellName;
            this.m_cellType = _CellType;
        }
    }

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

    [System.Serializable]
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

    [System.Serializable]
    public class Method
    {
        [SerializeField] private string m_MethodSyntax = "";

        public string MethodSyntax
        {
            get
            {
                return m_MethodSyntax;
            }
        }

        public Method(string _syntax)
        {
            this.m_MethodSyntax = _syntax;
        }
    }

    public enum CellType
    {
        VariableDeclaration,
        MethodDeclaration,
        VariableSet,
        MethodCall,
        DataHandler,
        Condition,
        Loop
    }
}