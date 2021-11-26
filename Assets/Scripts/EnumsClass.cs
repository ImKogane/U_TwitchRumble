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

    public enum WeaponType
    {
        Hammer,
        Scythe,
        Rifle
    }

    public enum ChosenCard
    {
        Card1,
        Card2,
        Card3
    }

    public enum ChoiceType
    {
        Weapon,
        WeaponBuff,
        MovementBuff,
        UltimateBuff
    }
}

