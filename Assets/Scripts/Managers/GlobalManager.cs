using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GlobalManager : SingletonMonobehaviour<GlobalManager>
{
    public override bool DestroyOnLoad => true;

    private EnumClass.GameState _currentGameState;
    private float _currentTimer;
    private int _turnCount;

    [Header("Expose Variables")]
    [SerializeField] private float _actionsTimerDuration;
    [SerializeField] private float _buffTimerDuration;
    [SerializeField] private float _startTimerDuration;
    [SerializeField] private Transform _winPoint;
    public List<int> _listTurnsToMakeChoice = new List<int>();

    [Header("Text Game Variables")]
    public string _phaseActionsTitle;
    public string _phaseActionsParagraph;
    public string _phaseChoiceTitle;
    public string _phaseChoiceParagraph;
    public string _phaseIntroTitle;
    public string _phaseIntroParagraph;


    private void Start()
    {
        StartGameManager.Instance.LaunchGame();
    }

    #region Coroutines
    public IEnumerator LaunchNewGameCoroutine()
    {
        //Get ref to managers
        UIManager UiManager = UIManager.Instance;
        GoogleSheetManager GoogleManager = GoogleSheetManager.Instance;

        _turnCount = 1;
        
        //Set up variable with google sheet datas.
        _actionsTimerDuration = GoogleManager._variablesGetFromSheet[3];
        _buffTimerDuration = GoogleManager._variablesGetFromSheet[4];

        //Saving datas with async methods. 
        GoogleManager.StartGoogleSheetSaving();

        //Manage UI to display
        UiManager.UpdateTurnCount(_turnCount);
        UiManager.DisplayGameScreen(true);
        UiManager.DisplayEndScreen(false);
        UiManager.DisplayPauseScreen(false);

        AudioManager.Instance.EnableAmbienceSounds(true);

        //Manage timer
        _currentTimer = _startTimerDuration;
        UiManager.ActivateTimerBar(true);
        while (_currentTimer > 0)
        {
            yield return null;
            _currentTimer -= Time.deltaTime;
            UiManager.UpdateTimerBar(_currentTimer / _startTimerDuration);
        }

        //Start game
        StartState(EnumClass.GameState.IntroTurn);
    }

    IEnumerator ActionsChoiceCoroutine()
    {
        //Get ref to managers
        UIManager UiManager = UIManager.Instance;

        //Manage UI to display
        UiManager.DisplayPhaseTitle(_phaseActionsTitle);
        UiManager.DisplayPhaseDescription(_phaseActionsParagraph);
        UiManager.ActivateTimerBar(true);

        //Players can make actions.
        TwitchManager.Instance.playersCanMakeActions = true;
        InputManager.Instance.EnableActionInputs(true);

        //Manage timer
        _currentTimer = _actionsTimerDuration;
        while (_currentTimer > 0)
        {
            yield return null;
            _currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerBar(_currentTimer/_actionsTimerDuration);
        }

        UiManager.ActivateTimerBar(false);

        //Players can't make actions.
        InputManager.Instance.EnableActionInputs(false);
        TwitchManager.Instance.playersCanMakeActions = false;

        StartState(EnumClass.GameState.ActionTurn);
    }
    
    IEnumerator BuffChoiceCoroutine()
    {
        //Get ref to managers
        UIManager UiManager = UIManager.Instance;

        //Manage UI to display
        UiManager.DisplayPhaseTitle(_phaseChoiceTitle);
        UiManager.DisplayPhaseDescription(_phaseChoiceParagraph);
        UiManager.ActivateTimerBar(true);
        UiManager.UpdateChoiceCardsImage();
        UiManager.DisplayChoiceScreen(true);
        
        //Players can make choices
        InputManager.Instance.EnableChoiceInputs(true);
        TwitchManager.Instance.playersCanMakeChoices = true;

        //Manage timer
        _currentTimer = _buffTimerDuration;
        while (_currentTimer > 0)
        {
            yield return null;
            _currentTimer -= Time.deltaTime;
            UiManager.UpdateTimerBar(_currentTimer/_buffTimerDuration);
        }

        //Players stop make choices
        TwitchManager.Instance.playersCanMakeChoices = false;
        InputManager.Instance.EnableChoiceInputs(false);

        //Be sure that everybody have a choice
        CheckAllPlayersGetChoice();
        yield return new WaitForSeconds(2);

        UiManager.ActivateTimerBar(false);
        UiManager.DisplayChoiceScreen(false);

        CommandManager.Instance.StartAllCommands();
        ScriptableManager.Instance.IncreaseChoiceIndexCompteur();

        StartState(EnumClass.GameState.WaitingTurn);
    }

    private IEnumerator IntroTurnCoroutine()
    {
        //Get ref to managers
        UIManager UiManager = UIManager.Instance;

        UiManager.DisplayPhaseTitle(_phaseIntroTitle);
        UiManager.DisplayPhaseDescription(_phaseIntroParagraph);
        UiManager.DisplayChoiceScreen(false);

        //All the players will be dropped on the board
        foreach (var item in PlayerManager.Instance._listPlayers)
        {
            Debug.Log("Player : " + item);
            Debug.Log("Tile of this player : " + item._currentTile);
            item.gameObject.transform.DOMove(item._currentTile.transform.position, 1).SetEase(Ease.OutSine);

            yield return new WaitForSeconds(2);
        }

        StartState(EnumClass.GameState.ChoseBuffTurn);
    }

    public IEnumerator LoadSavedTurn(int turn)
    {
        //Get ref to managers
        UIManager UiManager = UIManager.Instance;

        _turnCount = turn;
        AdaptChoiceIndexCompteurToLoadedTurn(_turnCount);

        //Manage UI display
        UiManager.UpdateTurnCount(_turnCount);
        UiManager.DisplayGameScreen(true);
        UiManager.DisplayEndScreen(false);
        UiManager.DisplayPauseScreen(false);
        UiManager.ActivateTimerBar(true);

        AudioManager.Instance.EnableAmbienceSounds(true);
        GoogleSheetManager.Instance.StartGoogleSheetSaving();

        //Manage Timer
        _currentTimer = _startTimerDuration;
        while (_currentTimer > 0)
        {
            yield return null;
            _currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerBar(_currentTimer / _startTimerDuration);
        }

        CheckNextTurn();
    }

    #endregion

    #region GameState Handling
    
    void StartState(EnumClass.GameState nextState)
    {
        _currentGameState = nextState;
        
        switch (_currentGameState)
        {
            case(EnumClass.GameState.WaitingTurn):
                StartCoroutine(ActionsChoiceCoroutine());
                break;
            
            case(EnumClass.GameState.ActionTurn):
                PlayerManager.Instance.ManagePlayersDebuffs();
                CommandManager.Instance.StartAllCommands();
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
        WaitingBeforeLaunchState();
    }

    public void WaitingBeforeLaunchState()
    {
        int remainingPlayers = PlayerManager.Instance.GetPlayerCount();

        if (remainingPlayers > 1)
        {
            _turnCount++;
            UIManager.Instance.UpdateTurnCount(_turnCount);
            CheckNextTurn();
        }
        else
        {
            StartState(EnumClass.GameState.GameEnd);
        }
    }

    void CheckNextTurn()
    {
        foreach (int choice in _listTurnsToMakeChoice)
        {
            if (choice == _turnCount)
            {
                StartState(EnumClass.GameState.ChoseBuffTurn);
                return;
            }
        }

        StartState(EnumClass.GameState.WaitingTurn);
    }


    public EnumClass.GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

    public int GetCurrentTurn()
    {
        return _turnCount;
    }

    #endregion
    
    


    #region Choices
    public int GetRandomChoiceIndex()
    {
        int index = (int) Random.Range(0, 3);
        return index;
    }

    private void CheckAllPlayersGetChoice()
    {
        foreach (var player in PlayerManager.Instance._listPlayers)
        {
            List<CommandInGame> playerCommands = CommandManager.Instance.FindPlayerCommands(player);

            if (playerCommands.Count == 0)
            {
                InputManager.Instance.ChoiceCommand(player, ScriptableManager.Instance.GetRandomIndexChoice());
            }
        }
    }

    public bool DoesAllPlayersHaveChoice()
    {
        foreach (var player in PlayerManager.Instance._listPlayers)
        {
            List<CommandInGame> playerCommands = CommandManager.Instance.FindPlayerCommands(player);

            if (playerCommands.Count == 0)
            {
                return false;
            }
        }
        return true;
    }

    private void AdaptChoiceIndexCompteurToLoadedTurn(int turn)
    {
        foreach (int choiceTurn in _listTurnsToMakeChoice)
        {
            if (turn > choiceTurn)
            {
                ScriptableManager.Instance.IncreaseChoiceIndexCompteur();
            }
        }
    }

    #endregion

    #region EndGame/PauseGame
    public void EndGame()
    {
        UIManager.Instance.EndGameUI();

        //TODO : mettre dans une seule méthode !!
        
        PlayerManager.Instance.GetLastPlayer().transform.position = _winPoint.position;
        PlayerManager.Instance.GetLastPlayer().ResetPlayerRotation();
        PlayerManager.Instance.GetLastPlayer().CanvasVisibility(false);
        PlayerManager.Instance.GetLastPlayer()._animatorComponent.SetBool("IsFalling", false);

        //Manage the last player for victory ending.
        Player endGamePlayer = PlayerManager.Instance.GetLastPlayer();
        endGamePlayer.transform.position = _winPoint.position;
        endGamePlayer.ResetPlayerRotation();
        endGamePlayer.CanvasVisibility(false);
        endGamePlayer._animatorComponent.SetBool("IsFalling", false);
        PlayerManager.Instance._listPlayers.Clear();

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
    #endregion
}
