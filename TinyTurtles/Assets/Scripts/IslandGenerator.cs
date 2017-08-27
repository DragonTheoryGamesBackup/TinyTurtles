using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject TerrainHexPrefab;
    [SerializeField]
    Material[] Materials;

    static int landSize = 256; //sets the size of the landmass
    static int sizeFloor = (int)Mathf.Floor(Mathf.Log10(landSize * landSize) + 1) * 2;     //wheatley number to keep the mapsize under control
    static int mapSize = (landSize / sizeFloor) + 16; //and sets how many total tiles there will be
    
    private GameObject[] allNodes; //tracks every node.
    private List<GameObject> allBeachNodes = new List<GameObject>(); //tracks of every land node that can be expanded
    private List<GameObject> allLandLockedNodes = new List<GameObject>(); //tracks every land node that can not be expanded  


    private void Start()
    {
        GenerateMap();
    }

    /// <summary>
    /// Step 1: Generate water grid.
    /// Step 2: Have all tiles find their Neighbors.
    /// </summary>
    private void GenerateMap()
    {
        //Step 1: Generate water grid.
        for (int column = 0; column < mapSize; column++)
        {
            for (int row = 0; row < mapSize; row++)
            {
                //creates all the island babies
                GameObject TerrainNode = (GameObject)
                    Instantiate(
                    TerrainHexPrefab,
                    new Vector3(0, 0, 0),
                    Quaternion.identity,
                    this.transform
                    );
                TerrainNode.name = string.Format("{0},{1}", column, row); //Name our babies
                TerrainNode.GetComponent<IslandClass>().SetPosition(column, row, this.gameObject); //place them in there new home
                TerrainNode.GetComponent<IslandClass>().UpdateTileType(Materials[0], "SeaNode"); //Make them sea
                
                //next two lines are debug tools for setting position text on each node.
                //TerrainNode.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
                TerrainNode.GetComponentInChildren<TextMesh>().text = ("");
            }
        }
        //Step 2: Have all tiles find their Neighbors.
        allNodes = GameObject.FindGameObjectsWithTag("SeaNode");
        foreach (GameObject node in allNodes)
        {
            Vector3 location = node.GetComponent<IslandClass>().GetLocation();
            GameObject[] Neighbors = new GameObject[6];
            for (int i = 0; i < 6; i++)
            {
                Neighbors[i] = FindNeighbor(location.x, location.y, i);
            }
            node.GetComponent<IslandClass>().SetNeighbors(Neighbors);
        }
        //Time to make the Land
        GenerateLandMass();
    }

    /// <summary>
    /// Step 1: Identify Neighbors position.
    /// Step 2: Locate matching Neighbor.
    /// </summary>
    /// <param name="q">Column</param>
    /// <param name="r">Row</param>
    /// <param name="side">Neighbor To Return Location</param>
    /// <returns>Neighbors Position</returns>
    private GameObject FindNeighbor(float q, float r, int side)
    {
        GameObject ret = null; //return var
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

    /// <summary>
    /// Step 1: Create the first Land Node.
    /// Step 2: Create Additional Land Nodes until Land Size is reached.
    /// </summary>
    private void GenerateLandMass()
    {
        //Step 1: Create teh first Land Node.
        GameObject targetNode = null;
        if (allBeachNodes.Count < 1)
        {
            int intCenterNode = mapSize / 2;
            foreach (GameObject node in allNodes)
            {
                if (node.GetComponent<IslandClass>().GetLocation() == new Vector2(intCenterNode, intCenterNode))
                {
                    MakeLand(node);
                    break;
                }
            }
        }

        //Step 2: Create Additional Land Nodes until Land Size is reached.
        while ((allBeachNodes.Count + allLandLockedNodes.Count) < landSize)
        {
            targetNode = allBeachNodes[Random.Range(0, allBeachNodes.Count - 1)];  //get random tile from array.
            if (targetNode.GetComponent<IslandClass>().neighbors.Count == 0)
            {
                MoveLand(targetNode);
            }
            else
            {
                targetNode = targetNode.GetComponent<IslandClass>().GetNeighbor(); //get that tiles neighbor.
                if (targetNode.GetComponent<Transform>().tag != "LandNode") //if that tile is not already land
                {
                    MakeLand(targetNode);
                }
            }
        }
    }

    /// <summary>
    /// Step 1: Change Sea Node into Land Node.
    /// Step 2: Remove Node from their Neighbors Neighbors.
    /// </summary>
    /// <param name="targetNode">Node to be turned into Land.</param>
    private void MakeLand(GameObject targetNode)
    {
        // Step 1: Change Sea Node into Land Node.
        allBeachNodes.Add(targetNode); //add it to the list
        targetNode.GetComponent<IslandClass>().UpdateTileType(Materials[1], "LandNode"); // and make it land
        // Step 2: Remove Node from their Neighbors Neighbors.
        targetNode.GetComponent<IslandClass>().RemoveFromNeighbors(); //Let its neighbors know
    }

    /// <summary>
    /// Step 1: Remove Node from allBeachNodes and add it to allLandLockedNodes
    /// </summary>
    /// <param name="fullLand">Node to be moved.</param>
    private void MoveLand(GameObject fullLand)
    {
        allLandLockedNodes.Add(fullLand);
        allBeachNodes.Remove(fullLand);
    }
}