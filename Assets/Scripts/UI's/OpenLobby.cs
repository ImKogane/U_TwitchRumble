using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLobby : MonoBehaviour
{
    public void GoToLobby()
    {
        SceneManager.LoadScene("ConnexionTwitch", LoadSceneMode.Single);
    }
}
