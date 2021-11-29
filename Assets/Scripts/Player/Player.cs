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

    public Weapon playerWeapon = null;
    public WeaponBuff playerWeaponBuff = null;

    public EnumClass.WeaponType typeOfWeapon;

    public Action EndOfAttack;

    public Action LastMoveActionChosen;

    public Action LastAttackActionChosen;
    
    public Material materialForTryCell;

    public int LifeOfPlayer = 100;

    public int AttackPlayer = 25;

    [Header("UI variables")]
    public Canvas playerUIPrefab;
    public Vector3 UIOffset;
    
    public Canvas playerCanvas;
    private Slider playerHealthBar;
    private TMP_Text playerNameText;

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;
        transform.position = CurrentTile.transform.position;

        playerCanvas = Instantiate(playerUIPrefab);
        playerHealthBar = playerCanvas.GetComponentInChildren<Slider>();
        playerNameText = playerCanvas.GetComponentInChildren<TMP_Text>();
        
        playerCanvas.worldCamera = Camera.main;
        playerCanvas.transform.position = transform.position;
        playerNameText.text = namePlayer;
        playerHealthBar.maxValue = LifeOfPlayer;
        playerHealthBar.minValue = 0;
        playerHealthBar.value = LifeOfPlayer;
        UpdatePlayerCanvas();
    }

    public void Attack()
    {
        List<Tile> listTileAffect = playerWeapon.Attack(CurrentTile.GetCoord(), playerMovement.RotationOfPlayer);

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                if (tile.hasPlayer)
                {
                    ApplyWeaponBuff();
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

        playerHealthBar.value = LifeOfPlayer;
        
        if (LifeOfPlayer <= 0)
        {
            Debug.Log("Player is dead !");

            PlayerManager.Instance.AllPlayersName.Remove(namePlayer);
            PlayerManager.Instance.PlayerList.Remove(this);

            CurrentTile.hasPlayer = false;
            CurrentTile.currentPlayer = null;

            GlobalManager.Instance.DestroyAllCommandsOfPlayer(this);
            
            Destroy(playerCanvas.gameObject);
            Destroy(gameObject);
        }
    }

    public void ApplyWeaponBuff()
    {
        if (playerWeaponBuff != null)
        {
            playerWeaponBuff.ApplyWeaponBuff();
        }
    }

    public void UpdatePlayerCanvas()
    {
        playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        playerCanvas.transform.position = transform.position + UIOffset;
    }
}
