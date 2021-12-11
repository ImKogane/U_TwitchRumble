using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;


public class ScenesManager : SingletonMonobehaviour<ScenesManager>
{
    List<AsyncOperation> AsyncOperations = new List<AsyncOperation>();

    [Header("Non destructible scenes")]
    public List<string> _NeverDestroyScenes = new List<string>();

    [Header("Starting Level")]
    public SO_level _startLevel;

    [Header("Ref to UI")]
    public ChargingUI _chargementUI;

    public override bool DestroyOnLoad => true;



    public void Start()
    {
        List<string> StartGameScenesNames = new List<string>();
        StartGameScenesNames.AddRange(_NeverDestroyScenes);
        StartGameScenesNames.AddRange(_startLevel.LevelsToCharge);
        ChargeALevel(StartGameScenesNames);
    }

    #region Charge levels
    public void ChargeALevel(List<string> ScenesOfNextLevel)
    {
        StartCoroutine(_chargementUI.FadeInCoroutine(ScenesOfNextLevel));
    }

    public IEnumerator AfterFadeIn(List<string> ScenesOfNextLevel)
    {
        List<string> ScenesOfCurrentLevel = GetAllOpenedScenes();

        List<string> ScenesToUnload = new List<string>();
        ScenesToUnload.AddRange(ScenesOfCurrentLevel);

        List<string> ScenesToLoad = new List<string>();
        ScenesToLoad.AddRange(ScenesOfNextLevel);

        //Remove communes scenes 
        for (int i = 0; i < ScenesOfCurrentLevel.Count; i++)
        {
            for (int y = 0; y < ScenesOfNextLevel.Count; y++)
            {
                if (ScenesOfNextLevel[y] == ScenesOfCurrentLevel[i])
                {
                    ScenesToUnload.Remove(ScenesOfNextLevel[y]);
                    ScenesToLoad.Remove(ScenesOfNextLevel[y]);
                }
            }
        }

        //Remove scenes to never unload
        for (int i = 0; i < _NeverDestroyScenes.Count; i++)
        {
            for (int y = 0; y < ScenesToUnload.Count; y++)
            {
                if (_NeverDestroyScenes[i] == ScenesToUnload[y])
                {
                    ScenesToUnload.Remove(ScenesToUnload[y]);
                }
            }
        }
        #endregion

        #region debug log 
        foreach (var item in ScenesToUnload)
        {
            Debug.Log("Scene to unload : " + item);
        }
        foreach (var item in ScenesToLoad)
        {
            Debug.Log("Scene to load : " + item);
        }
        #endregion

        //Add scenes to async operations. 
        AddUnloadToAsyncOp(ScenesToUnload);
        AddLoadToAsyncOp(ScenesToLoad);

        //Async operations for load and unload
        foreach (var AOp in AsyncOperations)
        {
            yield return new WaitUntil(() => AOp.isDone);
            Debug.Log($"WaitUntilChargementOfScenes");
            _chargementUI.AdaptSlider(1, AsyncOperations.Count);
        }

        AsyncOperations.Clear();
    }

    #region usefull functions
    private void AddLoadToAsyncOp(List<string> scenesToLoad)
    {
        foreach (var scene in scenesToLoad)
        {
            AsyncOperations.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            Debug.Log($"{scene} - ToLoad - AddToAsyncOp");
        }
    }

    private void AddUnloadToAsyncOp(List<string> scenesToUnload)
    {
        foreach (var scene in scenesToUnload)
        {
            AsyncOperations.Add(SceneManager.UnloadSceneAsync(scene));
            Debug.Log($"{scene} - ToUnload - AddToAsyncOp");
        }
    }

    private List<string> GetAllOpenedScenes()
    {
        List<string> ListOfOpenedScene = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            ListOfOpenedScene.Add(SceneManager.GetSceneAt(i).name);
            Debug.Log("Scene already open : " + SceneManager.GetSceneAt(i).name);
        }

        return ListOfOpenedScene;
    }

    public void SetActiveScene(string nameOfScene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameOfScene));
    }
    #endregion
}
