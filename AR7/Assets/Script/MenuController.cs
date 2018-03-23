using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject menu;
    public GameObject arUI;
    public GameObject[] Video;
    public GameObject[] Model3D;

    public void Videos()
    {
        for (int i = 0; i < Video.Length; i++)
        {
            Video[i].SetActive(true);
        }

        for (int j = 0; j < Model3D.Length; j++)
        {
            Model3D[j].SetActive(false);
        }
        arUI.SetActive(true);
        menu.SetActive(false);
    }

    public void Models()
    {
        for (int j = 0; j < Model3D.Length; j++)
        {
            Model3D[j].SetActive(true);
        }

        for (int i = 0; i < Video.Length; i++)
        {
            Video[i].SetActive(false);
        }
        arUI.SetActive(true);
        menu.SetActive(false);
    }

    public void Back()
    {
        menu.SetActive(true);
        arUI.SetActive(false);
    }
}