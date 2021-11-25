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
        HandleInputs();
    }
    
    public void EnableInputs(bool value)
    {
        inputsEnabled = value;
    }

    void HandleInputs()
    {
        //Actions de déplacements. 
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //On fait une action toujours sur le meme player il faudra changer �a en fonction du player qui a rentrer l'input. 
            ActionMoving ActionToDo = new ActionMoving(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Up);
            GlobalManager.Instance.AddActionInGameToList(ActionToDo);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActionMoving ActionToDo = new ActionMoving(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Down);
            GlobalManager.Instance.AddActionInGameToList(ActionToDo);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActionMoving ActionToDo = new ActionMoving(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Right);
            GlobalManager.Instance.AddActionInGameToList(ActionToDo);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActionMoving ActionToDo = new ActionMoving(PlayerManager.Instance.PlayerList[0], EnumClass.Direction.Left);
            GlobalManager.Instance.AddActionInGameToList(ActionToDo);
        }

        //Action d'attaque.
        if (Input.GetKeyDown(KeyCode.A))
        {
            ActionAttack ActionToDo = new ActionAttack(PlayerManager.Instance.PlayerList[0]);
            GlobalManager.Instance.AddActionInGameToList(ActionToDo);
        }
    }
    
}
