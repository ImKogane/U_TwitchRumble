using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaveData
{
    public int _currentTurn;

    //Board values
    public List<Vector2Int> _tilesCoords;
    public List<Vector3> _tilesPositions;

    //Players values
    public List<string> _playerNames;
    public List<Vector2Int> _playerTiles;
    public List<Vector2Int> _playerRotations;
    //public List<List<Debuff>> _playerDebuffs;
    public List<int> _playerHealth;
    public List<EnumClass.WeaponType> _playerWeapons;
    public List<EnumClass.WeaponBuffType> _playerWeaponBuffs;
    public List<EnumClass.MovementBuffType> _playerMovementBuffs;
}
