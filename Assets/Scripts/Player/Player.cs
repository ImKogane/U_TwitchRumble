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

    public string namePlayer;

    private Weapon weaponOfPlayer = null;

    public EnumClass.WeaponType typeOfWeapon;

    public Action EndOfAttack;

    public Action LastMoveActionChosen;

    public Action LastAttackActionChosen;
    
    public Material materialForTryCell;

    public int LifeOfPlayer = 100;

    public int AttackPlayer = 25;

    [Header("UI variables")]
    public TMP_Text playerNameText;
    public Slider playerLifeBar;
    public Canvas playerCanvas;

    private void Start()
    {
        //playerMovement = GetComponent<PlayerMovement>();
        weaponOfPlayer = new Weapon(typeOfWeapon);
        playerCanvas.worldCamera = Camera.main;
        UpdateRotationOfUI();
    }

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;
        transform.position = CurrentTile.transform.position;

        playerNameText.text = namePlayer;
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
                else
                {
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().material = materialForTryCell;
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

            PlayerManager.Instance.AllPlayersName.Remove(namePlayer);
            PlayerManager.Instance.PlayerList.Remove(this);

            CurrentTile.hasPlayer = false;
            CurrentTile.currentPlayer = null;

            GlobalManager.Instance.DestroyAllCommandsOfPlayer(this);

            Destroy(gameObject);
        }
    }

    public void UpdateRotationOfUI()
    {
        playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
}
