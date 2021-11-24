using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerManager.Instance.MoveAllPlayer(EnumClass.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerManager.Instance.MoveAllPlayer(EnumClass.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerManager.Instance.MoveAllPlayer(EnumClass.Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerManager.Instance.MoveAllPlayer(EnumClass.Direction.Left);
        }
    }
}

public static class EnumClass
{
    public enum Direction { Up, Down, Right, Left };
}
