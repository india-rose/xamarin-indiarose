using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using Storm.Mvvm.Events;

namespace Services.Tablet
{
    /// <summary>
    /// Utilitaire pour la gestion des sons
    /// </summary>
    public static class SoundUtilities
    {
        /// <summary>
        /// Event de fin de lecture
        /// </summary>
        public static event EventHandler Completed;
        //todo voir avec julien à quoi ça sert
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
                    _masteringVoice.SetVolume(1);
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


        /// <summary>
        /// Lance la lecture d'un son
        /// </summary>
        /// <param name="soundfile">Chemin d'accès au son</param>
        /// <param name="volume">Volume auquel le son doit être lu</param>
        public static void PlaySound(string soundfile, float volume = 1)
        {
            SourceVoice sourceVoice;
            if (!LoadedSounds.ContainsKey(soundfile))
            {

                var buffer = GetBuffer(soundfile);
                sourceVoice = new SourceVoice(XAudio, buffer.WaveFormat, true);
                sourceVoice.SetVolume(volume);
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

        /// <summary>
        /// Lance la lecture d'un son
        /// </summary>
        /// <param name="soundStream">Stream du fichier son</param>
        /// <param name="volume">Volume auquel le son doit être lu</param>
        public static void PlaySound(Stream soundStream, float volume = 1)
        {
            var buffer = GetBuffer(soundStream);
            var sourceVoice = new SourceVoice(XAudio, buffer.WaveFormat, true);
            sourceVoice.SetVolume(volume);
            sourceVoice.SubmitSourceBuffer(buffer, buffer.DecodedPacketsInfo);
            sourceVoice.BufferEnd += SourceVoiceOnBufferEnd;
            sourceVoice.Start();

        }

        /// <summary>
        /// Callback de fin de lecture.
        /// Lance l'event Completed.
        /// </summary>
        /// <see cref="Completed"/>
        private static void SourceVoiceOnBufferEnd(IntPtr intPtr)
        {
            new object().RaiseEvent(Completed);
        }

        /// <summary>
        /// Initialise le buffer à partir d'un fichier son
        /// </summary>
        /// <param name="soundfile">Le chemin d'accès au fichier</param>
        /// <returns>Le buffer du son</returns>
        private static AudioBufferAndMetaData GetBuffer(string soundfile)
        {
            if (!AudioBuffers.ContainsKey(soundfile))
            {
                var nativefilestream = new NativeFileStream(
                        soundfile,
                        NativeFileMode.Open,
                        NativeFileAccess.Read);

                var soundstream = new SoundStream(nativefilestream);

                var buffer = new AudioBufferAndMetaData
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
        
        /// <summary>
        /// Initialise le buffer à partir d'un fichier son
        /// </summary>
        /// <param name="soundfile">Le Stream du sonr</param>
        /// <returns>Le buffer du son</returns>
        private static AudioBufferAndMetaData GetBuffer(Stream soundfile)
        {
            var soundstream = new SoundStream(soundfile);

            var buffer = new AudioBufferAndMetaData
            {
                Stream = soundstream.ToDataStream(),
                AudioBytes = (int)soundstream.Length,
                Flags = BufferFlags.EndOfStream,
                WaveFormat = soundstream.Format,
                DecodedPacketsInfo = soundstream.DecodedPacketsInfo
            };
            return buffer;

        }
       
        /// <summary>
        /// AudioBuffer classe avec des informations en plus
        /// </summary>
        /// <see cref="AudioBuffer"/>
        private sealed class AudioBufferAndMetaData : AudioBuffer
        {
            public WaveFormat WaveFormat { get; set; }
            public uint[] DecodedPacketsInfo { get; set; }
        }
    }
}
