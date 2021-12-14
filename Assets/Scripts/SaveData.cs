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
public struct WeaponData
{
    public EnumClass.WeaponType _weaponType;
    
    public WeaponData(SO_Weapon weapon)
    {
        _weaponType = weapon.weaponType;
    }
}


[System.Serializable]
public struct WeaponBuffData
{
    public EnumClass.WeaponBuffType _weaponBuffType;
    
    public WeaponBuffData(SO_BuffWeapon weaponBuff)
    {
        _weaponBuffType = weaponBuff.weaponBuffType;
    }
}


[System.Serializable]
public struct MovementBuffData
{
    public EnumClass.MovementBuffType _movementBuffType;
    
    public MovementBuffData(SO_BuffMoving _moveBuff)
    {
        _movementBuffType = _moveBuff.movementBuffType;
    }
}

[System.Serializable]
public struct PlayerData
{
    public string _playerName;
    public Vector2Int _playerTile;
    public List<int> _durationOfActiveBurningDebuff;
    public List<int> _durationOfActiveFreezeDebuff;
    public int _playerHealth;
    public WeaponData _weaponData;
    public WeaponBuffData _weaponBuffData;
    public MovementBuffData _movementBuffData;
    public int _materialIndex;
    public int _skinnedMeshIndex;
}
