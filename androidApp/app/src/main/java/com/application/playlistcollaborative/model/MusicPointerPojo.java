package com.application.playlistcollaborative.model;

/**
 * Created by Hugo on 01/02/2015.
 */
public class MusicPointerPojo {
    private String id;
    private float duration;
    private float position;

    public MusicPointerPojo(String id, float duration, float position) {
        this.id = id;
        this.duration = duration;
        this.position = position;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public float getPosition() {
        return position;
    }

    public void setPosition(float position) {
        this.position = position;
    }
}
