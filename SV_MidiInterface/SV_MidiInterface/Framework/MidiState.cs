using System;
using Commons.Music.Midi;
using CoreMidi;


namespace SpaceBaby.MidiInterface.Framework
{
    public enum NoteState
    {
        off,
        on
    }

    public class MidiKey
    {
        NoteState state;
        byte velocity;
        byte keyPressure;

        public MidiKey()
        {
            state = NoteState.off;
            velocity = 0;
            keyPressure = 0;
        }

    }

    public class MidiController
    {
        private readonly Int16 value;

        public MidiController()
        {
            value = 0;
        }
    }

    public class MidiChannel
    {
        MidiKey[] midiNotes;
        MidiController[] midiControllers;
        readonly byte channelPressure;

        public MidiChannel()
        {
            midiNotes = new MidiKey[127];
            midiControllers = new MidiController[120];
            channelPressure = 0;
        }

    }

    public class MidiState
    {
        byte[] rawData;
        readonly MidiChannel[] channels;

        public MidiState()
        {
            rawData = null;
            channels = new MidiChannel[16];
        }

        public void UpdateState(MidiReceivedEventArgs args)
        {
            rawData = args.Data;
        }

    }
}
