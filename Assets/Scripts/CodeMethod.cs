using System;
using UnityEngine;

namespace VS.Parser
{
    [Serializable]
    public class CodeMethod
    {
        [SerializeField] private string m_MethodSyntax = "";

        public string MethodSyntax
        {
            get
            {
                return m_MethodSyntax;
            }
        }

        public CodeMethod(string _syntax)
        {
            this.m_MethodSyntax = _syntax;
        }
    }
}