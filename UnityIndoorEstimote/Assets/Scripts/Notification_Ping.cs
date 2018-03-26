using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Ping : Notification {

	// Use this for initialization
	public override void Start () {
		
	}

    public override float Notify(float distance, float rotation)
    {
        Debug.Log("PINGING");
        return 2;
        //DO ANYTHING USING AMOUNT TO NOTIFY

        //return how long to wait until next notification
        return 1;
    }
}
