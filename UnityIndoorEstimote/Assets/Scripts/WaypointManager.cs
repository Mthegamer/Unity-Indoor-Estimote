using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour {
    static WaypointManager instance;
    
    public float checkpointDistance;        //How far the checkpoint should be from last one
    public Waypoint[] waypointPath;         //Holds the path the user needs to walk
    int curWaypointIndex;
    Waypoint curWaypoint;                  //Where the user is trying to head to
    public Checkpoint checkpoint;           //Where the next checkpoint is
	// Use this for initialization
	void Start () {
        instance = this;
        curWaypointIndex = 0;
        curWaypoint = waypointPath[0];

        //hide everything but the first waypoint
        waypointPath[0].Show();
        for(int i = 1; i < waypointPath.Length; i++)
        {
            waypointPath[i].Hide();
        }

        //move the checkpoint to the first waypoint
        checkpoint.transform.position = curWaypoint.transform.position;
        currentCheckpointPosition = checkpoint.transform.position;
	}

    Vector3 currentCheckpointPosition, checkpointToWaypointDisplacement;
    public void MoveCheckpoint()
    {
        checkpointToWaypointDisplacement = curWaypoint.transform.position - currentCheckpointPosition;
        
        //if the user is right on top of the current waypoint
        if (checkpointToWaypointDisplacement.sqrMagnitude < .1f)
        {
            //moving to next checkpoint
            curWaypoint.Hide();  //hide the current waypoint
            curWaypointIndex++;
            if (curWaypointIndex >= waypointPath.Length)
                curWaypointIndex = 0;
            curWaypoint = waypointPath[curWaypointIndex];
            curWaypoint.Show();  //show the next waypoint

            //update the distance of checkpoint -> waypoint
            checkpointToWaypointDisplacement = curWaypoint.transform.position - currentCheckpointPosition;
        }
        
        //if the user is close to next waypoint (less than next checkpointDistance)
        if (checkpointToWaypointDisplacement.magnitude < checkpointDistance)
        {
            //move the checkpoint to the next waypoint
            checkpoint.transform.position = curWaypoint.transform.position;
        }
        //if the user is far from the next waypoint (more than checkpointDistance)
        else
        {
            //move the checkpoint in the direction of the next waypoint

            //get the normalized vector towards next waypoint
            Vector3 temp = curWaypoint.transform.position - checkpoint.transform.position;
            temp.y = 0;
            temp.Normalize();

            //scale by checkPointDistance
            temp *= checkpointDistance;
            checkpoint.transform.position = currentCheckpointPosition + temp;
        }

        //update the current checkpoint position
        currentCheckpointPosition = checkpoint.transform.position;
    }

    public void SetCheckpointDistance(float newDist)
    {
        checkpointDistance = newDist;
    }

    public static void ReachedCheckpoint()
    {
        instance.MoveCheckpoint();
    }
}
