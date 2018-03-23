using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SavWav
{

    const int HEADER_SIZE = 44;


    public static string GetAndroidContextExternalFilesDir()
    {
        string path = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                using (AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        path = ajo.Call<AndroidJavaObject>("getExternalStorageDirectory", null).Call<string>("getAbsolutePath");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error fetching native Android external storage dir: " + e.Message);
            }
        }
        return path;
    }


    public static bool Save(string filename, AudioClip clip)
    {
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        var filepath = Path.Combine("/mnt/sdcard", filename);


        Debug.Log(filepath);

        // memastikan directory exists jika user save ke sub dir.
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (var fileStream = CreateEmpty(filepath))
        {

            ConvertAndWrite(fileStream, clip);

            WriteHeader(fileStream, clip);
        }

        return true; // TODO: return false if there's a failure saving the file
    }

    public static AudioClip TrimSilence(AudioClip clip, float min)
    {
        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
    }

    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz)
    {
        return TrimSilence(samples, min, channels, hz, false, false);
    }

    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream)
    {
        int i;

        for (i = 0; i < samples.Count; i++)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }
        }

        samples.RemoveRange(0, i);

        for (i = samples.Count - 1; i > 0; i--)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }
        }

        samples.RemoveRange(i, samples.Count - i);

        var clip = AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);

        clip.SetData(samples.ToArray(), 0);

        return clip;
    }

    static FileStream CreateEmpty(string filepath)
    {
        var fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < HEADER_SIZE; i++) //menyiapkan header
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];
        //bytesData array dua kali ukurannya
        //dataSource Array karena float yang dikonversi di Int16 adalah 2 byte.

        int rescaleFactor = 32767; //convert float ke Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }

    static void WriteHeader(FileStream fileStream, AudioClip clip)
    {

        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number dalam channels, dengan frekuensi =  44100*2*2
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);
    }
}

