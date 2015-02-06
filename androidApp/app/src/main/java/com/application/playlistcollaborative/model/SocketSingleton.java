package com.application.playlistcollaborative.model;

import android.animation.AnimatorSet;
import android.animation.ObjectAnimator;
import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.View;
import android.view.animation.DecelerateInterpolator;
import android.view.animation.LinearInterpolator;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.application.playlistcollaborative.R;
import com.application.playlistcollaborative.Tool.JSONBuilder;
import com.application.playlistcollaborative.main.MyCustomAdapter;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.w3c.dom.Text;

import java.util.ArrayList;

import io.socket.IOAcknowledge;
import io.socket.IOCallback;
import io.socket.SocketIO;
import io.socket.SocketIOException;

/**
 * Created by Quentin Bitschene on 12/01/2015.
 */
public class SocketSingleton {

    private static SocketSingleton instance;
    private static final String SERVER_ADDRESS = "http://nodejs-ihmdj.rhcloud.com:8000";
    private static final String ANDROID = "android_";
    private SocketIO socket;
    private ListView lw;
    private MusicDB db;
    private Activity mainthread;
    private TextView artist;
    private TextView titre;
    private ProgressBar music;
    private ObjectAnimator animation;
    private String currentmusic;
    private long playtime;

    public static SocketSingleton get(){
        if(instance == null){
            instance = getSync();
        }
        return instance;
    }

    public static synchronized SocketSingleton getSync(){
        if (instance == null) {
            instance = new SocketSingleton();
        }
        return instance;
    }

    public SocketIO getSocket(){
        return this.socket;
    }

    private SocketSingleton(){

        this.socket = getServerSocket();
        this.currentmusic = "";
        this.playtime = 0;

    }



    private SocketIO getServerSocket(){

         SocketIO socketinit = null;

        try{
            socketinit = new SocketIO(SERVER_ADDRESS);
        }catch (Exception e){
            Log.i("HUGO", e.getMessage());
        }

        final SocketIO socket = socketinit;

        if(socket != null) {

            socket.connect(new IOCallback() {
                @Override
                public void on(String event, IOAcknowledge ack, Object... args) {
                    if ("echo back".equals(event) && args.length > 0) {

                    }else if((ANDROID + "sendmusic").equals(event) && args.length > 0){
                        try{
                            JSONArray jsa = new JSONArray((String)args[0]);
                            final ArrayList<MusicPojo> m = JSONBuilder.JSONToMusicList(jsa);

                            if(!m.equals(db.getMusics())){
                                final Context c = mainthread.getBaseContext();
                                mainthread.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        MyCustomAdapter adapter = new MyCustomAdapter(m, c,db);
                                        lw.setAdapter(adapter);
                                    }
                                });
                                db.removeAllMusics();
                                db.insertMusics(m);
                            }


                        }catch (Exception e){
                            e.printStackTrace();
                        }

                    }else if((ANDROID + "broadcast_plus").equals(event) && args.length > 0){
                        String id = (String)args[0];
                        final MyCustomAdapter ca = (MyCustomAdapter)lw.getAdapter();
                        ca.upvote(id);
                        mainthread.runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                ca.notifyDataSetChanged();
                            }
                        });

                    }else if((ANDROID + "music_pointer").equals(event) && args.length > 0){
                        JSONObject jo = new JSONObject();

                        try {
                            jo = new JSONObject((String)args[0]);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                        final MusicPointerPojo mpp = JSONBuilder.JSONToMP(jo);

                        if(currentmusic.equals("") || !currentmusic.equals(mpp.getId())){

                           currentmusic = mpp.getId();

                            mainthread.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    LinearLayout ll = (LinearLayout) mainthread.findViewById(R.id.player);
                                    ll.setVisibility(View.VISIBLE);

                                    MusicPojo mp = db.getMusicById(mpp.getId());

                                    if(mp == null){
                                        Log.i("Probleme bdd", "impossible d'obtenir la musique");
                                        return;
                                    }
                                    artist.setText(mp.getArtist());
                                    titre.setText(mp.getTitle());

                                    music.setMax(300);

                                    if(animation == null){
                                        animation = ObjectAnimator.ofInt(music, "progress",0,300);
                                        animation.setDuration(Math.round(mpp.getDuration())*1000); // 0.5 second
                                        animation.setInterpolator(new LinearInterpolator());
                                    }else {
                                        animation.pause();
                                        animation.setDuration(Math.round(mpp.getDuration())*1000);
                                        animation.setCurrentPlayTime(Math.round(mpp.getPosition()));
                                        animation.resume();


                                    }




                                    animation.start();


                                }
                            });

                        }else{

                            mainthread.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    animation.resume();
                                }
                            });
                        }

                    }else if((ANDROID + "stop_music").equals(event)){
                        mainthread.runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                               if(animation != null){
                                   animation.pause();
                               }

                            }
                        });


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

    public void getMusic(ListView lw,Activity a, MusicDB db, TextView artist, TextView titre, ProgressBar p){
        socket.emit(ANDROID+"getmusic", "need music");
        this.lw = lw;
        this.db = db;
        this.mainthread = a;
        this.artist = artist;
        this.titre = titre;
        this.music = p;
    }

    public void sendplus(String id){
        socket.emit(ANDROID + "plus", id);
    }


    public void setArtist(TextView artist) {
        this.artist = artist;
    }

    public void setTitre(TextView titre) {
        this.titre = titre;
    }

    public void setMusic(ProgressBar music) {
        this.music = music;
    }
}
