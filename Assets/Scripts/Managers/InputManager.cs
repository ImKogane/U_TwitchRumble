using UnityEngine;

public class InputManager : SingletonMonobehaviour<InputManager>
{
    private bool _actionInputsEnabled;
    private bool _choiceInputsEnabled;

    public override bool DestroyOnLoad => false;

    void Update()
    {
        if (_actionInputsEnabled) InputPlayerLocal();
        if (_choiceInputsEnabled) ChoiceInputPlayerLocal();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalManager.Instance.SetGamePause(true);
        }
    }
    
    public void EnableActionInputs(bool value)
    {
        _actionInputsEnabled = value;
    }

    public void EnableChoiceInputs(bool value)
    {
        _choiceInputsEnabled = value;
    }

    void ChoiceInputPlayerLocal()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChoiceCommand(PlayerManager.Instance._listPlayers[0], 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChoiceCommand(PlayerManager.Instance._listPlayers[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChoiceCommand(PlayerManager.Instance._listPlayers[0], 2);
        }
    }
    
    void InputPlayerLocal()
    {
        //Actions de déplacements. 
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCommand(PlayerManager.Instance._listPlayers[0], EnumClass.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCommand(PlayerManager.Instance._listPlayers[0], EnumClass.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCommand(PlayerManager.Instance._listPlayers[0], EnumClass.Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCommand(PlayerManager.Instance._listPlayers[0], EnumClass.Direction.Left);
        }

        //Action d'attaque.
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttackCommand(PlayerManager.Instance._listPlayers[0]);
        }
    }

    public void MoveCommand(Player player, EnumClass.Direction direction)
    {
        CommandManager.Instance.RemoveMoveCommandOfPlayer(player);

        CommandMoving ActionToDo = null;
        switch (direction)
        {
            case EnumClass.Direction.Up:
                ActionToDo = new CommandMoving(player, new Vector2Int(0, 1));
                player.DisplayCommandTxt("[Up]");
                break;
            case EnumClass.Direction.Down:
                ActionToDo = new CommandMoving(player, new Vector2Int(0, -1));
                player.DisplayCommandTxt("[Down]");
                break;
            case EnumClass.Direction.Right:
                ActionToDo = new CommandMoving(player, new Vector2Int(1, 0));
                player.DisplayCommandTxt("[Right]");
                break;
            case EnumClass.Direction.Left:
                ActionToDo = new CommandMoving(player, new Vector2Int(-1, 0));
                player.DisplayCommandTxt("[Left]");
                break;
            default:
                break;
        }

        CommandManager.Instance.AddCommandToList(ActionToDo);
    }

    public void AttackCommand(Player player)
    {
        CommandManager.Instance.RemoveAttackCommandOfPlayer(player);
        CommandAttack ActionToDo = new CommandAttack(player);
        CommandManager.Instance.AddCommandToList(ActionToDo);
        player.DisplayCommandTxt("[Attack]");
    }

    public void ChoiceCommand(Player player, int choiceIndex)
    {
        int currentIndexChoice = ScriptableManager.Instance.GetChoiceIndexCompteur();

        if (ScriptableManager.Instance._turnChoiceList.Count <= currentIndexChoice)
        {
            Debug.Log("Number of command is out of range !");
            return;
        }

        if (ScriptableManager.Instance._turnChoiceList[currentIndexChoice].choiceList.Count <= choiceIndex)
        {
            Debug.Log("Number of command is out of range !");
            return;
        }

        SO_Choice currentChoice = ScriptableManager.Instance.GetChoiceFromIndex(currentIndexChoice, choiceIndex);

        if (currentChoice)
        {
            currentChoice.ApplyChoice(player);
        }

        if (_choiceInputsEnabled) //Just to block display UI for players who will have a random choice.
        {
            UIManager.Instance.DisplayChoiceTxt("[" + player._name + "]", choiceIndex);
        }
    }

    
    
}
