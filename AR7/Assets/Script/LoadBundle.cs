using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;

public class LoadBundle : MonoBehaviour
{
    public string gulatUrl;
    public string kabbadiUrl;
    public string video1Url;
    public string video2Url;

    public Text loadingText;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/gulat.unity3d") && File.Exists(Application.persistentDataPath + "/kabadi.unity3d") && File.Exists(Application.persistentDataPath + "/video1.mp4") && File.Exists(Application.persistentDataPath + "/video2.mp4"))
        {
            StartCoroutine(LoadAR());
        }
        else
        {
            StartCoroutine(DownloadAssetGulat());
        }
    }

    IEnumerator LoadAR()
    {
        loadingText.text = "LOADING 100%";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SceneAR");
    }

    IEnumerator DownloadAssetGulat()
    {
        UnityWebRequest www = UnityWebRequest.Get(gulatUrl);
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
            string dataFileName = "gulat";
            string tempPath = Application.persistentDataPath;

            tempPath = Path.Combine(tempPath, dataFileName + ".unity3d");

            //Save
            save(handle.data, tempPath);
            loadingText.text = "Proses Download gulat 25%";
            StartCoroutine(DownloadAssetKabbadi());
        }
    }


    IEnumerator DownloadAssetKabbadi()
    {
        yield return new WaitForSeconds(3);
        UnityWebRequest www = UnityWebRequest.Get(kabbadiUrl);
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
            string dataFileName = "kabadi";
            string tempPath = Application.persistentDataPath;
            tempPath = Path.Combine(tempPath, dataFileName + ".unity3d");

            //Save
            save(handle.data, tempPath);
            loadingText.text = "Proses Download kabadi 50%";
            StartCoroutine(DownloadAssetVideo1());
        }
    }

    IEnumerator DownloadAssetVideo1()
    {
        UnityWebRequest www = UnityWebRequest.Get(video1Url);
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
            loadingText.text = "Proses Download Video1 75%";
            StartCoroutine(DownloadAssetVideo2());
        }
    }

    IEnumerator DownloadAssetVideo2()
    {
        UnityWebRequest www = UnityWebRequest.Get(video2Url);
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
            string dataFileName = "video2";
            string tempPath = Application.persistentDataPath;
            tempPath = Path.Combine(tempPath, dataFileName + ".mp4");

            //Save
            save(handle.data, tempPath);
            loadingText.text = "Proses Download video2 100%";
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("SceneAr");
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
