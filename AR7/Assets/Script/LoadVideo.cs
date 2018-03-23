using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Networking;
using System;

public class LoadVideo : MonoBehaviour
{
    public string url;
    public VideoPlayer vP;

	private void Start()
	{
        if (File.Exists(Application.persistentDataPath + "/video1.mp4"))
        {
            vP.url = Application.persistentDataPath + "/video1.mp4";
            vP.Play();
        }
        else
        {
            StartCoroutine(DownloadAssetGulat());
        }
	}

	IEnumerator DownloadAssetGulat()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        DownloadHandler handle = www.downloadHandler;

        //Send Request and wait
        yield return www.Send();

        if (www.isNetworkError)
        {

            UnityEngine.Debug.Log("Error while Downloading Data: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Success");

            //handle.data

            //Construct path to save it
            string dataFileName = "video1";
            string tempPath = Application.persistentDataPath;
            tempPath = Path.Combine(tempPath, dataFileName + ".mp4");

            //Save
            save(handle.data, tempPath);
        }
    }

    void save(byte[] data, string path)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, data);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
}
