using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RythmGameManager rythmManager;

    [SerializeField] private Sprite NoFill, QuaterFill, HalfFill, ThreeQuarterFill, FullFill;

    [SerializeField] private UnityEngine.UI.Image imgRenderer;

    private void Update() {
        float value = (float)rythmManager.progress / (float)rythmManager.map.Count;
        if(value < 0.25f){
            imgRenderer.sprite = NoFill;
        } 
        if(value > 0.25f && value < 0.5f){
            imgRenderer.sprite = QuaterFill;
        }
        if(value > 0.5f && value < 0.75f){
            imgRenderer.sprite = HalfFill;
        }
        if(value > 0.75f && value < 0.9f){
            imgRenderer.sprite = ThreeQuarterFill;
        }
        if(value > 0.9f){
            imgRenderer.sprite = FullFill;
        }
    }
}
