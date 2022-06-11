using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDebug : MonoBehaviour
{

    public Text text;

    // Start is called before the first frame update
    void OnMouseEnter()
    {

        Dictionary<Vector3Int, Cube.Logic.CubePiece> pieces = GetComponentInParent<Cube.Logic.CubeLogic>().current_state.pieces;

        foreach (Vector3Int key in pieces.Keys)
        {

            if (transform == pieces[key].transform)
            {

                text.text = key + " " + transform.name;

            }
        }
    }
}
