using System.Collections.Generic;
using UnityEngine;

public class CommandManager : SingletonMonobehaviour<CommandManager>
{
    public override bool DestroyOnLoad => true;

    public List<CommandInGame> _listCommandsInGame = new List<CommandInGame>();

    #region Start And End Of Commands
    public void ManageEndOfCommand(CommandInGame command)
    {
        if (_listCommandsInGame.Count > 0 && _listCommandsInGame[0] == command) //Somme nous bien l'action a la base de la liste
        {
            command.DestroyCommand();
            
            if (_listCommandsInGame.Contains(command))
            {
                _listCommandsInGame.Remove(command); //On s'enleve de la liste. 
            }
            
            if (_listCommandsInGame.Count > 0)
            {
                _listCommandsInGame[0].LaunchActionInGame(); //On lance la prochaine action. 
            }
            else
            {
                if (GlobalManager.Instance.GetCurrentGameState() == EnumClass.GameState.ActionTurn) GlobalManager.Instance.EndActionTurn();
            }
        }
        else if (_listCommandsInGame.Count == 0)
        {
            if (GlobalManager.Instance.GetCurrentGameState() == EnumClass.GameState.ActionTurn) GlobalManager.Instance.EndActionTurn();
        }
    }

    public void StartAllCommands()
    {
        if (_listCommandsInGame.Count > 0)
        {
            UIManager.Instance.DisplayPhaseTitle("[Action Phase]");
            UIManager.Instance.DisplayPhaseDescription("Wait, all actions are running.");
            _listCommandsInGame[0].LaunchActionInGame();
        }
        else
        {
            GlobalManager.Instance.EndActionTurn();
        }
    }
    #endregion

    #region Specific actions on commands
    public List<CommandInGame> FindPlayerCommands(Player ownerOfCommands)
    {
        List<CommandInGame> listToReturn = new List<CommandInGame>();

        foreach (CommandInGame command in _listCommandsInGame)
        {
            if (command._ownerPlayer == ownerOfCommands)
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

            if (_listCommandsInGame.Contains(listToDestroy[i]))
            {
                _listCommandsInGame.Remove(listToDestroy[i]);
            }
        }
    }

    public void DestroyAllCommandsOfDeadPlayer(Player ownerOfCommands)
    {
        DestroyAllCommandsOfPlayer(ownerOfCommands);

        if (_listCommandsInGame.Count > 0)
        {
            _listCommandsInGame[0].LaunchActionInGame();
        }
        else
        {
            if (GlobalManager.Instance.GetCurrentGameState() == EnumClass.GameState.ActionTurn) GlobalManager.Instance.EndActionTurn();
        }
    }

    public void RemoveMoveCommandOfPlayer(Player ownerOfCommands)
    {
        List<CommandInGame> AllCommandsOfPlayer = FindPlayerCommands(ownerOfCommands);

        for (int i = 0; i < AllCommandsOfPlayer.Count; i++)
        {
            if (_listCommandsInGame.Contains(AllCommandsOfPlayer[i]) && AllCommandsOfPlayer[i] is CommandMoving)
            {
                AllCommandsOfPlayer[i].DestroyCommand();
                _listCommandsInGame.Remove(AllCommandsOfPlayer[i]);
            }
        }
    }

    public void RemoveAttackCommandOfPlayer(Player ownerOfCommands)
    {
        List<CommandInGame> AllCommandsOfPlayer = FindPlayerCommands(ownerOfCommands);

        for (int i = 0; i < AllCommandsOfPlayer.Count; i++)
        {
            if (_listCommandsInGame.Contains(AllCommandsOfPlayer[i]) && AllCommandsOfPlayer[i] is CommandAttack)
            {
                AllCommandsOfPlayer[i].DestroyCommand();
                _listCommandsInGame.Remove(AllCommandsOfPlayer[i]);
            }
        }
    }

    public void InsertCommandInList(int index, CommandInGame command)
    {
        if (_listCommandsInGame.Count > 1)
        {
            _listCommandsInGame.Insert(index, command);
        }
        else
        {
            _listCommandsInGame.Add(command);
        }
    }

    public void AddCommandToList(CommandInGame ActionToAdd)
    {
        _listCommandsInGame.Add(ActionToAdd);
    }
    #endregion
}
