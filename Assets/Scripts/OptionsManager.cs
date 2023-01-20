using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public GameObject settings;
    public GameObject credits;
    public GameObject controls;


    public void Back(){
        GameManager.Instance.loadScene(0);
        GameManager.Instance.unloadScene(1);
    }
    public void ShowSettings(){
        settings.SetActive(true);
        credits.SetActive(false);
        controls.SetActive(false);
    }
    public void ShowCredits(){
        settings.SetActive(false);
        credits.SetActive(true);
        controls.SetActive(false);
    }
    public void ShowControls(){
        settings.SetActive(false);
        credits.SetActive(false);
        controls.SetActive(true);
    }
}
