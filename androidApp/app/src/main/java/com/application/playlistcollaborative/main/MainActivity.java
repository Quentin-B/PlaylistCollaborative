package com.application.playlistcollaborative.main;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.application.playlistcollaborative.R;
import com.application.playlistcollaborative.model.SocketSingleton;

import org.json.JSONException;
import org.json.JSONObject;


public class MainActivity extends Activity {

    private TextView tvResponse;
    private Button btSend;
    private EditText etMessage;

    private SocketSingleton socket;
    private BroadcastReceiver receiver;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        socket = SocketSingleton.get(getBaseContext());


        tvResponse = (TextView) findViewById(R.id.tvResponse);

        btSend = (Button) findViewById(R.id.btSend);
        btSend.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                // Perform action on click
                JSONObject jso = new JSONObject();
                try {
                    jso.put("msg1",etMessage.getText());
                    jso.put("msg2","text static");
                } catch (JSONException e) {
                    e.printStackTrace();
                }

                // remplacer test par les events ecoute par le server
                socket.getSocket().emit("test", jso);

            }
        });

        etMessage = (EditText) findViewById(R.id.etMessage);

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }
}
