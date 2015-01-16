using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass;

namespace ProjetSurface
{
    public class Player
    {
        int stream;
        public Player()
        {
            Bass.BASS_Init(1, 44100, BASSInit.BASS_DEVICE_DEFAULT, 0, null);
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

        public void LoadSong(string location)
        {
            stream = Bass.BASS_StreamCreateFile(location, 0, 0,
                BASSStream.BASS_SAMPLE_FLOAT);
        }

        public void PlaySong(bool loop)
        {
            Bass.BASS_ChannelPlay(stream, false);
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
