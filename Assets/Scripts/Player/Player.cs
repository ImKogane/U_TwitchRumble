using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Tile CurrentTile;

    public PlayerMovement playerMovement;

    public int IDOfPlayer;

    private Weapon weaponOfPlayer = null;

    public EnumClass.WeaponType typeOfWeapon;

    public Action EndOfAttack;

    public Material materialForTryCell;

    public int LifeOfPlayer = 100;

    public int AttackPlayer = 25;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        weaponOfPlayer = new Weapon(typeOfWeapon);
    }

    public void SpawnPlayerInGame(Tile TileForStart)
    {
        CurrentTile = TileForStart;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;

        transform.position = CurrentTile.transform.position;
    }

    public void Attack()
    {
        List<Tile> listTileAffect = weaponOfPlayer.Attack(CurrentTile.GetCoord(), playerMovement.RotationOfPlayer);

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                if (tile.hasPlayer)
                {
                    tile.currentPlayer.ReceiveDammage(AttackPlayer);
                }
            }
        }

        EndOfAttack.Invoke();
    }

    public void ReceiveDammage(int dammage)
    {
        LifeOfPlayer -= dammage;

        if (LifeOfPlayer <= 0)
        {
            Debug.Log("Player is dead !");
        }

        if (CurrentTile)
        {
            CurrentTile.gameObject.GetComponentInChildren<MeshRenderer>().material = materialForTryCell;
        }
    }
}
