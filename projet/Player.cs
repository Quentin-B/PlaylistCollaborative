using Microsoft.Surface.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Un4seen.Bass;

namespace ProjetSurface
{
    public class Player
    {
        int stream;
        FileDeLecture file;
        private SYNCPROC _mySync;
        Song currentSong;
        SurfaceWindow1 surface;
        public Player(SurfaceWindow1 surface)
        {
            Bass.BASS_Init(1, 44100, BASSInit.BASS_DEVICE_DEFAULT, 0, null);
            file = FileDeLecture.Instance;
            this.surface = surface;
        }

        public void SetDevice(int device)
        {
            Bass.BASS_Free();
            Bass.BASS_Init(device, 44100, BASSInit.BASS_DEVICE_DEFAULT, 0, null);
        }

        public int GetDeviceCount()
        {
            return Bass.BASS_GetDeviceCount();
        }

        public string[] GetDeviceDescriptions()
        {
            return Bass.BASS_GetDeviceDescriptions();
        }

        public void LoadSong(Song s)
        {
            currentSong = s;
            stream = Bass.BASS_StreamCreateFile(s.Location, 0, 0,
                BASSStream.BASS_SAMPLE_FLOAT);
        }

        public void PlaySong(bool loop)
        {
            _mySync = new SYNCPROC(EndSync);
            Bass.BASS_ChannelSetSync(stream, BASSSync.BASS_SYNC_END | BASSSync.BASS_SYNC_MIXTIME,
                         0, _mySync, 0);
            Bass.BASS_ChannelPlay(stream, false);
        }

        public void PlaySong(bool loop, Song s, bool stop)
        {
            if(stop)
            {
                StopSong();
            }
            LoadSong(s);
            PlaySong(loop);
        }

        private void EndSync(int handle, int channel, int data, int user)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => surface.deleteBubble(currentSong)));
            Song nextSong = file.Next();
            
            if(nextSong != null)
                PlaySong(false, nextSong, true);
        }

        public void StopSong()
        {
            Bass.BASS_ChannelStop(stream);
        }

        ~Player()
        {
            Bass.BASS_Free();
        }
    }
}
