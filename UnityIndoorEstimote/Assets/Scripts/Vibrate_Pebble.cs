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
        text_status.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void TryInit()
    {
        text_status.color = Color.white;
        text_status.text += "Trying to init...\n";
        try
        {
            //instance of Koi plugin
            AndroidJavaObject androidObject;
            //Reference to Unity Player
            AndroidJavaObject unityObject;


            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass androidClass = new AndroidJavaClass("com.kennesaw.guitar.pebblelibrary.VibratePebble");
            androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

            text_status.text += "~~~ MadeObjectsAndClasses...";
            text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
            //androidObject.Call("VibrateWatch");
            androidObject.Call("InitWatch", unityObject);
        }
        catch(System.Exception e)
        {
            text_status.text += "" + e.Message + "\n";
        }
    }

    /// <summary>
    /// Call to send message to android studio to send vibration to pebble
    /// </summary>
    public void TryVibrate()
    {
        text_status.color = Color.white;
        text_status.text += "Trying to vibrate...";

        //instance of Koi plugin
        AndroidJavaObject androidObject;
        //Reference to Unity Player
        AndroidJavaObject unityObject;


        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass androidClass = new AndroidJavaClass ("com.kennesaw.guitar.pebblelibrary.VibratePebble");
        androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

        text_status.text += "MadeObjectsAndClasses...";
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
        //androidObject.Call("VibrateWatch");
        androidObject.Call("VibrateWatch", unityObject);
    }

    /// <summary>
    /// Called when pebble is vibrated
    /// </summary>
    public void VibrateSuccess(string message)
    {
        text_status.text += "++ Successfully vibrated!";
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
    }

    /// <summary>
    /// Called when pebble fails to vibrate
    /// </summary>
    public void VibrateFail(string error)
    {
        text_status.text += "\n -- Error: " + error;
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
    }
}
