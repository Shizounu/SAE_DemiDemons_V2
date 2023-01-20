using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BlockSlider : MonoBehaviour
{
    [Range(1,10)] public int Value = 0;
    public UnityEngine.Events.UnityAction OnValueChange;

    public void inreaseValue(){
        Value +=1;
        if(Value > 9)
            Value = 9;
        OnValueChange.Invoke();
    }
    public void decreaseValue(){
        Value -= 1;
        if(Value <= 0)
            Value = 0;
        OnValueChange.Invoke();
    }


    [Header("Visuals")]
    public UnityEngine.UI.Image img;
    public Sprite[] Visuals = new Sprite[10];
    private void OnEnable() {
        OnValueChange += UpdateVisuals;
    }
    void UpdateVisuals(){
        img.sprite = Visuals[Value];
    }
}
