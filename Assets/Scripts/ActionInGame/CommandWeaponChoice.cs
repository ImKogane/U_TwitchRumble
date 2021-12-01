using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWeaponChoice : CommandInGame
{
    private EnumClass.WeaponType weaponType;
    
    public CommandWeaponChoice(Player ownerOfAction, EnumClass.WeaponType typeOfWeapon) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
        weaponType = typeOfWeapon;
    }
    public override void LaunchActionInGame()
    {
        OwnerPlayer.playerWeapon = WeaponFactory.CreateWeapon(weaponType, OwnerPlayer);
        Debug.Log("New Player's Weapon : " + OwnerPlayer.playerWeapon.GetType());
        EndActionInGame();
    }
}
