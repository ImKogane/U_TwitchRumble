using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWeaponChoice : CommandInGame
{
    private EnumClass.WeaponType weaponType;
    
    public CommandWeaponChoice(Player ownerOfAction, EnumClass.WeaponType typeOfWeapon) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
    }
    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        Debug.Log("Start choice Action");
        OwnerPlayer.weaponOfPlayer = WeaponFactory.CreateWeapon(weaponType);
    }
}
