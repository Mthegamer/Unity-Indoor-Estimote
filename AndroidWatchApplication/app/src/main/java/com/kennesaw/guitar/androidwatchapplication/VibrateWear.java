package com.kennesaw.guitar.androidwatchapplication;

import android.content.Context;
import android.os.Bundle;
import android.os.Parcel;
import android.os.VibrationEffect;
import android.os.Vibrator;
import android.support.annotation.NonNull;
import android.support.wearable.activity.WearableActivity;
import android.view.View;
import android.widget.TextView;

import com.google.android.gms.wearable.MessageClient;
import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.Wearable;

import java.util.ArrayList;

public class VibrateWear extends WearableActivity implements
        MessageClient.OnMessageReceivedListener{

    private TextView mTextView;
    private static final String IS_PHONE = "is_phone";
    private static final String IS_WATCH = "is_watch";
    private static final String IS_WATCH_TRANSCRIPTION_MESSAGE_PATH= "/is_watch_transcription";
    private static final String IS_PHONE_TRANSCRIPTION_MESSAGE_PATH= "/is_phone_transcription";

    private static Vibrator vibrator;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_vibrate_wear);

        mTextView = (TextView) findViewById(R.id.text);
        // Enables Always-on
        setAmbientEnabled();

        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
        //VibrateWatch((byte)3, (byte)100, (byte)100);
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

    public void onClick(View v) {
        //VibrateWatch((byte)3, (byte)100, (byte)100);
    }

    void VibrateWatch(int times, int timeOn, int timeOff)
    {
        try {
            if (vibrator != null) {
                long[] vibrationPattern = new long[(times * 2) + 1];
                vibrationPattern[0] = 0;
                for(int i = 0; i < times; i++)
                {
                    vibrationPattern[(i*2) + 1] = timeOn;
                    vibrationPattern[(i*2) + 2] = timeOff;
                }
                VibrationEffect vibe = VibrationEffect.createWaveform(vibrationPattern, -1);
                vibrator.vibrate(vibe);
            }
        }
        catch(Exception e)
        {
            mTextView.setText(e.getMessage());
        }
    }

    @Override
    public void onMessageReceived(@NonNull MessageEvent messageEvent) {
        try {
            byte[] array = messageEvent.getData();
            int numTimes = array[0];
            int timeOn = 1 << (array[1]-1);
            int timeOff = 1 << (array[2]-1);
            VibrateWatch(numTimes, timeOn, timeOff);
            mTextView.setText("" + numTimes + ", " + timeOn + ", " + timeOff);
        }
        catch(Exception e)
        {
            mTextView.setText(e.getMessage());
        }

        /*if(messageEvent.getPath().equals(IS_PHONE_TRANSCRIPTION_MESSAGE_PATH))
        {
            byte[] array = messageEvent.getData();
            VibrateWatch(array);
        }*/
    }
}
