using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public List<CommandInGame> ListActionsInGame = new List<CommandInGame>();

    private EnumClass.GameState currentGameState;

    [SerializeField] private int actionsTimerDuration;
    [SerializeField] private int buffTimerDuration;
    private int currentTimer;

    public SO_Choice[] choicesArray;
    
    private int turnCount;

    #region Unity Basic Events
    
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
        StartState(EnumClass.GameState.ChoseBuffTurn);
        turnCount = 1;
        UIManager.Instance.UpdateTurnCount(turnCount);
        UIManager.Instance.DisplayGameScreen(true);
        UIManager.Instance.DisplayEndScreen(false);
    }

    #endregion
    
    #region Coroutines
    
    IEnumerator ActionsChoiceCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("Phase du choix des actions");
        UIManager.Instance.DisplayPhaseDescription("Choisissez votre prochain déplacement avec les flèches directionnelles, et A pour attaquer.");

        TwitchManager.Instance.playersCanMakeActions = true;

        currentTimer = actionsTimerDuration;

        InputManager.Instance.EnableActionInputs(true);
        UIManager.Instance.ActivateTimerBar(true);
        
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            UIManager.Instance.UpdateTimerBar((float)currentTimer/actionsTimerDuration);
        }
        
        UIManager.Instance.ActivateTimerBar(false);
        InputManager.Instance.EnableActionInputs(false);

        TwitchManager.Instance.playersCanMakeActions = false;

        StartState(EnumClass.GameState.ActionTurn);
        
    }
    
    IEnumerator BuffChoiceCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("Phase d'amélioration");
        UIManager.Instance.DisplayPhaseDescription("Votre décision affectera grandement votre manière de jouer");
        UIManager.Instance.ActivateTimerBar(true);
        
        InputManager.Instance.EnableChoiceInputs(true);
        TwitchManager.Instance.playersCanMakeChoices = true;
        
        currentTimer = buffTimerDuration;

        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            UIManager.Instance.UpdateTimerBar((float)currentTimer/buffTimerDuration);
        }
        
        UIManager.Instance.ActivateTimerBar(false);
        
        TwitchManager.Instance.playersCanMakeChoices = false;
        InputManager.Instance.EnableChoiceInputs(false);
        
        StartState(EnumClass.GameState.WaitingTurn);
    }
    
    
    #endregion

    #region ActionsInGame Handling
    
    public void AddActionInGameToList(CommandInGame ActionToAdd)
    {
        //Ici on devra trier si le propri�taire de l'action que l'on ajoute a la liste n'avait pas deja une action dans la liste avant de remettre son action. 
        
        Debug.Log(ActionToAdd + "have been added to the list of actions");
        ListActionsInGame.Add(ActionToAdd);
    }

    public void StartAllActionsInGame()
    {
        if (ListActionsInGame.Count > 0)
        {
            UIManager.Instance.DisplayPhaseTitle("Phase d'action");
            UIManager.Instance.DisplayPhaseDescription("Déroulement des actions choisies précédemment");
            ListActionsInGame[0].LaunchActionInGame();
        }
        else
        {
            Debug.Log("Personne n'a choisi d'action pendant ce tour !");
            EndActionTurn();
        }
        
    }
    
    #endregion
    
    #region GameState Handling
    
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
                UIManager.Instance.DisplayEndScreen(false);
                UIManager.Instance.DisplayEndScreen(true);
                break;
        }
    }
    
    public void EndActionTurn()
    {
        turnCount++;
        UIManager.Instance.UpdateTurnCount(turnCount);
        CheckNextTurn(turnCount);
    }

    void CheckNextTurn(int nextTurn)
    {
        foreach (SO_Choice choice in choicesArray)
        {
            if (choice.turnToTakeEffect == turnCount)
            {
                StartState(EnumClass.GameState.ChoseBuffTurn);
                return;
            }
        }

        StartState(EnumClass.GameState.WaitingTurn);

    }

    EnumClass.GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public int GetCurrentTurn()
    {
        return turnCount;
    }

    #endregion
    
}
