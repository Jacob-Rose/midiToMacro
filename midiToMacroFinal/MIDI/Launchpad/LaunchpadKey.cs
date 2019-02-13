using System.Windows;
using System.Collections.Generic;
using midiToMacroFinal.KeyFunctions;
using midiToMacroFinal.KeyFunctions.Sound;

namespace midiToMacroFinal
{
    public class LaunchpadKey
    {
        public int row;
        public int col;

        public List<GenericKeyFunction> functions;
        public int currentFunction;

        public bool currentlyPressed = false;

        public byte currentKeyColor { get {
                if(currentlyPressed)
                {
                    return functions[currentFunction].lightControl.colorPressed;
                }
                else
                {
                    return functions[currentFunction].lightControl.colorReleased;
                } } }
        public byte currentKeyColorRed
        {
            get
            {
                if (currentlyPressed)
                {
                    return (byte)functions[currentFunction].lightControl.redPressed;
                }
                else
                {
                    return (byte)functions[currentFunction].lightControl.redReleased;
                }
            }
        }
        public byte currentKeyColorGreen
        {
            get
            {
                if (currentlyPressed)
                {
                    return (byte)functions[currentFunction].lightControl.greenPressed;
                }
                else
                {
                    return (byte)functions[currentFunction].lightControl.greenReleased;
                }
            }
        }

        public LaunchpadKey(int row, int col)
        {
            this.row = row;
            this.col = col;

            setButtonToDefault(); //set all properties to default
        }

        public void setButtonToDefault()
        {
            functions = new List<GenericKeyFunction>();
            functions.Add(new LightKeyFunction());
            currentFunction = 0;
        }

        public void buttonActivated(bool pressed)
        {
            if(functions.Count > 0)
            {
                if (pressed)
                {
                    currentFunction++;
                    if (currentFunction >= functions.Count)
                    {
                        currentFunction = 0;
                    }
                    functions[currentFunction].onButtonPressed();
                    currentlyPressed = true;
                }
                else
                {
                    functions[currentFunction].onButtonReleased();
                    currentlyPressed = false;
                }
            }
        }

        public void fileDropHandler(DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach(string f in files)
            {
                if (f.EndsWith(".wav"))
                {
                    SoundKeyFunction func = new SoundKeyFunction();
                    func.soundPlayer_Open(f);
                    functions.Add(func);
                }
            }
        }
    }
}
