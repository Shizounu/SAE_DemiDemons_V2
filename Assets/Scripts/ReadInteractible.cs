using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInteractible : MonoBehaviour, IInteractibles
{
    public DialogueManager dialogueManager;
    [TextArea] public string InputText;

    private void Start() {
        dialogueManager = FindObjectOfType<DialogueManager>();
        FindObjectOfType<PlayerController>().interactibles.Add(this);
    }

    public void Interact()
    {
        dialogueManager.showText(InputText);
    }
}
