using UnityEngine;

namespace Operations
{
    public class SelectionManager : MonoBehaviour
    {
        private Camera _camera;
        
        private OperationFigure _current;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    var hirFigure = hit.collider.GetComponent<OperationFigure>(); 
                    
                    if (hirFigure == null)
                    {
                        _current?.Deselect();
                        _current = null;
                    }
                    else if (hirFigure != _current)
                    {
                        _current?.Deselect();
                        _current = hirFigure;
                        _current.Select();
                    }
                }
                else
                {
                    _current?.Deselect();
                    _current = null;
                }
            }
        }
    }
}