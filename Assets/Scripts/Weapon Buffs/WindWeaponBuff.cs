using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWeaponBuff : WeaponBuff
{
    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        if (playerAttacking.playerWeapon is HammerWeapon || playerAttacking.playerWeapon is RifleWeapon)
        {
            //Faux = a changer pour adapter correctement la rotation du joueur qui prend l'attaque. 
            playerAffect.playerMovement.RotationOfPlayer = playerAttacking.playerMovement.RotationOfPlayer;
            playerAffect.gameObject.transform.rotation = playerAttacking.gameObject.transform.rotation;

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
                if (PosXAttacker < PosXReceiver) // Gauche
                {
                    playerAffect.playerMovement.RotateRightDirection();
                }
                if(PosXAttacker > PosXReceiver) //Droite
                {
                    playerAffect.playerMovement.RotateLeftDirection();
                }

                playerAffect.playerMovement.MakeMovement();
            }

            if (PosYAttacker != PosYReceiver) // Defenseur en haut ou en bas
            {
                if (PosYAttacker < PosYReceiver) //Haut
                {
                    playerAffect.playerMovement.RotateUpDirection(); 
                }
                else if (PosYAttacker > PosYReceiver) //Bas
                {
                    playerAffect.playerMovement.RotateDownDirection(); 
                }

                playerAffect.playerMovement.MakeMovement();
            }
        }
       
    }
}
