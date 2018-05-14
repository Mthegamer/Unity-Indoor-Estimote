using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Vibrate_Pebble : MonoBehaviour {
    static Vibrate_Pebble instance;
    public static void VIBRATE() { if (instance) instance.TryVibrate(); }
    public Text text_status;    //show the status on screen in top rights
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Init()
    {
        //Initialize Vibrator
        //Initialize Watch
    }


    /// <summary>
    /// Call to send message to android studio to send vibration to pebble
    /// </summary>
    public void TryVibrate()
    {
        text_status.color = Color.white;
        text_status.text = "Trying to vibrate Pebble...";

        //instance of Koi plugin
        AndroidJavaObject androidObject;
        //Reference to Unity Player
        AndroidJavaObject unityObject;


        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass androidClass = new AndroidJavaClass ("com.example.guitar.vibratewatchpebble.VibratePebble");
        androidObject = androidClass.GetStatic<AndroidJavaObject>("instance");

        text_status.text = "MadeObjectsAndClasses...";
        //androidObject.Call("VibrateWatch");
        androidObject.Call("launchAndroidActivity", unityObject);
    }

    /// <summary>
    /// Called when pebble is vibrated
    /// </summary>
    public void VibrateSuccess(string message)
    {
        text_status.color = Color.green;
        text_status.text = "Successfully vibrated!";
    }

    /// <summary>
    /// Called when pebble fails to vibrate
    /// </summary>
    public void VibrateFail(string error)
    {
        text_status.color = Color.red;
        text_status.text = "Error: " + error;
    }
}
