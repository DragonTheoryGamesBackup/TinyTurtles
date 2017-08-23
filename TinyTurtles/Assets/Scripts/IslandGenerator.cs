using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject TerrainHexPrefab;
    //don't set higher than 32 until we speek up the build.
    static int landSize = 16;
    int mapSize = (landSize * 2) + 1;
    [SerializeField]
    Material[] Materials;

    private List<GameObject> allFullNodes = new List<GameObject>();
    //private GameObject[] allLandNodes;
    private List<GameObject> allLandNodes = new List<GameObject>();
    private GameObject[] allNodes;

    private void Start()
    {
        GenerateMap();
    }

    //Generates the water grid with single island tile at its center.
    private void GenerateMap()
    {
        for (int column = 0; column < mapSize; column++)
        {
            for (int row = 0; row < mapSize; row++)
            {
                //creates all the island nodes.

                GameObject TerrainNode = (GameObject)
                    Instantiate(
                    TerrainHexPrefab,
                    new Vector3(0, 0, 0),
                    Quaternion.identity,
                    this.transform
                    );
                //Name our babies
                TerrainNode.name = string.Format("{0},{1}", column, row);
                TerrainNode.GetComponent<IslandClass>().SetPosition(column, row);
                //Make the sea
                TerrainNode.GetComponent<IslandClass>().UpdateTileType(Materials[0], "SeaNode");
                //!!!!!set debug coords.  Remove later!!!!!.
                TerrainNode.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
                //TerrainNode.GetComponentInChildren<TextMesh>().text = ("");
            }
        }
        //have all tiles find their neighbors
        allNodes = GameObject.FindGameObjectsWithTag("SeaNode");
        foreach (GameObject node in allNodes)
        {
            Vector3 location = node.GetComponent<IslandClass>().GetLocation();
            GameObject[] tempNeighbors = new GameObject[6];
            for (int i = 0; i < 6; i++)
            {
                tempNeighbors[i] = FindNeighbor(location.x, location.y, i);
            }
            node.GetComponent<IslandClass>().SetNeighbors(tempNeighbors);
        }
        //Then make landTiles
        GenerateLandMass();
    }

    private GameObject FindNeighbor(float q, float r, int side)
    {
        GameObject ret = null;
        switch (side)
        {
            case 0: r += 1; break;
            case 1: q += 1; break;
            case 2: q += 1; r -= 1; break;
            case 3: r -= 1; break;
            case 4: q -= 1; break;
            case 5: q -= 1; r += 1; break;
        }
        Vector2 newLocation = new Vector2(q, r);
        foreach (GameObject node in allNodes)
        {
            if (node.GetComponent<IslandClass>().GetLocation() == newLocation)
            {
                ret = node;
                break;
            }
        }
        return ret;
    }

    private void GenerateLandMass()
    {
        GameObject targetNode = null;
        if (allLandNodes.Count < 1)
        {
            int intCenterNode = mapSize / 2;
            foreach (GameObject node in allNodes)
            {
                if (node.GetComponent<IslandClass>().GetLocation() == new Vector2(intCenterNode, intCenterNode))
                {
                    targetNode = node;
                    targetNode.GetComponent<IslandClass>().UpdateTileType(Materials[1], "LandNode");
                    allLandNodes.Add(targetNode);
                    break;
                }
            }
        }
        int count = 0;
        while (landSize > (allLandNodes.Count + allFullNodes.Count) && count < 150)
        {
            targetNode = allLandNodes[Random.Range(0, allLandNodes.Count - 1)];  //get random tile from array.
            if (targetNode.GetComponent<IslandClass>().neighbors.Count > 0)
            {
                GameObject targetNeighbor = targetNode.GetComponent<IslandClass>().GetNeighbor(); //get that tiles neighbor.
                if (targetNeighbor.GetComponent<Transform>().tag != "LandNode") //if that tile is not already land
                {
                    allLandNodes.Add(targetNeighbor); //add it to the list
                    targetNeighbor.GetComponent<IslandClass>().UpdateTileType(Materials[1], "LandNode"); // and make it land
                }
                else { targetNode.GetComponent<IslandClass>().neighbors.Remove(targetNeighbor); }
            }
            else { MoveLand(targetNode); }
            count++;
        }
        Debug.Log(count);
        Debug.Log(allFullNodes.Count);
        Debug.Log(allFullNodes.Count + allLandNodes.Count);
    }

    public void MoveLand(GameObject fullLand)
    {
        allFullNodes.Add(fullLand);
        allLandNodes.Remove(fullLand);
    }
}