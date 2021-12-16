using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;


public class BootstrapManager : SingletonMonobehaviour<BootstrapManager>
{
    [Header("Non destructible scenes")]
    public List<string> _NeverDestroyScenes = new List<string>();

    [Header("Starting Level")]
    public SO_level _startLevel;

    [Header("Ref to UI")]
    public ChargingUI _chargementUI;

    private AudioClip _musicToPlayWhenLevelLoaded;
    List<AsyncOperation> _asyncOperations = new List<AsyncOperation>();

    public override bool DestroyOnLoad => true;

    public void Start()
    {
        List<string> StartGameScenesNames = new List<string>();
        StartGameScenesNames.AddRange(_NeverDestroyScenes);
        StartGameScenesNames.AddRange(_startLevel.LevelsToCharge);
        StartChargeALevel(StartGameScenesNames, _startLevel._levelMusic);
    }

    #region Charge levels
    public void StartChargeALevel(List<string> scenesOfNextLevel, AudioClip clip)
    {
        StartCoroutine(_chargementUI.FadeInCoroutine(scenesOfNextLevel));
        _musicToPlayWhenLevelLoaded = clip;
    }

    //This function is call after the fade In of the canvas.
    public IEnumerator ChargeLvl(List<string> scenesOfNextLevel)
    {
        List<string> scenesOfCurrentLevel = GetAllOpenedScenes();

        List<string> ScenesToUnload = new List<string>();
        ScenesToUnload.AddRange(scenesOfCurrentLevel);

        List<string> ScenesToLoad = new List<string>();
        ScenesToLoad.AddRange(scenesOfNextLevel);

        //Remove communes scenes 
        for (int i = 0; i < scenesOfCurrentLevel.Count; i++)
        {
            for (int y = 0; y < scenesOfNextLevel.Count; y++)
            {
                if (scenesOfNextLevel[y] == scenesOfCurrentLevel[i])
                {
                    ScenesToUnload.Remove(scenesOfNextLevel[y]);
                    ScenesToLoad.Remove(scenesOfNextLevel[y]);
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

        //Add scenes to async operations. 
        AddUnloadToAsyncOp(ScenesToUnload);
        AddLoadToAsyncOp(ScenesToLoad);

        //Async operations for load and unload
        foreach (var AOp in _asyncOperations)
        {
            yield return new WaitUntil(() => AOp.isDone);
            Debug.Log($"WaitUntilChargementOfScenes");
            _chargementUI.AdaptSlider(1, _asyncOperations.Count);
        }

        _asyncOperations.Clear();

        AudioManager.Instance.PlayBGMusic(_musicToPlayWhenLevelLoaded);
    }
    #endregion

    #region usefull functions
    private void AddLoadToAsyncOp(List<string> scenesToLoad)
    {
        foreach (var scene in scenesToLoad)
        {
            _asyncOperations.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            Debug.Log($"{scene} - ToLoad - AddToAsyncOp");
        }
    }

    private void AddUnloadToAsyncOp(List<string> scenesToUnload)
    {
        foreach (var scene in scenesToUnload)
        {
            _asyncOperations.Add(SceneManager.UnloadSceneAsync(scene));
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
