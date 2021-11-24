using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public EnumClass.WeaponType weaponType;

    public Weapon(EnumClass.WeaponType Type)
    {
        weaponType = Type;
    }

    public virtual List<Tile> Attack(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        switch (weaponType)
        {
            case EnumClass.WeaponType.Hammer:
                return AttackWithHammer(CurrentCellOfPlayer, RotationOfPlayer);
                break;
            case EnumClass.WeaponType.Scythe:
                return AttackWithScythe(CurrentCellOfPlayer);
                break;
            case EnumClass.WeaponType.Rifle:
                return AttackWithRifle(CurrentCellOfPlayer, RotationOfPlayer);
                break;
            default:
                return null;
                break;
        }
    }

    private List<Tile> AttackWithHammer(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer) 
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
    private List<Tile> AttackWithScythe(Vector2Int CurrentCellOfPlayer)
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
    private List<Tile> AttackWithRifle(Vector2Int CurrentCellOfPlayer, Vector2Int RotationOfPlayer)
    {
        List<Tile> returnList = new List<Tile>();
        Tile tileToAdd = null;

        if (RotationOfPlayer.x != 0)
        {
            for (int i = 1; i < 5; i++)
            {
                if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x * i, RotationOfPlayer.y)))
                {
                    if (tileToAdd.hasObstacle || tileToAdd.hasPlayer)
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
                if (tileToAdd = BoardManager.Instance.GetTileAtPos(CurrentCellOfPlayer + new Vector2Int(RotationOfPlayer.x, RotationOfPlayer.y * i)))
                {
                    if (tileToAdd.hasObstacle || tileToAdd.hasPlayer)
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
