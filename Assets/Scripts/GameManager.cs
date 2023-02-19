using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; protected set; }
    private void Awake() {
        if(Instance != null && Instance != this){
            Destroy(this);
            return;
        }
        Instance = this;

        
        _inputActions = new();
        runningLoadingOperations = new();
        
    }
    #endregion

    #region  Input
    private InputActions _inputActions;
    public InputActions inputActions{
        get {
            if(_inputActions == null){
                _inputActions = new();
            }
            return _inputActions;
        }
    }

    private void OnEnable() {
        
    }
    private void OnDisable() {
        
    }
    #endregion

    #region Loading
    private List<AsyncOperation> runningLoadingOperations;
    public void loadScene(int index){
        //SceneManager.LoadScene(5 ,LoadSceneMode.Additive);
        AsyncOperation op = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        op.allowSceneActivation = true;
        runningLoadingOperations.Add(op);
    }
    public void unloadScene(int index){
        runningLoadingOperations.Add(SceneManager.UnloadSceneAsync(index, UnloadSceneOptions.None));
    }

    public void finishLoadOperation(){
        foreach (var loadingOperation in runningLoadingOperations){
            loadingOperation.allowSceneActivation = true;
        }
    }

    public float percentageCompleted(){
        float unscaledProgress = 0;
        foreach (AsyncOperation loadOperation in runningLoadingOperations)
            unscaledProgress += loadOperation.progress;
        return unscaledProgress / runningLoadingOperations.Count; //scale the progress by how many operations there are
    }
    #endregion



}
