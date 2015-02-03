package com.application.playlistcollaborative.main;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.ListAdapter;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.application.playlistcollaborative.R;
import com.application.playlistcollaborative.model.MusicDB;
import com.application.playlistcollaborative.model.MusicPojo;
import com.application.playlistcollaborative.model.SocketSingleton;

import java.util.ArrayList;

import io.socket.SocketIO;

public class MyCustomAdapter extends BaseAdapter implements ListAdapter {
    private ArrayList<MusicPojo> list;
    private Context context;
    private SocketSingleton socket;
    private MusicDB db;


    public MyCustomAdapter(ArrayList<MusicPojo> list, Context context, MusicDB db) {
        this.list = list;
        this.context = context;
        this.db = db;
        this.list = list;
        socket = SocketSingleton.get();
    }

    @Override
    public int getCount() {
        return list.size();
    }

    @Override
    public Object getItem(int pos) {
        return list.get(pos);
    }

    @Override
    public long getItemId(int pos) {
        return 0;
        //just return 0 if your list items do not have an Id variable.
    }

    public void upvote(String id){
        for(MusicPojo m : this.list){
            if(m.getId().equals(id)){
                Log.i("WORKING", "LOL");
                m.setVotes(m.getVotes() + 1);
            }
        }
    }

    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        View view = convertView;
        if (view == null) {
            LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            view = inflater.inflate(R.layout.list_adapter_layout, null);
        }

        //Handle TextView and display string from your list
        TextView listItemText = (TextView) view.findViewById(R.id.title_string);
        TextView ArtistText = (TextView) view.findViewById(R.id.artist_string);

        final MusicPojo m = list.get(position);

        listItemText.setText(m.getTitle());
        ArtistText.setText(m.getArtist());

        //Handle buttons and add onClickListeners
        final ImageButton up = (ImageButton) view.findViewById(R.id.up);
        final Drawable d = context.getResources().getDrawable(R.drawable.up_disabled);


        if(!m.isVoted()){
            up.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    m.isVoted();
                    notifyDataSetChanged();//or some other task
                    db.updateVotedMusic(m, true);
                    socket.sendplus(m.getId());
                    up.setActivated(false);
                    up.setImageDrawable(d);

                }
            });
        }else{
            up.setImageDrawable(d);
        }

         ProgressBar mProgress = (ProgressBar) view.findViewById(R.id.circle_progress_bar);
        TextView tv = (TextView) view.findViewById(R.id.votes);
        tv.setText(Integer.toString(m.getVotes()));
         mProgress.setProgress(m.getVotes());


      
        return view;
    }
}