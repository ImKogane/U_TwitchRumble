using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public SO_Weapon weaponData;

    protected Player ownerPlayer;

    private GameObject weaponPrefab;

    protected List<Transform> VFXtransformList = new List<Transform>();
    
    public Weapon(Player newOwnerPlayer, EnumClass.WeaponType weaponType)
    {
        weaponData = DatasManager.Instance.GetWeaponData(weaponType);
        ownerPlayer = newOwnerPlayer;
        ownerPlayer.playerAnimator.runtimeAnimatorController = weaponData._weaponAnimatorController;
        
        foreach(Transform childTransform in ownerPlayer.GetComponentsInChildren<Transform>())
        {
            if(childTransform.name == "Hand_R")
                weaponPrefab = GameObject.Instantiate(weaponData._weaponPrefab, childTransform, false);
        }

    }
   

    public virtual List<Tile> GetAffectedTiles(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        return null;
    }


    public virtual void PlayWeaponVFX()
    {
        foreach (var currentTransform in VFXtransformList)
        {
            GameObject.Instantiate(weaponData._prefabWeaponVFX, currentTransform.position, Quaternion.identity); 
        }
        
        VFXtransformList.Clear();
    }

    public void PlayWeaponSFX()
    {
        AudioManager.Instance.PlaySFX(weaponData._weaponSFX);
    }


}
