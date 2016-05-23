using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace FwPercept
{
    public sealed class ModuleAudioRealSense : IModule, IModuleAudio
    {
        private AudioFormatEnum audioFormat;
        private UtilMCaptureFile capturePlayBack = null;

        private const int AUDIO_HEADER_SIZE = 44;   // WAVE PCM header is 44 bytes
        private const int MAX_FRAMES = 800;         // 800 frames is about 30s
        private const Int16 BITS_PER_SAMPLE = 16;   // 16 bits for each audio sample
        private const Int16 BYTES_PER_SAMPLE = 2;   // 16 bits = 2 bytes

        // A struct that will help define part of the WAVE file
        private struct FmtSubchunk
        {
            public Int32 size;
            public Int16 format;
            public Int16 num_channels;
            public UInt32 sample_rate;
            public UInt32 byte_rate;
            public Int16 block_align;
            public Int16 bits_per_sample;
        }

        public enum AudioFormatEnum
        {
            PCM = PXCMAudio.AudioFormat.AUDIO_FORMAT_PCM,
            IEEE_Float = PXCMAudio.AudioFormat.AUDIO_FORMAT_IEEE_FLOAT
        }

        public ModuleAudioRealSense(AudioFormatEnum AudioFormat)
            : base()
        {
            audioFormat = AudioFormat;
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "AudioRealSense";
        }

        public int GetIdModule()
        {
            return 2;
        }

        private byte[] GetAudioDataFromEvent(PXCMAudio audio)
        {
            PXCMAudio.AudioData adata;
            byte[] bytes = null;

            pxcmStatus status = audio.AcquireAccess(PXCMAudio.Access.ACCESS_READ, (PXCMAudio.AudioFormat)audioFormat, out adata);
            if (status == pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                bytes = adata.ToByteArray();
                audio.ReleaseAccess(ref adata);
            }
            return bytes;
        }

        public byte[] GetDataFromAudioFrame(EventArgs e)
        {
            return GetAudioDataFromEvent(((AudioFramesSenseEventArgs)e).ObjAudio);
        }

        /*
         Recebo a qtd em milesegundos
         * Divido por 1000, pra achar os segundos
         * Multiplico por os segundos por 30 (que são os frames por segundo)
         * Com o resultado final utilizo no loop pra gerar o arquivo de audio
         */
        public void RecordAudio(long Milliseconds, string output_file_name)
        {
            long seconds = 0;
            long nFrames = 0;

            if (Milliseconds > 0)
            {
                if (output_file_name.Contains(".wav"))
                {
                    seconds = Milliseconds / 1000;
                    nFrames = seconds * 30;

                    // Get a memory stream for the audio data, wrapped with a big try catch for simplicity.
                    using (MemoryStream writer = new MemoryStream())
                    {

                        // Create a session
                        PXCMSession session = null;
                        pxcmStatus status = PXCMSession.CreateInstance(out session);
                        if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                        {
                            //Console.Error.WriteLine("Failed to create the PXCMSession. status = " + status);
                        }

                        PXCMCapture.AudioStream.DataDesc request = new PXCMCapture.AudioStream.DataDesc();
                        request.info.nchannels = 2;
                        request.info.sampleRate = 44100;
                        uint subchunk2_data_size = 0;

                        // Use the capture utility
                        using (session)
                        using (UtilMCapture capture = new UtilMCapture(session))
                        {
                            // Locate a stream that meets our request criteria
                            status = capture.LocateStreams(ref request);
                            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                            {
                                //Console.Error.WriteLine("Unable to locate audio stream. status = " + status);
                            }

                            // Set the volume level
                            PXCMRangeF32 range;
                            float step, def;
                            bool isauto;
                            status = capture.device.QueryPropertyInfo(PXCMCapture.Device.Property.PROPERTY_AUDIO_MIX_LEVEL, out range, out step, out def, out isauto);
                            if (pxcmStatus.PXCM_STATUS_ITEM_UNAVAILABLE != status)
                            {
                                status = capture.device.SetProperty(PXCMCapture.Device.Property.PROPERTY_AUDIO_MIX_LEVEL, 0.2f);
                                if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                                {
                                    //Console.Error.WriteLine("Unable to set the volume level. status = " + status);
                                }
                            }

                            // Get the n frames of audio data.
                            for (int i = 0; i < nFrames; i++)
                            {
                                PXCMScheduler.SyncPoint sp = null;
                                PXCMAudio audio = null;

                                // We will asynchronously read the audio stream, which
                                // will create a synchronization point and a reference
                                // to an audio object.
                                status = capture.ReadStreamAsync(out audio, out sp);
                                if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                                {
                                    //Console.Error.WriteLine("Unable to ReadStreamAsync. status = " + status);
                                }

                                using (sp)
                                using (audio)
                                {

                                    // For each audio frame
                                    // 1) Synchronize so that you can access to the data
                                    // 2) acquire access
                                    // 3) write data while you have access,
                                    // 4) release access to the data

                                    status = sp.Synchronize();
                                    if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                                    {
                                        //Console.Error.WriteLine("Unable to Synchronize. status = " + status);
                                    }

                                    PXCMAudio.AudioData adata;

                                    status = audio.AcquireAccess(PXCMAudio.Access.ACCESS_READ, (PXCMAudio.AudioFormat)audioFormat, out adata);
                                    if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
                                    {
                                        //Console.Error.WriteLine("Unable to AcquireAccess. status = " + status);
                                    }

                                    byte[] data = adata.ToByteArray();
                                    int len = data.Length;
                                    writer.Write(data, 0, len);

                                    // keep a running total of how much audio data has been captured
                                    subchunk2_data_size += (uint)(adata.dataSize * BYTES_PER_SAMPLE);

                                    audio.ReleaseAccess(ref adata);
                                }
                            }
                        }

                        // The header needs to know how much data there is. Now that we are done recording audio
                        // we know that information and can write out the header and the audio data to a file.
                        using (BinaryWriter bw = new BinaryWriter(File.Open(output_file_name, FileMode.Create, FileAccess.Write)))
                        {
                            bw.Seek(0, SeekOrigin.Begin);
                            WriteAudioHeader(bw, subchunk2_data_size, (short)request.info.nchannels, request.info.sampleRate);
                            bw.Write(writer.ToArray());
                        }

                    }
                }
                else
                {
                    //lançar exceção
                }
            }
            else
            {
                //lançar exceção
            }
        }

        public void RecordAudioFromBytes(List<byte[]> listBytes, string output_file_name)
        {

            if (output_file_name.Contains(".wav"))
            {
                // Get a memory stream for the audio data, wrapped with a big try catch for simplicity.
                using (MemoryStream writer = new MemoryStream())
                {
                    PXCMCapture.AudioStream.DataDesc request = new PXCMCapture.AudioStream.DataDesc();
                    request.info.nchannels = 2;
                    request.info.sampleRate = 44100;
                    uint subchunk2_data_size = 0;

                    foreach (var item in listBytes)
                    {
                        byte[] data = item;
                        int len = data.Length;
                        writer.Write(data, 0, len);

                        // keep a running total of how much audio data has been captured
                        subchunk2_data_size += (uint)((len/2) * BYTES_PER_SAMPLE);                            
                    }                    

                    // The header needs to know how much data there is. Now that we are done recording audio
                    // we know that information and can write out the header and the audio data to a file.
                    using (BinaryWriter bw = new BinaryWriter(File.Open(output_file_name, FileMode.Create, FileAccess.Write)))
                    {
                        bw.Seek(0, SeekOrigin.Begin);
                        WriteAudioHeader(bw, subchunk2_data_size, (short)request.info.nchannels, request.info.sampleRate);
                        bw.Write(writer.ToArray());
                    }
                }
            }
            else
            {
                //lançar exceção
            }
        }

        // Write the header for a PCM WAVE file give the data size, number of channels, and sample rate.
        private void WriteAudioHeader(BinaryWriter writer, UInt32 dataSize, Int16 channels, UInt32 sampleRate)
        {
            //// Audio Header looks like:
            //// RIFF descriptor
            //// subchunk1 - the 'fmt' chunk
            //// subchunk2 - the 'data' chunk

            //// RIFF
            // Need to get the raw bytes, otherwise Write(char[]) also writes the array length
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            UInt32 chunk_size = 36 + dataSize;
            writer.Write(chunk_size);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

            //// subchunk1
            writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));

            FmtSubchunk subchunk1;
            subchunk1.size = 16;  // 16 bytes for PCM
            subchunk1.format = 1; // 1 means PCM
            subchunk1.num_channels = channels;
            subchunk1.sample_rate = sampleRate;
            subchunk1.byte_rate = sampleRate * (UInt32)(channels * BYTES_PER_SAMPLE);
            subchunk1.block_align = (Int16)(channels * BYTES_PER_SAMPLE);
            subchunk1.bits_per_sample = BITS_PER_SAMPLE;

            // As an alternative to a bunch of writer.Write calls, you could have "used" InteropServices
            // and call Marshal.StructureToPtr. 
            writer.Write(subchunk1.size);
            writer.Write(subchunk1.format);
            writer.Write(subchunk1.num_channels);
            writer.Write(subchunk1.sample_rate);
            writer.Write(subchunk1.byte_rate);
            writer.Write(subchunk1.block_align);
            writer.Write(subchunk1.bits_per_sample);

            //// subchunk2
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(dataSize);
            // the rest of subchunk2 is the actual audio data and gets written elsewhere
        }

        public void PlayAudio(string output_file_name)
        {
            if (output_file_name.Contains(".wav"))
            {
                var bytes = File.ReadAllBytes(output_file_name);
                using (Stream s = new MemoryStream(bytes))
                {
                    SoundPlayer myPlayer = new SoundPlayer(s);
                    myPlayer.Play();
                }
            }
        }

        public AudioFormatEnum AudioFormat
        {
            get { return audioFormat; }
        }

    }
}
