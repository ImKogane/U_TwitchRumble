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

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        weaponOfPlayer = new Weapon(typeOfWeapon);
    }

    public void SpawnPlayerInGame(Tile TileForStart)
    {
        CurrentTile = TileForStart;
        transform.position = CurrentTile.transform.position;
        TileForStart.hasPlayer = true;
    }

    public void Attack()
    {
        List<Tile> listTileAffect = weaponOfPlayer.Attack(CurrentTile.GetCoord(), playerMovement.RotationOfPlayer);

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                tile.gameObject.GetComponentInChildren<MeshRenderer>().material = materialForTryCell;
            }
        }

        EndOfAttack.Invoke();
    }
}
