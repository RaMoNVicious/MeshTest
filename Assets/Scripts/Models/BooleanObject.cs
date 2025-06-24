using System;
using UnityEngine;

namespace Models
{
    
    [Serializable]
    public class BooleanObject
    {
        public GameObject figure;

        public OperationType operation;
    }

    public enum OperationType
    {
        Base,
        Union,
        Subtract
    }
}