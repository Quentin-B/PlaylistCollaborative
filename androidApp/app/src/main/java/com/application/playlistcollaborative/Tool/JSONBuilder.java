package com.application.playlistcollaborative.Tool;

import android.util.Log;

import com.application.playlistcollaborative.model.MusicPojo;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Created by Hugo on 16/01/2015.
 */
public final class JSONBuilder {

    public static JSONObject MusicToJSON(MusicPojo m){
        JSONObject ret = new JSONObject();
        try{
            ret.put("id", m.getId());
            ret.put("title", m.getTitle());
            ret.put("artist", m.getArtist());
            ret.put("genre", m.getGenre());
        }catch (Exception e){
            e.printStackTrace();
        }

        return ret;

    }

    public static ArrayList<MusicPojo> JSONToMusicList(JSONArray ja){
        ArrayList<MusicPojo> m = new ArrayList<MusicPojo>();
        for(int i = 0; i < ja.length(); i++){
            MusicPojo model = new MusicPojo();
            JSONObject music = new JSONObject();

            try{

                music = ja.getJSONObject(i);
                model.setArtist(music.getString("Artist"));
                model.setGenre(music.getString("Category"));
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
