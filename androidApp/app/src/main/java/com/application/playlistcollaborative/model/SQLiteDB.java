package com.application.playlistcollaborative.model;

/**
 * Created by Hugo on 31/01/2015.
 */

        import android.content.Context;
        import android.database.sqlite.SQLiteDatabase;
        import android.database.sqlite.SQLiteOpenHelper;
        import android.database.sqlite.SQLiteDatabase.CursorFactory;

public class SQLiteDB extends SQLiteOpenHelper {

    private static final String TABLE_MUSIC = "table_music";
    private static final String COL_ID = "ID";
    private static final String COL_ARTIST = "Artist";
    private static final String COL_TITRE = "Titre";
    private static final String COL_GENRE = "Genre";
    private static final String COL_VOTES = "Votes";
    private static final String COL_VOTED = "Voted";

    private static final String CREATE_BDD = "CREATE TABLE " + TABLE_MUSIC + " ("
            + COL_ID + " TEXT PRIMARY KEY, " + COL_ARTIST + " TEXT NOT NULL, "
            + COL_TITRE + " TEXT NOT NULL, "+ COL_GENRE+" TEXT NOT NULL, "+ COL_VOTES +" INTEGER NOT NULL, "+ COL_VOTED + " INTEGER NOT NULL );";

    public SQLiteDB(Context context, String name, CursorFactory factory, int version) {
        super(context, name, factory, version);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        //on créé la table à partir de la requête écrite dans la variable CREATE_BDD
        db.execSQL(CREATE_BDD);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        //On peut fait ce qu'on veut ici moi j'ai décidé de supprimer la table et de la recréer
        //comme ça lorsque je change la version les id repartent de 0
        db.execSQL("DROP TABLE " + TABLE_MUSIC + ";");
        onCreate(db);
    }

}
