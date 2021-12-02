using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon Data")]
public class SO_Weapon : ScriptableObject
{
    [Header("Skin Of Weapon")]

    public GameObject _weaponPrefab;

    [Header("Weapon Range (when player look up)")]

    public List<Vector2Int> _listOfCellAffects = new List<Vector2Int>();

    [Header("Animation")]

    public AnimatorController _weaponAnimatorController;

    [Header("VFX et SFX")]

    public GameObject _prefabWeaponVFX;

    public AudioClip _weaponSFX;

    [Header("Sprite of card")]

    public Sprite _cardSprite;
}
