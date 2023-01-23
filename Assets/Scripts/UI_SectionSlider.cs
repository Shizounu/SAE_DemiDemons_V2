using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UI_SectionSlider : MonoBehaviour
{
    [Header("Audio editing")]
    public AudioMixer audioMixer;
    public string PropertyName = "Main Volume";
    public Vector2 audioRange = new Vector2(-60, 20);
    public void setVolume(){
        audioMixer.SetFloat(PropertyName, Mathf.Lerp(audioRange.x, audioRange.y, (float)Index / 10));
    }

    [Header("Slider Stuff")]
    [SerializeField, Range(0,9)] private int Index = 0;
    public UnityEngine.Events.UnityEvent OnValueChange;
    public void IncreaseValue(){
        Index += 1;
        if(Index > 9)
            Index = 9;
        OnValueChange.Invoke();
    }
    public void DecreaseValue(){
        Index -= 1;
        if(Index < 0)
            Index = 0;
        OnValueChange.Invoke();
    }
    [Header("Visuals")]

    [SerializeField] private UnityEngine.UI.Image Visual;
    [SerializeField] private Sprite[] visuals = new Sprite[10];
    void UpdateVisuals(){
        Visual.sprite = visuals[Index];
    }
    private void OnEnable() {
        OnValueChange.AddListener(UpdateVisuals);
    }
    private void OnDisable() {
        OnValueChange.RemoveListener(UpdateVisuals);
    }

    

}
