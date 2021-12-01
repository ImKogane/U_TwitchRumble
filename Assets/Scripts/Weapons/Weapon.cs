using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{

    public SO_WeaponData weaponData;

    private Player ownerPlayer;
    
    public Weapon(Player ownerPlayer)
    {
        
    }
   

    public virtual List<Tile> Attack(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        return null;
    }


    public virtual void PlayWeaponVFX()
    {
        
    }

    public void PlayWeaponSFX()
    {
        AudioManager.Instance.PlaySFX(weaponData.weaponSFX);
    }
    
    
    
    

}
