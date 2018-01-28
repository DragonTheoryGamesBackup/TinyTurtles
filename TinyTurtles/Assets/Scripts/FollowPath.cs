using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    public MovementPath MyPath = null;  //The path that the player will follow.
    public float speed = 1; //The speed at which the player moves.
    public float maxDitanceToGoal = .3f; //How close the player is to the point 
                                         //before it begins moving to the following point.
                                         //Keeps player from 'spinning' as it gets close to the target.

    [SerializeField] private IEnumerator<Transform> pointInPath; //The Transfom for the current point player is moving to.

	/// <summary>
    /// Step 1:  Finds teh next point to move to.
    /// </summary>
	public void SetReady () {
	
        pointInPath = MyPath.GetNextPathPoint();
        pointInPath.MoveNext();
	}
	
	/// <summary>
    /// Step 1: Cancel if player does not have a path.
    /// Step 2: Use Pathagoreum Theorum to find distance from next point and move to it. 
    /// </summary>
	void Update () {

        // Step 1: Cancel if player does not have a path.
        if (MyPath == null) { return; }

        // Step 2: Use Pathagoreum Theorum to find distance from next point and move to it.
        transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        var distanceSquared = (transform.position - pointInPath.Current.position).sqrMagnitude;
        if (distanceSquared < maxDitanceToGoal * maxDitanceToGoal)
        {
            pointInPath.MoveNext();
        }
	}
}