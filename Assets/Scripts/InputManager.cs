using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private bool inputsEnabled;
    
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

    void Update()
    {
        if (!inputsEnabled) return;
        InputPlayerLocal();
    }
    
    public void EnableInputs(bool value)
    {
        inputsEnabled = value;
    }

    void InputPlayerLocal()
    {
        //Actions de déplacements. 
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCommand(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCommand(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCommand(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCommand(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Left);
        }

        //Action d'attaque.
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttackCommand(PlayerManager.Instance.PlayerList[0]);
        }
    }

    public void MoveCommand(Player player, EnumClass.Direction direction)
    {
        CommandMoving ActionToDo = null;
        switch (direction)
        {
            case EnumClass.Direction.Up:
                ActionToDo = new CommandMoving(player, EnumClass.Direction.Up);
                break;
            case EnumClass.Direction.Down:
                ActionToDo = new CommandMoving(player, EnumClass.Direction.Down);
                break;
            case EnumClass.Direction.Right:
                ActionToDo = new CommandMoving(player, EnumClass.Direction.Right);
                break;
            case EnumClass.Direction.Left:
                ActionToDo = new CommandMoving(player, EnumClass.Direction.Left);
                break;
            default:
                break;
        }
        GlobalManager.Instance.AddActionInGameToList(ActionToDo);
    }

    public void AttackCommand(Player player)
    {
        CommandAttack ActionToDo = new CommandAttack(player);
        GlobalManager.Instance.AddActionInGameToList(ActionToDo);
    }


}
