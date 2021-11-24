using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public List<ActionInGame> ListActionsInGame = new List<ActionInGame>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddActionInGameToList(ActionInGame ActionToAdd)
    {
        //Ici on devra trier si le propriétaire de l'action que l'on ajoute a la liste n'avait pas deja une action dans la liste avant de remettre son action. 
        Debug.Log(ActionToAdd + "have been added to the list of actions");
        ListActionsInGame.Add(ActionToAdd);
    }

    public void StartAllActionsInGame()
    {
        ListActionsInGame[0].LaunchActionInGame();
    }

}
