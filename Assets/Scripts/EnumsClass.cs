using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumClass
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    };

    public enum GameState
    {
        WaitingTurn,
        ActionTurn,
        ChoseBuffTurn,
        GameEnd
    }
}

