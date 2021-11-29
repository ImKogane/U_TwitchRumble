using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWeaponBuff : WeaponBuff
{
    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        //Enregistrer pour remettre sa vrai rotation a la fin de la poussée. 
        Vector2Int oldRotOfPlayer = playerAffect.playerMovement.RotationOfPlayer;

        if (playerAttacking.playerWeapon is HammerWeapon || playerAttacking.playerWeapon is RifleWeapon)
        {
            //Faux = a changer pour adapter correctement la rotation du joueur qui prend l'attaque. 
            playerAffect.playerMovement.RotationOfPlayer = playerAttacking.playerMovement.RotationOfPlayer;
            //playerAffect.gameObject.transform.rotation = playerAttacking.gameObject.transform.rotation;
            playerAffect.playerMovement.MakeMovement();
        }
        else if (playerAttacking.playerWeapon is ScytheWeapon)
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
                if(PosXAttacker > PosXReceiver) //Gauche
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
        }

        playerAffect.playerMovement.RotationOfPlayer = oldRotOfPlayer;
    }
}
