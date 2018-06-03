package com.kennesaw.guitar.pebblelibrary;

/**
 * Created by guitar on 5/1/2018.
 */

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Vibrator;

import com.getpebble.android.kit.PebbleKit;
import com.getpebble.android.kit.util.PebbleDictionary;
import com.unity3d.player.UnityPlayerActivity;

import java.util.ArrayList;
import java.util.Random;
import java.util.Timer;
import java.util.UUID;

public class VibratePebble extends UnityPlayerActivity{

    //set the keys for the different types of messages that can be sent (all unique)
    private final int AppKeyNumTimes = 0;
    private final int AppKeyTimeOn = 1;
    private final int AppKeyTimeOff = 2;
    //UUID of app (found in the cloud pebble development)
    private final UUID AppUUID = UUID.fromString("7608068c-39d3-4c70-8acb-0728360fa786");



    private static VibratePebble m_instance = new VibratePebble();
    public static VibratePebble instance() {
        if(m_instance == null)
            m_instance = new VibratePebble();
        return m_instance;
    }
    //The name of the GameObject in Unity's Scene which is used to communicate with this Plugin
    private static final String UNITY_GAMEOBJECT = "Vibrate";

    private Context unityActivity;


    public VibratePebble()
    {
        m_instance = this;
    }

    public void InitWatch(Context context)
    {
        unityActivity = context;
        sendMessageToUnity("VibrateFail", "Trying to initialize watch...");
        boolean connected = PebbleKit.isWatchConnected(unityActivity);
        if(connected) {
            VibratePebble.sendMessageToUnity("VibrateFail", "Watch is connected...");
            PebbleKit.startAppOnPebble(unityActivity, AppUUID);
            return;
        }
        VibratePebble.sendMessageToUnity("VibrateFail", "Watch is not connected...");
    }

    public void VibrateWatch(Context context, int numTimes, int timeOn, int timeOff)
    {
        unityActivity = context;
        VibratePebble.sendMessageToUnity("VibrateFail", "Trying to vibrate on watch...");
        //if the watch is connected, we will vibrate that also
        boolean connected = PebbleKit.isWatchConnected(unityActivity);
        if(connected) {
            VibratePebble.sendMessageToUnity("VibrateFail", "Connected, sending message to vibrate...");
            //make a dictionary to store values we are sending
            PebbleDictionary dict = new PebbleDictionary();
            //add the values with their associated keys
            dict.addInt32(AppKeyNumTimes, numTimes);
            dict.addInt32(AppKeyTimeOn, timeOn);
            dict.addInt32(AppKeyTimeOff, timeOff);

            //send the dictionary to watch using the UUID
            PebbleKit.sendDataToPebble(unityActivity, AppUUID, dict);
            VibratePebble.sendMessageToUnity("VibrateSuccess", "Successfully sent message...");
            return;
        }
        VibratePebble.sendMessageToUnity("VibrateFail", "Watch is not connected...");
    }

    /**
     * Send message to Unity's GameObject (named as UNITY_GAMEOBJECT)
     * @param method name of the method in GameObject's script
     * @param message the actual message
     */
    public static void sendMessageToUnity(String method, String message){
        UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, method, message);
    }
}
