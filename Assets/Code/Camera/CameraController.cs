using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {

        public static CameraController main;

        public float rotationX, rotationY;
        public float mouse_sensitivity = 2;

        public Vector3 target = Vector3.zero;
        [Range(0.1f, 2)] public float zoom_level;

        private void Awake()
        {
            main = this;
        }

        void Update()
        {
            rotationY -= Input.GetAxis("Mouse Y") * mouse_sensitivity;
            rotationX += Input.GetAxis("Mouse X") * mouse_sensitivity;

            if (Input.GetMouseButton(0)) RotationRounded90Degrees();

            transform.rotation = Quaternion.identity;
            transform.Rotate(rotationY, rotationX, 0);

            transform.position = target;
            transform.Translate(0, 0, -zoom_level);
        }

        public Quaternion RotationRounded90Degrees()
        {
            Quaternion rotation = transform.rotation;
            Vector3 euler_rounded = new Vector3(
                0,
                Mathf.RoundToInt(rotation.eulerAngles.y / 90) * 90 + 180,
                0);

            Debug.Log(euler_rounded);

            return Quaternion.Euler(euler_rounded);
        }
    }
}

