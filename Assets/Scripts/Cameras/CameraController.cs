using System;
using UnityEngine;

namespace Cameras
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 speedRotation = new(500f, 200f);

        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                var rotation = transform.rotation.eulerAngles;
                var x = -Input.GetAxis("Mouse Y") * speedRotation.x * Time.deltaTime + rotation.x;
                var y = Input.GetAxis("Mouse X") * speedRotation.y * Time.deltaTime + rotation.y;

                transform.eulerAngles = new Vector3(
                    Math.Clamp(x, 1, 89),
                    y,
                    0
                );
            }
        }
    }
}