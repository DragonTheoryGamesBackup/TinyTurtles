using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandClass : MonoBehaviour
{
    [SerializeField]
    public GameObject IslandGenerator;

    [SerializeField]
    GameObject[] staticNeighbors; //Unchanging list of all 6 neighbors
    public List<GameObject> neighbors; //list of neighbors that are still SeaNodes
    
    [SerializeField]
    int Q;//column
    [SerializeField]
    int R;//row

    //math for mapmaking
    static float radius = 1f;
    static float length = (radius * 2);
    static float WIDTH_MATH = Mathf.Sqrt(3) / 2;
    static float width = WIDTH_MATH * length;

    /// <summary>
    /// Step 1: Set Column and Row.
    /// Step 2: Set Island Generator.
    /// Step 3: Move this node to appropriate location in grid.
    /// </summary>
    /// <param name="q">Column</param>
    /// <param name="r">Row</param>
    /// <param name="obj">Island Generator</param>
    public void SetPosition(int q, int r, GameObject obj)
    {
        // Step 1: Set Column and Row.
        Q = q;
        R = r;
        // Step 2: Set Island Generator.
        IslandGenerator = obj;
        // Step 3: Move this node to appropriate location in grid.
        this.GetComponent<Transform>().position = new Vector3(width * (q + r / 2f), 0, (length * 0.75f) * r);
    }

    /// <summary>
    /// Step 1: Add +1 height on the Y axis for landNodes
    /// </summary>
    public void UpdatePosition()
    {
        // Step 1: Add +1 height on the Y axis for landNodes   
        this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, .5f, this.GetComponent<Transform>().position.z);
    }

    /// <summary>
    /// Step 1: Return this Nodes Location.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLocation()
    {
        return new Vector2(Q, R);
    }

    /// <summary>
    /// Step 1:  Set Arrays with Neighbors.
    /// </summary>
    /// <param name="newNeighbors">Array of Neighbors</param>
    public void SetNeighbors(GameObject[] newNeighbors)
    {
        // Step 1:  Set Arrays with Neighbors.
        neighbors = new List<GameObject>(newNeighbors);
        staticNeighbors = neighbors.ToArray();
    }

    /// <summary>
    /// Step 1: Get Random Tile from list of Neighbors
    /// </summary>
    /// <returns>Neighbor</returns>
    public GameObject GetNeighbor()
    {
        GameObject ret = neighbors[Random.Range(0, neighbors.Count - 1)];  //get random tile from neighbors.
        return ret;
    }

    /// <summary>
    /// Step 1: Iterate through every neighbor and call Remove Neighbor.
    /// </summary>
    public void RemoveFromNeighbors()
    {
        // Step 1: Iterate through every neighbor and call Remove Neighbor.
        foreach (GameObject n in staticNeighbors)
        {
            n.GetComponent<IslandClass>().RemoveNeighbor(this.gameObject);
        }
    }

    /// <summary>
    /// Remove explicit Neighbor.
    /// </summary>
    /// <param name="r">Neighbor to remove</param>
    public void RemoveNeighbor(GameObject r)
    {
        foreach (GameObject n in staticNeighbors)
        {
            if (n == r)
            {
                neighbors.Remove(n);
            }
        }
    }

    /// <summary>
    /// Step 1: Change node to Sea or Land Node.
    /// </summary>
    /// <param name="material">material to change node to</param>
    /// <param name="typeNode">tag to apply to node.</param>
    public void UpdateTileType(Material material, string typeNode)
    {
        //Step 1: Change node to Sea or Land Node.
        this.GetComponentInChildren<MeshRenderer>().material = material;
        this.GetComponentInChildren<Transform>().tag = typeNode;
    }
}