using UnityEngine;

namespace Figures
{
    [RequireComponent(typeof(Collider))]
    public class FigureBounds : MonoBehaviour
    {
        [SerializeField]
        private Material
            hoverMaterial,
            selectedMaterial;
        
        [SerializeField]
        [Range(0.02f, 0.2f)]
        private float boundOffset = 0.02f;
        
        [SerializeField]
        [Range(0.25f, 2.0f)]
        private float iconSize = 0.5f;
        
        private Collider _collider;

        private Vector3 _size;

        private TransformType? _state;

        private bool _hover;
        
        private readonly Vector3[] _corners = new Vector3[8];
        
        private readonly int[,] _edges = new int[12, 2]
        {
            {0,1}, {1,2}, {2,3}, {3,0},
            {4,5}, {5,6}, {6,7}, {7,4},
            {0,4}, {1,5}, {2,6}, {3,7}
        };

        void Awake()
        {
            _collider = gameObject.GetComponent<Collider>();
            
            var bounds = new Bounds(
                Vector3.zero, 
                _collider.bounds.size + boundOffset * Vector3.one
            );
            _corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            _corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            _corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            _corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            _corners[4] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            _corners[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            _corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            _corners[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        }

        public void SetState(TransformType? state = null)
        {
            _state = state;
        }

        private void OnMouseEnter()
        {
            _hover = true;
        }

        private void OnMouseExit()
        {
            _hover = false;
        }

        private void OnRenderObject()
        {
            if (selectedMaterial == null || hoverMaterial == null)
            {
                return;
            }
            
            if (_state != null)
            {
                DrawBounds();
            } else if (_hover)
            {
                DrawBounds(_hover);
            }

            if (_state == TransformType.Move)
            {
                DrawMove();
            }

            if (_state == TransformType.Rotate)
            {
                DrawRotate();
            }
        }

        private void DrawBounds(bool isHovered = false)
        {
            if (isHovered)
            {
                hoverMaterial.SetPass(0);
            } else
            {
                selectedMaterial.SetPass(0);
            }
            
            GL.Begin(GL.LINES);

            var rotation = -transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);
            
            var localCorners = new Vector3[8];
            for (var i = 0; i < 8; i++)
            {
                localCorners[i] = new Vector3(
                    _corners[i].x * cos - _corners[i].z * sin,
                    _corners[i].y,
                    _corners[i].x * sin + _corners[i].z * cos
                ) + transform.position;
            }

            for (var i = 0; i < 12; i++)
            {
                GL.Vertex(localCorners[_edges[i, 0]]);
                GL.Vertex(localCorners[_edges[i, 1]]);
            }

            GL.End();
        }

        private void DrawMove()
        {
            selectedMaterial.SetPass(0);
            GL.Begin(GL.LINES);

            var origin = transform.position;
            GL.Vertex(origin - transform.right * iconSize);
            GL.Vertex(origin + transform.right * iconSize);
            GL.Vertex(origin - transform.forward * iconSize);
            GL.Vertex(origin + transform.forward * iconSize);

            GL.End();
        }

        private void DrawRotate()
        {
            selectedMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            
            var center = transform.position;
            const int segments = 36;
            const float angleStep = 2 * Mathf.PI / segments;
            
            for (var i = 0; i < segments; i++)
            {
                var angleFrom = i * angleStep;
                var angleTo = (i + 1) * angleStep;

                var pointFrom = new Vector3(
                    Mathf.Cos(angleFrom) * iconSize,
                    0f,
                    Mathf.Sin(angleFrom) * iconSize
                ) + center;

                var pointTo = new Vector3(
                    Mathf.Cos(angleTo) * iconSize,
                    0f,
                    Mathf.Sin(angleTo) * iconSize
                ) + center;

                GL.Vertex(pointFrom);
                GL.Vertex(pointTo);
            }

            GL.End();
        }
    }
}