package com.application.playlistcollaborative.Tool;

import com.application.playlistcollaborative.model.MusicPojo;

import org.json.JSONArray;
import org.json.JSONObject;

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

}
