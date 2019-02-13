using NAudio.Wave;

namespace midiToMacroFinal
{
    class LowLatencySoundPlayer
    {
        private WaveOut[] waveOuts;
        private WaveStream[] waveStreams;
        private int playerCount;
        public LowLatencySoundPlayer(string file, int deviceCount)
        {
            waveOuts = new WaveOut[deviceCount];
            waveStreams = new WaveStream[deviceCount];
            playerCount = deviceCount;

            for(int i = 0; i < deviceCount; i++)
            {
                waveStreams[i] = new WaveFileReader(file);
                waveOuts[i] = new WaveOut();
                waveOuts[i].Init(waveStreams[i]);
            }
        }
        public void Play()
        {
            Stop();
            PlayOnly();
        }
        public void Stop()
        {
            foreach(WaveOut wo in waveOuts)
            {
                wo.Stop();
            }
            foreach(WaveStream ws in waveStreams)
            {
                ws.Position = 0;
            }
        }

        public void PlayOnly()
        {
            for (int i = 0; i < playerCount; i++)
            {
                if (waveOuts[i].PlaybackState == PlaybackState.Playing)
                {
                    waveOuts[i].Stop();
                    waveStreams[i].Position = 0;
                }
                else
                {
                    waveOuts[i].Play();
                }
            }
        }
    }
}
