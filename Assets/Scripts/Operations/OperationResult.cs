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
        private List<BooleanObject> booleanObjects = new();

        private MeshFilter _meshFilter;

        void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Operate();
            }
        }

        private void Operate()
        {
            if (booleanObjects.Count != 2)
            {
                Debug.LogError("Wrong objects count");
            }

            var result = CSG.Subtract(booleanObjects[0].figure, booleanObjects[1].figure);
            _meshFilter.sharedMesh = result.mesh;
        }
    }
}
