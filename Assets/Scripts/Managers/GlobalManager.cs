using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GlobalManager : SingletonMonobehaviour<GlobalManager>
{
    public List<CommandInGame> ListCommandsInGame = new List<CommandInGame>();

    private EnumClass.GameState currentGameState;

    [SerializeField] private float actionsTimerDuration;
    [SerializeField] private float buffTimerDuration;
    [SerializeField] private float startTimerDuration;
    [SerializeField] private Transform WinPoint;
    private float currentTimer;

    //public SO_PanelChoice[] panelChoiceArray;

    public List<int> _turnsToMakeChoice = new List<int>();

    private int turnCount;
    

    public override bool DestroyOnLoad => true;

    #region Unity Basic Events

    public IEnumerator LaunchGame()
    {
        //Set up variable with google sheet datas.
        actionsTimerDuration = GoogleSheetManager.Instance.VariablesGetFromSheet[3];
        buffTimerDuration = GoogleSheetManager.Instance.VariablesGetFromSheet[4];

        turnCount = 1;

        UIManager.Instance.UpdateTurnCount(turnCount);
        UIManager.Instance.DisplayGameScreen(true);
        UIManager.Instance.DisplayEndScreen(false);

        AudioManager.Instance.EnableAmbienceSounds(true);
        UIManager.Instance.DisplayPauseScreen(false);
        GoogleSheetManager.Instance.StartGoogleSheetSaving();

        //Timer wait before start
        currentTimer = startTimerDuration;
        UIManager.Instance.ActivateTimerBar(true);

        while (currentTimer > 0)
        {
            yield return null;
            currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerBar(currentTimer / startTimerDuration);
        }

        //Start game
        StartState(EnumClass.GameState.IntroTurn);
    }

    #endregion
    
    #region Coroutines
    
    IEnumerator ActionsChoiceCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("[Make actions]");
        UIManager.Instance.DisplayPhaseDescription("Attack with the command [!attack].\nMove with commands [!left] [!right] [!top] [!down]. ");
        //UIManager.Instance.DisplayAllPlayersUI(PlayerManager.Instance.PlayerList, true);

        TwitchManager.Instance.playersCanMakeActions = true;

        currentTimer = actionsTimerDuration;

        InputManager.Instance.EnableActionInputs(true);
        UIManager.Instance.ActivateTimerBar(true);

        while (currentTimer > 0)
        {
            yield return null;
            currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerBar(currentTimer/actionsTimerDuration);
        }

        //UIManager.Instance.DisplayAllPlayersUI(PlayerManager.Instance.PlayerList, false);
        UIManager.Instance.ActivateTimerBar(false);
        InputManager.Instance.EnableActionInputs(false);

        TwitchManager.Instance.playersCanMakeActions = false;

        StartState(EnumClass.GameState.ActionTurn);
        
    }
    
    IEnumerator BuffChoiceCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("[Choose a boost]");
        UIManager.Instance.DisplayPhaseDescription("Make a choice with commands [!choice1] [!choice2] [!choice3]");
        UIManager.Instance.ActivateTimerBar(true);

        UIManager.Instance.UpdateChoiceCardsImage();
        UIManager.Instance.DisplayChoiceScreen(true);
        
        InputManager.Instance.EnableChoiceInputs(true);
        TwitchManager.Instance.playersCanMakeChoices = true;

        currentTimer = buffTimerDuration;

        while (currentTimer > 0)
        {
            yield return null;
            currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerBar(currentTimer/buffTimerDuration);
        }
        
        UIManager.Instance.ActivateTimerBar(false);
        UIManager.Instance.DisplayChoiceScreen(false);
        
        TwitchManager.Instance.playersCanMakeChoices = false;
        InputManager.Instance.EnableChoiceInputs(false);

        CheckAllPlayersGetChoice();
        
        StartAllActionsInGame();

        ScriptableManager.Instance.IncreaseChoiceIndexCompteur();

        StartState(EnumClass.GameState.WaitingTurn);
    }

    private IEnumerator IntroTurnCoroutine()
    {
        UIManager.Instance.DisplayPhaseTitle("[Preparation phase]");
        UIManager.Instance.DisplayPhaseDescription("Players are dropped on the map, get ready for the fight.");
        UIManager.Instance.DisplayChoiceScreen(false);

        foreach (var item in PlayerManager.Instance.PlayerList)
        {
            Debug.Log("Player : " + item);
            Debug.Log("Tile of this player : " + item.CurrentTile);
            item.gameObject.transform.DOMove(item.CurrentTile.transform.position, 1).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(2);
        }

        StartState(EnumClass.GameState.ChoseBuffTurn);
    }

    #endregion

    #region ActionsInGame Handling

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
            UIManager.Instance.DisplayPhaseTitle("[Action Phase]");
            UIManager.Instance.DisplayPhaseDescription("Wait, all actions are running.");
            ListCommandsInGame[0].LaunchActionInGame();
        }
        else
        {
            Debug.Log("Personne n'a choisi d'action pendant ce tour ?");
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
                PlayerManager.Instance.ManagePlayersDebuffs();
                StartAllActionsInGame();
                break;
            
            case(EnumClass.GameState.ChoseBuffTurn):
                StartCoroutine(BuffChoiceCoroutine());
                break;

            case (EnumClass.GameState.IntroTurn):
                StartCoroutine(IntroTurnCoroutine());
                break;
            
            case(EnumClass.GameState.GameEnd):
                EndGame();
                break;
        }
    }
    
    public void EndActionTurn()
    {
        StartCoroutine(WaitingBeforeLaunchState());
    }

    public IEnumerator WaitingBeforeLaunchState()
    {
        yield return new WaitForSeconds(3);

        int remainingPlayers = PlayerManager.Instance.GetPlayerCount();

        if (remainingPlayers > 1)
        {
            turnCount++;
            UIManager.Instance.UpdateTurnCount(turnCount);
            CheckNextTurn(turnCount);
        }
        else
        {
            StartState(EnumClass.GameState.GameEnd);
        }
    }

    void CheckNextTurn(int nextTurn)
    {
        foreach (int choice in _turnsToMakeChoice)
        {
            if (choice == turnCount)
            {
                StartState(EnumClass.GameState.ChoseBuffTurn);
                return;
            }
        }

        StartState(EnumClass.GameState.WaitingTurn);
    }


    public EnumClass.GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public int GetCurrentTurn()
    {
        return turnCount;
    }

    #endregion
    
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

    public int GetRandomChoiceIndex()
    {
        int index = (int) Random.Range(0, 3);
        return index;
    }

    private void CheckAllPlayersGetChoice()
    {
        foreach (var player in PlayerManager.Instance.PlayerList)
        {
            List<CommandInGame> playerCommands = FindPlayerCommands(player);

            if (playerCommands.Count == 0)
            {
                InputManager.Instance.ChoiceCommand(player, ScriptableManager.Instance.GetRandomIndexChoice());
            }
        }
    }
    
    public void InsertCommandInList(int index, CommandInGame command)
    {
        if (ListCommandsInGame.Count>1)
        {
            ListCommandsInGame.Insert(index, command);
        }
        else
        {
            ListCommandsInGame.Add(command);
        }
    }
    
    public void EndGame()
    {
        UIManager.Instance.EndGameUI();

        //TODO : mettre dans une seule méthode !!
        
        PlayerManager.Instance.GetLastPlayer().transform.position = WinPoint.position;
        PlayerManager.Instance.GetLastPlayer().ResetPlayerRotation();
        PlayerManager.Instance.GetLastPlayer().CanvasVisibility(false);
        PlayerManager.Instance.GetLastPlayer()._animator.SetBool("IsFalling", false);
        PlayerManager.Instance.PlayerList.Clear();
    }

    public void SetGamePause(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
            UIManager.Instance.DisplayPauseScreen(state);
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.DisplayPauseScreen(state);
        }
        
    }
}
