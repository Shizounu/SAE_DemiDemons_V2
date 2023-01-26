using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHandler : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
    }

    public void OnWin(){
        canvas.SetActive(true);
    }
}
