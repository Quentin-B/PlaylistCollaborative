package com.application.playlistcollaborative.model;

import android.content.Context;
import android.content.Intent;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.net.MalformedURLException;
import java.net.URL;

import io.socket.IOAcknowledge;
import io.socket.IOCallback;
import io.socket.SocketIO;
import io.socket.SocketIOException;

/**
 * Created by Quentin Bitschene on 12/01/2015.
 */
public class SocketSingleton {

    private static SocketSingleton instance;
    private static final String SERVER_ADDRESS = "http://192.168.42.42:80"; // ipconfig => ipv4
    private SocketIO socket;
    private Context context;

    public static SocketSingleton get(Context context){
        if(instance == null){
            instance = getSync(context);
        }
        instance.context = context;
        return instance;
    }

    public static synchronized SocketSingleton getSync(Context context){
        if (instance == null) {
            instance = new SocketSingleton(context);
        }
        return instance;
    }

    public SocketIO getSocket(){
        return this.socket;
    }

    private SocketSingleton(Context context){
        this.context = context;
        this.socket = getServerSocket();
    }

    private SocketIO getServerSocket(){

        SocketIO socket = null;
        try {
            socket = new SocketIO(new URL(SERVER_ADDRESS));

            socket.connect(new IOCallback() {
                @Override
                public void onMessage(JSONObject json, IOAcknowledge ack) {
                    try {

                        System.out.println("Server said:" + json.toString(2));
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }

                @Override
                public void onMessage(String data, IOAcknowledge ack) {

                    System.out.println("Server said: " + data);
                }

                @Override
                public void onError(SocketIOException socketIOException) {
                    ;
                    System.out.println("an Error occured");
                    socketIOException.printStackTrace();
                }

                @Override
                public void onDisconnect() {

                    System.out.println("Connection terminated.");
                }

                @Override
                public void onConnect() {

                    System.out.println("Connection established");
                }

                @Override
                public void on(String event, IOAcknowledge ack, Object... args) {

                    System.out.println("Server triggered event '" + event + "'");
                    Log.i("SERVER SAID : ", args[0].toString());

                    Intent intent = new Intent();
                    intent.setAction("MSG_RECEIVE");
                    intent.putExtra("message", args[0].toString());
                    context.sendBroadcast(intent);
                }

            });
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }

        return socket;

    }
}
