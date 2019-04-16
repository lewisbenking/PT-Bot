using UnityEngine;
using System.Text;
using System.IO;
using System;

/// <summary>
/// WAV utility for recording and audio playback functions in Unity.
/// Version: 1.0 alpha 1
///
/// - Use "ToAudioClip" method for loading wav file / bytes.
/// Loads .wav (PCM uncompressed) files at 8,16,24 and 32 bits and converts data to Unity's AudioClip.
///
/// - Use "FromAudioClip" method for saving wav file / bytes.
/// Converts an AudioClip's float data into wav byte array at 16 bit.
/// </summary>
/// <remarks>
/// For documentation and usage examples: https://github.com/deadlyfingers/UnityWav
/// </remarks>

public class WavUtility
{
    // Force save as 16-bit .wav
    const int BlockSize_16Bit = 2;

    /// <summary>
    /// Load PCM format *.wav audio file (using Unity's Application data path) and convert to AudioClip.
    /// </summary>
    /// <returns>The AudioClip.</returns>
    /// <param name="filePath">Local file path to .wav file</param>
    public static AudioClip ToAudioClip(string filePath)
    {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        return ToAudioClip(fileBytes, 0);
    }

    public static AudioClip ToAudioClip(byte[] fileBytes, int offsetSamples = 0, string name = "wav")
    {
        int subchunk1 = BitConverter.ToInt32(fileBytes, 16);
        UInt16 audioFormat = BitConverter.ToUInt16(fileBytes, 20);
        UInt16 channels = BitConverter.ToUInt16(fileBytes, 22);
        int sampleRate = BitConverter.ToInt32(fileBytes, 24);
        UInt16 bitDepth = BitConverter.ToUInt16(fileBytes, 34);
        int headerOffset = 16 + 4 + subchunk1 + 4;
        int subchunk2 = BitConverter.ToInt32(fileBytes, headerOffset);
        float[] data;
        data = Convert16BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
        AudioClip audioClip = AudioClip.Create(name, data.Length, (int)channels, sampleRate, false);
        audioClip.SetData(data, 0);
        return audioClip;
    }

    private static float[] Convert16BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
    {
        int wavSize = BitConverter.ToInt32(source, headerOffset);
        headerOffset += sizeof(int);
        int x = sizeof(Int16); // block size = 2
        int convertedSize = wavSize / x;
        float[] data = new float[convertedSize];
        Int16 maxValue = Int16.MaxValue;
        int offset = 0;
        int i = 0;
        while (i < convertedSize)
        {
            offset = i * x + headerOffset;
            data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;
            ++i;
        }
        return data;
    }

    public static byte[] FromAudioClip(AudioClip audioClip) { return FromAudioClip(audioClip, out string file); }

    public static byte[] FromAudioClip(AudioClip audioClip, out string filepath)
    {
        MemoryStream stream = new MemoryStream();
        const int headerSize = 44;
        UInt16 bitDepth = 16;
        // NB: Only supports 16 bit. Total file size = 44 bytes for header format and audioClip.samples * factor due to float to Int16 / sbyte conversion
        int fileSize = audioClip.samples * BlockSize_16Bit + headerSize; // BlockSize (bitDepth)
        WriteFileHeader(ref stream, fileSize);
        WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
        WriteFileData(ref stream, audioClip, bitDepth);
        byte[] bytes = stream.ToArray();
        filepath = null;
        stream.Dispose();
        return bytes;
    }

    private static int WriteFileHeader(ref MemoryStream stream, int fileSize)
    {
        int count = 0;
        byte[] riff = Encoding.ASCII.GetBytes("RIFF");
        count += WriteBytesToMemoryStream(ref stream, riff, "ID");
        int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(chunkSize), "CHUNK_SIZE");
        byte[] wave = Encoding.ASCII.GetBytes("WAVE");
        count += WriteBytesToMemoryStream(ref stream, wave, "FORMAT");
        return count;
    }

    private static int WriteFileFormat(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
    {
        int count = 0;
        byte[] id = Encoding.ASCII.GetBytes("fmt ");
        count += WriteBytesToMemoryStream(ref stream, id, "FMT_ID");
        int subchunk1Size = 16; // 24 - 8
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk1Size), "SUBCHUNK_SIZE");
        UInt16 audioFormat = 1;
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(audioFormat), "AUDIO_FORMAT");
        UInt16 numChannels = Convert.ToUInt16(channels);
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(numChannels), "CHANNELS");
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(sampleRate), "SAMPLE_RATE");
        int byteRate = sampleRate * channels * BytesPerSample(bitDepth);
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(byteRate), "BYTE_RATE");
        UInt16 blockAlign = Convert.ToUInt16(channels * BytesPerSample(bitDepth));
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(blockAlign), "BLOCK_ALIGN");
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(bitDepth), "BITS_PER_SAMPLE");
        return count;
    }

    private static int WriteFileData(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
    {
        int count = 0;
        // Copy float[] data from AudioClip
        float[] data = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(data, 0);
        byte[] bytes = ConvertAudioClipDataToInt16ByteArray(data);
        byte[] id = Encoding.ASCII.GetBytes("data");
        count += WriteBytesToMemoryStream(ref stream, id, "DATA_ID");
        int subchunk2Size = Convert.ToInt32(audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
        count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk2Size), "SAMPLES");
        count += WriteBytesToMemoryStream(ref stream, bytes, "DATA");
        return count;
    }

    private static byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
    {
        MemoryStream dataStream = new MemoryStream();
        int x = sizeof(Int16);
        Int16 maxValue = Int16.MaxValue;
        int i = 0;
        while (i < data.Length)
        {
            dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
            ++i;
        }
        byte[] bytes = dataStream.ToArray();
        dataStream.Dispose();
        return bytes;
    }

    private static int WriteBytesToMemoryStream(ref MemoryStream stream, byte[] bytes, string tag = "")
    {
        int count = bytes.Length;
        stream.Write(bytes, 0, count);
        return count;
    }

    public static UInt16 BitDepth(AudioClip audioClip)
    {
        UInt16 bitDepth = Convert.ToUInt16(audioClip.samples * audioClip.channels * audioClip.length / audioClip.frequency);
        Debug.AssertFormat(bitDepth == 8 || bitDepth == 16 || bitDepth == 32, "Unexpected AudioClip bit depth: {0}. Expected 8 or 16 or 32 bit.", bitDepth);
        return bitDepth;
    }

    private static int BytesPerSample(UInt16 bitDepth) { return bitDepth / 8; }
}