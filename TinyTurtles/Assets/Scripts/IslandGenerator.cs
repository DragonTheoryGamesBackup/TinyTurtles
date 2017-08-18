using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{

    [SerializeField]
    GameObject TerrainHexPrefab;
    int mapSize = 64;
    int LandSize = 32;
    [SerializeField]
    Material[] Materials;

    private GameObject[] allLandNodes;
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
                    new Vector3(0,0,0),
                    Quaternion.identity,
                    this.transform
                    );
                TerrainNode.GetComponent<IslandClass>().SetPosition(column, row);
                //Make the sea
                TerrainNode.GetComponent<IslandClass>().UpdateTileType(Materials[0], "SeaNode");
            }
        }
        //have all tiles find their neighbors
        //allNodes = GameObject.FindGameObjectsWithTag("SeaNode");
        //foreach (GameObject node in allNodes)
        //{
        //    node.GetComponent<IslandClass>().SetNeighbors();
        //}
        //Then make landTiles
        //GenerateLandMass();
    }

    private void GenerateLandMass()
    {
        allLandNodes = GameObject.FindGameObjectsWithTag("LandNode");
        while (allLandNodes.Length < LandSize)
        {
            GameObject targetLandNode = allLandNodes[Random.Range(0, allLandNodes.Length - 1)];  //get random tile from array.

            //targetLandNode.GetComponent<IslandClass>().GetNeigbor();
            //targetLandNode.GetComponent<IslandClass>().MakeLand(Materials[1]);

            GameObject.FindGameObjectsWithTag("LandNode");
        }
    }
}
