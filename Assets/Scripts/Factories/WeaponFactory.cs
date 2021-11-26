using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponFactory
{

    public static Weapon CreateWeapon(EnumClass.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case(EnumClass.WeaponType.Hammer):
                return new HammerWeapon();
                break;
            
            case(EnumClass.WeaponType.Rifle):
                return new RifleWeapon();
                break;
            
            case(EnumClass.WeaponType.Scythe):
                return new ScytheWeapon();
                break;
            
        }

        return null;
    }

}
