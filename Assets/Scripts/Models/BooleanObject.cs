using System;
using Operations;

namespace Models
{
    
    [Serializable]
    public class BooleanObject
    {
        public OperationFigure figure;

        public OperationType operation;
    }

    public enum OperationType
    {
        Union,
        Subtract
    }
}