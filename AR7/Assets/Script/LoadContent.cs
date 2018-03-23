using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Networking;
using System;

public class LoadContent : MonoBehaviour
{
    public Transform MarkerGulat, MarkerKabaddi;
     
    public GameObject gulat, kabaddi;

	private void Start()
	{
        StartCoroutine(LoadContentGulat());
	}

	IEnumerator LoadContentGulat()
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
        Instantiate(obj);
        gulat = GameObject.Find("gulat(Clone)");
        myLoadedAssetBundle.Unload(false);
        gulat.transform.SetParent(MarkerGulat.transform);
        gulat.transform.localPosition= new Vector3(0, 0, 0);
        gulat.transform.localEulerAngles = new Vector3 (0, 0, 0);
        gulat.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        StartCoroutine(LoadContentKabadi());
    }

    IEnumerator LoadContentKabadi()
    {
        string poths = Application.persistentDataPath + "/kabbadi.unity3d"; 
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(poths);
        yield return bundle;

        AssetBundle myLoadedAssetBundle = bundle.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        AssetBundleRequest request = myLoadedAssetBundle.LoadAssetAsync<GameObject>("kabbadi");
        yield return request;

        GameObject obj = request.asset as GameObject;
        obj.transform.SetParent(MarkerGulat.transform);
        Instantiate(obj);
        kabaddi = GameObject.Find("kabbadi(Clone)");
        myLoadedAssetBundle.Unload(false);
        kabaddi.transform.SetParent(MarkerKabaddi.transform);
        kabaddi.transform.localPosition = new Vector3(0, 0, -0.5f);
        kabaddi.transform.localEulerAngles = new Vector3(0, 0, 0);
        kabaddi.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    IEnumerator PositioningToMarker()
    {
        yield return new WaitForSeconds(5);
       
       
    }
}
