using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheWeapon : Weapon
{
    public ScytheWeapon(Player ownerPlayer) : base(ownerPlayer)
    {
        weaponData = DatasManager.Instance.GetWeaponData(EnumClass.WeaponType.Scythe);
    }
    
    
    public override List<Tile> GetAffectedTiles(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
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
