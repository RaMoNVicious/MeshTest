using System.Collections.Generic;
using Models;
using Parabox.CSG;
using UnityEngine;

namespace Operations
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class OperationResult : MonoBehaviour
    {
        [SerializeField]
        private List<BooleanObject> booleanObjects = new();

        private MeshFilter _meshFilter;

        private MeshCollider _meshCollider;

        void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            
            booleanObjects.ForEach(item => item.figure.Setup(Join));
            Join();
        }

        private void Join()
        {
            var result = GetResult(booleanObjects[0], booleanObjects[1]);
            
            _meshFilter.sharedMesh = _meshCollider.sharedMesh = result.mesh;
        }

        private static Model GetResult(BooleanObject baseFigure, BooleanObject booleanFigure) =>
            booleanFigure.operation switch {
                OperationType.Union => CSG.Union(
                    baseFigure.figure.gameObject,
                    booleanFigure.figure.gameObject
                ),
                OperationType.Subtract => CSG.Subtract(
                    baseFigure.figure.gameObject,
                    booleanFigure.figure.gameObject
                ),
            };
    }
}
