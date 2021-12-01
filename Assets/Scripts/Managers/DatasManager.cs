using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatasManager : MonoBehaviour
{
    public static DatasManager Instance;

    public List<SO_WeaponData> weaponDataList = new List<SO_WeaponData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public SO_WeaponData GetWeaponData(EnumClass.WeaponType weaponType)
    {
        foreach(SO_WeaponData weaponData in weaponDataList)
        {
            if (weaponData.weaponType == weaponType)
            {
                return weaponData;
            }
        }

        return null;
    }
    
    
}
