using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPath : MonoBehaviour {

    public int movementDirection; // Tells the player to move up or down the path. 1 or -1.
    public int movingTo; //The next point in the path that the Player is moving torward.
    bool firstMove = true; //REDACTED
    public Transform[] PathSequence; //Tracks the Path the Player travels on.
    public GameObject Player; //Tacks the Player.

    /// <summary>
    /// Step 1: Draws a line so that the Path is visible.
    /// </summary>
    public void OnDrawGizmos()
    {
        for (int i = 1; i < PathSequence.Length; i++)
        {
            Gizmos.DrawLine(PathSequence[i - 1].position, PathSequence[i].position);
        }
    }

    /// <summary>
    /// Step 1: If the player is at the end of the path then the player stops following it.
    /// Step 2: Return next point in the path the player needs to move to.
    /// </summary>
    /// <returns>Transform of next Point.</returns>
    public IEnumerator<Transform> GetNextPathPoint()
    {
        if (PathSequence == null)
        {
            yield break;
        }
        while (true)
        {
            // Step 1: If the player is at the end of the path then the player stops following it.
            if (movingTo < 0 || movingTo > PathSequence.Length)
            {
                Player.GetComponent<FollowPath>().MyPath = null;
                yield return null;
            }

            // Step 2: Return next point in the path the player needs to move to.
            else
            {
                if (movementDirection == -1 && firstMove == true) { movingTo = 4; firstMove = false; }

                yield return PathSequence[movingTo];
                
                movingTo = movingTo + movementDirection;
                Player.GetComponent<PlayerController>().targetPosition = PathSequence[movingTo].transform.position;    
            }
        }
    }
}