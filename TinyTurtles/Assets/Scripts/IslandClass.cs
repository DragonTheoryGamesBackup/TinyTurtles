using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandClass : MonoBehaviour
{
    GameObject IslandGenerator;

    [SerializeField]
    int Q;//column
    [SerializeField]
    int R;//row
    [SerializeField]
    int S;//sum

    //keeps neighbors in clockwise order
    GameObject[] Neighbors = new GameObject[6];

    [SerializeField]
    static GameObject[] AllNodes;

    //math for mapmaking
    static float radius = 1f;
    static float length = (radius * 2);
    static float WIDTH_MATH = Mathf.Sqrt(3) / 2;
    static float width = WIDTH_MATH * length;

    public int GetCoords(string coord)
    {
        int ret;
        if (coord == "Q") { ret = Q; }
        else if (coord == "R") { ret = R; }
        else if (coord == "S") { ret = S; }
        else ret = 0;
        return ret;
    }


    //return world space of this Hex
    //Column = q, Row = r
    public void SetPosition(int q, int r)
    {
        Q = q;
        R = r;
        S = -(q + r);
        this.GetComponent<Transform>().position=new Vector3(width * (q + r / 2f), 0, (length * 0.75f) * r);
    }

    public void SetNeighbors()
    {
        AllNodes = GameObject.FindGameObjectsWithTag("SeaNode");
        foreach (GameObject node in AllNodes)
        {
            Debug.Log(node);
            //int nodeQ = node.GetComponentsInParent<IslandClass>().
            //int nodeR = node.GetComponents<IslandClass>().R;
            //int nodeS = node.GetComponents<IslandClass>().S;
            //if (node.Q == Q && node.R == (R + 1)) { }
        }
    }

    public void UpdateTileType(Material material, string typeNode)
    {
        this.GetComponentInChildren<MeshRenderer>().material = material;
        this.GetComponentInChildren<Transform>().tag = typeNode;
    }
}
