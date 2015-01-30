package com.application.playlistcollaborative.main;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.ListAdapter;
import android.widget.TextView;

import com.application.playlistcollaborative.R;
import com.application.playlistcollaborative.model.MusicPojo;
import com.application.playlistcollaborative.model.SocketSingleton;

import java.util.ArrayList;

import io.socket.SocketIO;

public class MyCustomAdapter extends BaseAdapter implements ListAdapter {
    private ArrayList<MusicPojo> list = new ArrayList<MusicPojo>();
    private Context context;
    private SocketSingleton socket;


    public MyCustomAdapter(ArrayList<MusicPojo> list, Context context) {
        this.list = list;
        this.context = context;
        socket = SocketSingleton.get(context);
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

        MusicPojo m = list.get(position);

        listItemText.setText(m.getTitle());
        ArtistText.setText(m.getArtist());

        //Handle buttons and add onClickListeners
        ImageButton up = (ImageButton) view.findViewById(R.id.up);


        up.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //do something
                MusicPojo m = list.get(position); //or some other task
                socket.sendplus(m.getId());
            }
        });
      
        return view;
    }
}