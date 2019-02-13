using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal
{
    public abstract class Effect
    {
        public float SampleRate { get; set; }
        public float Tempo { get; set; }
        public bool Enabled { get; set; }

        public Effect()
        {
            Enabled = true;
            Tempo = 120;
            SampleRate = 44100;
        }

        /// <summary>
        /// Should be called on effect load, 
        /// sample rate changes, and start of playback
        /// </summary>
        public virtual void Init()
        { }

        /// <summary>
        /// called for each sample
        /// </summary>
        public abstract void Sample(ref float spl0, ref float spl1);


        /// <summary>
        /// called before each block is processed
        /// </summary>
        /// <param name="samplesblock">number of samples in this block</param>
        public virtual void Block(int samplesblock)
        {
        }
    }
}
