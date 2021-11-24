using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public List<ActionInGame> ListActionsInGame = new List<ActionInGame>();

    private EnumClass.GameState currentGameState;

    [SerializeField] private int actionsTimerDuration;
    [SerializeField] private int buffTimerDuration;
    private int currentTimer;

    [SerializeField] private int[] buffChoiceTurns;
    private int turnCount;

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

    private void Start()
    {
        StartState(EnumClass.GameState.WaitingTurn);
    }

    public void AddActionInGameToList(ActionInGame ActionToAdd)
    {
        //Ici on devra trier si le propri�taire de l'action que l'on ajoute a la liste n'avait pas deja une action dans la liste avant de remettre son action. 
        Debug.Log(ActionToAdd + "have been added to the list of actions");
        ListActionsInGame.Add(ActionToAdd);
    }

    public void StartAllActionsInGame()
    {
        if (ListActionsInGame.Count > 0)
        {
            ListActionsInGame[0].LaunchActionInGame();
        }
        else
        {
            EndActionTurn();
        }
        
    }
    

    void StartState(EnumClass.GameState nextState)
    {
        currentGameState = nextState;
        
        switch (currentGameState)
        {
            case(EnumClass.GameState.WaitingTurn):
                StartCoroutine(ActionsChoiceCoroutine());
                break;
            
            case(EnumClass.GameState.ActionTurn):
                StartAllActionsInGame();
                break;
            
            case(EnumClass.GameState.ChoseBuffTurn):
                StartCoroutine(BuffChoiceCoroutine());
                break;
            
            case(EnumClass.GameState.GameEnd):
                //Display End Screen
                break;
        }
    }

    IEnumerator ActionsChoiceCoroutine()
    {
        //Display Input Choice screen (and players life+name ?)
        
        currentTimer = actionsTimerDuration;

        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            //Update ProgressBar
        }
        
        //HideBuffScreen
        StartState(EnumClass.GameState.ActionTurn);
        
    }

    IEnumerator BuffChoiceCoroutine()
    {
        //SetBuffScreen
        //DisplayBuffScreen

        currentTimer = buffTimerDuration;

        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            //Update ProgressBar
        }
        
        //HideBuffScreen
        StartState(EnumClass.GameState.WaitingTurn);
    }
    
    void EndActionTurn()
    {
        turnCount++;
        //Update the UI turn counter
        CheckNextTurn(turnCount);
    }

    void CheckNextTurn(int nextTurn)
    {
        if (buffChoiceTurns.Length > 0)
        {
            foreach (var buffChoiceTurn in buffChoiceTurns)
            {
                if (nextTurn == buffChoiceTurn)
                {
                    StartState(EnumClass.GameState.ChoseBuffTurn);
                    return;
                }
            }
        }
        
        StartState(EnumClass.GameState.WaitingTurn);
    }
    
}
