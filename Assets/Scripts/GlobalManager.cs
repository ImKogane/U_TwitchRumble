using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public List<CommandInGame> ListCommandsInGame = new List<CommandInGame>();

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
        StartState(EnumClass.GameState.ChoseBuffTurn);
        turnCount = 1;
        UIManager.Instance.UpdateTurnCount(turnCount);
        UIManager.Instance.DisplayGameScreen(true);
        UIManager.Instance.DisplayEndScreen(false);
    }

    public void AddActionInGameToList(CommandInGame ActionToAdd)
    {
        //Ici on devra trier si le propri�taire de l'action que l'on ajoute a la liste n'avait pas deja une action dans la liste avant de remettre son action. 
        
        Debug.Log(ActionToAdd + "have been added to the list of actions");
        ListCommandsInGame.Add(ActionToAdd);
    }

    public void StartAllActionsInGame()
    {
        if (ListCommandsInGame.Count > 0)
        {
            UIManager.Instance.DisplayPhaseTitle("Phase d'action");
            UIManager.Instance.DisplayPhaseDescription("Déroulement des actions choisies précédemment");
            ListCommandsInGame[0].LaunchActionInGame();
        }
        else
        {
            Debug.Log("Personne n'a choisi d'action pendant ce tour !");
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
                UIManager.Instance.DisplayEndScreen(false);
                UIManager.Instance.DisplayEndScreen(true);
                break;
        }
    }

    IEnumerator ActionsChoiceCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("Phase du choix des actions");
        UIManager.Instance.DisplayPhaseDescription("Choisissez votre prochain déplacement avec les flèches directionnelles, et A pour attaquer.");

        TwitchManager.Instance.playersCanMakeActions = true;

        currentTimer = actionsTimerDuration;

        InputManager.Instance.EnableInputs(true);
        UIManager.Instance.ActivateTimerBar(true);
        
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            UIManager.Instance.UpdateTimerBar((float)currentTimer/actionsTimerDuration);
        }
        
        UIManager.Instance.ActivateTimerBar(false);
        InputManager.Instance.EnableInputs(false);

        TwitchManager.Instance.playersCanMakeActions = false;

        StartState(EnumClass.GameState.ActionTurn);
        
    }

    IEnumerator BuffChoiceCoroutine()
    {
        //SetBuffScreen
        UIManager.Instance.DisplayPhaseTitle("Phase d'amélioration");
        UIManager.Instance.DisplayPhaseDescription("Votre décision affectera grandement votre manière de jouer");
        UIManager.Instance.ActivateTimerBar(true);
        
        currentTimer = buffTimerDuration;

        
        
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            UIManager.Instance.UpdateTimerBar((float)currentTimer/buffTimerDuration);
        }
        
        UIManager.Instance.ActivateTimerBar(false);
        
        //HideBuffScreen
        StartState(EnumClass.GameState.WaitingTurn);
    }
    
    public void EndActionTurn()
    {
        turnCount++;
        UIManager.Instance.UpdateTurnCount(turnCount);
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

    public List<CommandInGame> FindPlayerCommands(Player ownerOfCommands)
    {
        List<CommandInGame> listToReturn = new List<CommandInGame>();

        foreach (CommandInGame command in ListCommandsInGame)
        {
            if (command.OwnerPlayer == ownerOfCommands)
            {
                listToReturn.Add(command);
            }
        }

        return listToReturn;
    }

    public void DestroyAllCommandsOfPlayer(Player ownerOfCommands)
    {
        List<CommandInGame> listToDestroy = FindPlayerCommands(ownerOfCommands);

        for (int i = 0; i < listToDestroy.Count; i++)
        {
            listToDestroy[i].DestroyCommand();

            if (ListCommandsInGame.Contains(listToDestroy[i]))
            {
                ListCommandsInGame.Remove(listToDestroy[i]);
            }
        }
    }

    public void RemoveMoveCommandOfPlayer(Player ownerOfCommands)
    {
        List<CommandInGame> AllCommandsOfPlayer = FindPlayerCommands(ownerOfCommands);

        for (int i = 0; i < AllCommandsOfPlayer.Count; i++)
        {
            if (ListCommandsInGame.Contains(AllCommandsOfPlayer[i]) && AllCommandsOfPlayer[i] is CommandMoving)
            {
                AllCommandsOfPlayer[i].DestroyCommand();
                ListCommandsInGame.Remove(AllCommandsOfPlayer[i]);
            }
        }
    }

    public void RemoveAttackCommandOfPlayer(Player ownerOfCommands)
    {
        List<CommandInGame> AllCommandsOfPlayer = FindPlayerCommands(ownerOfCommands);

        for (int i = 0; i < AllCommandsOfPlayer.Count; i++)
        {
            if (ListCommandsInGame.Contains(AllCommandsOfPlayer[i]) && AllCommandsOfPlayer[i] is CommandAttack)
            {
                AllCommandsOfPlayer[i].DestroyCommand();
                ListCommandsInGame.Remove(AllCommandsOfPlayer[i]);
            }
        }
    }

}
