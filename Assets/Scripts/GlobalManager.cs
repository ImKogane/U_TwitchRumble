using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public List<ActionInGame> ListActionsInGame = new List<ActionInGame>();

    private EnumClass.GameState gameState;

    [SerializeField] private float waitingForActionTimerDuration;
    [SerializeField] private float waitingForBuffChoiceDuration;
    private float timer = 0;

    private int turnCount;

    private bool currentlyPlayingActions;
    
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
        gameState = EnumClass.GameState.WaitingTurn;
    }

    private void Update()
    {
        HandleGameStates(Time.deltaTime);
    }


    void HandleGameStates(float deltaTime)
    {
        switch (gameState)
        {
            case EnumClass.GameState.WaitingTurn:

                timer += Time.deltaTime;

                if (timer >= waitingForActionTimerDuration)
                {
                    SwitchToState(EnumClass.GameState.ActionTurn);
                    ResetTimer();
                }
                
                break;
            
            case EnumClass.GameState.ActionTurn:
                StartAllActionsInGame();
                break;
            
            case EnumClass.GameState.ChoseBuffTurn:

                //UIManager.Instance.DisplayBuffChoices(turnCount);
                
                timer += Time.deltaTime;

                if (timer >= waitingForBuffChoiceDuration)
                {
                    SwitchToState(EnumClass.GameState.WaitingTurn);
                    ResetTimer();
                }
                
                break;
            
            case EnumClass.GameState.GameEnd:

                break;
            
        }
    }
    
    public void AddActionInGameToList(ActionInGame ActionToAdd)
    {
        //Ici on devra trier si le propri�taire de l'action que l'on ajoute a la liste n'avait pas deja une action dans la liste avant de remettre son action. 
        Debug.Log(ActionToAdd + "have been added to the list of actions");
        ListActionsInGame.Add(ActionToAdd);
    }

    public void StartAllActionsInGame()
    {
        currentlyPlayingActions = true;
        ListActionsInGame[0].LaunchActionInGame();
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    void SwitchToState(EnumClass.GameState nextState)
    {
        gameState = nextState;
    }

    void EndActionTurn()
    {
        currentlyPlayingActions = false;
        turnCount++;
        CheckNextTurn(turnCount);
    }

    void CheckNextTurn(int nextTurn)
    {
        
    }
    
}
