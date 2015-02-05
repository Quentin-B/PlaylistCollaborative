package com.application.playlistcollaborative.Tool;

import android.util.Log;

import com.application.playlistcollaborative.model.MusicPointerPojo;
import com.application.playlistcollaborative.model.MusicPojo;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Created by Hugo on 16/01/2015.
 */
public final class JSONBuilder {

    public static MusicPointerPojo JSONToMP(JSONObject jo){
        try {
            return new MusicPointerPojo(jo.getString("Id"), jo.getLong("Duration"), jo.getLong("Position"));
        }catch (Exception e){
            e.printStackTrace();
        }

        return  null;

    }

    public static ArrayList<MusicPojo> JSONToMusicList(JSONArray ja){
        ArrayList<MusicPojo> m = new ArrayList<MusicPojo>();
        for(int i = 0; i < ja.length(); i++){
            MusicPojo model = new MusicPojo();
            JSONObject music = new JSONObject();

            try{

                music = ja.getJSONObject(i);
                model.setArtist(music.getString("Artist"));
                model.setGenre(music.getString("_Category"));
                model.setTitle(music.getString("Name"));
                model.setId(music.getString("Id"));
                m.add(model);

            }catch (Exception e){
                e.printStackTrace();
                return null;
            }


        }
        return  m;
    }


}
