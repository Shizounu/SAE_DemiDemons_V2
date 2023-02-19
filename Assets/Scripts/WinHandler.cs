using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHandler : MonoBehaviour
{
    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
    }

    public bool isLoading = false;
    public void ReturnToBar(){
        if(isLoading)
            return;
        isLoading = true;
        gameManager.loadScene(2);
        gameManager.unloadScene(3);
    }
}
