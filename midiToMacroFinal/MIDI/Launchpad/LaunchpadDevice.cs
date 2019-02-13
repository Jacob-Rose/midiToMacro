using Windows.Devices.Midi;
using System.Collections.Generic;

namespace midiToMacroFinal
{
    /// <summary>
    /// Launchpad Device which handles everything besides UI for the midi device
    /// </summary>
    public class LaunchpadDevice : MidiDevice
    {
        //create launchpad key array to store individual functions, one 9x9 array per mode
        public List<LaunchpadKey[,]> launchpadKeyArrays = new List<LaunchpadKey[,]> { new LaunchpadKey[9, 9] }; //starts with one mode active

        public LaunchpadDevice()
        {
            #region init all launchpad keys
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    launchpadKeyArrays[0][x, y] = new LaunchpadKey(x, y);
                }
            }
            #endregion
        }

        /// <summary>
        /// reset all buttons on launchpad and the launchpad key values
        /// </summary>
        public void resetLaunchpad()
        {
            midiOut.SendMessage(new MidiControlChangeMessage(0, 0, 0)); //signal clears all launchpad settings
            foreach (var x in launchpadKeyArrays[GlobalVariables.currentMode])
            {
                x.setButtonToDefault();
            }
        }

        public void updateButtonLight()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    updateButtonLight(x, y);
                }
            }
        }

        /// <summary>
        /// updates physical button color to the colorReleased Value
        /// </summary>
        /// <param name="row">key's row</param>
        /// <param name="col">key's col</param>
        public void updateButtonLight(int row, int col)
        {
            sendMidiMessage(convertToIMidiMessage(row, col, launchpadKeyArrays[GlobalVariables.currentMode][row, col].currentKeyColor));
        }

        public override void midiMessageRecieved(IMidiMessage msg)
        {
            LaunchpadKeySignalInfo sigInfo = convertToLaunchpadKeySignalInfo(msg);
            LaunchpadKey key = launchpadKeyArrays[GlobalVariables.currentMode][sigInfo.row, sigInfo.col];
            if (key.functions.Count > 0)
            {
                if (sigInfo.velocity == 127)
                {
                    key.buttonActivated(true);
                    sendMidiMessage(convertToIMidiMessage(sigInfo.row, sigInfo.col, key.functions[key.currentFunction].lightControl.colorPressed));

                }
                else
                {
                    key.buttonActivated(false);
                    sendMidiMessage(convertToIMidiMessage(sigInfo.row, sigInfo.col, key.functions[key.currentFunction].lightControl.colorReleased));
                }

            }


        }

        public LaunchpadKeySignalInfo convertToLaunchpadKeySignalInfo(IMidiMessage msg)
        {
            int velocity = 0;
            int row = 0;
            int col = 0;

            switch (msg.Type)
            {
                case MidiMessageType.NoteOn:

                    row = (((MidiNoteOnMessage)msg).Note / 16) + 1;
                    col = (((MidiNoteOnMessage)msg).Note % 16);

                    velocity = ((MidiNoteOnMessage)msg).Velocity;

                    break;

                case MidiMessageType.ControlChange:

                    col = (((MidiControlChangeMessage)msg).Controller) - 104;

                    velocity = ((MidiControlChangeMessage)msg).ControlValue;
                    break;
            }
            return new LaunchpadKeySignalInfo(row, col, velocity);
        }

        public IMidiMessage convertToIMidiMessage(int row, int col, int velocity)
        {
            if (row == 0)
            {
                return new MidiControlChangeMessage(OutputMidiChannel, (byte)(col + 104), (byte)velocity);
            }
            else
            {
                return new MidiNoteOnMessage(OutputMidiChannel, (byte)(((row - 1) * 16) + col), (byte)velocity);
            }
        }
    }

    public class LaunchpadKeySignalInfo {
        public int row;
        public int col;
        public int velocity;
        public bool pressed
        {
            get
            {
                if (velocity == 127)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public LaunchpadKeySignalInfo(int row, int col, int velocity)
        {
            this.row = row;
            this.col = col;
            this.velocity = velocity;
        }
    }
}
