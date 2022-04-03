using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Music.Midi;
using StardewModdingAPI;
using System.Threading.Tasks;

namespace SpaceBaby.MidiInterface.Framework.Manager
{
    public class MidiManager
    {
        public IMidiAccess2 midi;
        public List<IMidiInput> inDevice;
        public byte prevEvent, prevValue;
        IMonitor Monitor;

        public MidiManager(IMonitor monitor)
        {
            this.Monitor = monitor;
            this.Monitor.Log("Initializing MIDI Framework", LogLevel.Info);
            // As per managed-midi 1.9.14, MidiAccessManager.Default outputs an IMidiAccess.
            // Casting it because making midi an IMidiAccess creates a warning:
            midi = (IMidiAccess2)MidiAccessManager.Default;
            var numdevices = midi.Inputs.Count();
            if (numdevices <= 0)
            {
                this.Monitor.Log("No Devices Found. Please connect MIDI Device to use this mod.", LogLevel.Info);
                this.Monitor.Log("Any Mods requiring this framework will be disabled.", LogLevel.Info);
                return;
            }
            inDevice = new List<IMidiInput>();
            this.Monitor.Log($"Midi Framework Initialized. Found {numdevices} device{ ((numdevices == 0 || numdevices > 1) ? ("s") : ("")) }", LogLevel.Info);
            for (int i = 0; i < numdevices; i++)
            {
                inDevice.Add(midi.OpenInputAsync(midi.Inputs.ElementAt(i).Id).Result);
                this.Monitor.Log($"Device {i}: {inDevice[i].Details.Name}", LogLevel.Debug);
            }
        }

        internal async Task Poll()
        {
            await PollMidiConnection();
            if (inDevice.Any())
                await PollMidi();
        }

        private async Task PollMidiConnection()
        {
            //Checking for added devices
            int numDevices = midi.Inputs.Count();
            if (numDevices > inDevice.Count)
            {
                foreach (IMidiPortDetails details in midi.Inputs)
                {
                    IMidiInput input = inDevice.Find(x => x.Details.Name.Equals(details.Name));
                    if (input == null)
                    {
                        IMidiInput newInput = midi.OpenInputAsync(details.Id).Result;
                        inDevice.Add(newInput);
                        this.Monitor.Log($"{newInput.Details.Name} connected", LogLevel.Info);
                        break;
                    }
                }
            }
            //Checking for removed devices
            if (numDevices < inDevice.Count)
            {
                foreach (IMidiInput input in inDevice)
                {
                    IMidiPortDetails details = midi.Inputs.ToList().Find(x => x.Id.Equals(input.Details.Id));
                    if (details == null)
                    {
                        this.Monitor.Log($"{input.Details.Name} disconnected", LogLevel.Info);
                        await input.CloseAsync();
                        inDevice.Remove(input);
                        break;
                    }
                }
            }

            await Task.Delay(1000 / 60);
        }

        //
        private async Task PollMidi()
        {
            foreach (IMidiInput device in inDevice)
            {
                device.MessageReceived += (obj, ev) =>
                {
                    byte[] data = new byte[ev.Length];
                    Array.Copy(ev.Data, ev.Start, data, 0, ev.Length);

                    if (prevEvent != ev.Data[0])
                        Monitor.Log($"[{String.Join(",", data)}]", LogLevel.Debug);

                    prevEvent = ev.Data[0];
                };
            }

            await Task.Yield();
        }

    }
}
