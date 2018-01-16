using System;
using System.IO;

namespace Yata.SoundNodes.Nodes.Output
{
    public class WaveStream : MemoryStream
    {
        private const int headerSize = 44;
        private readonly int bufferSizeInBytes;

        public WaveStream(short[] buffer, int frequency)
            : base((buffer.Length * 2) + headerSize)
        {
            bufferSizeInBytes = buffer.Length * 2;
            WriteSoundHeaderToByteStream();
            WriteShortBufferToByteStream(buffer);

            Position = 0;
        }

        private void WriteShortBufferToByteStream(short[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                WriteShort(buffer[i]);
            }
        }

        private void WriteSoundHeaderToByteStream()
        {
            WriteRIFFChunckDescriptor();
            WriteFmtSubChunk();
            WriteDataSubChunk();
        }

        private void WriteRIFFChunckDescriptor()
        {
            WriteByte('R');
            WriteByte('I');
            WriteByte('F');
            WriteByte('F');

            WriteInt(36 + bufferSizeInBytes);

            WriteByte('W');
            WriteByte('A');
            WriteByte('V');
            WriteByte('E');
            // Total = 12
        }

        private void WriteFmtSubChunk()
        {
            WriteByte('f');
            WriteByte('m');
            WriteByte('t');
            WriteByte(' ');
            
            WriteInt(16); // Subchunk1Size
            WriteShort(1); // PCM format

            WriteShort(1); // Num channels
            WriteInt(44100); // sample rate
            WriteInt(44100 * 1 * 16 / 8); // byte rate, SampleRate * NumChannels * BitsPerSample/8
            WriteShort(1 * 16 / 8); // Block align, NumChannels * BitsPerSample/8
            WriteShort(16); // BitsPerSample

            // Total = 24
        }

        private void WriteDataSubChunk()
        {
            WriteByte('d');
            WriteByte('a');
            WriteByte('t');
            WriteByte('a');

            WriteInt(bufferSizeInBytes); // data size
            // total = 8
        }

        private void WriteInt(int value)
        {
            Write(BitConverter.GetBytes(value), 0, 4);
        }

        private void WriteShort(short value)
        {
            Write(BitConverter.GetBytes(value), 0, 2);
        }

        private void WriteByte(char value)
        {
            Write(BitConverter.GetBytes(value), 0, 1);
        }
    }
}
