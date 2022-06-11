using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Logic
{
    public class CubeLogic : MonoBehaviour
    {

        public CubeState current_state = new CubeState();


        public void Awake()
        {
            InitializeCubeState();
        }

        public void InitializeCubeState()
        {
            current_state = new CubeState();

            foreach (Transform transform in GetComponentsInChildren<Transform>())
            {
                if (transform.name.Contains("PIECE"))
                {
                    CubePiece piece = new CubePiece(transform);
                    current_state.pieces.Add(piece.default_position, piece);
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        public void RotateLayer(Vector3Int axis, int direction = 1)
        {
            List<CubePiece> layer_contents = current_state.GetLayerFromVector(axis);

            foreach (CubePiece piece in layer_contents) 
            {
                piece.transform.Rotate(Quaternion.Inverse(piece.transform.localRotation) * axis, 90 * direction);            
            }

            current_state.RotateLayerInDictionary(layer_contents, axis, direction);
                    
        }

        public void RotateLayer(CubeOperation op)
        {
            RotateLayer(op.axis, op.direction);
        }

        public void RotateCube(Vector3Int axis, int direction = 1)
        {
            transform.Rotate(axis, direction * 90, Space.World);
        }

        public void Scramble(int num_operations = 1)
        {
            for (int i = 0; i < num_operations; i++)
            {
                RotateLayer(new CubeOperation().Randomize());
            }
        }
    }


    [System.Serializable]
    public class CubeState
    {
        public Dictionary<Vector3Int, CubePiece> pieces = new Dictionary<Vector3Int, CubePiece> ();

        public List<CubePiece> GetLayerFromVector(Vector3Int axis)
        {
            List<CubePiece> layer = new List<CubePiece>();

            foreach (Vector3Int key in this.pieces.Keys)
            {
                if (Vector3.Dot(key, axis) > 0.5)
                {
                    layer.Add(this.pieces[key]);
                }
            }

            return layer;
        }

        public void RotateLayerInDictionary (List<CubePiece> layer_contents, Vector3Int axis, int direction)
        {
            //if (axis == Vector3Int.up) direction *= -1;

            Dictionary<Vector3Int, CubePiece> new_pieces = new Dictionary<Vector3Int, CubePiece>();  // Create a shallow copy of the dictionary so we can shift the keys around
            foreach (Vector3Int key in pieces.Keys)
            {
                if (layer_contents.Contains(pieces[key]))
                {
                    Vector3 new_key = RotateVector3 (key, axis, direction);  // Transform the default position vector by the rotation of the transform to get the new position

                    new_pieces.Add(Vector3Int.RoundToInt(new_key), pieces[key]);
                }

                else
                {
                    new_pieces.Add(key, pieces[key]);
                }
            }

            pieces = new_pieces;
        }

        public void RefreshKeys()
        {

            Dictionary<Vector3Int, CubePiece> new_pieces = new Dictionary<Vector3Int, CubePiece>();  // Create a shallow copy of the dictionary so we can shift the keys around
            foreach (Vector3Int key in pieces.Keys)
            {
                Vector3 position = pieces[key].locator.position.normalized;

                Vector3Int new_key = Vector3Int.CeilToInt(position);

                if (!new_pieces.ContainsKey(new_key)) new_pieces.Add(new_key, pieces[key]);            
            }

            pieces = new_pieces;
        }

        Vector3 RotateVector3(Vector3 input, Vector3 axis, int direction)
        {
            Vector3 vector = Quaternion.AngleAxis(90 * direction, axis) * input;
            return vector;
        }
    }


    [System.Serializable]
    public class CubePiece
    {
        public Transform transform;
        public Transform locator;
        public PieceType pieceType = PieceType.Core;
        public Vector3Int default_position = Vector3Int.zero;

        public enum PieceType
        {
            Core,
            Corner,
            Edge,
        }

        public CubePiece (Transform transform)
        {
            this.transform = transform;
            pieceType = GetPieceType(transform.name);
            default_position = GetDefaultPosition(transform);
        }

        public PieceType GetPieceType(string name)
        {

            if (name.Contains("edge"))
            {
                return PieceType.Edge;
            }

            else if (name.Contains("corner"))
            {
                return PieceType.Corner;
            }

            else
            {
                return PieceType.Core;
            }
        }

        // Parse object names within the cube to determine where each piece belongs on the cube
        public Vector3Int GetDefaultPosition(Transform transform)
        {

            string[] subs = transform.name.Split('_');

            if (subs.Length >= 2)
            {

                int x = 0, y = 0, z = 0;

                if (subs[1].Contains("n")) z = 1;
                else if (subs[1].Contains("s")) z = -1;

                if (subs[1].Contains("e")) x = 1;
                else if (subs[1].Contains("w")) x = -1;

                if (subs[1].Contains("t")) y = 1;
                else if (subs[1].Contains("b")) y = -1;

                return new Vector3Int(x, y, z);

            }

            else return Vector3Int.zero;
        }
    }

    [System.Serializable]
    public class CubeOperation
    {
        public Vector3Int axis;
        public int direction;

        public CubeOperation Randomize()
        {
            int random_vector = Random.Range(0, 6);
            int random_direction = Random.Range(0, 2) * 2 - 1;
            return new CubeOperation(random_vector, random_direction);
        }

        public CubeOperation()
        {
            axis = Vector3Int.up;
            direction = 0;
        }

        // Default constructor
        public CubeOperation (Vector3Int axis, int direction)
        {
            this.axis = axis;
            this.direction = direction;
        }

        // Overload so we can input an int value (for AI output)
        public CubeOperation(int axis, int direction)
        {
            switch(axis)
            {
                case 0:
                    this.axis = Vector3Int.up;
                    break;

                case 1:
                    this.axis = Vector3Int.down;
                    break;

                case 2:
                    this.axis = Vector3Int.left;
                    break;

                case 3:
                    this.axis = Vector3Int.right;
                    break;

                case 4:
                    this.axis = Vector3Int.back;
                    break;

                case 5:
                    this.axis = Vector3Int.forward;
                    break;

                default:
                    this.axis = Vector3Int.up;
                    break;
            }

            this.direction = direction;
        }
    }
}

