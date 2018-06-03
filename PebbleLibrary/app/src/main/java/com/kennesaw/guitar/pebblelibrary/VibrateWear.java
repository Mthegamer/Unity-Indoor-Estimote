package com.kennesaw.guitar.pebblelibrary;

/**
 * Created by guitar on 5/1/2018.
 */

import android.app.Activity;
import android.content.Context;
import android.support.annotation.NonNull;

import com.google.android.gms.tasks.Task;
import com.google.android.gms.wearable.CapabilityClient;
import com.google.android.gms.wearable.CapabilityInfo;
import com.google.android.gms.wearable.MessageClient;
import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.Wearable;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.util.Set;

import com.google.android.gms.tasks.Tasks;
import java.util.concurrent.ExecutionException;

public class VibrateWear extends UnityPlayerActivity implements
        MessageClient.OnMessageReceivedListener{

    private static VibrateWear m_instance = new VibrateWear();
    public static VibrateWear instance() {
        if(m_instance == null)
            m_instance = new VibrateWear();
        return m_instance;
    }

    //The name of the GameObject in Unity's Scene which is used to communicate with this Plugin
    private static final String UNITY_GAMEOBJECT = "Vibrate";
    private static final String IS_PHONE = "is_phone";
    private static final String IS_WATCH = "is_watch";
    public static final String IS_WATCH_TRANSCRIPTION_MESSAGE_PATH= "/is_watch_transcription";
    public static final String IS_PHONE_TRANSCRIPTION_MESSAGE_PATH= "/is_phone_transcription";
    private Context unityActivity;


    public VibrateWear()
    {
        m_instance = this;
    }

    private void InitWatch(Context context) throws ExecutionException, InterruptedException {
        unityActivity = context;
        sendMessageToUnity("VibrateFail", "Trying to initialize watch...");

        CapabilityInfo capabilityInfo = Tasks.await(
                Wearable.getCapabilityClient(context).getCapability(
                        IS_PHONE, CapabilityClient.FILTER_REACHABLE));
        // capabilityInfo has the reachable nodes with the transcription capability
        updateTranscriptionCapability(capabilityInfo);

    }

    private String transcriptionNodeId = null;
    private void updateTranscriptionCapability(CapabilityInfo capabilityInfo) {
        Set<Node> connectedNodes = capabilityInfo.getNodes();

        transcriptionNodeId = pickBestNodeId(connectedNodes);
    }

    private String pickBestNodeId(Set<Node> nodes) {
        String bestNodeId = null;
        // Find a nearby node or pick one arbitrarily
        for (Node node : nodes) {
            if (node.isNearby()) {
                return node.getId();
            }
            bestNodeId = node.getId();
        }
        return bestNodeId;
    }

    private void sendTranscription(byte[] vibeData) {
        if (transcriptionNodeId != null) {
            Task<Integer> sendTask =
                    Wearable.getMessageClient(unityActivity).sendMessage(
                            transcriptionNodeId, IS_WATCH_TRANSCRIPTION_MESSAGE_PATH, vibeData);
            // You can add success and/or failure listeners,
            // Or you can call Tasks.await() and catch ExecutionException
            //sendTask.addOnSuccessListener();
            //sendTask.addOnFailureListener();
        } else {
            // Unable to retrieve node with transcription capability
            sendMessageToUnity("VibrateFail", "Watch is not connected...");
        }
    }

    public void VibrateWatch(Context context, int _numTimes, int _timeOn, int _timeOff)
    {
        unityActivity = context;
        VibrateWear.sendMessageToUnity("VibrateFail", "Trying to vibrate on watch...");

        byte numTimes = (byte)_numTimes;
        byte timeOn = nextExponentOf2(_timeOn);
        byte timeOff = nextExponentOf2(_timeOff);

        byte[] array = new byte[3];
        array[0] = numTimes;
        array[1] = timeOn;
        array[2] = timeOff;
        VibrateWear.sendMessageToUnity("VibrateFail", "Got... " + array[0] + ", " + array[1] + ", " + array[2]);
        sendTranscription(array);
    }

    static byte nextExponentOf2(int n)
    {
        int count = 0;

        while(n != 0)
        {
            n >>= 1;
            count += 1;
        }

        return (byte)count;
    }

    /**
     * Send message to Unity's GameObject (named as UNITY_GAMEOBJECT)
     * @param method name of the method in GameObject's script
     * @param message the actual message
     */
    public static void sendMessageToUnity(String method, String message){
        UnityPlayer.UnitySendMessage(UNITY_GAMEOBJECT, method, message);
    }


    @Override
    protected void onResume() {
        super.onResume();
        Wearable.getMessageClient(this).addListener(this);
    }

    @Override
    protected void onPause() {
        super.onPause();
        Wearable.getMessageClient(this).removeListener(this);
    }

    @Override
    public void onMessageReceived(@NonNull MessageEvent messageEvent) {
        if(messageEvent.getPath().equals(IS_WATCH_TRANSCRIPTION_MESSAGE_PATH))
        {
            sendMessageToUnity("VibrateFail", "Received message from watch... " + messageEvent.getData().toString());
        }
    }
}
