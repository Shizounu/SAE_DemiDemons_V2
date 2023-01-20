using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int index){
        GameManager.Instance.loadScene(index);
    }
}
