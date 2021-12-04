using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data")]
public class SO_BuffWeapon : SO_Choice
{
    public EnumClass.WeaponBuffType buffType;

    public int dotDamages = 15;
    public int duration = 1;

    public void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        switch (buffType)
        {
            case EnumClass.WeaponBuffType.Fire:

                foreach (var debuff in playerAffect.debuffList)
                {
                    if (debuff is BurningDebuff)
                    {
                        debuff.duration = duration;
                        return;
                    }
                }

                playerAffect.debuffList.Add(new BurningDebuff(duration, playerAffect, dotDamages));

                break;

            case EnumClass.WeaponBuffType.Frost:

                playerAffect.debuffList.Add(new FreezeDebuff(duration, playerAffect));

                break;
            case EnumClass.WeaponBuffType.Wind:

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

                CommandMoving moveCommand = new CommandMoving(playerAffect, direction);
                GlobalManager.Instance.InsertCommandInList(1, moveCommand);

                break;
            default:
                break;
        }
    }
}
