using System;

namespace midiToMacroFinal.KeyFunctions.ModeChange
{
    class ModeChangeKeyFunction : GenericKeyFunction
    {

        public int modeAccess; //when checked, the mode that will be set
        private int? modeConst; //mode to set when pressed

        private bool whileHeld;

        public override void EditFunction()
        {
            FunctionSettingsWindow settingsWindow = new FunctionSettingsWindow();
            settingsWindow.ShowDialog();
        }

        public override string ToString()
        {
            if (modeConst == null)
            {
                return "Mode Control";
            }
            return "Mode Switch | Mode: " + modeConst;
        }

        public override string getToolTip()
        {
            throw new NotImplementedException();
        }

        public override void onButtonPressed()
        {
            Global.currentMode = modeAccess;
            Global.ModeChangedEvent += ((sender, e) => {
                //add reference to set released button to pressed
            });
        }

        public override void onButtonReleased()
        {
            //do nothing
        }

        public override void ResetFunctionToDefault()
        {
            lightControl = new LightControl();
            modeAccess = 1;
            modeConst = 1;
            whileHeld = false;
        }
    }
}
