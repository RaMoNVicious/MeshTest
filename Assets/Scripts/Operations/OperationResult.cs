using System.Collections.Generic;
using Models;
using Parabox.CSG;
using UnityEngine;

namespace Operations
{
    [RequireComponent(typeof(MeshFilter))]
    public class OperationResult : MonoBehaviour
    {
        [SerializeField]
        private GameObject cacheFigure;
        
        [SerializeField]
        private List<BooleanObject> booleanObjects = new();

        private MeshFilter _meshFilter;

        private MeshFilter _cacheMeshFilter;

        private MeshRenderer _cacheMeshRenderer;

        private MeshCollider _cacheMeshCollider;

        void Awake()
        {
            Application.targetFrameRate = 144;
            
            _meshFilter = GetComponent<MeshFilter>();
            _cacheMeshFilter = cacheFigure.GetComponent<MeshFilter>();
            _cacheMeshRenderer = cacheFigure.GetComponent<MeshRenderer>();
            _cacheMeshCollider = cacheFigure.GetComponent<MeshCollider>();
            
            booleanObjects.ForEach(item => item.figure.Setup(Join));
            Join();
        }

        private void Join()
        {
            for (var i = 0; i < booleanObjects.Count - 1; i++)
            {
                var result = GetResult(
                    i == 0 ? booleanObjects[i].figure.gameObject : cacheFigure,
                    booleanObjects[i + 1]
                );
                UpdateCacheFigure(result);
            }
            
            _meshFilter.sharedMesh = cacheFigure.GetComponent<MeshFilter>().mesh;
        }

        private void UpdateCacheFigure(Model result)
        {
            _cacheMeshFilter.sharedMesh = _cacheMeshCollider.sharedMesh = result.mesh;
            _cacheMeshRenderer.sharedMaterials = result.materials.ToArray();
        }

        private static Model GetResult(GameObject baseFigure, BooleanObject booleanFigure) =>
            booleanFigure.operation switch {
                OperationType.Union => CSG.Union(
                    baseFigure,
                    booleanFigure.figure.gameObject
                ),
                OperationType.Subtract => CSG.Subtract(
                    baseFigure,
                    booleanFigure.figure.gameObject
                ),
            };
    }
}
