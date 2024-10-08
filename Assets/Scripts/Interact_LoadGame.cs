using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_LoadGame : MonoBehaviour, IInteractibles
{

    private void Awake() {
        FindObjectOfType<PlayerController>().interactibles.Add(this);
    }
    public void Interact()
    {
        GameManager.Instance.unloadScene(3);
        GameManager.Instance.loadScene(4);
    }
}
