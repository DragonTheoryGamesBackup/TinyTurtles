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
    [SerializeField]
    public List<GameObject> neighbors;
    //GameObject[] neighbors = new GameObject[6];

    //math for mapmaking
    static float radius = 1f;
    static float length = (radius * 2);
    static float WIDTH_MATH = Mathf.Sqrt(3) / 2;
    static float width = WIDTH_MATH * length;

    public Vector2 GetLocation()
    {
        return new Vector2(Q, R);
    }

    public void SetNeighbors(GameObject[] newNeighbors)
    {
        neighbors = new List<GameObject>(newNeighbors);
    }

    public GameObject GetNeighbor()
    {
        GameObject ret = neighbors[Random.Range(0, neighbors.Count - 1)];  //get random tile from neighbors.
        neighbors.Remove(ret);
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

    public void UpdateTileType(Material material, string typeNode)
    {
        this.GetComponentInChildren<MeshRenderer>().material = material;
        this.GetComponentInChildren<Transform>().tag = typeNode;
    }
}
