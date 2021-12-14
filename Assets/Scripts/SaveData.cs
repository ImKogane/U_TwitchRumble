using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaveData
{
    public int _currentTurn;
    public List<PlayerData> _playersDatas;
    public List<TileData> _tilesDatas;
}

[System.Serializable]
public struct TransformData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    public TransformData(Transform transform)
    {
        Position = transform.position;
        Rotation = transform.rotation.eulerAngles;
        Scale = transform.localScale;
    }
}

[System.Serializable]
public struct TileData
{
    public Vector2Int _tileCoords;
    public bool _hasObstacle;
    public TransformData _tileTransform;
    public int _tileTrapCount;
}


[System.Serializable]
public struct PlayerData
{
    public string _playerName;
    public Vector2Int _playerTile;
    public List<Debuff> _playerDebuffs;
    public int _playerHealth;
    public SO_Weapon _playerWeapon;
    public SO_BuffWeapon _playerWeaponBuff;
    public SO_BuffMoving _playerMovementBuff;
    public int _materialIndex;
    public int _skinnedMeshIndex;
}
