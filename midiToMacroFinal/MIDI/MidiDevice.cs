using System;
using Windows.Devices.Midi;

namespace midiToMacroFinal
{
    /// <summary>
    /// Handles all midi signals to and from a single midi device. Use as base to create multiple types of midi devices
    /// </summary>
    public abstract class MidiDevice
    {
        //default channel to send midi signals
        protected const byte InputMidiChannel = 0;
        protected const byte OutputMidiChannel = 0;

        //midi in and out ports
        protected MidiInPort midiIn;
        protected MidiOutPort midiOut;

        /// <summary>
        /// sets the input device to receive midi messages from
        /// </summary>
        /// <param name="devInfo">device to use</param>
        public async void connectToInputDevice(MidiDeviceInformation devInfo)
        {
            midiIn = await MidiInPort.FromIdAsync(devInfo.Id);
            midiIn.MessageReceived += MidiInputDevice_MessageReceived;
        }

        /// <summary>
        /// sets the output device to send midi messages to
        /// </summary>
        /// <param name="devInfo">device to connect to</param>
        public async void connectToOutputDevice(MidiDeviceInformation devInfo)
        {
            midiOut = (MidiOutPort)await MidiOutPort.FromIdAsync(devInfo.Id);
        }

        public void sendMidiMessage(IMidiMessage msg)
        {
            midiOut.SendMessage(msg);
        }

        private void MidiInputDevice_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage msg = args.Message;
            midiMessageRecieved(msg);
        }

        public abstract void midiMessageRecieved(IMidiMessage msg);
    }
}
