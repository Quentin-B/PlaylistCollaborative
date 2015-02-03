package com.application.playlistcollaborative.model;

import java.util.Objects;

/**
 * Created by Hugo on 16/01/2015.
 */
public class MusicPojo implements  Comparable<MusicPojo>{

    private String id;
    private String title;
    private String artist;
    private String genre;
    private int votes;
    private boolean voted;

    public MusicPojo(String m_id, String m_title, String m_artist, String m_genre){
        this.id = m_id;
        this.title = m_title;
        this.artist = m_artist;
        this.genre = m_genre;
        this.votes = 0;
        this.voted = false;

    }

    public MusicPojo(){

    }

    @Override
    public String toString(){
        return this.title;
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

    public boolean isVoted() {
        return voted;
    }

    public void setVoted(boolean voted) {
        this.voted = voted;
    }

    public int getVotes() {
        return votes;
    }

    public void setVotes(int votes) {
        this.votes = votes;
    }

    @Override
    public int compareTo(MusicPojo musicPojo) {
        return this.id.compareTo(musicPojo.getId());
    }

    @Override
    public boolean equals(Object m){
        return m.getClass().equals(this.getClass()) && this.getId().equals(((MusicPojo) m).getId());

    }
}
