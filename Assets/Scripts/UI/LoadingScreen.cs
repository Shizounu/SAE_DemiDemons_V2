using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager manager;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image loadingScreenBG;

    [Header("Stuff")]
    [SerializeField] private float ChangeTime = 5;
    [SerializeField] private List<Sprite> backgrounds;



    private void Start() {
        manager = GameManager.Instance;
        progressBar.value = 0;
        loadingScreenBG.sprite = chooseRandomSprite(loadingScreenBG.sprite);
        timeSinceLastChage = 0;
    }
    [SerializeField] float timeSinceLastChage;



    private void FixedUpdate() {
        progressBar.value = manager.percentageCompleted();
        timeSinceLastChage += Time.fixedDeltaTime;
        if(timeSinceLastChage >= ChangeTime){
            timeSinceLastChage =0;
            loadingScreenBG.sprite = chooseRandomSprite(loadingScreenBG.sprite);
        }
        

        
    
    }


    private Sprite chooseRandomSprite(Sprite dontUse = null){
        List<Sprite> possibilities = new();
        possibilities.AddRange(backgrounds);
        possibilities.Remove(dontUse);

        return possibilities[Random.Range(0, possibilities.Count)];
    }
    private Sprite chooseRandomSprite(List<Sprite> dontUse){
        List<Sprite> possibilities = new();
        possibilities.AddRange(backgrounds);
        foreach (var item in dontUse)
            possibilities.Remove(item);

        return possibilities[Random.Range(0, possibilities.Count)];
    }
}
