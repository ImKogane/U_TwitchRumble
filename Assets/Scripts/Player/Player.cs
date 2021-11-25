using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Tile CurrentTile;

    public PlayerMovement playerMovement;

    public string namePlayer;

    private Weapon weaponOfPlayer = null;

    public EnumClass.WeaponType typeOfWeapon;

    public Action EndOfAttack;

    public Action LastMoveActionChosen;

    public Action LastAttackActionChosen;
    
    public Material materialForTryCell;

    public int LifeOfPlayer = 100;

    public int AttackPlayer = 25;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        weaponOfPlayer = new Weapon(typeOfWeapon);
    }

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;

        transform.position = CurrentTile.transform.position;
    }

    public void Attack()
    {
        Debug.Log("ATTTACKKKKKKKKKKK");

        List<Tile> listTileAffect = weaponOfPlayer.Attack(CurrentTile.GetCoord(), playerMovement.RotationOfPlayer);
        Color randColor = chooseRandomColor();

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                Debug.Log("Tile Affect : " + tile.tileRow + "/" + tile.tileColumn);
                //tile.currentPlayer.ReceiveDammage(AttackPlayer);
                tile.GetComponentInChildren<MeshRenderer>().material.color = randColor;
            }
            if (tile.hasPlayer)
            {
                tile.currentPlayer.ReceiveDammage(AttackPlayer);
            }
        }

        EndOfAttack.Invoke();
    }

    public Color chooseRandomColor()
    {
        List<Color> listColor = new List<Color>() { Color.black, Color.blue, Color.green, Color.red, Color.yellow , Color.white};
        int randIndex = UnityEngine.Random.Range(0, listColor.Count);

        return listColor[randIndex];
    }

    public void ReceiveDammage(int dammage)
    {
        LifeOfPlayer -= dammage;

        if (LifeOfPlayer <= 0)
        {
            Debug.Log("Player is dead !");
            Destroy(gameObject);
        }

        if (CurrentTile)
        {
            CurrentTile.gameObject.GetComponentInChildren<MeshRenderer>().material = materialForTryCell;
        }
    }
}
