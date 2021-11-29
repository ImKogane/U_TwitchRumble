using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWeaponBuffChoice : CommandInGame
{
    private EnumClass.WeaponBuffType weaponBuffType;
    
    public CommandWeaponBuffChoice(Player ownerOfAction, EnumClass.WeaponBuffType newWeaponBuffType) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
        weaponBuffType = newWeaponBuffType;
    }
    public override void LaunchActionInGame()
    {
        OwnerPlayer.playerWeaponBuff = WeaponBuffFactory.CreateWeaponBuff(weaponBuffType);
        Debug.Log("New Player's Weapon Buff : " + OwnerPlayer.playerWeapon.GetType());
        EndActionInGame();
    }
}
