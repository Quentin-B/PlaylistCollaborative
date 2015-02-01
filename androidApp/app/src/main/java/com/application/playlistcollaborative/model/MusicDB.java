package com.application.playlistcollaborative.model;

/**
 * Created by Hugo on 31/01/2015.
 */
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import java.util.ArrayList;

public class MusicDB {

    private static final int VERSION_BDD = 4;
    private static final String NOM_BDD = "eleves.db";

    private static final String TABLE_MUSIC = "table_music";
    private static final String COL_ID = "ID";
    private static final int NUM_COL_ID = 0;
    private static final String COL_ARTIST = "Artist";
    private static final int NUM_COL_ARTIST = 1;
    private static final String COL_TITRE = "Titre";
    private static final int NUM_COL_TITRE = 2;
    private static final String COL_GENRE = "Genre";
    private static final int NUM_COL_GENRE = 3;
    private static final String COL_VOTES = "Votes";
    private static final int NUM_COL_VOTES = 4;
    private static final String COL_VOTED = "Voted";
    private static final int NUM_COL_VOTED = 5;

    private static final String CREATE_BDD = "CREATE TABLE " + TABLE_MUSIC + " ("
            + COL_ID + " TEXT PRIMARY KEY, " + COL_ARTIST + " TEXT NOT NULL, "
            + COL_TITRE + " TEXT NOT NULL, "+ COL_GENRE+" TEXT NOT NULL, "+ COL_VOTES +" INTEGER NOT NULL, "+ COL_VOTED + " INTEGER NOT NULL );";


    private SQLiteDatabase bdd;

    private SQLiteDB maBaseSQLite;

    public MusicDB(Context context){
        //On créer la BDD et sa table
        maBaseSQLite = new SQLiteDB(context, NOM_BDD, null, VERSION_BDD);
    }

    public void open(){
        //on ouvre la BDD en écriture
        bdd = maBaseSQLite.getWritableDatabase();
    }

    public void close(){
        //on ferme l'accès à la BDD
        bdd.close();
    }

    public SQLiteDatabase getBDD(){
        return bdd;
    }

    public long insertMusic(MusicPojo m){
        //Création d'un ContentValues (fonctionne comme une HashMap)
        ContentValues values = new ContentValues();

        //on lui ajoute une valeur associé à une clé (qui est le nom de la colonne dans laquelle on veut mettre la valeur)
        values.put(COL_ARTIST, m.getArtist());
        values.put(COL_GENRE, m.getGenre());
        values.put(COL_ID, m.getId());
        values.put(COL_TITRE, m.getTitle());

        int value = (m.isVoted())?1:0;

        values.put(COL_VOTED, value);
        values.put(COL_VOTES, m.getVotes());

        //on insère l'objet dans la BDD via le ContentValues
        return bdd.insert(TABLE_MUSIC, null, values);
    }

    public void insertMusics(ArrayList<MusicPojo> m ){
        for(MusicPojo mp : m){
            insertMusic(mp);
        }
    }


    public int removeMusic(int id){
        bdd.delete(TABLE_MUSIC, COL_ID + " = " +id, null);
        //Suppression d'un livre de la BDD grâce à l'ID
        bdd.execSQL("DROP TABLE " + TABLE_MUSIC + ";");
        bdd.execSQL(CREATE_BDD);
        return 1;


    }

    public void removeAllMusics(){
        bdd.delete( TABLE_MUSIC, null,null);
    }

    //Cette méthode permet de convertir un cursor en un livre
    private ArrayList<MusicPojo> cursorToMusic(Cursor c){
        //si aucun élément n'a été retourné dans la requête, on renvoie null
        if (c.getCount() == 0)
            return null;

        ArrayList<MusicPojo> mlist = new ArrayList<MusicPojo>();

        if (c.moveToFirst()) {
            do {
                //On créé une musique
                MusicPojo m = new MusicPojo();

                //on lui affecte toutes les infos grâce aux infos contenues dans le Cursor
                m.setId(c.getString(NUM_COL_ID));
                m.setArtist(c.getString(NUM_COL_ARTIST));
                m.setTitle(c.getString(NUM_COL_TITRE));
                m.setGenre(c.getString(NUM_COL_GENRE));
                m.setVotes(c.getInt(NUM_COL_VOTES));
                int vot = c.getInt(NUM_COL_VOTED);

                if(vot == 1) m.setVoted(true);
                else m.setVoted(false);

                mlist.add(m);

            } while (c.moveToNext());
        }


        //On ferme le cursor
        c.close();

        //On retourne les musiques
        return mlist;
    }
    public ArrayList<MusicPojo> getMusics(){
        //Récupère dans un Cursor les valeur correspondant à un livre contenu dans la BDD (ici on sélectionne le livre grâce à son titre)
        Cursor c = bdd.rawQuery("SELECT  * FROM " + TABLE_MUSIC, null);
        return cursorToMusic(c);
    }

    public int updateVotedMusic(MusicPojo m, boolean b){
        //La mise à jour d'un livre dans la BDD fonctionne plus ou moins comme une insertion
        //il faut simple préciser quelle livre on doit mettre à jour grâce à l'ID

        int value = (b)?1:0;
        ContentValues values = new ContentValues();
        values.put(COL_VOTED, value);
        values.put(COL_VOTES, m.getVotes()+1);
        return bdd.update(TABLE_MUSIC, values, COL_ID + " = '" +m.getId() + "'", null);
    }


}