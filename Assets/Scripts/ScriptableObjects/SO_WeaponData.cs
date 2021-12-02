using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon Data")]
public class SO_WeaponData : ScriptableObject
{
    public EnumClass.WeaponType weaponType;

    public GameObject weaponVFX;

    public AudioClip weaponSFX;

    public GameObject weaponPrefab;

    public AnimatorController weaponAnimatorController;
}
