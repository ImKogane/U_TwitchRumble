using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheWeapon : Weapon
{
    public ScytheWeapon(Player ownerPlayer, EnumClass.WeaponType weaponType) : base(ownerPlayer, weaponType)
    {
        
    }
    
    public override void PlayWeaponVFX()
    {
        VFXtransformList.Add(ownerPlayer.transform);
        base.PlayWeaponVFX();
    }
    
    
    public override List<Tile> Attack(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        List<Tile> returnList = new List<Tile>();
        Tile tileToAdd = null;

        if (tileToAdd = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentCellOfPlayer.x - 1, CurrentCellOfPlayer.y)))
        {
            returnList.Add(tileToAdd);
        }
        if (tileToAdd = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentCellOfPlayer.x + 1, CurrentCellOfPlayer.y)))
        {
            returnList.Add(tileToAdd);
        }
        if (tileToAdd = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentCellOfPlayer.x, CurrentCellOfPlayer.y -1)))
        {
            returnList.Add(tileToAdd);
        }
        if (tileToAdd = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentCellOfPlayer.x , CurrentCellOfPlayer.y + 1)))
        {
            returnList.Add(tileToAdd);
        }

        return returnList;
    }

}
