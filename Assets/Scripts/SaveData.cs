using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //Game values
    public int _currentTurn;
    public EnumClass.GameState _currentGameState;
    
    //Board values
    public List<Vector2Int> _tilesCoords = new List<Vector2Int>();
    public List<Vector3> _tilesPositions = new List<Vector3>();

    //Players values
    public List<string> _playerNames = new List<string>();
    public List<Vector2Int> _playerTiles = new List<Vector2Int>();
    public List<int> _playerHealth = new List<int>();
    public List<EnumClass.WeaponType> _playerWeapons = new List<EnumClass.WeaponType>();
    public List<EnumClass.WeaponBuffType> _playerWeaponBuffs = new List<EnumClass.WeaponBuffType>();
    public List<EnumClass.MovementBuffType> _playerMovementBuffs = new List<EnumClass.MovementBuffType>();
    

}
