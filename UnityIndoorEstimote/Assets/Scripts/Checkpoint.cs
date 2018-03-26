using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Used to tell the user where the need to go
/// The transform position is where the current checkpoint is
/// When the user gets close to the checkpoint, it is moved in WaypointManager
/// </summary>
public class Checkpoint : MonoBehaviour {
    //use these to vibrate watch to tell user how close they are
    float distanceOffset;       //change delay between Vibes
    float rotationOffset;       //change Vibe

    public float radius;        //how close the player needs to be to go to the next checkpoint
    float radiusSquared;

    [Header("Notification Options")]
    public Notification notification_vibrate;
    public Notification notification_ping;
    public Notification notification_voice;
    
	// Use this for initialization
	void Start () {
        radiusSquared = radius * radius;
        //start coroutines to send notifications at intervals
        StartCoroutine("Check_Notification", notification_vibrate);
        StartCoroutine("Check_Notification", notification_ping);
        StartCoroutine("Check_Notification", notification_voice);
    }


    Vector3 temp;
    // Called once a frame
    private void Update()
    {
        //get distance from player and their rotation compared to where it should be
        temp = transform.position - UserAvatar.user_position;
        distanceOffset = temp.magnitude;
        rotationOffset = Vector3.Dot(temp, UserAvatar.user_forward);

        //if close to the checkpoint, move the checkpoint
        if (temp.sqrMagnitude < radiusSquared)
        {
            WaypointManager.ReachedCheckpoint();
        }
    }

    // Called when object destroyed
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //check the player and which notificaiton options are active and send a message
    IEnumerator Check_Notification(Notification notif)
    {
        while(true)
        {
            //if the player is in the room && notification is toggled on
            while(notif.will_notify.isOn)
            {
                yield return new WaitForSecondsRealtime(notif.Notify(distanceOffset, rotationOffset));
            }

            //wait until next frame
            yield return null;
        }
    }
}