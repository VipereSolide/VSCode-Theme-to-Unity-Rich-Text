using System;
using UnityEngine;

namespace VS.Parser
{
    [Serializable]
    public class CodeCell
    {
        protected string m_cellSyntax;

        public string CellSyntax
        {
            get
            {
                return m_cellSyntax;
            }
        }
    }

    [Serializable]
    public class VariableDeclarationCell : CodeCell
    {
        protected VariablePrivacy m_variablePrivacy = VariablePrivacy.Private;
        protected DefaultDataClasses m_variableType = DefaultDataClasses.Object;
        protected string m_variableName = string.Empty;
        protected string m_variableValue = string.Empty;
        
        protected bool m_publicGetter = true;
        protected bool m_publicSetter = true;

        public VariablePrivacy VariablePrivacy
        {
            get
            {
                return m_variablePrivacy;
            }
        }

        public DefaultDataClasses VariableType
        {
            get
            {
                return m_variableType;
            }
        }

        public string VariableName
        {
            get
            {
                return m_variableName;
            }
        }

        public string VariableValue
        {
            get
            {
                return m_variableValue;
            }
        }

        public bool IsPublicGetter
        {
            get
            {
                return m_publicGetter;
            }
        }

        public bool IsPublicSetter
        {
            get
            {
                return m_publicSetter;
            }
        }

        public VariableDeclarationCell(VariablePrivacy _VariablePrivacy, DefaultDataClasses _VariableType, string _VariableName, string _VariableValue = "", bool _IsPublicGetter = true, bool _IsPublicSetter = true)
        {
            this.m_publicGetter = _IsPublicGetter;
            this.m_publicSetter = _IsPublicSetter;
            this.m_variableName = _VariableName;
            this.m_variablePrivacy = _VariablePrivacy;
            this.m_variableType = _VariableType;
            this.m_variableValue = _VariableValue;
        }

        public VariableDeclarationCell InterpretLine(string _Line)
        {
            string _spaceFreeLine = _Line;
            int _indexAdder = 0;

            while (_spaceFreeLine[0] == ' ')
                _spaceFreeLine = _spaceFreeLine.Remove(0,1);

            /*private string helloworld = "haha"*/
            string[] _lineCells = _spaceFreeLine.Split(' ');

            // Detects if the line contains any VariablePrivacy.
            if (
                _Line.Contains(VariablePrivacy.Private.ToString()) ||
                _Line.Contains(VariablePrivacy.Public.ToString()) ||
                _Line.Contains(VariablePrivacy.Override.ToString()) ||
                _Line.Contains(VariablePrivacy.Virtual.ToString()) ||
                _Line.Contains(VariablePrivacy.Protected.ToString())
            )
            {
                // Sets _indexAdder to 1, to indicate that you must use +1 on the lineCells index.
                _indexAdder++;

                // If it does, then store it in our m_variablePrivacy variable.
                this.m_variablePrivacy = (VariablePrivacy)System.Enum.Parse(typeof(VariablePrivacy), _lineCells[0]);
            }
            else // If it does not contain any word of privacy.
            {
                // Then the variable is private.
                this.m_variablePrivacy = VariablePrivacy.Private;
            }

            // For the type...
            string _variableTypeName = _lineCells[0 + _indexAdder];
            _variableTypeName = _variableTypeName.Remove(0,1).Insert(0, _variableTypeName[0].ToString().ToUpper()); 
            this.m_variableType = (DefaultDataClasses)System.Enum.Parse(typeof(DefaultDataClasses), _variableTypeName);

            // For the name...
            this.m_variableName = _lineCells[1 + _indexAdder];

            // For the value...
            this.m_variableValue = _Line.Split('=')[1].Replace(" ","");

            return this;
        }

        public static VariableDeclarationCell StringToVDC(string _Line)
        {
            VariableDeclarationCell _output = null;

            string _spaceFreeLine = _Line;
            int _indexAdder = 0;

            while (_spaceFreeLine[0] == ' ')
                _spaceFreeLine = _spaceFreeLine.Remove(0,1);

            /*private string helloworld = "haha"*/
            string[] _lineCells = _spaceFreeLine.Split(' ');

            // Detects if the line contains any VariablePrivacy.
            if (
                _Line.Contains(VariablePrivacy.Private.ToString()) ||
                _Line.Contains(VariablePrivacy.Public.ToString()) ||
                _Line.Contains(VariablePrivacy.Override.ToString()) ||
                _Line.Contains(VariablePrivacy.Virtual.ToString()) ||
                _Line.Contains(VariablePrivacy.Protected.ToString())
            )
            {
                // Sets _indexAdder to 1, to indicate that you must use +1 on the lineCells index.
                _indexAdder++;

                // If it does, then store it in our m_variablePrivacy variable.
                _output.m_variablePrivacy = (VariablePrivacy)System.Enum.Parse(typeof(VariablePrivacy), _lineCells[0]);
            }
            else // If it does not contain any word of privacy.
            {
                // Then the variable is private.
                _output.m_variablePrivacy = VariablePrivacy.Private;
            }

            // For the type...
            string _variableTypeName = _lineCells[0 + _indexAdder];
            _variableTypeName = _variableTypeName.Remove(0,1).Insert(0, _variableTypeName[0].ToString().ToUpper()); 
            _output.m_variableType = (DefaultDataClasses)System.Enum.Parse(typeof(DefaultDataClasses), _variableTypeName);

            // For the name...
            _output.m_variableName = _lineCells[1 + _indexAdder];

            // For the value...
            _output.m_variableValue = _Line.Split('=')[1].Replace(" ","");

            return _output;
        }
    }
}