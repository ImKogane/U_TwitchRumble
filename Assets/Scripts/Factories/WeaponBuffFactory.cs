using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponBuffFactory
{
    public static WeaponBuff CreateWeaponBuff(EnumClass.WeaponBuffType weaponBuffType)
    {
        switch (weaponBuffType)
        {
            case(EnumClass.WeaponBuffType.Fire):
                return new FireWeaponBuff();
                break;
            
            case(EnumClass.WeaponBuffType.Frost):
                return new FrostWeaponBuff();
                break;
            
            case(EnumClass.WeaponBuffType.Wind):
                return new WindWeaponBuff();
                break;
            
        }

        return null;
    }
}
