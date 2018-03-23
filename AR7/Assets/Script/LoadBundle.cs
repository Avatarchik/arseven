using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;

public class LoadBundle : MonoBehaviour
{
    public string url;
    public Text loadingText;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/gulat.unity3d") && File.Exists(Application.persistentDataPath + "/kabbadi.unity3d"))
        {
            loadingText.text = "LOADING 100%";
            StartCoroutine(LoadAr());
          //  StartCoroutine(LoadObjectGulat());
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
            string dataFileName = "gulat";
            string tempPath = Application.persistentDataPath;
            tempPath = Path.Combine(tempPath, dataFileName + ".unity3d");

            //Save
            save(handle.data, tempPath);
            loadingText.text = "LOADING 25%";
            StartCoroutine(DownloadAssetKabbadi());
        }
    }

    IEnumerator DownloadAssetKabbadi()
    {
        yield return new WaitForSeconds(3);
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
            string dataFileName = "kabadi";
            string tempPath = Application.persistentDataPath;
            tempPath = Path.Combine(tempPath, dataFileName + ".unity3d");

            //Save
            save(handle.data, tempPath);
            loadingText.text = "LOADING 50%";
        }
    }

    IEnumerator LoadAr()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(LoadObjectGulat());

    }

    IEnumerator LoadObjectGulat()
    {
        
        string paths = Application.persistentDataPath + "/gulat.unity3d";
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(paths);
        yield return bundle;

        AssetBundle myLoadedAssetBundle = bundle.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        AssetBundleRequest request = myLoadedAssetBundle.LoadAssetAsync<GameObject>("gulat");
        yield return request;

        GameObject obj = request.asset as GameObject;
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.Rotate(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);

        Instantiate(obj);

        myLoadedAssetBundle.Unload(false);
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
