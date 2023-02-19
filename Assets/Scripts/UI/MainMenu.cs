using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int index){
        GameManager.Instance.loadScene(index);
    }
    public void UnloadMainMenu(){
        GameManager.Instance.unloadScene(0);
    }
    public void QuitGame(){
        Application.Quit();
    }
}
