using System;
using UnityEngine;

namespace VS.Parser
{
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
}