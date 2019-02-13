using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal
{
    public static class GlobalVariables
    {

        public delegate void ModeChangeEventHandler(object sender, EventArgs e); //event active when mode is changed

        public static event ModeChangeEventHandler ModeChangedEvent;

        private static int _mode;
        public static int currentMode
        {
            get
            {
                // Reads are usually simple
                return _mode;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                ModeChangedEvent?.Invoke(currentMode, null);
                _mode = value;
            }
        }
    }
}
