using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
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

        //Pour tester le lancement de toute les actions
        if (Input.GetKeyDown(KeyCode.T))
        {
            GlobalManager.Instance.StartAllActionsInGame();
        }
    }
}
