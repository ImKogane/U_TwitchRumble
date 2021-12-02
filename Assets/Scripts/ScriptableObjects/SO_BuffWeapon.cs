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

                /*playerAffect.playerMovement.RotationOfPlayer = playerAttacking.playerMovement.RotationOfPlayer;
                playerAffect.playerMovement.MakeMovement();*/

                CommandMoving moveCommand = new CommandMoving(playerAffect, playerAttacking.playerMovement.RotationOfPlayer);
                GlobalManager.Instance.ListCommandsInGame.Insert(1, moveCommand);

                /*else if (playerAttacking.playerWeapon is ScytheWeapon)
                {
                    int PosXAttacker = playerAttacking.CurrentTile.tileRow;
                    int PosYAttacker = playerAttacking.CurrentTile.tileColumn;

                    int PosXReceiver = playerAffect.CurrentTile.tileRow;
                    int PosYReceiver = playerAffect.CurrentTile.tileColumn;

                    if (PosXAttacker != PosXReceiver) // Defenseur a droite ou a gauche
                    {
                        if (PosXAttacker < PosXReceiver) // Droite
                        {
                            playerAffect.playerMovement.RotationOfPlayer = new Vector2Int(1, 0);
                        }
                        if (PosXAttacker > PosXReceiver) //Gauche
                        {
                            playerAffect.playerMovement.RotationOfPlayer = new Vector2Int(-1, 0);
                        }

                        playerAffect.playerMovement.MakeMovement();
                    }

                    if (PosYAttacker != PosYReceiver) // Defenseur en haut ou en bas
                    {
                        if (PosYAttacker < PosYReceiver) //Haut
                        {
                            playerAffect.playerMovement.RotationOfPlayer = new Vector2Int(0, 1);
                        }
                        else if (PosYAttacker > PosYReceiver) //Bas
                        {
                            playerAffect.playerMovement.RotationOfPlayer = new Vector2Int(0, -1);
                        }

                        playerAffect.playerMovement.MakeMovement();
                    }
                }*/

                break;
            default:
                break;
        }
    }
}
