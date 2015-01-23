package com.application.playlistcollaborative.model;

import android.content.Context;
import android.content.Intent;
import android.util.Log;

import com.application.playlistcollaborative.Tool.JSONBuilder;

import org.json.JSONArray;
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
    //private static final String SERVER_ADDRESS = "http://nodejs-ihmdj.rhcloud.com:8000"; // ipconfig => ipv4
    private static final String SERVER_ADDRESS = "http://nodejs-ihmdj.rhcloud.com:8000";
    private static final String ANDROID = "android_";
    private static final String SURFACE = "surface_";
    private SocketIO socket;
    private Context context;
    private JSONArray lastresult;
    private JSONObject lastresultobj;

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

    public JSONArray getLastresult(){
        return this.lastresult;
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

        try{
            socket = new SocketIO(SERVER_ADDRESS);
        }catch (Exception e){
            Log.i("HUGO", e.getMessage());
        }

        if(socket != null) {

            socket.connect(new IOCallback() {
                @Override
                public void on(String event, IOAcknowledge ack, Object... args) {
                    if ("echo back".equals(event) && args.length > 0) {

                    }else if((ANDROID + "sendmusic").equals(event) && args.length > 0){
                        try{
                            lastresultobj =  new JSONObject((String)args[0]);
                            Log.i("nice",lastresultobj.toString());
                        }catch (Exception e){
                            e.printStackTrace();
                        }

                    }
                }

                @Override
                public void onMessage(JSONObject json, IOAcknowledge ack) {
                }

                @Override
                public void onMessage(String data, IOAcknowledge ack) {
                }

                @Override
                public void onError(SocketIOException socketIOException) {
                }

                @Override
                public void onDisconnect() {
                }

                @Override
                public void onConnect() {
                }
            });



        }
        return socket;

    }

    public void getMusic(){
        socket.emit(ANDROID+"getmusic", "need music");
    }

    public void setLastresult(JSONArray lastresult) {
        this.lastresult = lastresult;
    }

    public JSONObject getLastresultobj() {
        return lastresultobj;
    }
}
