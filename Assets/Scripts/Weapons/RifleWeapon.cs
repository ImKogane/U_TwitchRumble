using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon
{
    public RifleWeapon(Player ownerPlayer, EnumClass.WeaponType weaponType) : base(ownerPlayer, weaponType)
    {

    }
    
    public override void PlayWeaponVFX()
    {
        Transform socket = weaponData.weaponPrefab.transform.Find("VFX_Socket");
        VFXtransformList.Add(socket);
        base.PlayWeaponVFX();
    }
    
    public override List<Tile> GetAffectedTiles(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        List<Tile> returnList = new List<Tile>();
        Tile tileToAdd = null;

        if (RotationOfPlayer.x != 0)
        {
            for (int i = 1; i < 5; i++)
            {
                if (BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x * i, RotationOfPlayer.y)))
                {
                    tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x * i, RotationOfPlayer.y));
                    if (tileToAdd.hasObstacle)
                    {
                        break;
                    }
                    returnList.Add(tileToAdd);
                }
            }
        }

        if (RotationOfPlayer.y != 0)
        {
            for (int i = 1; i < 5; i++)
            {
                if (BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y * i)))
                {
                    tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y * i));
                    if (tileToAdd.hasObstacle)
                    {
                        break;
                    }
                    returnList.Add(tileToAdd);
                }
            }
        }

        return returnList;
    }
}
