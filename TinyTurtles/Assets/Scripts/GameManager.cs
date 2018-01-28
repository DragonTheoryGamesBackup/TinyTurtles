using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] public GameObject MainMenuPanel;  //Holds UI elements
    [SerializeField] public GameObject IslandGenerator; //Holds the Island Generator Script.
    [SerializeField] GameObject PlayerPrefab;  //Holds the player model and info.
    [SerializeField] public GameObject PathTilePrefab; //Holds info for the paths to be created.

    [SerializeField] GameObject[] allPaths;  //tracks every Path in the game players can use.
    [SerializeField] GameObject CurrentPhaseText; //UI element to tell player what their current phase is.


    private GameObject[] allStarts;  //Holds a list of every starting point.
    private bool isGame = true; //Tracks if the game is currently playable.

    /// <summary>
    /// Step 1: Calls the start of the game.
    /// </summary>
    void Start ()
    {
        if (!isGame) { MainMenuPanel.GetComponentInChildren<MenuController>().SwitchPanel(MainMenuPanel); }
        if (isGame) { StartGame(); }
    }

    /// <summary>
    /// Step 1: Iterates through every phase of the starting game sequence to prepare the game for play.
    /// </summary>
    void StartGame()
    {
        CurrentPhaseText.GetComponent<UnityEngine.UI.Text>().text = "Generating Map";
        IslandGenerator.GetComponent<IslandGenerator>().CallMethod("GenerateMap");
        IslandGenerator.GetComponent<IslandGenerator>().CallMethod("SetStarts");
        allStarts = GameObject.FindGameObjectsWithTag("Start");
        CreatePlayer();
    }

    /// <summary>
    /// Step 1: Creates the player and passes variables to them.
    /// </summary>
    void CreatePlayer()
    {
        GameObject Player = (GameObject)
            Instantiate(
            PlayerPrefab,
            new Vector3(0, 0, 0),
            Quaternion.identity,
            this.transform
            );
        Player.GetComponent<PlayerController>().GetStarts(allStarts);
        Player.GetComponent<PlayerController>().SetGameManager(this);
        Player.GetComponent<PlayerController>().CurrentPhaseText = CurrentPhaseText;
    }

    /// <summary>
    /// Finds the Path the player is looking for from among all the paths currently available.
    /// </summary>
    /// <param name="path">CUrrent Path game is looking for</param>
    /// <returns></returns>
    public GameObject GetPath(int path)
    {
        return allPaths[path];
    }
}