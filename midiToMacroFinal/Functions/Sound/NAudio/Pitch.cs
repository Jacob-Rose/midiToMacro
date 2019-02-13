using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal.NAudio
{
    class Pitch : Effect
    {
        float pitch;


        public override void Init()
        {
            base.Init();
        }

        public override void Sample(ref float spl0, ref float spl1)
        {
            throw new NotImplementedException();
        }
    }
}
