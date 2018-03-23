using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiorecordHandler : MonoBehaviour
{

    AudioClip audioclip;
    public GameObject micanim;
    public AudioSource audio;

    public bool boel;

    public float CountUp;

    private void Update()
    {
        if (boel == true)
        {
            CountUp += Time.deltaTime;
            micanim.SetActive(true);
        }

        if (boel == false)
        {
            CountUp = 0;
            micanim.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Record()
    {
        boel = true;
      
        audio.clip = audioclip;
        audioclip = Microphone.Start(null, true, 20, 44100); //20 detik dengan frekuensi 44100
    }

    public void Playback()
    {
        audio.clip = audioclip;
        SavWav.Save("myfile", audioclip); //save file dengan nama myfile pada base directory aplikasi
        audio.Play();
        StartCoroutine(ienum());
        boel = false;

    }

    IEnumerator ienum()
    {
        yield return new WaitForSeconds(CountUp);
        audio.Stop();
    }

    IEnumerator pb()
    {
        yield return new WaitForSeconds(20);
        boel = false;
        audio.Play();
    }
}