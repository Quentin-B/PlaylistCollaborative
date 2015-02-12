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

        public Song CurrentSong
        {
            get { return currentSong; }
            set { currentSong = value; }
        }
        SurfaceWindow1 surface;
        int volume;

        public Player(SurfaceWindow1 surface)
        {
            Bass.BASS_Init(1, 44100, BASSInit.BASS_DEVICE_DEFAULT, 0, null);
            file = FileDeLecture.Instance;
            this.surface = surface;
            volume = 50;
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
            stream = Bass.BASS_StreamCreateFile(s.Location, 0, 0,
                BASSStream.BASS_SAMPLE_FLOAT);
        }

        public void PlusVolume()
        {
            if (volume == 100)
                return;
            Bass.BASS_SetVolume(++volume);
        }

        public void LessVolume()
        {
            if (volume == 0)
                return;
            Bass.BASS_SetVolume(--volume);
        }

        public void PlaySong(bool loop, Song s)
        {
            currentSong = s;
            _mySync = new SYNCPROC(EndSync);
            Bass.BASS_ChannelSetSync(stream, BASSSync.BASS_SYNC_END | BASSSync.BASS_SYNC_MIXTIME,
                         0, _mySync, 0);
            Bass.BASS_ChannelPlay(stream, false);

        }

        public float getCurrentSongLength()
        {
            long byteduration = 0;
            byteduration = Bass.BASS_ChannelGetLength(stream);
            float seconds = Bass.BASS_ChannelBytes2Seconds(stream, byteduration);
            return seconds;
        }

        public float getCurrentSongPos()
        {
            long byteduration = 0;
            byteduration = Bass.BASS_ChannelGetPosition(stream);
            float seconds = Bass.BASS_ChannelBytes2Seconds(stream, byteduration);
            return seconds;
        }


        public void PlaySong(bool loop,Song s, bool stop)
        {
            if(stop)
            {
                StopSong();
            }
            LoadSong(s);
            PlaySong(loop, s);

        }

        private void EndSync(int handle, int channel, int data, int user)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => surface.deleteBubble(currentSong)));
            file.removeCurrent();
            //Song nextSong = file.Next();
            Song nextSong = file.getCurrentSong();

            if (nextSong != null)
            {
                
        
                PlaySong(false, nextSong, true);
                SongPointer sp = new SongPointer(nextSong.Id, getCurrentSongLength(), getCurrentSongPos());
                surface._Sock.sendmusicstarting(sp);
                Application.Current.Dispatcher.Invoke(new Action(() => surface.updateBubble()));
                
            }
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
