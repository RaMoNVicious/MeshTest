using System;
using UnityEngine;

namespace Operations
{
    [RequireComponent(typeof(Collider))]
    public class OperationFigure : MonoBehaviour
    {
        private Camera _camera;

        private bool _selected;

        private bool _moving;
        
        private Vector3 _offset;

        void Awake()
        {
            _camera = Camera.main;
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
            
            _moving = true;
            _offset = gameObject.transform.position - GetMouseXZPosition();
        }

        private void OnMouseUp()
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
            gameObject.transform.position = _offset + new Vector3(
                position.x,
                0f,
                position.z
            );
        }

        private Vector3 GetMouseXZPosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, Vector3.zero);
            
            return plane.Raycast(ray, out var distance)
                ? ray.GetPoint(distance)
                : gameObject.transform.position;

        }
    }
}