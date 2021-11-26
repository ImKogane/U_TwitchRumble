using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private bool actionInputsEnabled;
    private bool choiceInputsEnabled;
    
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
        if (!actionInputsEnabled) InputPlayerLocal();
        if (!choiceInputsEnabled) ChoiceInputPlayerLocal();

    }
    
    public void EnableActionInputs(bool value)
    {
        actionInputsEnabled = value;
    }

    public void EnableChoiceInputs(bool value)
    {
        choiceInputsEnabled = value;
    }

    void ChoiceInputPlayerLocal()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChoiceCommand(PlayerManager.Instance.PlayerList[0], EnumClass.ChosenCard.Card1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChoiceCommand(PlayerManager.Instance.PlayerList[0], EnumClass.ChosenCard.Card2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChoiceCommand(PlayerManager.Instance.PlayerList[0], EnumClass.ChosenCard.Card3);
        }
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
        GlobalManager.Instance.RemoveMoveCommandOfPlayer(player);

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
        GlobalManager.Instance.RemoveAttackCommandOfPlayer(player);
        CommandAttack ActionToDo = new CommandAttack(player);
        GlobalManager.Instance.AddActionInGameToList(ActionToDo);
    }

    public void ChoiceCommand(Player player, EnumClass.ChosenCard chosenCard)
    {
        int currentTurn = GlobalManager.Instance.GetCurrentTurn();

        SO_Choice[] choicesArray = GlobalManager.Instance.choicesArray;
        
        foreach (SO_Choice choice in choicesArray)
        {
            if (currentTurn == choice.turnToTakeEffect && choice.choiceType == EnumClass.ChoiceType.Weapon)
            {
                CommandWeaponChoice WeaponToChoose = null;
                
                switch (chosenCard)
                {
                    case(EnumClass.ChosenCard.Card1):
                        WeaponToChoose = new CommandWeaponChoice(player, EnumClass.WeaponType.Hammer);
                        break;
                    
                    case(EnumClass.ChosenCard.Card2):
                        WeaponToChoose = new CommandWeaponChoice(player, EnumClass.WeaponType.Scythe);
                        break;
                    
                    case(EnumClass.ChosenCard.Card3):
                        WeaponToChoose = new CommandWeaponChoice(player, EnumClass.WeaponType.Rifle);
                        break;
                }
                
                GlobalManager.Instance.AddActionInGameToList(WeaponToChoose);
            }
            
            else if (currentTurn == choice.turnToTakeEffect && choice.choiceType == EnumClass.ChoiceType.WeaponBuff)
            {
                //TODO : CommandWeaponBuffChoice
            }
            
            else if (currentTurn == choice.turnToTakeEffect && choice.choiceType == EnumClass.ChoiceType.MovementBuff)
            {
                //TODO : CommandMovementBuffChoice
            }
            
            else if (currentTurn == choice.turnToTakeEffect && choice.choiceType == EnumClass.ChoiceType.UltimateBuff)
            {
                //TODO : CommandUltimateBuffChoice
            }
        }
    }

}
