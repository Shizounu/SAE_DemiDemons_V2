using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueField;
    [SerializeField] private TMPro.TMP_Text TextBox;

    private GameManager gameManager;
    [SerializeField, Range(0.01f, 0.1f)] private float animTime;

    private void Start() {
        gameManager = GameManager.Instance;
        disableTextBoxMode();
    }

    public void showText(string text){
        enableTextBoxMode();
        StartCoroutine(animateText(text));
    }
    private IEnumerator animateText(string fullText){
        string currentText ="";

        for (int i = 0; i < fullText.Length; i++){
            currentText += fullText[i];
            TextBox.text = currentText;

            yield return new WaitForSeconds(animTime);    
        }

        while (!GameManager.Instance.inputActions.TextBoxControls.Confirm.WasPressedThisFrame())
        {
            yield return new WaitForSeconds(animTime);            
        }
        disableTextBoxMode();
    }
    void enableTextBoxMode(){
        gameManager.inputActions.PlayerControls.Disable();
        gameManager.inputActions.TextBoxControls.Enable();

        TextBox.text = "";

        dialogueField.SetActive(true);
    }
    
    void disableTextBoxMode(){
        gameManager.inputActions.PlayerControls.Enable();
        gameManager.inputActions.TextBoxControls.Disable();

        TextBox.text = "";

        dialogueField.SetActive(false);
    }

    [ContextMenu("Test")]
    void testText(){
        showText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
    }
}
