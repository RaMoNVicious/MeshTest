using System;
using UnityEngine;

namespace Operations
{
    [RequireComponent(typeof(Collider))]
    public class OperationFigure : MonoBehaviour
    {
        [SerializeField]
        private bool isMovable = false;
        
        private Camera _camera;

        private bool _selected;

        private bool _moving;
        
        private Vector3 _offset;

        private float _yPosition;

        private Action _onReposition = null;

        public void Setup(Action onReposition)
        {
            _onReposition = onReposition;
        }

        void Awake()
        {
            _camera = Camera.main;
            _yPosition = transform.position.y;
        }

        public void Select()
        {
            Debug.Log($"{this.name} Selected");
            _selected = true;
        }

        public void Deselect()
        {
            Debug.Log($"{this.name} Deselected");
            _selected = false;
        }

        void OnMouseDown()
        {
            if (!_selected)
            {
                return;
            }
            
            _moving = isMovable;
            _offset = gameObject.transform.position - GetMouseXZPosition();
        }

        void OnMouseUp()
        {
            _moving = false;
        }

        void OnMouseDrag()
        {
            if (!_selected || !_moving)
            {
                return;
            }
            
            var position = GetMouseXZPosition();
            var target = _offset + new Vector3(
                position.x,
                _yPosition,
                position.z
            );

            var isRepositioned = Vector3.Distance(transform.position, target) > 0f;
            transform.position = target;
            if (isRepositioned)
            {
                _onReposition?.Invoke();
            }
        }

        private Vector3 GetMouseXZPosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, Vector3.zero);
            
            return plane.Raycast(ray, out var distance)
                ? ray.GetPoint(distance)
                : transform.position;

        }
    }
}