using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public SO_WeaponData weaponData;

    protected Player ownerPlayer;
    
    public Weapon(Player OwnerPlayer)
    {
        ownerPlayer = OwnerPlayer;
    }
   

    public virtual List<Tile> GetAffectedTiles(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
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
