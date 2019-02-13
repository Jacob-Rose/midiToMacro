using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midiToMacroFinal
{
    public static class MidiDeviceEngine
    {
        public static LaunchpadDevice launchpadDevice; //handles everything besides UI... plain and simple

        public static Midi midi; //gets info to connect to midi device

        public static void Midi_InputDevicesEnumerated(object sender, EventArgs e)
        {
            int i = 0;
            int count = -1;
            //Simple program to find the device that contains the word aunch. Simple way to avoid device selection screen
            foreach (var device in midi.ConnectedInputDevices)
            {
                count++;
                if (device.Name.Contains("aunch"))
                {
                    i = count;
                }
            }
            System.Diagnostics.Debug.WriteLine("Name: " + midi.ConnectedInputDevices[i].Name);
            launchpadDevice.connectToInputDevice(midi.ConnectedInputDevices[i]);
        }

        public static void Midi_OutputDevicesEnumerated(object sender, EventArgs e)
        {
            int i = 0;
            int count = -1;
            foreach (var device in midi.ConnectedOutputDevices)
            {
                count++;
                if (device.Name.Contains("aunch"))
                {
                    i = count;
                }
            }
            System.Diagnostics.Debug.WriteLine("Name: " + midi.ConnectedOutputDevices[i].Name);
            launchpadDevice.connectToOutputDevice(midi.ConnectedOutputDevices[i]);
        }

        public static string intToString(int var)
        {
            switch (var)
            {
                case 0:
                    return "A";
                case 1:
                    return "B";
                case 2:
                    return "C";
                case 3:
                    return "D";
                case 4:
                    return "E";
                case 5:
                    return "F";
                case 6:
                    return "G";
                case 7:
                    return "H";
                case 8:
                    return "I";
                default:
                    return null;
            }
        }
    }
}
