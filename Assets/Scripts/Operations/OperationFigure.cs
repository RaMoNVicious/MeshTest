using System;
using Figures;
using UnityEngine;
using Utils;

namespace Operations
{
    [RequireComponent(typeof(FigureBounds))]
    public class OperationFigure : MonoBehaviour {
        
        private Camera _camera;

        private FigureBounds _figureBounds;

        private bool _isSelected;

        private TransformType _state = TransformType.Move;

        private bool _moving;

        private bool _rotating;
        
        private Vector3 _offset;

        private float _yPosition;

        private Action _onReposition;

        public void Setup(Action onReposition)
        {
            _onReposition = onReposition;
        }

        void Awake()
        {
            _camera = Camera.main;
            _figureBounds = GetComponent<FigureBounds>();
            
            _yPosition = transform.position.y;
        }

        void Update()
        {
            if (!_isSelected)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_state == TransformType.Move)
                {
                    _moving = true;
                    _offset = transform.position.IgnoreY() - GetMouseXZPosition();
                } else if (_state == TransformType.Rotate)
                {
                    _rotating = true;
                }
            } else if (Input.GetMouseButtonUp(0))
            {
                _moving = false;
                _rotating = false;
            } else if (Input.GetMouseButtonUp(1))
            {
                _state = _state switch {
                    TransformType.Rotate => TransformType.Move,
                    TransformType.Move => TransformType.Rotate,
                };
                _figureBounds.SetState(_state);
            }

            if (Input.GetMouseButton(0) && (_moving || _rotating))
            {
                OnMouseMove();
            }
        }

        public void Select()
        {
            if (_isSelected)
            {
                return;
            }
            
            _isSelected = true;
            _state = TransformType.Move;
            _figureBounds.SetState(_state);
        }

        public void Deselect()
        {
            _isSelected = false;
            _figureBounds.SetState();
        }

        void OnMouseMove()
        {
            if (!_isSelected || (!_moving && !_rotating))
            {
                return;
            }
            
            var isRepositioned = _moving 
                ? Move() 
                : _rotating && Rotate();

            if (isRepositioned)
            {
                _onReposition?.Invoke();
            }
        }

        private bool Move()
        {
            var position = GetMouseXZPosition();
            var target = _offset + new Vector3(
                position.x,
                _yPosition,
                position.z
            );

            var isRepositioned = Vector3.Distance(transform.position, target) > 0f;
            transform.position = target;

            return isRepositioned;
        }

        private bool Rotate()
        {
            const float speed = 50f;
            var y = Input.GetAxis("Mouse X") * speed * Time.deltaTime + transform.rotation.eulerAngles.y;
            transform.eulerAngles = new Vector3(0f, y, 0f);
            
            return y > 0;
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