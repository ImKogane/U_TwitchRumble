using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerWeapon : Weapon
{

    public HammerWeapon()
    {
        weaponData = DatasManager.Instance.GetWeaponData(EnumClass.WeaponType.Hammer);
    }
    
    
    public override List<Tile> Attack(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        List<Tile> returnList = new List<Tile>();
        Tile tileToAdd = null;

        if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + RotationOfPlayer))
        {
            returnList.Add(tileToAdd);
        }

        if (RotationOfPlayer.x != 0)
        {
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x * 2, RotationOfPlayer.y)))
            {
                returnList.Add(tileToAdd);
            }
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y +1)))
            {
                returnList.Add(tileToAdd);
            }
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y - 1)))
            {
                returnList.Add(tileToAdd);
            }
        }

        if (RotationOfPlayer.y != 0)
        {
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y * 2)))
            {
                returnList.Add(tileToAdd);
            }
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x + 1, RotationOfPlayer.y )))
            {
                returnList.Add(tileToAdd);
            }
            if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x - 1, RotationOfPlayer.y)))
            {
                returnList.Add(tileToAdd);
            }
        }

        return returnList;
    }
}
