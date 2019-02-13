using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal.KeyFunctions
{
    class LightKeyFunction : GenericKeyFunction
    {
        public override void EditFunction()
        {
            EditLighting();
        }

        public override string ToString()
        {
            return "Trigger Light";
        }

        public override string getToolTip()
        {
            throw new NotImplementedException();
        }

        public override void onButtonPressed()
        {
            
        }

        public override void onButtonReleased()
        {

        }

        public override void ResetFunctionToDefault()
        {
            
        }
    }
}
