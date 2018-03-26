using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Vibrate : Notification {

    //vibes to represent the different rotations
    public Vibe[] vibes;

    // Use this for initialization
    public override void Start()
    {

    }

    public override float Notify(float distance, float rotation)
    {
        Debug.Log("VIBRATING");
        //return 3;
        //stop any previous vibrations
        StopAllCoroutines();

        //player facing wrong way
        if (rotation < 0) {
            StartCoroutine("Vibrate", vibes[0]);
        }
        //level 1
        else if (rotation < .2f) {
            StartCoroutine("Vibrate", vibes[1]);
        }
        //level 2
        else if (rotation < .7f) {
            StartCoroutine("Vibrate", vibes[2]);
        }
        //level 3
        else {
            StartCoroutine("Vibrate", vibes[3]);
        }


        //return how long to wait until next notification
        return 3;
    }

    /// <summary>
    /// Vibrate the phone according to the vibe
    /// </summary>
    /// <param name="vibe">
    /// Struct holding the vibration pattern
    /// </param>
    /// <returns></returns>
    IEnumerator Vibrate(Vibe vibe)
    {
        yield return null;
        for (int i = 0; i < vibe.numTimes; i++)
        {
            Vibration.Vibrate(10);
            yield return new WaitForSecondsRealtime(vibe.delay);
        }
    }
}

/// <summary>
/// used to hold the vibration pattern ("Vibe")
/// </summary>
[System.Serializable]
public struct Vibe
{
    //how long to delay between vibrations
    public float delay;
    public int numTimes;
    public Vibe(float _delay, int _times)
    {
        delay = _delay;
        numTimes = _times;
    }
}