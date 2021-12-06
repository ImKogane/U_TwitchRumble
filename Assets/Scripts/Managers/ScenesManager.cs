using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;


public class ScenesManager : SingletonMonobehaviour<ScenesManager>
{

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public List<string> MainMenuScenesNames = new List<string>();
    public List<string> LobbyScenesNames = new List<string>();
    public List<string> GameScenesNames = new List<string>();

    public override bool DestroyOnLoad => false;

    public void LaunchMainMenuScenes()
    {
        StartCoroutine(NextLevelCoroutine(MainMenuScenesNames));
    }

    public void LaunchLobbyScenes()
    {
        StartCoroutine(NextLevelCoroutine(LobbyScenesNames));
    }

    public void LaunchGameScenes()
    {
        StartCoroutine(NextLevelCoroutine(GameScenesNames));
    }


    public void SetActiveScene(string nameOfScene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameOfScene));
    }


    private IEnumerator NextLevelCoroutine(List<string> scenesTooad)
    {
        List<string> scenesToUnload = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scenesToUnload.Add(SceneManager.GetSceneAt(i).name);
        }

        yield return StartCoroutine(LoadAll(scenesTooad));
        yield return StartCoroutine(UnloadAll(scenesToUnload));
        Debug.Log("Done");
    }

    private IEnumerator UnloadAll(List<string> scenesToUnload)
    {
        List<AsyncOperation> pendingOps = new List<AsyncOperation>();

        foreach (var scene in scenesToUnload)
        {
            pendingOps.Add(SceneManager.UnloadSceneAsync(scene));
        }

        foreach (var op in pendingOps)
        {
            yield return new WaitUntil(() => op.isDone);
        }

        Debug.Log("Done unloading");
    }


    private IEnumerator LoadAll(List<string> scenesToLoad)
    {
        List<AsyncOperation> pendingOps = new List<AsyncOperation>();

        foreach (var scene in scenesToLoad)
        {
            pendingOps.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        }

        foreach (var op in pendingOps)
        {
            yield return new WaitUntil(() => op.isDone);
        }

        Debug.Log("Done loading");
    }
}
