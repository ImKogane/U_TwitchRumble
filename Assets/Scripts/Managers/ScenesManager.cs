using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;


public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public List<string> MainMenuScenesNames = new List<string>();
    public List<string> LobbyScenesNames = new List<string>();
    public List<string> GameScenesNames = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LaunchMainMenuScenes()
    {
        LoadAllScenesOfAList(MainMenuScenesNames);
    }

    public void LaunchLobbyScenes()
    {
        LoadAllScenesOfAList(LobbyScenesNames);
    }

    public void LaunchGameScenes()
    {
        LoadAllScenesOfAList(GameScenesNames);
    }

    private void LoadAllScenesOfAList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0)
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync(list[i]));
            }
            else
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync(list[i], LoadSceneMode.Additive));
            }
        }
    }

    public void SetActiveScene(string nameOfScene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameOfScene));
    }
}
