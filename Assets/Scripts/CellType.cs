using UnityEngine;

namespace VS.Parser
{
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