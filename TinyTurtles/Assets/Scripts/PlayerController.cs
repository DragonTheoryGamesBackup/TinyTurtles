using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController: MonoBehaviour {

    [SerializeField] GameManager GameManager; //Holds GameManager Script.
    public GameObject CurrentPhaseText; //Holds CurrentPhase UI.
    GameObject[] allStarts;  //tracks all the starting locations to iterate through
    Camera cam; //set the camera to send the player to so the cam can follow
    public GameObject CamLock; //Position Camera is to follow.
    [SerializeField] private GameObject NextTile; //Holds the Tile that the player is about to move on,
    GameObject TrailTilePrefab; //Holds the Prefab for the random trail.


    bool isStarting = true;  //tracks the setup part of the game where players select their starting location
    bool isGame = false; //tracks the main play of the game
    GameObject Startpoint; //used to track a random startpoint on the bord to be used in methods
    public Vector3 targetPosition; //track the various objects the player should be facing.
    bool findingPath = false; //Tracks if player is attempting to move.

    /// <summary>
    /// Step 1: Find the camer and send it the player.
    /// Step 2: Call the initial RandomStart and MoveAtStart methods. 
    /// </summary>
    void Start()
    {
        // Step 1: find the camera and send it the player.
        cam = Camera.main;
        cam.GetComponent<CameraFollow>().target = this.transform;
        cam.GetComponent<CameraFollow>().Loc = CamLock.transform;
        // Step 2: Call the initial RandomStart and MoveAtStart methods.
        TrailTilePrefab = GameManager.PathTilePrefab;
        RandomStart();
        MoveAtStart();
    }

    /// <summary>
    /// Step 1: Runs Various Methods every Frame.
    /// </summary>
    private void FixedUpdate()
    {
        Controls(); //Accepts Players input.
        transform.LookAt(targetPosition); //Ensures Player is always looking ahead.
        SeeTile(); 
    }

    /// <summary>
    /// Step 1: Identify GameManager
    /// </summary>
    /// <param name="GM">GameManager</param>
    public void SetGameManager(GameManager GM)
    {
        GameManager = GM;
    }

    /// <summary>
    /// Get all starts array.
    /// </summary>
    /// <param name="starts">Start Points</param>
    public void GetStarts(GameObject[] starts)
    {
        allStarts = starts;
    }

    /// <summary>
    /// Step 1: Allow player to choose their starting loation.
    /// Step 2: Allow Player to play their Trail.
    /// </summary>
    void Controls()
    {
        // Step 1: Allow player to choose their starting loation.
        if (isStarting)
        {
            if (Input.GetKeyDown(KeyCode.R)) { RandomStart(); MoveAtStart(); }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                isStarting = false;
                isGame = true;
            };
        }

        // Step 2: Allow Player to play their Trail.
        if (isGame)
        {
            CurrentPhaseText.GetComponent<UnityEngine.UI.Text>().text = "Place Trail.";
            if (Input.GetKeyDown(KeyCode.Return)) { SpawnTrail(); findingPath = true; }
        }
    }

    /// <summary>
    /// Step 1: Get a random start location.
    /// </summary>
    void RandomStart()
    {
        Startpoint = allStarts[Random.Range(0, allStarts.Length)];
    }

    /// <summary>
    /// Step 1: Move to selected Startpoint.
    /// </summary>
    void MoveAtStart()
    {
        CurrentPhaseText.GetComponent<UnityEngine.UI.Text>().text = "Choose starting Location";
        this.GetComponent<Transform>().position = Startpoint.GetComponent<Transform>().position;
        SetLookAtTarget(Startpoint.transform.parent);
    }

    /// <summary>
    /// Step 1: Ensure Player is always looking ahead.
    /// </summary>
    /// <param name="Target"></param>
    void SetLookAtTarget(Transform Target)
    {
        Transform LootAtTarget = Target;
        targetPosition = new Vector3(LootAtTarget.position.x, transform.position.y, LootAtTarget.position.z);
    }

    /// <summary>
    /// Step 1: Check for and tag tile that is ahead of the player.
    /// </summary>
    void SeeTile()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            if (hit.transform.tag == "LandNode")
            {
                NextTile = hit.transform.gameObject;
            }
        }
    }

    /// <summary>
    /// Step 1: calls CreatePaths from the Gamemanager.
    /// </summary>
    void SpawnTrail()
    {
        NextTile.GetComponent<IslandClass>().CreatePaths(GameManager);
    }

    /// <summary>
    /// Step 1: Ensure the playmode is currently set to moving on path.
    /// Step 2: If the player is in contact with a path, send that path to the FollowPath script.
    /// Step 3: Send the Player to the MovementPath script.
    /// Step 4: Stop the player from trying to move.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        // Step 1: Ensure the playmode is currently set to moving on path.
        if (findingPath == true)
        {
            // Step 2: If the player is in contact with a path, send that path tp the FollowPath script.
            if (col.transform.parent.transform.tag == "Path")
            {
                int dir = 0; //which side of the path the player is starting on.
                CurrentPhaseText.GetComponent<UnityEngine.UI.Text>().text = "Moving On Path";

                if (col.transform.name == "P0") { dir = 1; }
                if (col.transform.name == "P5") { dir = -1; }
                col.gameObject.GetComponentInParent<MovementPath>().movementDirection = dir;

                this.GetComponent<FollowPath>().MyPath = col.gameObject.GetComponentInParent<MovementPath>();
                this.GetComponent<FollowPath>().SetReady();

                // Step 3: Send the Player to the MovementPath script.
                col.gameObject.GetComponentInParent<MovementPath>().Player = this.gameObject;
                
                // Step 4: Stop the player from trying to move.
                findingPath = false;
            }
        }
    }
}