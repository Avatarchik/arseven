using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CekSDCard : MonoBehaviour {
    public Text txt;
	

    void Start()
	{
        txt.text = GetAndroidContextExternalFilesDir();
    }

    public static string GetAndroidInternalFilesDir()
	{
	    string[] potentialDirectories = new string[]
	    {
	        "/mnt/sdcard",
	        "/sdcard",
	        "/storage/sdcard0",
	        "/storage/sdcard1"
	    };

	    if(Application.platform == RuntimePlatform.Android)
	    {
	        for(int i = 0; i < potentialDirectories.Length; i++)
	        {
	            if(Directory.Exists(potentialDirectories[i]))
	            {
	                return potentialDirectories[i];
	            }
	        }
	    }
	    return "";
	}

	public string GetAndroidContextExternalFilesDir()
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
                txt.text = e.Message;

            }
        }
        return path;
    }


}
