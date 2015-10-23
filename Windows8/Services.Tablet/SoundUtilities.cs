using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using Storm.Mvvm.Events;

namespace IndiaRose.Services
{
    public static class SoundUtilities
    {
        public static event EventHandler Completed;
        private static readonly Dictionary<string, SourceVoice> LoadedSounds = new Dictionary<string, SourceVoice>();
        private static readonly Dictionary<string, AudioBufferAndMetaData> AudioBuffers = new Dictionary<string, AudioBufferAndMetaData>();
        private static MasteringVoice _masteringVoice;
        public static MasteringVoice MasteringVoice
        {
            get
            {
                if (_masteringVoice == null)
                {
                    _masteringVoice = new MasteringVoice(XAudio);
                    _masteringVoice.SetVolume(1, 0);
                }
                return _masteringVoice;
            }
        }
        private static XAudio2 _xaudio;
        public static XAudio2 XAudio
        {
            get
            {
                if (_xaudio == null)
                {
                    _xaudio = new XAudio2();
                    var voice = MasteringVoice; //touch voice to create it
                    _xaudio.StartEngine();
                }
                return _xaudio;
            }
        }
        public static void PlaySound(string soundfile, float volume = 1)
        {
            SourceVoice sourceVoice;
            if (!LoadedSounds.ContainsKey(soundfile))
            {

                var buffer = GetBuffer(soundfile);
                sourceVoice = new SourceVoice(XAudio, buffer.WaveFormat, true);
                sourceVoice.SetVolume(volume, SharpDX.XAudio2.XAudio2.CommitNow);
                sourceVoice.SubmitSourceBuffer(buffer, buffer.DecodedPacketsInfo);
                sourceVoice.Start();

            }
            else
            {
                sourceVoice = LoadedSounds[soundfile];
                if (sourceVoice != null)
                    sourceVoice.Stop();
            }
        }
        public static void PlaySound(Stream soundStream, float volume = 1)
        {
            SourceVoice sourceVoice;

            var buffer = GetBuffer(soundStream);
            sourceVoice = new SourceVoice(XAudio, buffer.WaveFormat, true);
            sourceVoice.SetVolume(volume, SharpDX.XAudio2.XAudio2.CommitNow);
            sourceVoice.SubmitSourceBuffer(buffer, buffer.DecodedPacketsInfo);
            sourceVoice.BufferEnd += SourceVoiceOnBufferEnd;
            sourceVoice.Start();

        }

        private static void SourceVoiceOnBufferEnd(IntPtr intPtr)
        {
            new object().RaiseEvent(Completed);
        }

        private static AudioBufferAndMetaData GetBuffer(string soundfile)
        {
            if (!AudioBuffers.ContainsKey(soundfile))
            {
                var nativefilestream = new NativeFileStream(
                        soundfile,
                        NativeFileMode.Open,
                        NativeFileAccess.Read,
                        NativeFileShare.Read);

                var soundstream = new SoundStream(nativefilestream);

                var buffer = new AudioBufferAndMetaData()
                {
                    Stream = soundstream.ToDataStream(),
                    AudioBytes = (int)soundstream.Length,
                    Flags = BufferFlags.EndOfStream,
                    WaveFormat = soundstream.Format,
                    DecodedPacketsInfo = soundstream.DecodedPacketsInfo
                };
                AudioBuffers[soundfile] = buffer;
            }
            return AudioBuffers[soundfile];

        }
        private static AudioBufferAndMetaData GetBuffer(Stream soundfile)
        {
            var soundstream = new SoundStream(soundfile);

            var buffer = new AudioBufferAndMetaData()
            {
                Stream = soundstream.ToDataStream(),
                AudioBytes = (int)soundstream.Length,
                Flags = BufferFlags.EndOfStream,
                WaveFormat = soundstream.Format,
                DecodedPacketsInfo = soundstream.DecodedPacketsInfo
            };
            return buffer;

        }
        private sealed class AudioBufferAndMetaData : AudioBuffer
        {
            public WaveFormat WaveFormat { get; set; }
            public uint[] DecodedPacketsInfo { get; set; }
        }
    }
}
