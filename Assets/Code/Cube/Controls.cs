using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cube.Logic;


namespace Cube
{
    public class Controls : MonoBehaviour
    {
        CubeLogic logic;

        void Start() => logic = GetComponent<CubeLogic>();

        void Update()
        {


            // Toggle direction
            int direction;

            if (Input.GetKey(KeyCode.LeftShift))
                direction = -1;

            else
                direction = 1;


            // Rotate layers
            if (Input.GetKeyDown(KeyCode.U))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.up), direction);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.forward), direction);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.left), direction);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.right), direction);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.back), direction);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                logic.RotateLayer(TransformByCamera(Vector3Int.down), direction);
            }

            // Rotate cube
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                logic.RotateCube(Vector3Int.right, 1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                logic.RotateCube(Vector3Int.right, -1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                logic.RotateCube(Vector3Int.up, 1);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                logic.RotateCube(Vector3Int.up, -1);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                logic.Scramble(1);
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                logic.Scramble(10);
            }

            // Reset Cube
            if (Input.GetKeyDown(KeyCode.Home))
            {
                logic.InitializeCubeState();
            }
        }

        public Vector3Int TransformByCamera(Vector3Int input)
        {
            Quaternion camera_space = Camera.CameraController.main.RotationRounded90Degrees();
            Quaternion cube_space = logic.transform.rotation;
            return Vector3Int.RoundToInt(Quaternion.Inverse(cube_space) * camera_space * input);
        }
    }
}

