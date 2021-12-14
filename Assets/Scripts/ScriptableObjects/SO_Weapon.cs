using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon Data")]
[System.Serializable]
public class SO_Weapon : SO_Choice
{
    [Header("Weapon Type (used for saves)")]
    public EnumClass.WeaponType weaponType;
    
    [Header("Skin Of Weapon")]

    public GameObject _weaponPrefab;

    [Header("Weapon Range (when player look up)")]

    public List<Vector2Int> _listOfCellAffects = new List<Vector2Int>();

    [Header("Animation")]

    public AnimatorController _weaponAnimatorController;

    [Header("Events Called")]
    public SO_GameEvent _onHitEventSFX;
    
    public void RaiseEvent()
    {
        _onHitEventSFX.Raise();
    }
}
