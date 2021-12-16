using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data/Wind Buff")]
public class WindWeaponBuff : SO_BuffWeapon
{
    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        Vector2Int rotOfPlayerAttacking = playerAttacking.playerMovement.RotationOfPlayer;
        Vector2Int posPlayerAffect = new Vector2Int(playerAffect.CurrentTile.tileRow, playerAffect.CurrentTile.tileColumn);
        Vector2Int posPlayerAttacking = new Vector2Int(playerAttacking.CurrentTile.tileRow, playerAttacking.CurrentTile.tileColumn);

        Vector2Int direction = Vector2Int.zero;

        if (rotOfPlayerAttacking.x != 0) //Si je joueur est tourné vers la gauche ou droite
        {
            if (posPlayerAttacking.x != posPlayerAffect.x) //Droite ou gauche
            {
                if (posPlayerAttacking.x > posPlayerAffect.x) //Gauche
                {
                    direction = new Vector2Int(-1, 0);
                }
                if (posPlayerAttacking.x < posPlayerAffect.x) //Droite
                {
                    direction = new Vector2Int(1, 0);
                }
            }
            else //Haut et bas
            {
                if (posPlayerAttacking.y > posPlayerAffect.y) //Bas
                {
                    direction = new Vector2Int(0, -1);
                }
                if (posPlayerAttacking.y < posPlayerAffect.y) //Haut
                {
                    direction = new Vector2Int(0, 1);
                }
            }
        }
        else //Si le joueur est tourné vers le haut ou le bas
        {
            if (posPlayerAttacking.y != posPlayerAffect.y) 
            {
                if (posPlayerAttacking.y > posPlayerAffect.y) //Bas
                {
                    direction = new Vector2Int(0, -1);
                }
                if (posPlayerAttacking.y < posPlayerAffect.y) //Haut
                {
                    direction = new Vector2Int(0, 1);
                }
            }
            else
            {
                if (posPlayerAttacking.x > posPlayerAffect.x) //Gauche
                {
                    direction = new Vector2Int(-1, 0);
                }
                if (posPlayerAttacking.x < posPlayerAffect.x) //Droite
                {
                    direction = new Vector2Int(1, 0);
                }
            }
        }

        CommandMoving moveCommand = new CommandMoving(playerAffect, direction, true);
        CommandManager.Instance.InsertCommandInList(1, moveCommand);
    }
}