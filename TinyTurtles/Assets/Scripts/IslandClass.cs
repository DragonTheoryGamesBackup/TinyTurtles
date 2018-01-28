using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandClass : MonoBehaviour
{
    [SerializeField] GameObject IslandGenerator; //Holds IslandGenerator scripts.

    [SerializeField] GameObject[] staticNeighbors; //Unchanging list of all 6 neighbors
    public List<GameObject> neighbors; //list of neighbors that are still SeaNodes
    
    [SerializeField] int Q; //column
    [SerializeField] int R; //row

    //math for mapmaking
    static float radius = 1f;
    static float length = (radius * 2);
    static float WIDTH_MATH = Mathf.Sqrt(3) / 2;
    static float width = WIDTH_MATH * length;

    bool haspath = false; //tracks if a path has been placed on this.

    /// <summary>
    /// Step 1: Set Column and Row.
    /// Step 2: Set Island Generator.
    /// Step 3: Move this node to appropriate location in grid.
    /// </summary>
    /// <param name="q">Column</param>
    /// <param name="r">Row</param>
    /// <param name="obj">Island Generator</param>
    public void SetPosition(int q, int r, GameObject Island)
    {
        // Step 1: Set Column and Row.
        Q = q;
        R = r;
        // Step 2: Set Island Generator and GameManager.
        IslandGenerator = Island;
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

    /// <summary>
    /// Dtep 1: See if Tile already has a path.
    /// Step 2: Find random Path.
    /// Step 3: Rotate Path to its appropriate position.
    /// Step 4: Create Path.
    /// </summary>
    /// <param name="GameManager"></param>
    public void CreatePaths(GameManager GameManager)
    {
        // Dtep 1: See if Tile already has a path.
        if (haspath == true) { return; }

        List<int> allPoints = new List<int>();

        // Step 2: Find random Path.
        for (int i = 0; i < 12; i++)
        {
            allPoints.Add(i);
        }

        for (int i = 0; i < 6; i++)
        {
            int pointA = allPoints[0];
            int pointB = allPoints[Random.Range(1, allPoints.Count)];
            allPoints.Remove(pointA);
            allPoints.Remove(pointB);

            int diff = pointB - pointA;

            GameObject Path;

            // Step 3: Rotate Path to its appropriate position.
            float rot = 0;
            if (pointA == 2 || pointA == 3) { rot = -60; }
            else if (pointA == 4 || pointA == 5) { rot = -120; }
            else if (pointA == 6 || pointA == 7) { rot = -180; }
            else if (pointA == 8 || pointA == 9) { rot = -240; }
            else if (pointA == 10 || pointA == 11) { rot = -300; }
            //find path -- see if a is even or odd subtact a from b,
            if (pointA % 2 == 0)
            {
                diff = diff - 1;
                Path = GameManager.GetPath(diff);
            }
            else
            {
                diff = diff + 10;
                Path = GameManager.GetPath(diff);
            }

            // Step 4: Create Path.
            GameObject PlayerPath = (GameObject)
            Instantiate(
            Path,
            this.transform.position + new Vector3(0, .3f, 0),
            Quaternion.Euler(0f, rot, 0f),
            this.transform
            );
        }
        haspath = true; //set hasPath to true so another path can not be made on this tile.
    }
}