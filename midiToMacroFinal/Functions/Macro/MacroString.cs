using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal
{
    class MacroString
    {
        public string macro;
        public int delay; // in miliseconds

        public MacroString(string m, int d)
        {
            macro = m;
            delay = d;
        }

        public override string ToString()
        {
            return "Macro: " + macro + " with a delay of " + delay + " miliseconds";
        }
    }
}
