package com.application.playlistcollaborative.model;

/**
 * Created by Hugo on 16/01/2015.
 */
public class MusicPojo {

    private String id;
    private String title;
    private String artist;
    private String genre;

    public MusicPojo(String m_id, String m_title, String m_artist, String m_genre){
        this.id = m_id;
        this.title = m_title;
        this.artist = m_artist;
        this.genre = m_genre;

    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setArtist(String artist) {
        this.artist = artist;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getArtist() {
        return artist;
    }

    public String getGenre() {
        return genre;
    }

    public void setGenre(String genre) {
        this.genre = genre;
    }
}
