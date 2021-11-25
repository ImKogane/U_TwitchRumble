using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour
{
    public Tile CurrentTile;

    public PlayerMovement playerMovement;
    
    public TMP_Text playerNameText;
    public Slider playerLifeBar;

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

        playerNameText.text = namePlayer;
        playerNameText.color = UnityEngine.Random.ColorHSV();
        playerLifeBar.maxValue = LifeOfPlayer;
        playerLifeBar.minValue = 0;
        playerLifeBar.value = LifeOfPlayer;
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

    public void ReceiveDammage(int damage)
    {
        LifeOfPlayer -= damage;

        playerLifeBar.value = LifeOfPlayer;
        
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
